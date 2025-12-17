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
    public partial class UserAccountDetails : FuseCPModuleBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            BindAccount();
        }

        private void BindAccount()
        {
            // load user
            UserInfo user = UsersHelper.GetUser(PanelSecurity.SelectedUserId);
            if (user != null)
            {
                imgAdmin.Visible = (user.Role == UserRole.Administrator);
                imgReseller.Visible = (user.Role == UserRole.Reseller);
                imgUser.Visible = (user.Role == UserRole.User);

                // bind account details
                litUsername.Text = user.Username;
                litFullName.Text = Utils.EllipsisString(PortalAntiXSS.Encode(user.FirstName) + " " + PortalAntiXSS.Encode(user.LastName), 25);
                litSubscriberNumber.Text = PortalAntiXSS.Encode(user.SubscriberNumber);
                litRole.Text = PanelFormatter.GetUserRoleName(user.RoleId);
                litCreated.Text = user.Created.ToString();
                litUpdated.Text = user.Changed.ToString();
				lnkEmail.Text = Utils.EllipsisString(user.Email, 25);
                lnkEmail.NavigateUrl = "mailto:" + user.Email;

                // load owner account
                //UserInfo owner = UsersHelper.GetUser(user.OwnerId);
                //if(owner != null)
                //{
                //    litReseller.Text = owner.Username;
                //}


                // bind account status
                UserStatus status = user.Status;
                litStatus.Text = PanelFormatter.GetAccountStatusName((int)status);

                cmdActive.Visible = (status != UserStatus.Active);
                cmdSuspend.Visible = (status == UserStatus.Active);
                cmdCancel.Visible = (status != UserStatus.Cancelled);

                StatusBlock.Visible = (PanelSecurity.SelectedUserId != PanelSecurity.EffectiveUserId);



                // links
                lnkSummaryLetter.NavigateUrl = EditUrl("UserID", PanelSecurity.SelectedUserId.ToString(), "summary_letter");
                lnkSummaryLetter.Visible = (PanelSecurity.SelectedUser.Role != UserRole.Administrator);

                if (PanelSecurity.LoggedUserId == PanelSecurity.SelectedUserId)
                    lnkEditAccountDetails.NavigateUrl = "/Default.aspx?pid=LoggedUserDetails";
                else
                    lnkEditAccountDetails.NavigateUrl = EditUrl("UserID", PanelSecurity.SelectedUserId.ToString(), "edit_details");

                lnkChangePassword.NavigateUrl = EditUrl("UserID", PanelSecurity.SelectedUserId.ToString(), "change_password");
                lnkChangePassword.Visible = !((PanelSecurity.SelectedUserId == PanelSecurity.EffectiveUserId) && PanelSecurity.LoggedUser.IsPeer);

                lnkDelete.NavigateUrl = EditUrl("UserID", PanelSecurity.SelectedUserId.ToString(), "delete");

                if (!((PanelSecurity.LoggedUser.Role == UserRole.Reseller) | (PanelSecurity.LoggedUser.Role == UserRole.Administrator))) 
                    lnkDelete.Visible = false;
                else 
                    lnkDelete.Visible = (PanelSecurity.SelectedUserId != PanelSecurity.EffectiveUserId);
            }
        }

        protected void statusButton_Click(object sender, ImageClickEventArgs e)
        {
            string sStatus = ((ImageButton)sender).CommandName;
            UserStatus status = (UserStatus)Enum.Parse(typeof(UserStatus), sStatus, true);

            // chanhe user status
            try
            {
                int result = PortalUtils.ChangeUserStatus(PanelSecurity.SelectedUserId, status);

                if (result < 0)
                {
                    ShowResultMessage(result);
                    return;
                }
            }
            catch (Exception ex)
            {
                ShowErrorMessage("USER_CHANGE_STATUS", ex);
                return;
            }

            // re-bind user
            BindAccount();
        }
    }
}
