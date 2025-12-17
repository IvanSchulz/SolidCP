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
using FuseCP.Providers.OS;
using FuseCP.Server.Utils;

namespace FuseCP.Providers.DNS
{
    public class MsDNS2016 : MsDNS2012, IDnsServer
    {
        private PowerShellHelper ps = null;
        private bool bulkRecords;

        public MsDNS2016()
        {
            // Create PowerShell helper
            ps = new PowerShellHelper();
            /* FIX: this code here is bogus 
             if (!this.IsInstalled())
                return; */
        }

        #region Zones

        public override DnsRecord[] GetZoneRecords(string zoneName)
        {
            return ps.GetZoneRecords(zoneName);
        }

        public override void AddZoneRecord(string zoneName, DnsRecord record)
        {
            try
            {
                string name = record.RecordName;
                if (String.IsNullOrEmpty(name))
                    name = ".";

                if (record.RecordTTL == 0)
                    record.RecordTTL = DNSRecordDefaultTTL;

                if (record.RecordType == DnsRecordType.A)
                    ps.Add_DnsServerResourceRecordA(zoneName, name, record.RecordData, TimeSpan.FromSeconds(record.RecordTTL));
                else if (record.RecordType == DnsRecordType.AAAA)
                    ps.Add_DnsServerResourceRecordAAAA(zoneName, name, record.RecordData, TimeSpan.FromSeconds(record.RecordTTL));
                else if (record.RecordType == DnsRecordType.CNAME)
                    ps.Add_DnsServerResourceRecordCName(zoneName, name, record.RecordData, TimeSpan.FromSeconds(record.RecordTTL));
                else if (record.RecordType == DnsRecordType.MX)
                    ps.Add_DnsServerResourceRecordMX(zoneName, name, record.RecordData, (ushort)record.MxPriority, TimeSpan.FromSeconds(record.RecordTTL));
                else if (record.RecordType == DnsRecordType.NS)
                    ps.Add_DnsServerResourceRecordNS(zoneName, name, record.RecordData, TimeSpan.FromSeconds(record.RecordTTL));
                else if (record.RecordType == DnsRecordType.TXT)
                    ps.Add_DnsServerResourceRecordTXT(zoneName, name, record.RecordData, TimeSpan.FromSeconds(record.RecordTTL));
                else if (record.RecordType == DnsRecordType.SRV)
                    ps.Add_DnsServerResourceRecordSRV(zoneName, name, record.RecordData, (ushort)record.SrvPort, (ushort)record.SrvPriority, (ushort)record.SrvWeight, TimeSpan.FromSeconds(record.RecordTTL));
                else if (record.RecordType == DnsRecordType.CAA)
                    ps.Add_DnsServerResourceRecordCAA(zoneName, name, record.RecordData, TimeSpan.FromSeconds(record.RecordTTL));
                else if (record.RecordType == DnsRecordType.PTR)
                    ps.Add_DnsServerResourceRecordPTR(zoneName, name, record.RecordData, TimeSpan.FromSeconds(record.RecordTTL));
                else
                    throw new Exception("Unknown record type");
            }
            catch (Exception ex)
            {
                // log exception
                Log.WriteError(ex);
            }
        }

        public override void AddZoneRecords(string zoneName, DnsRecord[] records)
        {
            bulkRecords = true;
            try
            {
                foreach (DnsRecord record in records)
                    AddZoneRecord(zoneName, record);
            }
            finally
            {
                bulkRecords = false;
            }

            UpdateSoaRecord(zoneName);
        }

        public override void DeleteZoneRecord(string zoneName, DnsRecord record)
        {
            try
            {
                string rrType;
                if (!RecordTypes.rrTypeFromRecord.TryGetValue(record.RecordType, out rrType))
                    throw new Exception("Unknown record type");
                ps.Remove_DnsServerResourceRecord(zoneName, record);
            }
            catch (Exception ex)
            {
                // log exception
                Log.WriteError(ex);
            }
        }

        public override void DeleteZoneRecords(string zoneName, DnsRecord[] records)
        {
            foreach (DnsRecord record in records)
                DeleteZoneRecord(zoneName, record);
        }
        #endregion


        #region SOA Record

        public override void UpdateSoaRecord(string zoneName, string host, string primaryNsServer, string primaryPerson)
        {
            try
            {
                ps.Update_DnsServerResourceRecordSOA(zoneName, ExpireLimit, MinimumTTL, primaryNsServer, RefreshInterval, primaryPerson, RetryDelay, null);
            }
            catch (Exception ex)
            {
                Log.WriteError(ex);
            }
        }

        private void UpdateSoaRecord(string zoneName)
        {
            if (bulkRecords)
                return;

            try
            {
                ps.Update_DnsServerResourceRecordSOA(zoneName, ExpireLimit, MinimumTTL, null, RefreshInterval, null, RetryDelay, null);
            }
            catch (Exception ex)
            {
                Log.WriteError(ex);
            }
        }
        #endregion

        public override bool IsInstalled()
        {
            if (!OSInfo.IsWindows) return false;

            return IsDNSInstalled() && WindowsVersion.WindowsServer2016 <= OSInfo.WindowsVersion;
        }
    }
}
