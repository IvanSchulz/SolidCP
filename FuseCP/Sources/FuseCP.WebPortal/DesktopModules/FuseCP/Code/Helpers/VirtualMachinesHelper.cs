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
using FuseCP.EnterpriseServer.Client;

namespace FuseCP.Portal
{
    // TODO: Move this extension to a separate file later.
    public static class VirtualMachinesExtensions
    {
        #region Privates with specific purposes (eq. caching, usability, performance and etc)
        /// <summary>
        /// This method supports the Portal internal infrastructure and is not intended to be used directly from your code. Gets a cached copy of virtual machine object of the specified type or retrieves it for the first time and then caches it.
        /// </summary>
        /// <typeparam name="T">Type of virtual machine to be retrieved (possible types are VirtualMachine|VMInfo)</typeparam>
        /// <param name="cacheIdentityKey">Virtual machine item id</param>
        /// <param name="getVmFunc">Function to retrieve the virtual machine data from Enterprise Server</param>
        /// <returns>An instance of the specified virtual machine</returns>
        internal static T GetCachedVirtualMachine<T>(object cacheIdentityKey, Func<T> getVmFunc)
        {
            // TODO: Make this method private when all dependents will be consolidated in the extension.
            string cacheKey = "CachedVirtualMachine_" + cacheIdentityKey;
            if (HttpContext.Current.Items[cacheKey] != null)
                return (T)HttpContext.Current.Items[cacheKey];

            // load virtual machine
            T virtualMachine = getVmFunc();

            // place to cache
            if (virtualMachine != null)
                HttpContext.Current.Items[cacheKey] = virtualMachine;

            return virtualMachine;
        }
        #endregion

        #region Extension methods
        /// <summary>
        /// Gets a cached copy of virtual machine object of the specified type or retrieves it for the first time and then caches it.
        /// </summary>
        /// <param name="client"></param>
        /// <param name="itemId">Virtual machine id</param>
        /// <returns>An instance of the virtual machine speficied</returns>
        public static VMInfo GetCachedVirtualMachine(this esVirtualizationServerForPrivateCloud client, int itemId)
        {
            return GetCachedVirtualMachine<VMInfo>(
                itemId, () => ES.Services.VPSPC.GetVirtualMachineItem(itemId));
        }
        #endregion
    }

    public class VirtualMachinesHelper
    {
        public static bool IsVirtualMachineManagementAllowed(int packageId)
        {
            bool manageAllowed = false;
            PackageContext cntx = PackagesHelper.GetCachedPackageContext(packageId);
            if (cntx.Quotas.ContainsKey(Quotas.VPS_MANAGING_ALLOWED))
                manageAllowed = !cntx.Quotas[Quotas.VPS_MANAGING_ALLOWED].QuotaExhausted;

            if (PanelSecurity.EffectiveUser.Role == UserRole.Administrator)
                manageAllowed = true;
            else if (PanelSecurity.EffectiveUser.Role == UserRole.Reseller)
            {
                // check if the reseller is allowed to manage on its parent level
                PackageInfo package = ES.Services.Packages.GetPackage(PanelSecurity.PackageId);
                if (package.UserId != PanelSecurity.EffectiveUserId)
                {
                    cntx = PackagesHelper.GetCachedPackageContext(package.ParentPackageId);
                    if (cntx != null && cntx.Quotas.ContainsKey(Quotas.VPS_MANAGING_ALLOWED))
                        manageAllowed = !cntx.Quotas[Quotas.VPS_MANAGING_ALLOWED].QuotaExhausted;
                }
            }
            return manageAllowed;
        }

        // TODO: Move this method to the corresponding extension later.
        public static VirtualMachine GetCachedVirtualMachine(int itemId)
        {
            return VirtualMachinesExtensions.GetCachedVirtualMachine<VirtualMachine>(
                itemId, () => ES.Services.VPS.GetVirtualMachineItem(itemId));
        }

        #region Virtual Machines
        VirtualMachineMetaItemsPaged vms;
        public VirtualMachineMetaItem[] GetVirtualMachines(int packageId, string filterColumn, string filterValue,
            string sortColumn, int maximumRows, int startRowIndex)
        {
            vms = ES.Services.VPS.GetVirtualMachines(packageId, filterColumn, filterValue,
                    sortColumn, startRowIndex, maximumRows, true);
            return vms.Items;
        }

        public int GetVirtualMachinesCount(int packageId, string filterColumn, string filterValue)
        {
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

        public PackageIPAddress[] GetPackageIPAddresses(int packageId, int orgId, IPAddressPool pool, string filterColumn, string filterValue,
            string sortColumn, int maximumRows, int startRowIndex)
        {
            packageAddresses = ES.Services.Servers.GetPackageIPAddresses(packageId, orgId, pool,
                filterColumn, filterValue, sortColumn, startRowIndex, maximumRows, true);
            return packageAddresses.Items;
        }

        public int GetPackageIPAddressesCount(int packageId, int orgId, IPAddressPool pool, string filterColumn, string filterValue)
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
    }
}
