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

using FuseCP.EnterpriseServer;

namespace FuseCP.Portal
{
    public class VLANsHelper
    {
        VLANsPaged vlans;
        public VLANInfo[] GetVLANsPaged(int serverId, string filterColumn, string filterValue,
            string sortColumn, int maximumRows, int startRowIndex)
        {
            vlans = ES.Services.Servers.GetPrivateNetworkVLANsPaged(serverId, filterColumn, filterValue, sortColumn, startRowIndex, maximumRows);
            return vlans.Items;
        }

        public int GetVLANsPagedCount(int serverId, string filterColumn, string filterValue)
        {
            return vlans.Count;
        }
    }
}
