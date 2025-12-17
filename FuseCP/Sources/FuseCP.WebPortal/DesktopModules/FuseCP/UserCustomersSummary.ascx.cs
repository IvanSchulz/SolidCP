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
using System.Web.UI;
using FuseCP.EnterpriseServer;

namespace FuseCP.Portal
{
    public partial class UserCustomersSummary : FuseCPModuleBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // bind groups
            BindGroupings();

            // other controls
            this.ContainerControl.Visible = (PanelSecurity.SelectedUser.Role != UserRole.User);
            lnkAllCustomers.NavigateUrl = NavigatePageURL(PortalUtils.GetUserCustomersPageId(),
                PortalUtils.USER_ID_PARAM, PanelSecurity.SelectedUserId.ToString());

            if (!IsPostBack)
            {
                /*searchBox.AddCriteria("Username", GetLocalizedString("SearchField.Username"));
                searchBox.AddCriteria("FullName", GetLocalizedString("SearchField.Name"));
                searchBox.AddCriteria("Email", GetLocalizedString("SearchField.EMail"));
                searchBox.AddCriteria("CompanyName", GetLocalizedString("SearchField.CompanyName"));
                searchBox.Focus();*/
            }
        }

        private void BindGroupings()
        {
            DataSet dsUsers = ES.Services.Users.GetUsersSummary(PanelSecurity.SelectedUserId);

            // all customers
			lnkAllCustomers.Text = PortalAntiXSS.Encode(String.Format(GetLocalizedString("AllCustomers.Text"),
				dsUsers.Tables[0].Rows[0]["UsersNumber"]));

            // by status
            repUserStatuses.DataSource = dsUsers.Tables[1];
            repUserStatuses.DataBind();

            // by role
            repUserRoles.DataSource = dsUsers.Tables[2];
            repUserRoles.DataBind();
        }

        public string GetUserCustomersPageUrl(string parameterName, string parameterValue)
        {
            return NavigatePageURL(PortalUtils.GetUserCustomersPageId(),
                PortalUtils.USER_ID_PARAM, PanelSecurity.SelectedUserId.ToString(),
                parameterName + "=" + parameterValue);
        }

        protected void btnCreate_Click(object sender, EventArgs e)
        {
            Response.Redirect(EditUrl(PortalUtils.USER_ID_PARAM, PanelSecurity.SelectedUserId.ToString(), "create_user"));
        }
    }
}
