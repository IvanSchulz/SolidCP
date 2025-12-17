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
using FuseCP.WebPortal;

namespace FuseCP.Portal.SkinControls
{
    public partial class SignedInUser : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindUser();
            }
        }

        private void BindUser()
        {
			UserInfo user = PanelSecurity.LoggedUser;
            if (user != null)
            {
               // lnkEditUserDetails.Text = PanelSecurity.LoggedUser.Username;
                lnkEditUserDetails.NavigateUrl = PortalUtils.GetLoggedUserAccountPageUrl();
                lnkEditUserDetailsSm.NavigateUrl = PortalUtils.GetLoggedUserAccountPageUrl();
                lnkEditUserDetails.Text = user.Username;
            }

			AnonymousPanel.Visible = !Request.IsAuthenticated;
			LoggedPanel.Visible = Request.IsAuthenticated;

			lnkSignIn.NavigateUrl = PortalUtils.LoginRedirectUrl;

            string imagePath = String.Concat("~/", DefaultPage.THEMES_FOLDER, "/", Page.Theme, "/", "Images", "/");

            //imgSignOut.ImageUrl = imagePath + "signout_24.png";
        }

        protected void cmdSignOut_Click(object sender, EventArgs e)
        {
            PortalUtils.UserSignOut();
        }
    }
}
