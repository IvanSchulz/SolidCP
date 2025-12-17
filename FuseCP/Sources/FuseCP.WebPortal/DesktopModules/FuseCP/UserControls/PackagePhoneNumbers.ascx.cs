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
    public partial class PackagePhoneNumbers : FuseCPControlBase
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
            if (!IsPostBack)
            {
                searchBox.AddCriteria("ExternalIP", GetLocalizedString("SearchField.IPAddress"));
                searchBox.AddCriteria("ItemName", GetLocalizedString("SearchField.ItemName"));
                searchBox.AddCriteria("Username", GetLocalizedString("SearchField.Username"));
            }
            searchBox.AjaxData = this.GetSearchBoxAjaxData();

            bool isUserSelected = PanelSecurity.SelectedUser.Role == FuseCP.EnterpriseServer.UserRole.User;
            bool isUserLogged = PanelSecurity.EffectiveUser.Role == FuseCP.EnterpriseServer.UserRole.User;
            spaceOwner = PanelSecurity.EffectiveUserId == PanelSecurity.SelectedUserId;

            gvAddresses.Columns[3].Visible = !isUserSelected; // space
            gvAddresses.Columns[4].Visible = !isUserSelected; // user

            // managing external network permissions
            gvAddresses.Columns[0].Visible = !isUserLogged && ManageAllowed;
            btnAllocateAddress.Visible = !isUserLogged && !spaceOwner && ManageAllowed && !String.IsNullOrEmpty(AllocateAddressesControl);
            btnDeallocateAddresses.Visible = !isUserLogged && ManageAllowed;
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
                messageBox.ShowErrorMessage("PACKAGE_PHONE_NUMBER", e.Exception);
                e.ExceptionHandled = true;
            }
        }

        protected void btnAllocateAddress_Click(object sender, EventArgs e)
        {
            Response.Redirect(HostModule.EditUrl("ItemID", PanelRequest.ItemID.ToString(), AllocateAddressesControl,
                PortalUtils.SPACE_ID_PARAM + "=" + PanelSecurity.PackageId));
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
                    messageBox.ShowWarningMessage("PHONE_EDIT_LIST_EMPTY_ERROR");
                    return;
                }

                ResultObject res = ES.Services.Servers.DeallocatePackageIPAddresses(PanelSecurity.PackageId, items.ToArray());
                messageBox.ShowMessage(res, "DEALLOCATE_SPACE_PHONE_NUMBER", "VPS");
                gvAddresses.DataBind();
            }
            catch (Exception ex)
            {
                messageBox.ShowErrorMessage("DEALLOCATE_SPACE_PHONE_NUMBER", ex);
            }
        }

        protected void odsExternalAddressesPaged_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
        {
            e.InputParameters["pool"] = Pool;
        }

        public string GetSearchBoxAjaxData()
        {
            StringBuilder res = new StringBuilder();
            res.Append("PagedStored: 'PackageIPAddresses'");
            res.Append(", RedirectUrl: '" + GetItemEditUrl("{0}").Substring(2) + "'");
            res.Append(", PackageID: " + (String.IsNullOrEmpty(Request["SpaceID"]) ? "0" : Request["SpaceID"]));
            res.Append(", OrgID: " + (String.IsNullOrEmpty(Request["ItemID"]) ? "0" : Request["ItemID"]));
            res.Append(", PoolID: " + Pool != null ? Pool.ToString() : "0");
            res.Append(", Recursive: true");
            return res.ToString();
        }
    }
}
