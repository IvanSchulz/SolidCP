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
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using FuseCP.Providers.Common;
using FuseCP.Providers.Virtualization;
using FuseCP.Server.Client;
// FuseCP.Providers.VirtualizationProxmox;

namespace FuseCP.EnterpriseServer.Code.VirtualizationProxmox
{
    public static class ReplicationHelper
    {

        public static void CleanUpReplicaServer(VirtualMachine originalVm)
        {
            throw new NotImplementedException();
        }

        public static ReplicationServerInfo GetReplicaInfoForService(int serviceId, ref ResultObject result)
        {
            throw new NotImplementedException();
        }

        public static VirtualizationServerProxmox GetReplicaForService(int serviceId, ref ResultObject result)
        {
            throw new NotImplementedException();
        }

        public static void CheckReplicationQuota(int packageId, ref ResultObject result)
        {
            throw new NotImplementedException();
        }
    }
}
