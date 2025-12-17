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
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FuseCP.EnterpriseServer;

namespace FuseCP.Portal.VPSForPC
{
    public partial class VdcHome : FuseCPModuleBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                searchBox.AddCriteria("ItemName", GetLocalizedString("SearchField.ItemName"));
                searchBox.AddCriteria("Username", GetLocalizedString("SearchField.Username"));
                searchBox.AddCriteria("ExternalIP", GetLocalizedString("SearchField.ExternalIP"));
                searchBox.AddCriteria("IPAddress", GetLocalizedString("SearchField.IPAddress"));
            }
            searchBox.AjaxData = this.GetSearchBoxAjaxData();

            // toggle columns
            bool isUserSelected = PanelSecurity.SelectedUser.Role == FuseCP.EnterpriseServer.UserRole.User;
            gvServers.Columns[3].Visible = !isUserSelected;
            gvServers.Columns[4].Visible = !isUserSelected;

            // check package quotas
            bool manageAllowed = VirtualMachinesForPCHelper.IsVirtualMachineManagementAllowed(PanelSecurity.PackageId);

            btnCreate.Visible = manageAllowed;
            gvServers.Columns[5].Visible = manageAllowed; // delete column

            // admin operations column
            gvServers.Columns[6].Visible = (PanelSecurity.EffectiveUser.Role == UserRole.Administrator);
        }

        public string GetServerEditUrl(string itemID)
        {
            return EditUrl("SpaceID", PanelSecurity.PackageId.ToString(), "vps_general",
                    "ItemID=" + itemID);
        }

        public string GetSpaceHomeUrl(string spaceId)
        {
            return EditUrl("SpaceID", spaceId, "");
        }

        public string GetUserHomeUrl(int userId)
        {
            return PortalUtils.GetUserHomePageUrl(userId);
        }

        protected void odsServersPaged_Selected(object sender, ObjectDataSourceStatusEventArgs e)
        {
            if (e.Exception != null)
            {
                messageBox.ShowErrorMessage("VPS_ERROR_GET_VDC_HOME", e.Exception);
                e.ExceptionHandled = true;
            }
        }

        protected void btnCreate_Click(object sender, EventArgs e)
        {
            Response.Redirect(EditUrl("SpaceID", PanelSecurity.PackageId.ToString(), "vdc_create_server"));
        }

        protected void btnImport_Click(object sender, EventArgs e)
        {
            Response.Redirect(EditUrl("SpaceID", PanelSecurity.PackageId.ToString(), "vdc_import_server"));
        }

        protected void btnFastCreate_Click(object sender, EventArgs e)
        {
            Response.Redirect(EditUrl("SpaceID", PanelSecurity.PackageId.ToString(), "vdc_FastCreate_server"));
        }

        protected void gvServers_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "DeleteItem")
            {
                // get server ID
                int itemId = Utils.ParseInt(e.CommandArgument.ToString(), 0);

                // go to delete page
                Response.Redirect(EditUrl("SpaceID", PanelSecurity.PackageId.ToString(), "vps_tools_delete",
                    "ItemID=" + itemId));
            }
            else if (e.CommandName == "Detach")
            {
                // remove item from meta base
                int itemId = Utils.ParseInt(e.CommandArgument.ToString(), 0);

                int result = ES.Services.Packages.DetachPackageItem(itemId);
                if (result < 0)
                {
                    ShowResultMessage(result);
                    return;
                }

                // refresh the list
                gvServers.DataBind();
            }
            else if (e.CommandName == "Move")
            {
                // get server ID
                int itemId = Utils.ParseInt(e.CommandArgument.ToString(), 0);

                // go to delete page
                Response.Redirect(EditUrl("SpaceID", PanelSecurity.PackageId.ToString(), "vps_tools_move",
                    "ItemID=" + itemId));
            }
        }

        public string GetSearchBoxAjaxData()
        {
            StringBuilder res = new StringBuilder();
            res.Append("PagedStored: 'VirtualMachines'");
            res.Append(", RedirectUrl: '" + GetServerEditUrl("{0}").Substring(2) + "'");
            res.Append(", PackageID: " + (String.IsNullOrEmpty(Request["SpaceID"]) ? "0" : Request["SpaceID"]));
            res.Append(", Recursive: true");
            res.Append(", VPSTypeID: 'VPSForPC'");
            return res.ToString();
        }
    }
}
