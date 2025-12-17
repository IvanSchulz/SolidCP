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
using FuseCP.EnterpriseServer;

namespace FuseCP.Portal
{
    public partial class UserAccountEditDetails : FuseCPModuleBase
    {
        private const string UserStatusConst = "UserStatus";
        
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindUser();

                if (PortalUtils.GetHideDemoCheckbox()) rowDemo.Visible = false;
            }

            if (PanelSecurity.LoggedUser.Role == UserRole.User)
            {
                txtSubscriberNumber.ReadOnly = true;
            }


        }

        private void BindUser()
        {
            try
            {
                UserInfo user = UsersHelper.GetUser(PanelSecurity.SelectedUserId);
                if (user != null)
                {
                    // bind roles
                    BindRoles(PanelSecurity.EffectiveUserId);

                    bool editAdminAccount = (user.UserId == PanelSecurity.EffectiveUserId);

                    if(!editAdminAccount)
                        role.Items.Remove("Administrator");

                    // select role
                    Utils.SelectListItem(role, user.Role.ToString());

                    // select loginStatus
                    loginStatus.SelectedIndex = user.LoginStatusId;

                    rowRole.Visible = !editAdminAccount;

                    // select status
                    chkDemo.Checked = user.IsDemo;
                    rowDemo.Visible = !editAdminAccount;

                    // account info
                    txtFirstName.Text = PortalAntiXSS.DecodeOld(user.FirstName);
                    txtLastName.Text = PortalAntiXSS.DecodeOld(user.LastName);
                    txtSubscriberNumber.Text = PortalAntiXSS.DecodeOld(user.SubscriberNumber);
                    txtEmail.Text = user.Email;
                    txtSecondaryEmail.Text = user.SecondaryEmail;
                    ddlMailFormat.SelectedIndex = user.HtmlMail ? 1 : 0;
                    lblUsername.Text = user.Username;
                    cbxMfaEnabled.Checked = user.MfaMode > 0 ? true: false;
                    cbxMfaEnabled.Enabled = ES.Services.Users.CanUserChangeMfa(PanelSecurity.SelectedUserId);
                    lblMfaEnabled.Visible = cbxMfaEnabled.Checked;

                    // contact info
                    contact.CompanyName = user.CompanyName;
                    contact.Address = user.Address;
                    contact.City = user.City;
                    contact.Country = user.Country;
                    contact.State = user.State;
                    contact.Zip = user.Zip;
                    contact.PrimaryPhone = user.PrimaryPhone;
                    contact.SecondaryPhone = user.SecondaryPhone;
                    contact.Fax = user.Fax;
                    contact.MessengerId = user.InstantMessenger;

                    ViewState[UserStatusConst] = user.Status;
                }
                else
                {
                    // can't be found
                    RedirectAccountHomePage();
                }

            }
            catch (Exception ex)
            {
                ShowErrorMessage("USER_GET_USER", ex);
                return;
            }
        }

        private void SaveUser()
        {
            if (Page.IsValid)
            {
                // gather data from form
                UserInfo user = new UserInfo();
                user.UserId = PanelSecurity.SelectedUserId;
                user.Role = (UserRole)Enum.Parse(typeof(UserRole), role.SelectedValue);
                user.IsDemo = chkDemo.Checked;
                user.Status = ViewState[UserStatusConst] != null ? (UserStatus) ViewState[UserStatusConst]: UserStatus.Active;

                user.LoginStatusId = loginStatus.SelectedIndex;
                
                // account info
                user.FirstName = txtFirstName.Text;
                user.LastName = txtLastName.Text;
                user.SubscriberNumber = txtSubscriberNumber.Text;
                user.Email = txtEmail.Text;
                user.SecondaryEmail = txtSecondaryEmail.Text;
                user.HtmlMail = ddlMailFormat.SelectedIndex == 1;

                // contact info
				user.CompanyName = contact.CompanyName;
                user.Address = contact.Address;
                user.City = contact.City;
                user.Country = contact.Country;
                user.State = contact.State;
                user.Zip = contact.Zip;
                user.PrimaryPhone = contact.PrimaryPhone;
                user.SecondaryPhone = contact.SecondaryPhone;
                user.Fax = contact.Fax;
                user.InstantMessenger = contact.MessengerId;

                // update existing user
                try
                {
                    int result = PortalUtils.UpdateUserAccount(/*TaskID, */user);

                    //int result = ES.Services.Users.UpdateUserTaskAsynchronously(TaskID, user);
                    AsyncTaskID = TaskID;

                    if (result.Equals(-102))
                    {
                        if (user.RoleId.Equals(3))
                        {
                            ShowResultMessage(result);
                            return;
                        }
                    }
                    else
                    {
                        if (result < 0)
                        {
                            ShowResultMessage(result);
                            return;
                        }
                    }
                }
                catch (Exception ex)
                {
                    ShowErrorMessage("USER_UPDATE_USER", ex);
                    return;
                }

                // return back to the list
                RedirectAccountHomePage();
            }
        }

        private void BindRoles(int userId)
        {
            // load selected user
            UserInfo user = UsersHelper.GetUser(userId);

            if (user != null)
            {
                if (user.Role == UserRole.Reseller || user.Role == UserRole.User)
                    role.Items.Remove("Administrator");
                if ((user.Role == UserRole.User) |(PanelSecurity.LoggedUser.Role == UserRole.ResellerCSR) |
                    (PanelSecurity.LoggedUser.Role == UserRole.ResellerHelpdesk))
                    role.Items.Remove("Reseller");
            }
        }

        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            SaveUser();
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            RedirectAccountHomePage();
        }

        protected void cbxMfaEnabled_CheckedChanged(object sender, EventArgs e)
        {
            UserInfo user = ES.Services.Users.GetUserById(PanelSecurity.SelectedUserId);
            bool result = PortalUtils.UpdateUserMfa(user.Username, cbxMfaEnabled.Checked);
            lblMfaEnabled.Visible = result;
            cbxMfaEnabled.Checked = result;
        }
    }
}
