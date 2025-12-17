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
using System.Net;

namespace FuseCP.EnterpriseServer
{
	/// <summary>
	/// Summary description for IpAddressInfo.
	/// </summary>
	[Serializable]
	public class IPAddressInfo
	{
        public int AddressId { get; set; }
        public string ExternalIP { get; set; }
        public string InternalIP { get; set; }
        public int PoolId { get; set; }
        
        public int ServerId { get; set; }
        public string ServerName { get; set; }

        public int ItemId { get; set; }
        public string ItemName { get; set; }

        public int UserId { get; set; }
        public string Username { get; set; }

        public int PackageId { get; set; }
        public string PackageName { get; set; }

        public string SubnetMask { get; set; }
        public string DefaultGateway { get; set; }
        public string Comments { get; set; }

        public int VLAN { get; set; }

        public IPAddressPool Pool
        {
            get { return (IPAddressPool)PoolId; }
            set { PoolId = (int)value; }
        }
	}
}
