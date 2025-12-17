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
using System.Data;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

using FuseCP.EnterpriseServer;
using FuseCP.WebPortal;

namespace FuseCP.Portal.SkinControls
{
    public partial class SpaceOrganizationsSelector : UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindSpaceOrgs();
            }
        }

        private void BindSpaceOrgs()
        {
            // organization
            bool orgVisible = (PanelRequest.ItemID > 0 && Request[DefaultPage.PAGE_ID_PARAM].Equals(UserSpaceBreadcrumb.PID_SPACE_EXCHANGE_SERVER, StringComparison.InvariantCultureIgnoreCase));

            spanOrgsSelector.Visible = orgVisible;

            if (orgVisible)
            {
                OrganizationsHelper helper = new OrganizationsHelper();

                ddlSpaceOrgs.DataSource = helper.GetOrganizations(PanelSecurity.PackageId, false);
                ddlSpaceOrgs.DataTextField = "ItemName";
                ddlSpaceOrgs.DataValueField = "ItemID";
                ddlSpaceOrgs.DataBind();

                ddlSpaceOrgs.Items.FindByValue(PanelRequest.ItemID.ToString()).Selected = true; 

                lnkOrgnsList.NavigateUrl = PortalUtils.NavigatePageURL(
                        PortalUtils.GetCurrentPageId(), "SpaceID", PanelSecurity.PackageId.ToString());
            }
        }

        protected void ddlSpaceOrgs_SelectedIndexChanged(object sender, EventArgs e)
        {
            Response.Redirect(GetOrganizationEditUrl(ddlSpaceOrgs.SelectedValue));
        }

        private string GetOrganizationEditUrl(string itemId)
        {
            return PortalUtils.EditUrl("SpaceID", PanelSecurity.PackageId.ToString(), "organization_home",
                    "ItemID=" + itemId);
        }
    }
}
