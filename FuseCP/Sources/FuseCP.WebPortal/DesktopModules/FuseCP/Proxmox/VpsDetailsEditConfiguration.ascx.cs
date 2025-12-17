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
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FuseCP.Providers.Virtualization;
using FuseCP.Providers.Common;
using FuseCP.EnterpriseServer;
﻿using FuseCP.Portal.Code.Helpers;

namespace FuseCP.Portal.Proxmox
{
    public partial class VpsDetailsEditConfiguration : FuseCPModuleBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindConfiguration();
            }
        }

        private void BindConfiguration()
        {
            VirtualMachine vm = null;

            try
            {
                // load machine
                vm = ES.Services.Proxmox.GetVirtualMachineItem(PanelRequest.ItemID);

                if (vm == null)
                {
                    messageBox.ShowErrorMessage("VPS_LOAD_VM_META_ITEM");
                    return;
                }




                // bind CPU cores
                int maxCores = ES.Services.Proxmox.GetMaximumCpuCoresNumber(vm.PackageId);
                PackageContext cntx = PackagesHelper.GetCachedPackageContext(PanelSecurity.PackageId);
                if (cntx.Quotas.ContainsKey(Quotas.PROXMOX_CPU_NUMBER))
                {
                    QuotaValueInfo cpuQuota = cntx.Quotas[Quotas.PROXMOX_CPU_NUMBER];

                    if (cpuQuota.QuotaAllocatedValue != -1
                        && maxCores > cpuQuota.QuotaAllocatedValue)
                        maxCores = cpuQuota.QuotaAllocatedValue;
                }

                for (int i = 1; i < maxCores + 1; i++)
                    ddlCpu.Items.Add(i.ToString());

                // bind item
                ddlCpu.SelectedValue = vm.CpuCores.ToString();
                txtRam.Text = vm.RamSize.ToString();
                txtHdd.Text = vm.HddSize[0].ToString();
                txtSnapshots.Text = vm.SnapshotsNumber.ToString();

                chkDvdInstalled.Checked = vm.DvdDriveInstalled;
                chkBootFromCd.Checked = vm.BootFromCD;
                //chkNumLock.Checked = vm.NumLockEnabled;

                chkStartShutdown.Checked = vm.StartTurnOffAllowed;
                chkPauseResume.Checked = vm.PauseResumeAllowed;
                chkReset.Checked = vm.ResetAllowed;
                chkReboot.Checked = vm.RebootAllowed;
                chkReinstall.Checked = vm.ReinstallAllowed;

                chkExternalNetworkEnabled.Checked = vm.ExternalNetworkEnabled;
                chkPrivateNetworkEnabled.Checked = vm.PrivateNetworkEnabled;

                // other quotas
                BindCheckboxOption(chkDvdInstalled, Quotas.PROXMOX_DVD_ENABLED);
                chkBootFromCd.Enabled = PackagesHelper.IsQuotaEnabled(PanelSecurity.PackageId, Quotas.PROXMOX_BOOT_CD_ALLOWED);

                BindCheckboxOption(chkStartShutdown, Quotas.PROXMOX_START_SHUTDOWN_ALLOWED);
                BindCheckboxOption(chkPauseResume, Quotas.PROXMOX_PAUSE_RESUME_ALLOWED);
                BindCheckboxOption(chkReset, Quotas.PROXMOX_RESET_ALOWED);
                BindCheckboxOption(chkReboot, Quotas.PROXMOX_REBOOT_ALLOWED);
                BindCheckboxOption(chkReinstall, Quotas.PROXMOX_REINSTALL_ALLOWED);

                BindCheckboxOption(chkExternalNetworkEnabled, Quotas.PROXMOX_EXTERNAL_NETWORK_ENABLED);
                BindCheckboxOption(chkPrivateNetworkEnabled, Quotas.PROXMOX_PRIVATE_NETWORK_ENABLED);

                // Diksk Resize funktioniert nicht wenn snapshots existieren
                VirtualMachineSnapshot[] snapshots = ES.Services.Proxmox.GetVirtualMachineSnapshots(PanelRequest.ItemID);
                txtHdd.Enabled = (snapshots.Length <= 0);

                this.BindSettingsControls(vm);
            }
            catch (Exception ex)
            {
                messageBox.ShowErrorMessage("VPS_LOAD_VM_META_ITEM", ex);
            }
        }

        private void BindCheckboxOption(CheckBox chk, string quotaName)
        {
            chk.Enabled = PackagesHelper.IsQuotaEnabled(PanelSecurity.PackageId, quotaName);
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            RedirectBack("cancel");
        }

        private void RedirectBack(string action)
        {
            Response.Redirect(EditUrl("ItemID", PanelRequest.ItemID.ToString(), "vps_config",
                "SpaceID=" + PanelSecurity.PackageId.ToString(),
                "action=" + action));
        }

        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid)
                return;
            
            try
            {
                // check rights
                bool manageAllowed = VirtualMachinesProxmoxHelper.IsVirtualMachineManagementAllowed(PanelSecurity.PackageId);
                if (!manageAllowed)
                {
                    return;
                }

                VirtualMachine virtualMachine = new VirtualMachine();

                // the custom provider control
                this.SaveSettingsControls(ref virtualMachine);

                ResultObject res = ES.Services.Proxmox.UpdateVirtualMachineConfiguration(PanelRequest.ItemID,
                    Utils.ParseInt(ddlCpu.SelectedValue),
                    Utils.ParseInt(txtRam.Text.Trim()),
                    Utils.ParseInt(txtHdd.Text.Trim()),
                    Utils.ParseInt(txtSnapshots.Text.Trim()),
                    chkDvdInstalled.Checked,
                    chkBootFromCd.Checked,
                    false,
                    chkStartShutdown.Checked,
                    chkPauseResume.Checked,
                    chkReboot.Checked,
                    chkReset.Checked,
                    chkReinstall.Checked,
                    chkExternalNetworkEnabled.Checked,
                    chkPrivateNetworkEnabled.Checked,
                    virtualMachine);

                if (res.IsSuccess)
                {
                    // redirect back
                    RedirectBack("changed");
                }
                else
                {
                    // show error
                    messageBox.ShowMessage(res, "VPS_CHANGE_VM_CONFIGURATION", "VPS");
                }
            }
            catch (Exception ex)
            {
                messageBox.ShowErrorMessage("VPS_CHANGE_VM_CONFIGURATION", ex);
            }
        }
    }
}
