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
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

using FuseCP.EnterpriseServer;

namespace FuseCP.Portal
{
    public partial class SpaceNestedSpacesSummary : FuseCPModuleBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // bind groups
            BindGroupings();

            // other controls
            this.ContainerControl.Visible = (PanelSecurity.SelectedUser.Role != UserRole.User);
            lnkAllSpaces.NavigateUrl = NavigatePageURL(PortalUtils.GetNestedSpacesPageId(),
                PortalUtils.SPACE_ID_PARAM, PanelSecurity.PackageId.ToString());
        }

        private void BindGroupings()
        {
            DataSet dsSpaces = ES.Services.Packages.GetNestedPackagesSummary(PanelSecurity.PackageId);

            // all customers
            lnkAllSpaces.Text = String.Format("All Spaces ({0})", dsSpaces.Tables[0].Rows[0]["PackagesNumber"]);

            // by status
            repSpaceStatuses.DataSource = dsSpaces.Tables[1];
            repSpaceStatuses.DataBind();
        }

        public string GetNestedSpacesPageUrl(string parameterName, string parameterValue)
        {
            return NavigatePageURL(PortalUtils.GetNestedSpacesPageId(),
                PortalUtils.SPACE_ID_PARAM, PanelSecurity.PackageId.ToString(),
                parameterName + "=" + parameterValue);
        }

        protected void cmdSearch_Click(object sender, EventArgs e)
        {
            Response.Redirect(NavigatePageURL(PortalUtils.GetNestedSpacesPageId(),
                PortalUtils.SPACE_ID_PARAM, PanelSecurity.PackageId.ToString(),
                "FilterColumn=" + ddlFilterColumn.SelectedValue,
                "FilterValue=" + Server.UrlEncode(txtFilterValue.Text)));
        }
    }
}
