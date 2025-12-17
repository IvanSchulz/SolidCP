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
    public partial class UserDetails : FuseCPControlBase
    {
        private string username;

        public string Username
        {
            get { return this.username; }
            set { this.username = value; }
        }

        public int UserId
        {
            get { return (ViewState["UserId"] != null) ? (int)ViewState["UserId"] : 0; }
            set { ViewState["UserId"] = value; }
        }

        protected override void OnPreRender(EventArgs e)
        {
            lnkUser.Text = GetLocalizedString("Text.NotSelected");
            lnkUser.NavigateUrl = "";

            BindUser();
        }

        private void BindUser()
        {
            if (username == null)
            {
                // try to load user by userId
                UserInfo user = null;
                try
                {
                    user = UsersHelper.GetUser(UserId);
                }
                catch { }

                if (user != null)
                {
					lnkUser.Text = user.Username;
					lnkUser.NavigateUrl = PortalUtils.GetUserHomePageUrl(UserId);
                }
            }

            // load user details
            if (username != null)
            {
                lnkUser.Text = username;
				lnkUser.NavigateUrl = PortalUtils.GetUserHomePageUrl(UserId);
            }
        }
    }
}
