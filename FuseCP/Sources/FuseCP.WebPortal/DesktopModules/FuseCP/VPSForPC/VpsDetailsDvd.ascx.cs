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

namespace FuseCP.Portal.VPSForPC
{
    public partial class VpsDetailsDvd : FuseCPModuleBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack && Request["action"] == "inserted")
                messageBox.ShowSuccessMessage("VPS_DVD_INSERTED");

            if (!IsPostBack)
            {
                BindDvdDisk();
            }
        }

        private void BindDvdDisk()
        {
            LibraryItem disk = ES.Services.VPS.GetInsertedDvdDisk(PanelRequest.ItemID);

            if (disk != null)
            {
                txtInsertedDisk.Text = disk.Name;
                btnInsertDisk.Enabled = false;
                btnEjectDisk.Enabled = true;
            }
            else
            {
                txtInsertedDisk.Text = GetLocalizedString("NoDisk.Text");
                btnInsertDisk.Enabled = true;
                btnEjectDisk.Enabled = false;
            }
        }

        protected void btnInsertDisk_Click(object sender, EventArgs e)
        {
            Response.Redirect(EditUrl("ItemID", PanelRequest.ItemID.ToString(), "vps_insert_dvd",
                "SpaceID=" + PanelSecurity.PackageId.ToString()));
        }

        protected void btnEjectDisk_Click(object sender, EventArgs e)
        {
            try
            {
                ResultObject res = ES.Services.VPS.EjectDvdDisk(PanelRequest.ItemID);

                if (res.IsSuccess)
                {
                    // re-bind
                    messageBox.ShowSuccessMessage("VPS_DVD_EJECTED");
                    BindDvdDisk();
                    return;
                }
                else
                {
                    // show error
                    messageBox.ShowMessage(res, "VPS_ERROR_EJECT_DVD_DISK", "VPS");
                }
            }
            catch (Exception ex)
            {
                messageBox.ShowErrorMessage("VPS_ERROR_EJECT_DVD_DISK", ex);
            }
        }
    }
}
