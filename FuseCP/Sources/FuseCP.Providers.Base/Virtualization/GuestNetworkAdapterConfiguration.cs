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
using System.Linq;
using System.Text;

namespace FuseCP.Providers.Virtualization
{
    //https://docs.microsoft.com/en-us/windows/desktop/hyperv_v2/msvm-guestnetworkadapterconfiguration
    public class GuestNetworkAdapterConfiguration
    {
        public string InstanceID { get; set; }
        public string MAC { get; set; } //need only if we want to find a specific adapter.
        public UInt16 ProtocolIFType { get; set; } //ProtocolIFType -> Unknown(0) Other(1) IPv4(4096) IPv6(4097) IPv4/v6(4098)
        public bool DHCPEnabled { get; set; }
        public string[] IPAddresses { get; set; }
        public string[] Subnets { get; set; }
        public string[] DefaultGateways { get; set; }
        public string[] DNSServers { get; set; }
        public UInt16[] IPAddressOrigins { get; set; }
    }
}
