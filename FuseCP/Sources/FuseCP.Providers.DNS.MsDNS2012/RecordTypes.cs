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
using System.Globalization;
using System.Linq;

namespace FuseCP.Providers.DNS
{
	/// <summary>This static class holds 2 lookup tables, from/to DnsRecordType enum</summary>
	internal static class RecordTypes
	{
		static readonly Dictionary<string, DnsRecordType> s_lookup;
		static readonly Dictionary<DnsRecordType, string> s_lookupInv;

		static RecordTypes()
		{
			s_lookup = new Dictionary<string, DnsRecordType>()
			{
				{ "A",     DnsRecordType.A     },
				{ "AAAA",  DnsRecordType.AAAA  },
				{ "NS",    DnsRecordType.NS    },
				{ "MX",    DnsRecordType.MX    },
				{ "CNAME", DnsRecordType.CNAME },
				{ "SOA",   DnsRecordType.SOA   },
				{ "TXT",   DnsRecordType.TXT   },
				{ "SRV",   DnsRecordType.SRV   },
                { "PTR",   DnsRecordType.PTR   },
            };

			TextInfo ti = new CultureInfo( "en-US", false ).TextInfo;

			s_lookupInv = s_lookup
				.ToDictionary( kvp => kvp.Value, kvp => ti.ToTitleCase( kvp.Key ) );
		}

		/// <summary>The dictionary that maps string record types to DnsRecordType enum</summary>
		public static Dictionary<string, DnsRecordType> recordFromString { get { return s_lookup; } }

		/// <summary>the dictionary that maps DnsRecordType enum to strings, suitable for PowerShell </summary>
		public static Dictionary<DnsRecordType, string> rrTypeFromRecord { get { return s_lookupInv; } }
	}
}
