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

namespace FuseCP.Portal.VPS
{
    public partial class VpsDetailsConfiguration : FuseCPModuleBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack && Request["action"] == "changed")
                messageBox.ShowSuccessMessage("VPS_CHANGE_VM_CONFIGURATION");

            if (!IsPostBack)
            {
                // config
                BindConfiguration();

                // bind password policy
                password.SetPackagePolicy(PanelSecurity.PackageId, UserSettings.VPS_POLICY, "AdministratorPasswordPolicy");
            }
        }

        private void BindConfiguration()
        {
            VirtualMachine vm = null;

            try
            {
                // load machine
                vm = ES.Services.VPS.GetVirtualMachineItem(PanelRequest.ItemID);

                if (vm == null)
                {
                    messageBox.ShowErrorMessage("VPS_LOAD_VM_META_ITEM");
                    return;
                }

                // bind item
                litOperatingSystem.Text = vm.OperatingSystemTemplate;

                litCpu.Text = String.Format(GetLocalizedString("CpuCores.Text"), vm.CpuCores);
                litRam.Text = String.Format(GetLocalizedString("Ram.Text"), vm.RamSize);
                litHdd.Text = String.Format(GetLocalizedString("Hdd.Text"), vm.HddSize[0]);
                litSnapshots.Text = vm.SnapshotsNumber.ToString();

                optionDvdInstalled.Value = vm.DvdDriveInstalled;
                optionBootFromCD.Value = vm.BootFromCD;
                optionNumLock.Value = vm.NumLockEnabled;

                optionStartShutdown.Value = vm.StartTurnOffAllowed;
                optionPauseResume.Value = vm.PauseResumeAllowed;
                optionReset.Value = vm.ResetAllowed;
                optionReboot.Value = vm.RebootAllowed;
                optionReinstall.Value = vm.ReinstallAllowed;

                optionExternalNetwork.Value = vm.ExternalNetworkEnabled;
                optionPrivateNetwork.Value = vm.PrivateNetworkEnabled;

                // toggle buttons
                bool manageAllowed = VirtualMachinesHelper.IsVirtualMachineManagementAllowed(PanelSecurity.PackageId);
                btnEdit.Visible = manageAllowed;
            }
            catch (Exception ex)
            {
                messageBox.ShowErrorMessage("VPS_LOAD_VM_META_ITEM", ex);
            }
        }

        protected void btnChangePassword_Click(object sender, EventArgs e)
        {
            try
            {
                ResultObject res = ES.Services.VPS.ChangeAdministratorPassword(PanelRequest.ItemID, password.Password);

                if (res.IsSuccess)
                {
                    // show success message
                    messageBox.ShowSuccessMessage("VPS_CHANGE_ADMIN_PASSWORD");
                }
                else
                {
                    // show error
                    messageBox.ShowMessage(res, "VPS_CHANGE_ADMIN_PASSWORD", "VPS");
                }
            }
            catch (Exception ex)
            {
                messageBox.ShowErrorMessage("VPS_CHANGE_ADMIN_PASSWORD", ex);
            }
        }

        protected void btnEdit_Click(object sender, EventArgs e)
        {
            Response.Redirect(EditUrl("ItemID", PanelRequest.ItemID.ToString(), "vps_edit_config",
                "SpaceID=" + PanelSecurity.PackageId.ToString()));
        }
    }
}
