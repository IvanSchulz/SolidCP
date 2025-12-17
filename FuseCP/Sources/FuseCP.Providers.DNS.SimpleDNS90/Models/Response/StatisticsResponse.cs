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
using Newtonsoft.Json;

namespace FuseCP.Providers.DNS.SimpleDNS90.Models.Response
{
    public partial class StatisticsResponse
    {
        [JsonProperty("Sections")]
        public List<Section> Sections { get; set; }

        [JsonProperty("ReqPerSec")]
        public List<long> ReqPerSec { get; set; }
    }

    public partial class Section
    {
        [JsonProperty("ID")]
        public string Id { get; set; }

        [JsonProperty("Text")]
        public string Text { get; set; }

        [JsonProperty("Items")]
        public List<Item> Items { get; set; }
    }

    public partial class Item
    {
        [JsonProperty("ID")]
        public string Id { get; set; }

        [JsonProperty("Text")]
        public string Text { get; set; }

        [JsonProperty("Value")]
        public long Value { get; set; }

        [JsonProperty("ValueText", NullValueHandling = NullValueHandling.Ignore)]
        public string ValueText { get; set; }
    }

    public partial class StatisticsResponse
    {
        public static StatisticsResponse FromJson(string json) => JsonConvert.DeserializeObject<StatisticsResponse>(json, Converter.Settings);
    }
}
