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
    public partial class UserDeleteUserAccount : FuseCPModuleBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            BindPackages();
        }

        private void BindPackages()
        {
            try
            {
                gvPackages.DataSource = ES.Services.Packages.GetMyPackages(PanelSecurity.SelectedUserId);
                gvPackages.DataBind();
            }
            catch (Exception ex)
            {
                ShowErrorMessage("PACKAGE_GET_PACKAGE", ex);
                return;
            }
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            // load user
            int ownerId = PanelSecurity.SelectedUser.OwnerId;

            // delete user
            if (chkConfirm.Checked)
            {
                try
                {
                    //int result = UsersHelper.DeleteUser(PortalId, PanelRequest.UserID);
                    int result = PortalUtils.DeleteUserAccount(PanelSecurity.SelectedUserId);

                    if (result < 0)
                    {
                        ShowResultMessage(result);
                        return;
                    }
                }
                catch (Exception ex)
                {
                    ShowErrorMessage("USER_DELETE_USER", ex);
                    return;
                }

                RedirectBack(ownerId);
            }
            else
            {
                ShowWarningMessage("USER_CONFIRM_DELETE");
            }
        }
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            RedirectBack(PanelSecurity.SelectedUserId);
        }

        private void RedirectBack(int userId)
        {
            Response.Redirect(NavigateURL(PortalUtils.USER_ID_PARAM, userId.ToString()));
        }
    }
}
