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
using FuseCP.Providers.Common;
using FuseCP.Providers.Virtualization;

namespace FuseCP.Portal.VPS
{
    public partial class VpsToolsDeleteServer : FuseCPModuleBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindFormDetails();
            }

            ToogleControls();
        }

        private void ToogleControls()
        {
            txtExportPath.Visible = chkExport.Checked;
            ExportPathValidator.Enabled = chkExport.Checked;
        }

        private void BindFormDetails()
        {
            // load VM item
            VirtualMachine vm = VirtualMachinesHelper.GetCachedVirtualMachine(PanelRequest.ItemID);
            if (!String.IsNullOrEmpty(vm.CurrentTaskId))
            {
                messageBox.ShowWarningMessage("VPS_PROVISIONING_PROCESS");
                btnDelete.Enabled = false;
                return;
            }

            // load export settings
            if (PanelSecurity.EffectiveUser.Role == FuseCP.EnterpriseServer.UserRole.Administrator)
            {
                txtExportPath.Text = ES.Services.VPS.GetDefaultExportPath(PanelRequest.ItemID);
            }
            else
            {
                AdminOptionsPanel.Visible = false;
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect(EditUrl("SpaceID", PanelSecurity.PackageId.ToString(), ""));
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            if (!chkConfirmDelete.Checked)
            {
                messageBox.ShowWarningMessage("VPS_DELETE_CONFIRM");
                return;
            }

            // delete machine
            try
            {
                ResultObject res = ES.Services.VPS.DeleteVirtualMachine(PanelRequest.ItemID,
                    chkSaveFiles.Checked, chkExport.Checked, txtExportPath.Text.Trim());

                if (res.IsSuccess)
                {
                    // return to the list
                    Response.Redirect(EditUrl("SpaceID", PanelSecurity.PackageId.ToString(), ""));
                    return;
                }
                else
                {
                    // show error
                    messageBox.ShowMessage(res, "VPS_ERROR_DELETE", "VPS");
                }
            }
            catch (Exception ex)
            {
                messageBox.ShowErrorMessage("VPS_ERROR_DELETE", ex);
            }
        }
    }
}
