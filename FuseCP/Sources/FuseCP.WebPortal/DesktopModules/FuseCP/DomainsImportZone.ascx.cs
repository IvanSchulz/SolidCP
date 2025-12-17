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
using System.IO;
using System.Linq;
using System.Web;
using Newtonsoft.Json;
using FuseCP.Providers.DNS;

namespace FuseCP.Portal
{
    public partial class DomainsImportZone : FuseCPModuleBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //Get the domain information
            var domain = ES.Services.Servers.GetDomain(PanelRequest.DomainID);
            //Set the text of the literal to the domain name
            litDomainName.Text = domain.DomainName;
        }
    
        protected void UploadZoneFile_OnClick(object sender, EventArgs e)
        {
            //Get the uploaded zone file
            var zoneFile = file.PostedFile;
            //First check that there was actually a file uploaded
            if (zoneFile != null && zoneFile.ContentLength > 0)
            {
                //Get the contents from the file
                var contents = new StreamReader(zoneFile.InputStream).ReadToEnd();
                try
                {
                    //Get the domain id that gets used throughout the method
                    var domainId = PanelRequest.DomainID;
                    //Try and parse the JSON to an array of DNSRecords
                    var importRecords = JsonConvert.DeserializeObject<DnsRecord[]>(contents);
                    //Get the existing records on the DNS server
                    var existingRecords = ES.Services.Servers.GetDnsZoneRecords(domainId);
                    //Get the records that are new to the zone
                    var newRecords = importRecords.Except(existingRecords);
                    //Loop through the new records
                    foreach (var record in newRecords)
                    {
                        //Add each record
                        var result = ES.Services.Servers.AddDnsZoneRecord(
                            domainId,
                            record.RecordName,
                            record.RecordType,
                            record.RecordData,
                            record.MxPriority,
                            record.SrvPriority,
                            record.SrvWeight,
                            record.SrvPort,
                            record.RecordTTL);
                        //Check if the record couldn't be added for some reason
                        if (result < 0)
                            ShowResultMessage(result);
                    }
                    //Show success message
                    ShowSuccessMessage("DOMAIN_IMPORT");
                }
                catch
                {
                    //Show error message
                    ShowErrorMessage("DOMAIN_IMPORT");
                }
            }
            else
            {
                //Show error message
                ShowErrorMessage("DOMAIN_IMPORT_NO_FILE");
            }
        }
    }
}
