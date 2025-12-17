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

using FuseCP.Providers.Virtualization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FuseCP.EnterpriseServer.Code.Virtualization2012.Helpers.VM
{
    public class NetworkVLANHelper: ControllerBase
    {
        private const int DEFAULT_VLAN = 0;

        public NetworkVLANHelper(ControllerBase provider) : base(provider) { }

        public int GetExternalNetworkVLAN(int itemId)
        {
            int adaptervlan = DEFAULT_VLAN;
            VirtualMachine vm = null;
            try
            {
                VirtualMachine vmgeneral = VirtualMachineHelper.GetVirtualMachineGeneralDetails(itemId);
                vm = VirtualMachineHelper.GetVirtualMachineExtendedInfo(vmgeneral.ServiceId, vmgeneral.VirtualMachineId);
                vm.ExternalNicMacAddress = vmgeneral.ExternalNicMacAddress;
            }
            catch (Exception ex)
            {
                TaskManager.WriteError(ex, "VPS_GET_VM_DETAILS");
            }
            if (vm != null)
            {
                bool firstadapter = true;
                foreach (VirtualMachineNetworkAdapter adapter in vm.Adapters)
                {
                    if (firstadapter)
                    {
                        firstadapter = false;
                        adaptervlan = adapter.vlan;
                    }
                    // Overwrite First Adapter by Mac Match
                    if (adapter.MacAddress == vm.ExternalNicMacAddress)
                    {
                        adaptervlan = adapter.vlan;
                    }
                }
            }
            return adaptervlan;
        }
    }
}
