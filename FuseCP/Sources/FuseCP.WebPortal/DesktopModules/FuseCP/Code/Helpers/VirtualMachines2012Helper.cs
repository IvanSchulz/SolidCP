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
﻿using System.Collections.Specialized;

namespace FuseCP.Portal
{
    public class VirtualMachines2012Helper
    {
        public static bool IsVirtualMachineManagementAllowed(int packageId)
        {
            bool manageAllowed = false;
            PackageContext cntx = PackagesHelper.GetCachedPackageContext(packageId);
            if (cntx.Quotas.ContainsKey(Quotas.VPS2012_MANAGING_ALLOWED))
                manageAllowed = !cntx.Quotas[Quotas.VPS2012_MANAGING_ALLOWED].QuotaExhausted;

            if (PanelSecurity.EffectiveUser.Role == UserRole.Administrator)
                manageAllowed = true;
            else if (PanelSecurity.EffectiveUser.Role == UserRole.Reseller)
            {
                // check if the reseller is allowed to manage on its parent level
                PackageInfo package = ES.Services.Packages.GetPackage(PanelSecurity.PackageId);
                if (package.UserId != PanelSecurity.EffectiveUserId)
                {
                    cntx = PackagesHelper.GetCachedPackageContext(package.ParentPackageId);
                    if (cntx != null && cntx.Quotas.ContainsKey(Quotas.VPS2012_MANAGING_ALLOWED))
                        manageAllowed = !cntx.Quotas[Quotas.VPS2012_MANAGING_ALLOWED].QuotaExhausted;
                }
            }
            return manageAllowed;
        }
        public static bool IsReinstallAllowed(int packageId)
        {
            bool reinstallAllowed = false;
            PackageContext cntx = PackagesHelper.GetCachedPackageContext(packageId);
            if (cntx.Quotas.ContainsKey(Quotas.VPS2012_REINSTALL_ALLOWED))
                reinstallAllowed = !cntx.Quotas[Quotas.VPS2012_REINSTALL_ALLOWED].QuotaExhausted;

            if (PanelSecurity.EffectiveUser.Role == UserRole.Administrator)
                reinstallAllowed = true;
            else if (PanelSecurity.EffectiveUser.Role == UserRole.Reseller)
            {
                // check if the reseller is allowed to manage on its parent level
                PackageInfo package = ES.Services.Packages.GetPackage(PanelSecurity.PackageId);
                if (package.UserId != PanelSecurity.EffectiveUserId)
                {
                    cntx = PackagesHelper.GetCachedPackageContext(package.ParentPackageId);
                    if (cntx != null && cntx.Quotas.ContainsKey(Quotas.VPS2012_REINSTALL_ALLOWED))
                        reinstallAllowed = !cntx.Quotas[Quotas.VPS2012_REINSTALL_ALLOWED].QuotaExhausted;
                }
            }
            return reinstallAllowed;
        }

        // TODO: Move this method to the corresponding extension later.
        public static VirtualMachine GetCachedVirtualMachine(int itemId)
        {
            return VirtualMachinesExtensions.GetCachedVirtualMachine<VirtualMachine>(
                itemId, () => ES.Services.VPS2012.GetVirtualMachineItem(itemId));
        }

        #region Virtual Machines
        VirtualMachineMetaItemsPaged vms;
        public VirtualMachineMetaItem[] GetVirtualMachines(int packageId, string filterColumn, string filterValue,
            string sortColumn, int maximumRows, int startRowIndex)
        {
            vms = ES.Services.VPS2012.GetVirtualMachines(packageId, filterColumn, filterValue,
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

        #region Package Private Network VLANs
        PackageVLANsPaged packageVLANs;
        public PackageVLAN[] GetPackageVLANs(int packageId, bool isDmz, string sortColumn, int maximumRows, int startRowIndex)
        {
            if (isDmz)
            {
                packageVLANs = ES.Services.Servers.GetPackageDmzNetworkVLANs(packageId, sortColumn, startRowIndex, maximumRows);
            }
            else
            {
                packageVLANs = ES.Services.Servers.GetPackagePrivateNetworkVLANs(packageId, sortColumn, startRowIndex, maximumRows);
            }
            return packageVLANs.Items;
        }

        public int GetPackageVLANsCount(int packageId, bool isDmz)
        {
            return packageVLANs.Count;
        }
        #endregion

        #region Package Private IP Addresses
        PrivateIPAddressesPaged privateAddresses;
        public PrivateIPAddress[] GetPackagePrivateIPAddresses(int packageId, string filterColumn, string filterValue,
            string sortColumn, int maximumRows, int startRowIndex)
        {
            privateAddresses = ES.Services.VPS2012.GetPackagePrivateIPAddressesPaged(packageId, filterColumn, filterValue,
                sortColumn, startRowIndex, maximumRows);
            return privateAddresses.Items;
        }

        public int GetPackagePrivateIPAddressesCount(int packageId, string filterColumn, string filterValue)
        {
            return privateAddresses.Count;
        }
        #endregion

        #region Package DMZ IP Addresses
        DmzIPAddressesPaged dmzAddresses;
        public DmzIPAddress[] GetPackageDmzIPAddresses(int packageId, string filterColumn, string filterValue,
            string sortColumn, int maximumRows, int startRowIndex)
        {
            dmzAddresses = ES.Services.VPS2012.GetPackageDmzIPAddressesPaged(packageId, filterColumn, filterValue,
                sortColumn, startRowIndex, maximumRows);
            return dmzAddresses.Items;
        }

        public int GetPackageDmzIPAddressesCount(int packageId, string filterColumn, string filterValue)
        {
            return dmzAddresses.Count;
        }
        #endregion

        public static StringDictionary ConvertArrayToDictionary(string[] settings)
        {
            StringDictionary r = new StringDictionary();
            foreach (string setting in settings)
            {
                int idx = setting.IndexOf('=');
                r.Add(setting.Substring(0, idx), setting.Substring(idx + 1));
            }
            return r;
        }

        //public static bool IsReplicationEnabled(int packageId)
        //{
        //    var vmsMeta = (new VirtualMachines2012Helper()).GetVirtualMachines(packageId, null, null, null, 1, 0);
        //    if (vmsMeta.Length == 0) return false;

        //    var packageVm = ES.Services.VPS2012.GetVirtualMachineItem(vmsMeta[0].ItemID);
        //    if (packageVm == null) return false;

        //    var serviceSettings = ConvertArrayToDictionary(ES.Services.Servers.GetServiceSettings(packageVm.ServiceId));
        //    if (serviceSettings == null) return false;

        //    return serviceSettings["ReplicaMode"] == ReplicaMode.ReplicationEnabled.ToString();
        //}
    }
}
