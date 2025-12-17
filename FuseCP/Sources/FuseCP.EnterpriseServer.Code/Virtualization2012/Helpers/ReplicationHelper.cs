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
//using FuseCP.Providers.Virtualization2012;

namespace FuseCP.EnterpriseServer.Code.Virtualization2012
{
    public class ReplicationHelper: ControllerBase
    {
        public ReplicationHelper(ControllerBase provider): base(provider) { }

        public void CleanUpReplicaServer(VirtualMachine originalVm)
        {
            try
            {
                ResultObject result = new ResultObject();

                // Get replica server
                var replicaServer = GetReplicaForService(originalVm.ServiceId, ref result);

                // Clean up replica server
                var replicaVm = replicaServer.GetVirtualMachines().FirstOrDefault(m => m.Name == originalVm.Name);
                if (replicaVm != null)
                {
                    replicaServer.DisableVmReplication(replicaVm.VirtualMachineId);
                    replicaServer.ShutDownVirtualMachine(replicaVm.VirtualMachineId, true, "ReplicaDelete");
                    replicaServer.DeleteVirtualMachine(replicaVm.VirtualMachineId, null);
                }
            }
            catch { /* skip */ }
        }

        public ReplicationServerInfo GetReplicaInfoForService(int serviceId, ref ResultObject result)
        {
            // Get service id of replica server
            StringDictionary vsSesstings = ServerController.GetServiceSettings(serviceId);
            string replicaServiceId = vsSesstings["ReplicaServerId"];

            if (string.IsNullOrEmpty(replicaServiceId))
            {
                result.ErrorCodes.Add(VirtualizationErrorCodes.NO_REPLICA_SERVER_ERROR);
                return null;
            }

            // get replica server info for replica service id
            VirtualizationServer2012 vsReplica = VirtualizationHelper.GetVirtualizationProxy(Convert.ToInt32(replicaServiceId));
            StringDictionary vsReplicaSesstings = ServerController.GetServiceSettings(Convert.ToInt32(replicaServiceId));
            string computerName = vsReplicaSesstings["ServerName"];
            var replicaServerInfo = vsReplica.GetReplicaServer(computerName);

            if (!replicaServerInfo.Enabled)
            {
                result.ErrorCodes.Add(VirtualizationErrorCodes.NO_REPLICA_SERVER_ERROR);
                return null;
            }

            return replicaServerInfo;
        }

        public VirtualizationServer2012 GetReplicaForService(int serviceId, ref ResultObject result)
        {
            // Get service id of replica server
            StringDictionary vsSesstings = ServerController.GetServiceSettings(serviceId);
            string replicaServiceId = vsSesstings["ReplicaServerId"];

            if (string.IsNullOrEmpty(replicaServiceId))
            {
                result.ErrorCodes.Add(VirtualizationErrorCodes.NO_REPLICA_SERVER_ERROR);
                return null;
            }

            // get replica server for replica service id
            return VirtualizationHelper.GetVirtualizationProxy(Convert.ToInt32(replicaServiceId));
        }

        public void CheckReplicationQuota(int packageId, ref ResultObject result)
        {
            List<string> quotaResults = new List<string>();
            PackageContext cntx = PackageController.GetPackageContext(packageId);

            QuotaHelper.CheckBooleanQuota(cntx, quotaResults, Quotas.VPS2012_REPLICATION_ENABLED, true, VirtualizationErrorCodes.QUOTA_REPLICATION_ENABLED);

            if (quotaResults.Count > 0)
                result.ErrorCodes.AddRange(quotaResults);
        }
    }
}
