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
using FuseCP.Providers.Common;

namespace FuseCP.Portal.UserControls
{
    public partial class PackageIPAddresses : FuseCPControlBase
    {
        private bool spaceOwner;

        private IPAddressPool pool;
        public IPAddressPool Pool
        {
            get { return pool; }
            set { pool = value; }
        }

        private string editItemControl;
        public string EditItemControl
        {
            get { return editItemControl; }
            set { editItemControl = value; }
        }

        private string spaceHomeControl;
        public string SpaceHomeControl
        {
            get { return spaceHomeControl; }
            set { spaceHomeControl = value; }
        }

        private string allocateAddressesControl;
        public string AllocateAddressesControl
        {
            get { return allocateAddressesControl; }
            set { allocateAddressesControl = value; }
        }

        public bool ManageAllowed
        {
            get { return ViewState["ManageAllowed"] != null ? (bool)ViewState["ManageAllowed"] : false; }
            set { ViewState["ManageAllowed"] = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            bool vps = (Pool == IPAddressPool.VpsExternalNetwork || Pool == IPAddressPool.VpsManagementNetwork);

            if (!IsPostBack)
            {
                searchBox.AddCriteria("ExternalIP", GetLocalizedString("SearchField.IPAddress"));
                searchBox.AddCriteria("InternalIP", GetLocalizedString("SearchField.InternalIP"));
                if (vps)
                    searchBox.AddCriteria("DefaultGateway", GetLocalizedString("SearchField.DefaultGateway"));
                searchBox.AddCriteria("ItemName", GetLocalizedString("SearchField.ItemName"));
                searchBox.AddCriteria("Username", GetLocalizedString("SearchField.Username"));
            }
            searchBox.AjaxData = this.GetSearchBoxAjaxData();

            bool isUserSelected = PanelSecurity.SelectedUser.Role == FuseCP.EnterpriseServer.UserRole.User;
            bool isUserLogged = PanelSecurity.EffectiveUser.Role == FuseCP.EnterpriseServer.UserRole.User;
            spaceOwner = PanelSecurity.EffectiveUserId == PanelSecurity.SelectedUserId;

            gvAddresses.Columns[6].Visible = !isUserSelected; // space
            gvAddresses.Columns[7].Visible = !isUserSelected; // user

            // managing external network permissions
            gvAddresses.Columns[0].Visible = !isUserLogged && ManageAllowed;
            btnAllocateAddress.Visible = !isUserLogged && !spaceOwner && ManageAllowed && !String.IsNullOrEmpty(AllocateAddressesControl);
            btnDeallocateAddresses.Visible = !isUserLogged && ManageAllowed;

            //gvAddresses.Columns[2].Visible = !vps; // NAT address
            gvAddresses.Columns[3].Visible = vps; // gateway
            gvAddresses.Columns[5].Visible = vps; // is primary
        }

        public string GetItemEditUrl(string itemID)
        {
            return HostModule.EditUrl("SpaceID", PanelSecurity.PackageId.ToString(), EditItemControl,
                    "ItemID=" + itemID);
        }

        public string GetSpaceHomeUrl(string spaceId)
        {
            return HostModule.EditUrl("SpaceID", spaceId, SpaceHomeControl);
        }

        protected void odsExternalAddressesPaged_Selected(object sender, ObjectDataSourceStatusEventArgs e)
        {
            if (e.Exception != null)
            {
                messageBox.ShowErrorMessage("PACKAGE_IP_ADDRESS", e.Exception);
                e.ExceptionHandled = true;
            }
        }

        protected void btnAllocateAddress_Click(object sender, EventArgs e)
        {
            Response.Redirect(HostModule.EditUrl("SpaceID", PanelSecurity.PackageId.ToString(), AllocateAddressesControl));
        }

        protected void gvAddresses_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            PackageIPAddress item = e.Row.DataItem as PackageIPAddress;
            if (item != null)
            {
                // checkbox
                CheckBox chkSelect = e.Row.FindControl("chkSelect") as CheckBox;
                chkSelect.Enabled = (!spaceOwner || (PanelSecurity.PackageId != item.PackageId)) && item.ItemId == 0;
            }
        }

        protected void btnDeallocateAddresses_Click(object sender, EventArgs e)
        {
            List<int> ids = new List<int>();

            try
            {
                List<int> items = new List<int>();
                for (int i = 0; i < gvAddresses.Rows.Count; i++)
                {
                    GridViewRow row = gvAddresses.Rows[i];
                    CheckBox chkSelect = (CheckBox)row.FindControl("chkSelect");
                    if (chkSelect.Checked)
                        items.Add((int)gvAddresses.DataKeys[i].Value);
                }

                // check if at least one is selected
                if (items.Count == 0)
                {
                    messageBox.ShowWarningMessage("IP_EDIT_LIST_EMPTY_ERROR");
                    return;
                }

                ResultObject res = ES.Services.Servers.DeallocatePackageIPAddresses(PanelSecurity.PackageId, items.ToArray());
                messageBox.ShowMessage(res, "DEALLOCATE_SPACE_IP_ADDRESSES", "VPS");
                gvAddresses.DataBind();
            }
            catch (Exception ex)
            {
                messageBox.ShowErrorMessage("DEALLOCATE_SPACE_IP_ADDRESSES", ex);
            }
        }

        protected void odsExternalAddressesPaged_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
        {
            e.InputParameters["pool"] = Pool;
        }

        protected void ddlPageSize_SelectedIndexChanged(object sender, EventArgs e)
        {
            gvAddresses.PageSize = Convert.ToInt32(ddlPageSize.SelectedValue);
            gvAddresses.DataBind();
        }

        public string GetSearchBoxAjaxData()
        {
            StringBuilder res = new StringBuilder();
            res.Append("PagedStored: 'PackageIPAddresses'");
            res.Append(", RedirectUrl: '" + GetItemEditUrl("{0}").Substring(2) + "'");
            res.Append(", PackageID: " + (String.IsNullOrEmpty(Request["SpaceID"]) ? "0" : Request["SpaceID"]));
            res.Append(", OrgID: 0");
            res.Append(", PoolID: " + Pool != null ? Pool.ToString() : "0");
            res.Append(", Recursive: true");
            return res.ToString();
        }
    }
}
