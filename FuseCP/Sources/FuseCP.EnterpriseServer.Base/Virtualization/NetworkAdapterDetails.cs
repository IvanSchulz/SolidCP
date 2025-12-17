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

﻿using System;
using System.Collections.Generic;
using System.Text;

namespace FuseCP.EnterpriseServer
{
    public class NetworkAdapterDetails
    {
        public string NetworkFormat { get; set; }
        public bool IsDHCP { get; set; }
        public string NetworkId { get; set; }
        public string MacAddress { get; set; }
        public int VLAN { get; set; }
        public string SubnetMask { get; set; }
        public string SubnetMaskCidr { get; set; }
        public string DefaultGateway { get; set; }
        public string PreferredNameServer { get; set; }
        public string AlternateNameServer { get; set; }
        public NetworkAdapterIPAddress[] IPAddresses { get; set; }
    }
}
