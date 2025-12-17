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
using System.Web.Security;
using FuseCP.EnterpriseServer;
using FuseCP.Providers.ResultObjects;
using FuseCP.Providers.HostedSolution;


namespace FuseCP.Portal.HostedSolution
{
    public partial class OrganizationCreateUser : FuseCPModuleBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindPasswordSettings();

                string instructions = ES.Services.Organizations.GetOrganizationUserSummuryLetter(PanelRequest.ItemID, PanelRequest.AccountID, false, false, false);
                if (!string.IsNullOrEmpty(instructions))
                {
                    chkSendInstructions.Checked = chkSendInstructions.Visible = sendInstructionEmail.Visible = true;
                    PackageInfo package = ES.Services.Packages.GetPackage(PanelSecurity.PackageId);
                    if (package != null)
                    {
                        UserInfo user = ES.Services.Users.GetUserById(package.UserId);
                        if (user != null)
                            sendInstructionEmail.Text = user.Email;
                    }
                }
                else
                {
                    chkSendInstructions.Checked = chkSendInstructions.Visible = sendInstructionEmail.Visible = false;
                }
            }


            PackageContext cntx = PackagesHelper.GetCachedPackageContext(PanelSecurity.PackageId);
            if (cntx.Quotas.ContainsKey(Quotas.EXCHANGE2007_ISCONSUMER))
            {
                if (cntx.Quotas[Quotas.EXCHANGE2007_ISCONSUMER].QuotaAllocatedValue != 1)
                {
                    locSubscriberNumber.Visible = txtSubscriberNumber.Visible = valRequireSubscriberNumber.Enabled = false;
                }
            }

        }

        private void BindPasswordSettings()
        {
            var grainedPasswordSettigns = ES.Services.Organizations.GetOrganizationPasswordSettings(PanelRequest.ItemID);

            if (grainedPasswordSettigns != null)
            {
                password.SetUserPolicy(grainedPasswordSettigns);
            }
            else
            {
                messageBox.ShowErrorMessage("UNABLETOLOADPASSWORDSETTINGS");
            }
        }

        protected void btnCreate_Click(object sender, EventArgs e)
        {
            CreateMailbox();
        }

        private void CreateMailbox()
        {
            if (!Page.IsValid)
                return;

            try
            {
                var passwordString = password.Password;

                if (sendToControl.IsRequestSend)
                {
                    passwordString = Membership.GeneratePassword(16, 3);
                }

                int accountId = ES.Services.Organizations.CreateUser(PanelRequest.ItemID, txtDisplayName.Text.Trim(),
                    email.AccountName.ToLower(),
                    email.DomainName.ToLower(),
                    passwordString,
                    txtSubscriberNumber.Text.Trim(),
                    chkSendInstructions.Checked,
                    sendInstructionEmail.Text);

                if (accountId < 0)
                {
                    messageBox.ShowResultMessage(accountId);
                    return;
                }
                else
                {
                    if ((!string.IsNullOrEmpty(txtFirstName.Text)) | (!string.IsNullOrEmpty(txtLastName.Text)) | (!string.IsNullOrEmpty(txtInitials.Text)))
                    {
                        SetUserAttributes(accountId);
                    }
                }

                if (sendToControl.SendEmail)
                {
                    ES.Services.Organizations.SendUserPasswordRequestEmail(PanelRequest.ItemID, accountId, "User creation", sendToControl.Email, true);
                }
                else if (sendToControl.SendMobile)
                {
                    ES.Services.Organizations.SendUserPasswordRequestSms(PanelRequest.ItemID, accountId, "User creation", sendToControl.Mobile);
                }

                Response.Redirect(EditUrl("AccountID", accountId.ToString(), "edit_user",
                    "SpaceID=" + PanelSecurity.PackageId,
                    "ItemID=" + PanelRequest.ItemID,
                    "Context=User"));
            }
            catch (Exception ex)
            {
                messageBox.ShowErrorMessage("ORGANIZATION_CREATE_USER", ex);
            }
        }

        private void SetUserAttributes(int accountId)
        {
            OrganizationUser user = ES.Services.Organizations.GetUserGeneralSettings(PanelRequest.ItemID, accountId);

            ES.Services.Organizations.SetUserGeneralSettings(
                    PanelRequest.ItemID, accountId,
                    txtDisplayName.Text,
                    null,
                    false,
                    user.Disabled,
                    user.Locked,

                    txtFirstName.Text,
                    txtInitials.Text,
                    txtLastName.Text,

                    null,
                    null,
                    null,
                    null,
                    null,

                    null,
                    null,
                    null,
                    null,
                    null,
                    null,
                    null,
                    null,
                    null,
                    null,
                    null,
                    null,
                    user.ExternalEmail,
                    txtSubscriberNumber.Text,
                    0,
                    false,
                    chkUserMustChangePassword.Checked);
        }
    }
}
