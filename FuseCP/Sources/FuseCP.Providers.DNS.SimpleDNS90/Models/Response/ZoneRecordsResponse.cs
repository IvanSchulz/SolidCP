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
using System.Xml.Linq;
using Newtonsoft.Json;

namespace FuseCP.Providers.DNS.SimpleDNS90.Models.Response
{
    public partial class ZoneRecordsResponse
    {
        [JsonProperty("Name")]
        public string Name { get; set; }

        [JsonProperty("Type")]
        public string Type { get; set; }

        [JsonProperty("TTL")]
        public long? TTL { get; set; }

        [JsonProperty("Data")]
        public string Data { get; set; }

        [JsonProperty("Comment", NullValueHandling = NullValueHandling.Ignore)]
        public string Comment { get; set; }
    }

    public partial class ZoneRecordsResponse
    {
        public static List<ZoneRecordsResponse> FromJson(string json) => JsonConvert.DeserializeObject<List<ZoneRecordsResponse>>(json, Converter.Settings);
    }

    public static class ZoneRecordsResponseExtensions
    {
        public static DnsRecord[] ToDnsRecordArray(this List<ZoneRecordsResponse> records, string zoneName)
        {
            //Declare the result
            var dnsRecords = new List<DnsRecord>();

            //Loop through each record in the list
            foreach (var record in records)
            {
                //Convert the ZoneRecordsResponse to DnsRecord
                dnsRecords.Add(ZoneRecordResponseToDnsRecord(record, zoneName));
            }

            //Return the array of DnsRecords
            return dnsRecords.ToArray();
        }

        public static ZoneRecordsResponse ToZoneRecordsResponse(this DnsRecord record, string zoneName)
        {
            //Declare the result
            var response = new ZoneRecordsResponse();

            //Check that the record is in SDNS format
            if (!record.RecordName.Contains(zoneName) && record.RecordName.Length > 0)
                record.RecordName = $"{record.RecordName}.{zoneName}";

            if (record.RecordName == "")
                record.RecordName = $"{zoneName}";

            //Build up the response
            response.Name = record.RecordName;
            response.Type = record.RecordType.ToString();
            response.TTL = record.RecordTTL;
            // ReSharper disable once SwitchStatementMissingSomeCases
            switch (record.RecordType)
            {
                case DnsRecordType.MX:
                    if (!record.RecordData.EndsWith(".") && record.RecordData.Length > 0)
                    {
                        response.Data = $"{record.MxPriority} {record.RecordData}.";
                        break;
                    }
                    response.Data = $"{record.MxPriority} {record.RecordData}";
                    break;
                case DnsRecordType.SRV:
                    if (!record.RecordData.EndsWith(".") && record.RecordData.Length > 0)
                    {
                        response.Data = $"{record.SrvPriority} {record.SrvWeight} {record.SrvPort} {record.RecordData}.";
                        break;
                    }
                    response.Data = $"{record.SrvPriority} {record.SrvWeight} {record.SrvPort} {record.RecordData}";
                    break;
                case DnsRecordType.CNAME:
                case DnsRecordType.NS:
                case DnsRecordType.PTR:
                    if (!record.RecordData.EndsWith(".") && record.RecordData.Length > 0) {
                        response.Data = $"{record.RecordData}.";
                        break;
                    }
                    response.Data = $"{record.RecordData}";
                    break;
                case DnsRecordType.TXT:
                case DnsRecordType.CAA:
                    if (!record.RecordData.StartsWith("\"") && record.RecordData.Length > 0)
                    {
                        response.Data = $"\"{record.RecordData}\"";
                        break;
                    }
                    response.Data = record.RecordData ?? "";
                    break;
                default:
                    response.Data = record.RecordData ?? "";
                    break;
            }
            //Return the response
            return response;
        }

        /// <summary>
        /// Method to convert <see cref="ZoneRecordsResponse"/> to <see cref="DnsRecord"/>
        /// </summary>
        /// <param name="record">DNS Record in <see cref="ZoneRecordsResponse"/> format</param>
        private static DnsRecord ZoneRecordResponseToDnsRecord(ZoneRecordsResponse record, string zoneName)
        {
            //Null checking
            if (record == null)
                throw new ArgumentNullException(nameof(record));

            //Declare result variable
            var resultRecord = new DnsRecord();

            //Switch on the type
            switch (record.Type)
            {
                case "A":
                    resultRecord.RecordType = DnsRecordType.A;
                    break;
                case "AAAA":
                    resultRecord.RecordType = DnsRecordType.AAAA;
                    break;
                case "CAA":
                    resultRecord.RecordType = DnsRecordType.CAA;
                    break;
                case "CNAME":
                    resultRecord.RecordType = DnsRecordType.CNAME;
                    resultRecord.RecordData = record.Data.TrimEnd('.');
                    break;
                case "MX":
                    resultRecord.RecordType = DnsRecordType.MX;
                    resultRecord.MxPriority = Convert.ToInt32(record.Data.Split(' ')[0]);
                    resultRecord.RecordData = record.Data.Split(' ')[1].TrimEnd('.');
                    break;
                case "NS":
                    resultRecord.RecordType = DnsRecordType.NS;
                    resultRecord.RecordData = record.Data.TrimEnd('.');
                    break;
                case "SOA":
                    resultRecord.RecordType = DnsRecordType.SOA;
                    break;
                case "SRV":
                    resultRecord.RecordType = DnsRecordType.SRV;
                    resultRecord.SrvPriority = Convert.ToInt32(record.Data.Split(' ')[0]);
                    resultRecord.SrvWeight = Convert.ToInt32(record.Data.Split(' ')[1]);
                    resultRecord.SrvPort = Convert.ToInt32(record.Data.Split(' ')[2]);
                    resultRecord.RecordData = record.Data.Split(' ')[3].TrimEnd('.');
                    break;
                case "TXT":
                    resultRecord.RecordType = DnsRecordType.TXT;
                    break;
                case "PTR":
                    resultRecord.RecordType = DnsRecordType.PTR;
                    break;
                default:
                    resultRecord.RecordType = DnsRecordType.Other;
                    break;
            }

            //Build up the rest of the record
            //If data is already set, don't change it
            if (string.IsNullOrWhiteSpace(resultRecord.RecordData))
                resultRecord.RecordData = record.Data;
            //Build the remaining fields of the record
            resultRecord.RecordName = record.Name;
            resultRecord.RecordText = $"{record.Name}\t{record.TTL}\t{record.Type}\t{record.Data}";
            resultRecord.RecordTTL = (int)record.TTL;

            //Return the result
            return resultRecord;
        }
    }
}
