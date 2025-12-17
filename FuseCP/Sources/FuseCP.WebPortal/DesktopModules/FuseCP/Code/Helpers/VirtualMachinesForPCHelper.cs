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
using FuseCP.Providers.Virtualization;
using System.Web;
using System;

namespace FuseCP.Portal
{
    public class VirtualMachinesForPCHelper
    {
        public static bool IsVirtualMachineManagementAllowed(int packageId)
        {
            bool manageAllowed = false;
            PackageContext cntx = PackagesHelper.GetCachedPackageContext(packageId);
            if (cntx.Quotas.ContainsKey(Quotas.VPSForPC_MANAGING_ALLOWED))
                manageAllowed = !cntx.Quotas[Quotas.VPSForPC_MANAGING_ALLOWED].QuotaExhausted;

            if (PanelSecurity.EffectiveUser.Role == UserRole.Administrator)
                manageAllowed = true;
            else if (PanelSecurity.EffectiveUser.Role == UserRole.Reseller)
            {
                // check if the reseller is allowed to manage on its parent level
                PackageInfo package = ES.Services.Packages.GetPackage(PanelSecurity.PackageId);
                if (package.UserId != PanelSecurity.EffectiveUserId)
                {
                    cntx = PackagesHelper.GetCachedPackageContext(package.ParentPackageId);
                    if (cntx != null && cntx.Quotas.ContainsKey(Quotas.VPSForPC_MANAGING_ALLOWED))
                        manageAllowed = !cntx.Quotas[Quotas.VPSForPC_MANAGING_ALLOWED].QuotaExhausted;
                }
            }
            return manageAllowed;
        }

        public static VMInfo GetCachedVirtualMachineForPC(int itemId)
        {
            if (itemId == 0)
            {
                return new VMInfo();
            }

            string key = "CachedVirtualMachine" + itemId;
            if (HttpContext.Current.Items[key] != null)
                return (VMInfo)HttpContext.Current.Items[key];

            // load virtual machine
            VMInfo vm = ES.Services.VPSPC.GetVirtualMachineItem(itemId);

            // place to cache
            if (vm != null)
                HttpContext.Current.Items[key] = vm;

            vm.HostName = vm.HostName ?? String.Empty;

            return vm;
        }

        #region Virtual Machines
        VirtualMachineMetaItemsPaged vms;
        public VirtualMachineMetaItem[] GetVirtualMachines(int packageId, string filterColumn, string filterValue,
            string sortColumn, int maximumRows, int startRowIndex)
        {
            vms = ES.Services.VPSPC.GetVirtualMachines(packageId, filterColumn, filterValue,
                    sortColumn, startRowIndex, maximumRows, true);
            return vms.Items;
        }

        public int GetVirtualMachinesCount(int packageId, string filterColumn, string filterValue)
        {
            if (vms == null)
            {
                vms = ES.Services.VPSPC.GetVirtualMachines(packageId, filterColumn, filterValue,
                        String.Empty, 0, 10, true);
            }

            return vms.Count;
        }
        #endregion

        #region Package IP Addresses
        PackageIPAddressesPaged packageAddresses;
        public PackageIPAddress[] GetPackageIPAddresses(int packageId, IPAddressPool pool, string filterColumn, string filterValue,
            string sortColumn, int maximumRows, int startRowIndex)
        {
            packageAddresses = ES.Services.Servers.GetPackageIPAddresses(packageId, 0, pool,
                filterColumn, filterValue, sortColumn, startRowIndex, maximumRows, true);
            return packageAddresses.Items;
        }

        public int GetPackageIPAddressesCount(int packageId, IPAddressPool pool, string filterColumn, string filterValue)
        {
            return packageAddresses.Count;
        }
        #endregion

        #region Package Private IP Addresses
        PrivateIPAddressesPaged privateAddresses;
        public PrivateIPAddress[] GetPackagePrivateIPAddresses(int packageId, string filterColumn, string filterValue,
            string sortColumn, int maximumRows, int startRowIndex)
        {
            privateAddresses = ES.Services.VPS.GetPackagePrivateIPAddressesPaged(packageId, filterColumn, filterValue,
                sortColumn, startRowIndex, maximumRows);
            return privateAddresses.Items;
        }

        public int GetPackagePrivateIPAddressesCount(int packageId, string filterColumn, string filterValue)
        {
            return privateAddresses.Count;
        }
        #endregion

        #region Monitoring
        /// <summary>
        /// Get collection of MonitoredObjectEvent to selected VM
        /// </summary>
        /// <returns></returns>
        public MonitoredObjectEvent[] GetMonitoredObjectEvents()
        {
            return ES.Services.VPSPC.GetDeviceEvents(PanelRequest.ItemID);
        }

        /// <summary>
        /// Get collection of MonitoredObjectAlert to selected VM
        /// </summary>
        /// <returns></returns
        public MonitoredObjectAlert[] GetMonitoringAlerts()
        {
            return ES.Services.VPSPC.GetMonitoringAlerts(PanelRequest.ItemID);
        }                                         
        #endregion

    }
}
