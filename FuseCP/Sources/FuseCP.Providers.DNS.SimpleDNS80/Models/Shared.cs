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

using System.Collections.Generic;
using System.Globalization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using FuseCP.Providers.DNS.SimpleDNS80.Models.Request;
using FuseCP.Providers.DNS.SimpleDNS80.Models.Response;

namespace FuseCP.Providers.DNS.SimpleDNS80.Models
{
    public static class Serialize
    {
        public static string ToJson(this StatisticsResponse self) =>
            JsonConvert.SerializeObject(self, Converter.Settings);

        public static string ToJson(this SecondaryZoneRequest self) =>
            JsonConvert.SerializeObject(self, Converter.Settings);

        public static string ToJson(this List<ZoneRecordsResponse> self) =>
            JsonConvert.SerializeObject(self, Converter.Settings);

        public static string ToJson(this ZoneRecordsResponse self) =>
            JsonConvert.SerializeObject(self, Converter.Settings);

        public static string ToJson(this ZoneRecordsDeleteRequest self) =>
            JsonConvert.SerializeObject(self, Converter.Settings);

        public static string ToJson(this List<ZoneRecordsDeleteRequest> self) =>
            JsonConvert.SerializeObject(self, Converter.Settings);
    }

    internal static class Converter
    {
        public static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
        {
            MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
            DateParseHandling = DateParseHandling.None,
            Converters =
            {
                new IsoDateTimeConverter { DateTimeStyles = DateTimeStyles.AssumeUniversal }
            },
        };
    }
}
