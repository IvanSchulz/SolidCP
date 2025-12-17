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
using System.Collections.Generic;
using System.Configuration;
using FuseCP.Providers;
using FuseCP.Providers.Common;
using FuseCP.Providers.DNS;
using FuseCP.ServiceProviders.DNS.Nettica;

namespace FuseCP.Providers.DNS
{
    public class Nettica : HostingServiceProviderBase, IDnsServer 
    {
        public const string NetticaWebServiceUrl = "NetticaWebServiceUrl";
        private const int Success = 200;
        private readonly NetticaProxy proxy; 
        
        public string Login
        {
            get { return ProviderSettings[Constants.UserName]; }
        }

        public string Password
        {
            get
            {
                byte[] data = System.Text.Encoding.UTF8.GetBytes(ProviderSettings[Constants.Password]);
                string binPassword = Convert.ToBase64String(data);
                return binPassword;
            }
        }

        public bool ApplyDefaultTemplate
        {
            get
            {
                bool res;
                bool.TryParse(ProviderSettings["ApplyDefaultTemplate"], out res);
                return res;
            }
        }
        
        public Nettica()
        {
            proxy = new NetticaProxy();
            string url = ConfigurationManager.AppSettings[NetticaWebServiceUrl];
            if (!string.IsNullOrEmpty(url))
                proxy.Url = url;
        }

        private static DomainRecord ConvertToDomainRecord(DnsRecord dnsRecord, string zoneName)
        {                        
            DomainRecord domainRecord = new DomainRecord();

            domainRecord.Data = dnsRecord.RecordData;
            domainRecord.DomainName = zoneName;            
            domainRecord.Priority = dnsRecord.MxPriority;
            domainRecord.RecordType = dnsRecord.RecordType.ToString();
            domainRecord.HostName = dnsRecord.RecordName;
                                    
            return domainRecord;
        }
       
        private static DnsRecord ConvertToDnsRecord(DomainRecord record)
        {                        
            DnsRecord dnsRecord = new DnsRecord();
            
            dnsRecord.RecordName = record.HostName;            
            dnsRecord.MxPriority = record.Priority;
            dnsRecord.RecordData = record.Data;
                                    
            switch(record.RecordType)
            {
                case "A":
                    dnsRecord.RecordType = DnsRecordType.A;
                    break;
				case "AAAA":
					dnsRecord.RecordType = DnsRecordType.AAAA;
					break;
                case "MX":
                    dnsRecord.RecordType = DnsRecordType.MX;
                    break;
                case "CNAME":
                    dnsRecord.RecordType = DnsRecordType.CNAME;
                    break;
                case "NS":
                    dnsRecord.RecordType = DnsRecordType.NS;
                    break;                   
                case "SOA":
                    dnsRecord.RecordType = DnsRecordType.SOA;
                    break;
                case "TXT":
                    dnsRecord.RecordType = DnsRecordType.TXT;
                    break;                                  
            }
             
            return dnsRecord;
        }

        #region IDnsServer Members

        public bool ZoneExists(string zoneName)
        {
            if (string.IsNullOrEmpty(zoneName))
                throw new ArgumentNullException("zoneName");

            DomainResult res = proxy.ListDomain(Login, Password, zoneName);
            return res.Result.Status == Success;
        }

        public string[] GetZones()
        {            
            ZoneResult res = proxy.ListZones(Login, Password);
            if (res.Result.Status != Success)
                throw new Exception(
                    string.Format("Could not get zones. Error code {0}. {1}", 
                    res.Result.Status, 
                    res.Result.Description));            
            
            return res.Zone;                        
        }

        public void AddPrimaryZone(string zoneName, string[] secondaryServers)
        {
            if (string.IsNullOrEmpty(zoneName))
                throw new ArgumentNullException("zoneName");

            DnsResult res = proxy.CreateZone(Login, Password, zoneName, string.Empty/*no longer used*/);                        
            
            if (res.Status != Success)
                throw new Exception(
                    string.Format("Could not add primary zone with name {0}. Error code {1}. {2}", 
                    zoneName, 
                    res.Status,
                    res.Description));
            
            if (ApplyDefaultTemplate)
            {
                int result = ApplydefaultTemplate(Login, Password, zoneName, String.Empty);
                if (result != Success)
                {
                    throw new Exception(
                    string.Format("Could not apply default domain template for primary zone with name {0}. Error code {1}. {2}",
                    zoneName,
                    res.Status,
                    res.Description));
                }
            }
            
        }

        
        private int ApplydefaultTemplate(string login, string password, string zonename, string group)
        {
            DnsResult res = proxy.ApplyTemplate(login, password, zonename, group);
            
            return res.Status; 
        }
        

        public void AddSecondaryZone(string zoneName, string[] masterServers)
        {
            if (string.IsNullOrEmpty(zoneName))
                throw new ArgumentNullException("zoneName");
                        if (masterServers == null)
                throw new ArgumentNullException("masterServers");
            
            
            foreach (string master in masterServers)
            {
                
                DnsResult res = proxy.CreateSecondaryZone(Login, Password, zoneName, master, string.Empty);    
                if (res.Status != Success)
                    throw new Exception(
                        string.Format("Could not add secondary zone with name {0}. Master zone {1}. Error code {2}. {3}", 
                        zoneName, master, res.Status, res.Description));
            }            
        }

        public void DeleteZone(string zoneName)
        {
            if (string.IsNullOrEmpty(zoneName))
                throw new ArgumentNullException("zoneName");
            
            DnsResult res = proxy.DeleteZone(Login, Password, zoneName);
            if (res.Status != Success)
                throw new Exception(
                    string.Format("Could not delete zone with name {0}. Error code {1}, {2}", zoneName, res.Status, res.Description));
        }

        
        public void UpdateSoaRecord(string zoneName, string host, string primaryNsServer, string primaryPerson)
        {
        //    throw new Exception("The method or operation is not implemented.");
        }

        
        public DnsRecord[] GetZoneRecords(string zoneName)
        {
            if (string.IsNullOrEmpty(zoneName))
                throw new ArgumentNullException("zoneName");

            DomainResult res = proxy.ListDomain(Login, Password, zoneName);
            
            if (res.Result.Status != Success)
                throw new Exception(string.Format("Could not get zone records. Error code {0}. {1}", res.Result.Status, res.Result.Description));
                      
            List <DnsRecord> retRecords = new List<DnsRecord>(res.Record.Length);            
            foreach (DomainRecord record in res.Record)
            {
                DnsRecord tempRecord = ConvertToDnsRecord(record);
                retRecords.Add(tempRecord);
            }
            return retRecords.ToArray();
        }

        public void AddZoneRecord(string zoneName, DnsRecord record)
        {
            if (string.IsNullOrEmpty(zoneName))
                throw new ArgumentNullException("zoneName");

            if (record == null)
                throw new ArgumentNullException("record");

            DomainRecord rec = ConvertToDomainRecord(record, zoneName);
            DnsResult res = proxy.AddRecord(Login, Password, rec);            
            
            if (res.Status != Success)
                throw new Exception(string.Format("Could not add zone record. Error code {0}. {1}", res.Status, res.Description));                        
        }

        public void AddZoneRecords(string zoneName, DnsRecord[] records)
        {
            if (string.IsNullOrEmpty(zoneName))
                throw new ArgumentNullException("zoneName");

            if (records == null)
                throw new ArgumentNullException("records");

            foreach (DnsRecord record in records)
            {
               AddZoneRecord(zoneName, record);
            }            
        }

        public void DeleteZoneRecord(string zoneName, DnsRecord record)
        {                        
            if (string.IsNullOrEmpty(zoneName))
                throw new ArgumentNullException("zoneName");

            if (record == null)
                throw new ArgumentNullException("record");

            DomainRecord domainRecord = ConvertToDomainRecord(record, zoneName);
                        
            DnsResult res = proxy.DeleteRecord(Login, Password, domainRecord);
            
            
            if (res.Status != Success)
                throw new Exception(string.Format("Could not delete zone record. Error code {0}. {1}", res.Status, res.Description));                        
        }

        public void DeleteZoneRecords(string zoneName, DnsRecord[] records)
        {
            if (string.IsNullOrEmpty(zoneName))
                throw new ArgumentNullException("zoneName");

            if (records == null)
                throw new ArgumentNullException("records");

            foreach (DnsRecord record in records)
            {
                DeleteZoneRecord(zoneName, record);
            }
        }
        
        #endregion

        public override void DeleteServiceItems(ServiceProviderItem[] items)
        {
            if (items == null)
                throw new ArgumentNullException("items");

            foreach (ServiceProviderItem item in items)
            {
                DeleteZone(item.Name);    
            }
            
            base.DeleteServiceItems(items);
        }

        public override bool IsInstalled()
        {
            return true;
        }

    }
}
