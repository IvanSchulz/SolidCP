// Copyright (C) 2025 FuseCP
//
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.

using System;
using System.Linq;
using System.Management.Automation;
using Microsoft.Management.Infrastructure;
using Microsoft.Management.Infrastructure.Generic;
using FuseCP.Server.Utils;

namespace FuseCP.Providers.DNS
{
    /// <summary>Copy fields from CimInstance#DnsServerResourceRecord into DnsRecord</summary>
    /// <remarks>It's also possible to access native CIM object, and use Mgmtclassgen.exe for that.</remarks>
    internal static class RecordConverter
    {
        internal static string RemoveTrailingDot(string str)
        {
            if (!str.EndsWith("."))
                return str;
            return str.Substring(0, str.Length - 1);
        }

        internal static string CorrectHost(string zoneName, string host)
        {
            if ("@" == host || host.ToLower() == zoneName.ToLower())
                return String.Empty;

            if (host.ToLower().EndsWith("." + zoneName.ToLower()))
                return host.Substring(0, (host.Length - zoneName.Length - 1));
            return host;
        }

        public static DnsRecord asDnsRecord(this PSObject obj, string zoneName, bool get)
        {
            // Here's what comes from Server 2012 in the TypeNames:
            // "Microsoft.Management.Infrastructure.CimInstance#root/Microsoft/Windows/DNS/DnsServerResourceRecord"
            // "Microsoft.Management.Infrastructure.CimInstance#ROOT/Microsoft/Windows/DNS/DnsDomain"
            // "Microsoft.Management.Infrastructure.CimInstance#DnsServerResourceRecord"
            // "Microsoft.Management.Infrastructure.CimInstance#DnsDomain"
            // "Microsoft.Management.Infrastructure.CimInstance"
            // "System.Object"	string

            if (!obj.TypeNames.Contains("Microsoft.Management.Infrastructure.CimInstance#DnsServerResourceRecord"))
            {
                Log.WriteWarning("asDnsRecord: wrong object type {0}", obj.TypeNames.FirstOrDefault());
                return null;
            }

            string strRT = (string)obj.Properties["RecordType"].Value;
            DnsRecordType tp;
            if (!RecordTypes.recordFromString.TryGetValue(strRT, out tp))
                return null;

            /*// Debug code below: 
			obj.dumpProperties();
			CimInstance rd = (CimInstance)obj.Properties[ "RecordData" ].Value;
			rd.dumpProperties(); //*/

            CimKeyedCollection<CimProperty> data = ((CimInstance)obj.Properties["RecordData"].Value).CimInstanceProperties;
            string host = CorrectHost(zoneName, (string)obj.Properties["HostName"].Value);

            TimeSpan ttldata = (TimeSpan)obj.Properties["TimeToLive"].Value;
            int timetolive = (int)ttldata.TotalSeconds;

            switch (tp)
            {
                // The compiler should create a Dictionary<> from dis switch
                case DnsRecordType.A:
                    {
                        return new DnsRecord()
                        {
                            RecordType = tp,
                            RecordName = host,
                            RecordTTL  = timetolive,
                            RecordData = data["IPv4Address"].Value as string,
                        };
                    }
                case DnsRecordType.AAAA:
                    {
                        return new DnsRecord()
                        {
                            RecordType = tp,
                            RecordName = host,
                            RecordTTL  = timetolive,
                            RecordData = data["IPv6Address"].Value as string,
                        };
                    }
                case DnsRecordType.CNAME:
                    {
                        return new DnsRecord()
                        {
                            RecordType = tp,
                            RecordName = host,
                            RecordTTL  = timetolive,
                            RecordData = RemoveTrailingDot(data["HostNameAlias"].Value as string),
                        };
                    }
                case DnsRecordType.MX:
                    {
                        return new DnsRecord()
                        {
                            RecordType = tp,
                            RecordName = host,
                            RecordTTL  = timetolive,
                            RecordData = RemoveTrailingDot(data["MailExchange"].Value as string),
                            MxPriority = (UInt16)data["Preference"].Value,
                        };
                    }
                case DnsRecordType.NS:
                    {
                        return new DnsRecord()
                        {
                            RecordType = tp,
                            RecordName = host,
                            RecordTTL = timetolive,
                            RecordData = RemoveTrailingDot(data["NameServer"].Value as string),
                        };
                    }
                case DnsRecordType.TXT:
                    {
                        return new DnsRecord()
                        {
                            RecordType = tp,
                            RecordName = host,
                            RecordTTL = timetolive,
                            RecordData = data["DescriptiveText"].Value as string,
                        };
                    }
                case DnsRecordType.SOA:
                    {
                        string PrimaryServer = data["PrimaryServer"].Value as string;
                        string ResponsiblePerson = data["ResponsiblePerson"].Value as string;
                        UInt32? sn = (UInt32?)data["SerialNumber"].Value;
                        return new DnsSOARecord()
                        {
                            RecordType = tp,
                            RecordName = host,
                            RecordTTL = timetolive,
                            PrimaryNsServer = PrimaryServer,
                            PrimaryPerson = ResponsiblePerson,
                            SerialNumber = (sn.HasValue) ? sn.Value.ToString() : null,
                        };
                    }
                case DnsRecordType.SRV:
                    {
                        return new DnsRecord()
                        {
                            RecordType = tp,
                            RecordName = host,
                            RecordTTL = timetolive,
                            RecordData = RemoveTrailingDot(data["DomainName"].Value as string),
                            SrvPriority = (UInt16)data["Priority"].Value,
                            SrvWeight = (UInt16)data["Weight"].Value,
                            SrvPort = (UInt16)data["Port"].Value,
                        };
                    }
                case DnsRecordType.PTR:
                    {
                        return new DnsRecord()
                        {
                            RecordType = tp,
                            RecordName = host,
                            RecordTTL = timetolive,
                            RecordData = RemoveTrailingDot(data["PtrDomainName"].Value as string),
                        };
                    }
                case DnsRecordType.CAA:
                case DnsRecordType.UNKNOWN:
                    {
                        DnsRecordType type;
                        if (get)
                        {
                            type = DnsRecordType.CAA;
                        }
                        else
                        {
                            type = DnsRecordType.UNKNOWN;
                        }
                        return new DnsRecord()
                        {
                            RecordType = type,
                            RecordName = host,
                            RecordTTL = timetolive,
                            RecordData = data["Data"].Value as string,
                        };
                    }
            }
            return null;
        }
    }
}
