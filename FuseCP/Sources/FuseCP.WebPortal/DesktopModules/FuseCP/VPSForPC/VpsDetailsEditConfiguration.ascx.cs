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
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FuseCP.Providers.Virtualization;
using FuseCP.Providers.Common;
using FuseCP.EnterpriseServer;

namespace FuseCP.Portal.VPSForPC
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
            VMInfo vm = null;

            try
            {
                // load machine
                vm = ES.Services.VPSPC.GetVirtualMachineItem(PanelRequest.ItemID);

                if (vm == null)
                {
                    messageBox.ShowErrorMessage("VPS_LOAD_VM_META_ITEM");
                    return;
                }

                // bind CPU cores
                int maxCores = ES.Services.VPSPC.GetMaximumCpuCoresNumber(vm.PackageId, vm.Name);
                PackageContext cntx = PackagesHelper.GetCachedPackageContext(PanelSecurity.PackageId);

                QuotaValueInfo cpuQuota2 = cntx.Quotas[Quotas.VPSForPC_CPU_NUMBER];
                int cpuQuotausable = (cpuQuota2.QuotaAllocatedValue - cpuQuota2.QuotaUsedValue) + vm.CPUCount;

                if (cpuQuota2.QuotaAllocatedValue == -1)
                {
                    for (int i = 1; i < maxCores + 1; i++)
                        ddlCpu.Items.Add(i.ToString());

                    ddlCpu.SelectedIndex = ddlCpu.Items.Count - 1; // select last (maximum) item
                }
                else if (cpuQuota2.QuotaAllocatedValue >= cpuQuota2.QuotaUsedValue)
                {
                    if (cpuQuotausable > maxCores)
                    {
                        for (int i = 1; i < maxCores + 1; i++)
                            ddlCpu.Items.Add(i.ToString());

                        ddlCpu.SelectedIndex = ddlCpu.Items.Count - 1; // select last (maximum) item
                    }
                    else
                    {
                        for (int i = 1; i < cpuQuotausable + 1; i++)
                            ddlCpu.Items.Add(i.ToString());

                        ddlCpu.SelectedIndex = ddlCpu.Items.Count - 1; // select last (maximum) item
                    }
                }
                else
                {
                    for (int i = 1; i < vm.CPUCount + 1; i++)
                        ddlCpu.Items.Add(i.ToString());

                    ddlCpu.SelectedIndex = ddlCpu.Items.Count - 1; // select last (maximum) item

                }

                // bind item
                ddlCpu.SelectedValue = vm.CPUCount.ToString();
                txtRam.Text = vm.Memory.ToString();
                txtHdd.Text = vm.HddSize.ToString();
                txtSnapshots.Text = vm.SnapshotsNumber.ToString();

                chkDvdInstalled.Checked = vm.DvdDriver;
                chkBootFromCd.Checked = vm.BootFromCD;
                chkNumLock.Checked = vm.NumLockEnabled;

                chkStartShutdown.Checked = vm.StartTurnOffAllowed;
                chkPauseResume.Checked = vm.PauseResumeAllowed;
                chkReset.Checked = vm.ResetAllowed;
                chkReboot.Checked = vm.RebootAllowed;
                chkReinstall.Checked = vm.ReinstallAllowed;

                chkExternalNetworkEnabled.Checked = vm.ExternalNetworkEnabled;
                chkPrivateNetworkEnabled.Checked = vm.PrivateNetworkEnabled;

                // toggle controls


                // other quotas
                BindCheckboxOption(chkDvdInstalled, Quotas.VPSForPC_DVD_ENABLED);
                chkBootFromCd.Enabled = PackagesHelper.IsQuotaEnabled(PanelSecurity.PackageId, Quotas.VPSForPC_BOOT_CD_ALLOWED);

                BindCheckboxOption(chkStartShutdown, Quotas.VPSForPC_START_SHUTDOWN_ALLOWED);
                BindCheckboxOption(chkPauseResume, Quotas.VPSForPC_PAUSE_RESUME_ALLOWED);
                BindCheckboxOption(chkReset, Quotas.VPSForPC_RESET_ALOWED);
                BindCheckboxOption(chkReboot, Quotas.VPSForPC_REBOOT_ALLOWED);
                BindCheckboxOption(chkReinstall, Quotas.VPSForPC_REINSTALL_ALLOWED);

                BindCheckboxOption(chkExternalNetworkEnabled, Quotas.VPSForPC_EXTERNAL_NETWORK_ENABLED);
                BindCheckboxOption(chkPrivateNetworkEnabled, Quotas.VPSForPC_PRIVATE_NETWORK_ENABLED);
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
            try
            {
                // check rights
                bool manageAllowed = VirtualMachinesForPCHelper.IsVirtualMachineManagementAllowed(PanelSecurity.PackageId);
                if (!manageAllowed)
                {
                    return;
                }

                ResultObject res = ES.Services.VPSPC.UpdateVirtualMachineConfiguration(PanelRequest.ItemID,
                    Utils.ParseInt(ddlCpu.SelectedValue),
                    Utils.ParseInt(txtRam.Text.Trim()),
                    Utils.ParseInt(txtHdd.Text.Trim()),
                    Utils.ParseInt(txtSnapshots.Text.Trim()),
                    chkDvdInstalled.Checked,
                    chkBootFromCd.Checked,
                    chkNumLock.Checked,
                    chkStartShutdown.Checked,
                    chkPauseResume.Checked,
                    chkReboot.Checked,
                    chkReset.Checked,
                    chkReinstall.Checked,
                    chkExternalNetworkEnabled.Checked,
                    chkPrivateNetworkEnabled.Checked);

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
