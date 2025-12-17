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
    public partial class VdcAccountVLanAdd : FuseCPModuleBase
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
            try
            {
                UserInfo user = UsersHelper.GetUser(PanelSecurity.SelectedUserId);

                if (user != null)
                {
                    // account info
                    lblUsername.Text = user.Username;
                }
            }
            catch (Exception ex)
            {
                ShowErrorMessage("USER_GET_USER", ex);
                return;
            }
        }

        protected void btAddVLan_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid)
                return;

            UsersHelper.AddUserVLan(PanelSecurity.SelectedUserId, new UserVlan { VLanID = ushort.Parse(tbVLanID.Text), Comment = tbComment.Text });

            RedirectBack();
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            RedirectBack();
        }

        private void RedirectBack()
        {
            Response.Redirect(EditUrl("SpaceID", PanelSecurity.PackageId.ToString(), "vdc_account_vlan_network"));
        }
    }
}
