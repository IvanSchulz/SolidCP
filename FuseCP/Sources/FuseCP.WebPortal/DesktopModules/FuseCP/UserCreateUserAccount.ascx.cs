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

namespace FuseCP.Portal
{
    public partial class UserCreateUserAccount : FuseCPModuleBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // bind roles
                BindForm();
                BindRoles(PanelSecurity.SelectedUser);
            }
        }

        private void BindForm()
        {
            UserSettings settings = ES.Services.Users.GetUserSettings(PanelSecurity.LoggedUserId,
                UserSettings.ACCOUNT_SUMMARY_LETTER);
            bool accountSummaryEmailEnabled = !String.IsNullOrEmpty(settings["EnableLetter"]) &&
                                              Utils.ParseBool(settings["EnableLetter"], false);
            this.chkAccountLetter.Enabled = accountSummaryEmailEnabled;
            this.pnlDisabledSummaryLetterHint.Visible = !accountSummaryEmailEnabled;
            if (PortalUtils.GetHideDemoCheckbox())
                this.lblDemoAccount.Visible = this.chkDemo.Checked = this.chkDemo.Visible = false;

            //reseller.UserId = PanelSecurity.SelectedUserId;
            userPassword.SetUserPolicy(PanelSecurity.SelectedUserId, UserSettings.FuseCP_POLICY, "PasswordPolicy");
        }

        private void BindRoles(UserInfo user)
        {
            if (user.Role == UserRole.User)
                role.Items.Remove("Reseller");

            if ((PanelSecurity.LoggedUser.Role == UserRole.ResellerCSR) |
                (PanelSecurity.LoggedUser.Role == UserRole.ResellerHelpdesk))
                role.Items.Remove("Reseller");
        }

        private void SaveUser()
        {
            if (!Page.IsValid)
                return;

            // gather data from form
            UserInfo user = new UserInfo();
            user.UserId = 0;
            user.Role = (UserRole) Enum.Parse(typeof (UserRole), role.SelectedValue);
            user.StatusId = Int32.Parse(status.SelectedValue);
            user.OwnerId = PanelSecurity.SelectedUserId;
            user.IsDemo = chkDemo.Checked;
            user.IsPeer = false;

            // account info
            user.FirstName = txtFirstName.Text;
            user.LastName = txtLastName.Text;
            user.SubscriberNumber = txtSubscriberNumber.Text;
            user.Email = txtEmail.Text;
            user.SecondaryEmail = txtSecondaryEmail.Text;
            user.HtmlMail = ddlMailFormat.SelectedIndex == 1;
            user.Username = txtUsername.Text.Trim();
//            user.Password = userPassword.Password;

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

            // add a new user
            List<string> log = new List<string>();
            try
            {
                //int userId = UsersHelper.AddUser(log, PortalId, user);
                int userId = PortalUtils.AddUserAccount(log, user, chkAccountLetter.Checked, userPassword.Password);

                if (userId == BusinessErrorCodes.ERROR_INVALID_USER_NAME)
                {
                    ShowResultMessage(BusinessErrorCodes.ERROR_INVALID_USER_NAME);
                    return;
                }

                if (userId < 0)
                {
                    ShowResultMessage(userId);
                    return;
                }

                // show log records if any
                if (log.Count > 0)
                {
                    blLog.Items.Clear();
                    foreach (string error in log)
                        blLog.Items.Add(error);

                    return;
                }

                // go to user home
                Response.Redirect(PortalUtils.GetUserHomePageUrl(userId));
            }
            catch (Exception ex)
            {
                ShowErrorMessage("USER_ADD_USER", ex);
                return;
            }
        }

        protected void btnCreate_Click(object sender, EventArgs e)
        {
            SaveUser();
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect(NavigateURL(PortalUtils.USER_ID_PARAM, PanelSecurity.SelectedUserId.ToString()));
        }
    }
}
