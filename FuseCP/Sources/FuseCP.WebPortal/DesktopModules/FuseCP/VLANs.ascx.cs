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
using System.Web.UI.WebControls;
using System.Collections.Generic;
using FuseCP.Providers.Common;
using System.Text;
using FuseCP.Portal.Code.Helpers;

namespace FuseCP.Portal
{
    public partial class VLANs : FuseCPModuleBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // set display preferences
            if (!IsPostBack)
            {
                // page size
                gvVLANs.PageSize = UsersHelper.GetDisplayItemsPerPage();
                ddlItemsPerPage.SelectedValue = gvVLANs.PageSize.ToString();

                gvVLANs.PageIndex = PageIndex;
            }
            else
            {
                gvVLANs.PageSize = Utils.ParseInt(ddlItemsPerPage.SelectedValue, 10);
            }
        }

        protected void odsVLANs_Selected(object sender, ObjectDataSourceStatusEventArgs e)
        {
            if (e.Exception != null)
            {
                ProcessException(e.Exception);
                this.DisableControls = true;
                e.ExceptionHandled = true;
            }
        }

        public string GetSpaceHomeUrl(int spaceId)
        {
            return PortalUtils.GetSpaceHomePageUrl(spaceId);
        }

        public string GetReturnUrl()
        {
            var returnUrl = Request.Url.AddParameter("Page", gvVLANs.PageIndex.ToString());
            return Uri.EscapeDataString("~" + returnUrl.PathAndQuery);
        }

        public int PageIndex
        {
            get
            {
                return PanelRequest.GetInt("Page", 0);
            }
        }

        protected void btnAddItem_Click(object sender, EventArgs e)
        {
            Response.Redirect(EditUrl("ServerID", PanelRequest.ServerId.ToString(), "add_vlan", "ReturnUrl=" + GetReturnUrl()), true);
        }

        protected void ddlItemsPerPage_SelectedIndexChanged(object sender, EventArgs e)
        {
            gvVLANs.PageSize = Utils.ParseInt(ddlItemsPerPage.SelectedValue, 10);
            gvVLANs.DataBind();
        }

        protected void btnDeleteSelected_Click(object sender, EventArgs e)
        {
            int[] vlans = GetSelectedItems(gvVLANs);
            if (vlans.Length == 0)
            {
                ShowWarningMessage("VLAN_DELETE_LIST_EMPTY_ERROR");
                return;
            }

            try
            {
                // delete selected VLANs
                ResultObject res = ES.Services.Servers.DeletePrivateNetworkVLANs(vlans);

                if (!res.IsSuccess)
                {
                    messageBox.ShowMessage(res, "VLAN_DELETE_RANGE_VLAN", "VLAN");
                    return;
                }

                // refresh grid
                gvVLANs.DataBind();
            }
            catch (Exception ex)
            {
                ShowErrorMessage("VLAN_DELETE_RANGE_VLAN", ex);
                return;
            }
        }

        private int[] GetSelectedItems(GridView gv)
        {
            List<int> items = new List<int>();

            for (int i = 0; i < gv.Rows.Count; i++)
            {
                GridViewRow row = gv.Rows[i];
                CheckBox chkSelect = (CheckBox)row.FindControl("chkSelect");
                if (chkSelect.Checked)
                    items.Add((int)gv.DataKeys[i].Value);
            }

            return items.ToArray();
        }
    }
}
