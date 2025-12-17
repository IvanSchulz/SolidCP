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

namespace FuseCP.Providers.DNS
{
	/// <summary>
	/// Summary description for IDnsServer.
	/// </summary>
	public interface IDnsServer
	{
        // Zones
		bool ZoneExists(string zoneName);
		string[] GetZones();
		void AddPrimaryZone(string zoneName, string[] secondaryServers);
		void AddSecondaryZone(string zoneName, string[] masterServers);
		void DeleteZone(string zoneName);
        void UpdateSoaRecord(string zoneName, string host, string primaryNsServer,
            string primaryPerson);

        // Zone Records
        DnsRecord[] GetZoneRecords(string zoneName);
        
        void AddZoneRecord(string zoneName, DnsRecord record);
        void AddZoneRecords(string zoneName, DnsRecord[] records);

        void DeleteZoneRecord(string zoneName, DnsRecord record);
        void DeleteZoneRecords(string zoneName, DnsRecord[] records);
	}
}
