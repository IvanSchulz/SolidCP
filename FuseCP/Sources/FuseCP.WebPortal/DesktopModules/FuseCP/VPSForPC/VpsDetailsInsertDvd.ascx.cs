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

namespace FuseCP.Portal.VPSForPC
{
    public partial class VpsDetailsInsertDvd : FuseCPModuleBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindDisks();
            }
        }

        private void BindDisks()
        {
            gvDisks.DataSource = ES.Services.VPS.GetLibraryDisks(PanelRequest.ItemID);
            gvDisks.DataBind();
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            RedirectBack("cancel");
        }

        protected void gvDisks_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "insert")
            {
                string path = e.CommandArgument.ToString();

                try
                {
                    ResultObject res = ES.Services.VPS.InsertDvdDisk(PanelRequest.ItemID, path);

                    if (res.IsSuccess)
                    {
                        // return
                        RedirectBack("inserted");
                        return;
                    }
                    else
                    {
                        // show error
                        messageBox.ShowMessage(res, "VPS_ERROR_INSERT_DVD_DISK", "VPS");
                    }
                }
                catch (Exception ex)
                {
                    messageBox.ShowErrorMessage("VPS_ERROR_INSERT_DVD_DISK", ex);
                }
            }
        }

        private void RedirectBack(string action)
        {
            Response.Redirect(EditUrl("ItemID", PanelRequest.ItemID.ToString(), "vps_dvd",
                "SpaceID=" + PanelSecurity.PackageId.ToString(),
                "action=" + action));
        }
    }
}
