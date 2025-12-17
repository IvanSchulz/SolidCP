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
using FuseCP.EnterpriseServer;

namespace FuseCP.Portal.VPSForPC
{
    public partial class VdcFastCreateServer : FuseCPModuleBase
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
        }

        private void BindFormDetails()
        {
            VirtualMachinesForPCHelper helper = new VirtualMachinesForPCHelper();

            int count = helper.GetVirtualMachinesCount(PanelSecurity.PackageId, "ItemName", "%%");

            listOperatingSystems.Items.Clear();

            try
            {
                VirtualMachineMetaItem[] items = helper.GetVirtualMachines(PanelSecurity.PackageId, "ItemName", "%%", String.Empty, count, 0);

                if (items != null && items.Length > 0)
                {
                    listOperatingSystems.Items.Add(new ListItem(GetLocalizedString("SelectVM.Text"), ""));

                    for (int i = 0; i < items.Length; i++)
                    {
                        listOperatingSystems.Items.Add(new ListItem(items[i].ItemName, items[i].ItemID.ToString()));
                    }
                }
                else
                {
                    throw new Exception("no VM");
                }
            }
            catch (Exception ex)
            {
                listOperatingSystems.Items.Add(new ListItem(GetLocalizedString("SelectVM.Text"), ""));
                listOperatingSystems.Enabled = false;
                txtVmName.Enabled = false;
                btnCreate.Enabled = false;
                messageBox.ShowErrorMessage("VPS_ERROR_CREATE", new Exception("no VM", ex));
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect(EditUrl("SpaceID", PanelSecurity.PackageId.ToString(), ""));
        }

        protected void btnCreate_Click(object sender, EventArgs e)
        {
            try
            {
                VMInfo selectedVM = VirtualMachinesForPCHelper.GetCachedVirtualMachineForPC(Convert.ToInt32(listOperatingSystems.SelectedValue.Trim()));

                ResultObject res = ES.Services.VPSPC.CreateVMFromVM(PanelSecurity.PackageId
                    , selectedVM, txtVmName.Text.Trim());

                if (res.IsSuccess)
                {
                    // return to the list
                    Response.Redirect(EditUrl("SpaceID", PanelSecurity.PackageId.ToString(), ""));
                    return;
                }
                else
                {
                    // show error
                    messageBox.ShowMessage(res, "VPS_ERROR_CREATE", "VPS");
                }
            }
            catch (Exception ex)
            {
                messageBox.ShowErrorMessage("VPS_ERROR_CREATE", ex);
            }
        }
    }
}
