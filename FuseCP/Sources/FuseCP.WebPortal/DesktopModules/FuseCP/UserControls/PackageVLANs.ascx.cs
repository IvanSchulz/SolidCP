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
using System.Text;
using System.Web.UI.WebControls;
using FuseCP.EnterpriseServer;
using FuseCP.Providers.Common;

namespace FuseCP.Portal.UserControls
{
    public partial class PackageVLANs : FuseCPControlBase
    {
        private bool spaceOwner;

        private string spaceHomeControl;
        public string SpaceHomeControl
        {
            get { return spaceHomeControl; }
            set { spaceHomeControl = value; }
        }

        private string allocateVLANsControl;
        public string AllocateVLANsControl
        {
            get { return allocateVLANsControl; }
            set { allocateVLANsControl = value; }
        }

        public bool ManageAllowed
        {
            get { return ViewState["ManageAllowed"] != null ? (bool)ViewState["ManageAllowed"] : false; }
            set { ViewState["ManageAllowed"] = value; }
        }

        public bool IsDmz
        {
            get { return ViewState["IsDmz"] != null ? (bool)ViewState["IsDmz"] : false; }
            set { ViewState["IsDmz"] = value; }
        }

        private PackageVLANsPaged packageVLANs;

        protected void Page_Load(object sender, EventArgs e)
        {
            bool isUserSelected = PanelSecurity.SelectedUser.Role == FuseCP.EnterpriseServer.UserRole.User;
            bool isUserLogged = PanelSecurity.EffectiveUser.Role == FuseCP.EnterpriseServer.UserRole.User;
            spaceOwner = PanelSecurity.EffectiveUserId == PanelSecurity.SelectedUserId;

            cbIsDmz.Checked = IsDmz;

            gvVLANs.Columns[2].Visible = !isUserSelected; // space
            gvVLANs.Columns[3].Visible = !isUserSelected; // user

            // managing external network permissions
            gvVLANs.Columns[0].Visible = !isUserLogged && ManageAllowed;
            btnAllocateVLAN.Visible = !isUserLogged && !spaceOwner && ManageAllowed && !String.IsNullOrEmpty(AllocateVLANsControl);
            btnDeallocateVLANs.Visible = !isUserLogged && ManageAllowed;
        }

        public string GetSpaceHomeUrl(string spaceId)
        {
            return HostModule.EditUrl("SpaceID", spaceId, SpaceHomeControl);
        }

        protected void odsVLANsPaged_Selected(object sender, ObjectDataSourceStatusEventArgs e)
        {
            if (e.Exception != null)
            {
                messageBox.ShowErrorMessage("VLAN_GET_VLAN", e.Exception);
                e.ExceptionHandled = true;
            }
        }

        protected void btnAllocateVLAN_Click(object sender, EventArgs e)
        {
            Response.Redirect(HostModule.EditUrl("SpaceID", PanelSecurity.PackageId.ToString(), AllocateVLANsControl));
        }

        protected void gvVLANs_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            PackageVLAN item = e.Row.DataItem as PackageVLAN;
            if (item != null)
            {
                // checkbox
                CheckBox chkSelect = e.Row.FindControl("chkSelect") as CheckBox;
                chkSelect.Enabled = (!spaceOwner || (PanelSecurity.PackageId != item.PackageId));
            }
        }

        protected void btnDeallocateVLANs_Click(object sender, EventArgs e)
        {
            try
            {
                List<int> items = new List<int>();
                for (int i = 0; i < gvVLANs.Rows.Count; i++)
                {
                    GridViewRow row = gvVLANs.Rows[i];
                    CheckBox chkSelect = (CheckBox)row.FindControl("chkSelect");
                    if (chkSelect.Checked)
                        items.Add((int)gvVLANs.DataKeys[i].Value);
                }

                // check if at least one is selected
                if (items.Count == 0)
                {
                    messageBox.ShowWarningMessage("VLAN_EDIT_LIST_EMPTY_ERROR");
                    return;
                }

                ResultObject res = ES.Services.Servers.DeallocatePackageVLANs(PanelSecurity.PackageId, items.ToArray());
                messageBox.ShowMessage(res, "DEALLOCATE_SPACE_VLANS", "VPS");
                gvVLANs.DataBind();
            }
            catch (Exception ex)
            {
                messageBox.ShowErrorMessage("DEALLOCATE_SPACE_VLANS", ex);
            }
        }

        protected void ddlPageSize_SelectedIndexChanged(object sender, EventArgs e)
        {
            gvVLANs.PageSize = Convert.ToInt32(ddlPageSize.SelectedValue);
            gvVLANs.DataBind();
        }
    }
}
