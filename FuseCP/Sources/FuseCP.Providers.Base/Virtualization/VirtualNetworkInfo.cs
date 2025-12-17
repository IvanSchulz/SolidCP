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
    public partial class VirtualNetworkInfo
    {        
        public bool BoundToVMHost {get;set;}

        public string DNSServers { get; set; }

        public string DefaultGatewayAddress { get; set; }

        public string Description { get; set; }

        public string EnablingIPAddress { get; set; }

        public bool HighlyAvailable { get; set; }

        public ushort HostBoundVlanId { get; set; }

        public System.Guid Id { get; set; }

        public string Name { get; set; }

        public string NetworkAddress { get; set; }

        public string NetworkMask { get; set; }

        public string Tag { get; set; }

        public string VMHost { get; set; }

        public System.Guid VMHostId { get; set; }

        public string WINServers { get; set; }        
    }

}
