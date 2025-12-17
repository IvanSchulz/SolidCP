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
using FuseCP.Providers.HostedSolution;
using FuseCP.Providers.ResultObjects;

namespace FuseCP.Portal.ExchangeServer
{
    public partial class ExchangeCreateMailbox : FuseCPModuleBase
    {
        private bool IsNewUser
        {
            get
            {
                return NewUserDiv.Visible;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            PackageContext cntx = PackagesHelper.GetCachedPackageContext(PanelSecurity.PackageId);

            if (!IsPostBack)
            {
                BindPasswordSettings();

                string instructions = ES.Services.ExchangeServer.GetMailboxSetupInstructions(PanelRequest.ItemID, PanelRequest.AccountID, false, false, false, " ");
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



                FuseCP.Providers.HostedSolution.ExchangeMailboxPlan[] plans = ES.Services.ExchangeServer.GetExchangeMailboxPlans(PanelRequest.ItemID, false);

                if (plans.Length == 0)
                    btnCreate.Enabled = false;

                bool allowResourceMailbox = false;

                if (cntx.Quotas.ContainsKey(Quotas.EXCHANGE2007_ISCONSUMER))
                {
                    if (cntx.Quotas[Quotas.EXCHANGE2007_ISCONSUMER].QuotaAllocatedValue != 1)
                    {
                        locSubscriberNumber.Visible = txtSubscriberNumber.Visible = valRequireSubscriberNumber.Enabled = false;
                        allowResourceMailbox = true;
                    }
                }

                if (cntx.Quotas.ContainsKey(Quotas.EXCHANGE2013_RESOURCEMAILBOXES))
                {
                    if (cntx.Quotas[Quotas.EXCHANGE2013_RESOURCEMAILBOXES].QuotaAllocatedValue != 0)
                        allowResourceMailbox = true;
                }


                if (allowResourceMailbox)
                {
                    rbMailboxType.Items.Add(new System.Web.UI.WebControls.ListItem(GetLocalizedString("RoomMailbox.Text"), "5"));
                    rbMailboxType.Items.Add(new System.Web.UI.WebControls.ListItem(GetLocalizedString("EquipmentMailbox.Text"), "6"));
                }

                if (cntx.Quotas.ContainsKey(Quotas.EXCHANGE2013_SHAREDMAILBOXES))
                {
                    if (cntx.Quotas[Quotas.EXCHANGE2013_SHAREDMAILBOXES].QuotaAllocatedValue != 0)
                        rbMailboxType.Items.Add(new System.Web.UI.WebControls.ListItem(GetLocalizedString("SharedMailbox.Text"), "10"));
                }


                divRetentionPolicy.Visible = Utils.CheckQouta(Quotas.EXCHANGE2013_ALLOWRETENTIONPOLICY, cntx);
            }

            divArchiving.Visible = false;

            int planId = -1;
            int.TryParse(mailboxPlanSelector.MailboxPlanId, out planId);
            ExchangeMailboxPlan plan = ES.Services.ExchangeServer.GetExchangeMailboxPlan(PanelRequest.ItemID, planId);
            if (plan!=null)
                divArchiving.Visible = plan.EnableArchiving;

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
                string name = PortalAntiXSS.CheckExchangeRecipientName(IsNewUser ? email.AccountName : userSelector.GetPrimaryEmailAddress().Split('@')[0]);
                string displayName = PortalAntiXSS.CheckExchangeRecipientName(IsNewUser ? txtDisplayName.Text.Trim() : userSelector.GetDisplayName());
                string accountName = PortalAntiXSS.CheckExchangeRecipientName(IsNewUser ? string.Empty : userSelector.GetAccount());

                bool enableArchive = chkEnableArchiving.Checked;

                ExchangeAccountType type = IsNewUser
                                               ? (ExchangeAccountType)Utils.ParseInt(rbMailboxType.SelectedValue, 1)
                                               : ExchangeAccountType.Mailbox;

                string domain = IsNewUser ? email.DomainName : userSelector.GetPrimaryEmailAddress().Split('@')[1];

                int accountId = IsNewUser ? 0 : userSelector.GetAccountId();

                string subscriberNumber = IsNewUser ? txtSubscriberNumber.Text.Trim() : userSelector.GetSubscriberNumber();

                var passwordString = password.Password;

                if (sendToControl.IsRequestSend && IsNewUser)
                {
                    passwordString = Membership.GeneratePassword(16, 3);
                }

                accountId = ES.Services.ExchangeServer.CreateMailbox(PanelRequest.ItemID, accountId, type,
                                    accountName,
                                    displayName,
                                    name,
                                    domain,
                                    passwordString,
                                    chkSendInstructions.Checked,
                                    sendInstructionEmail.Text,
                                    Convert.ToInt32(mailboxPlanSelector.MailboxPlanId),
                                    Convert.ToInt32(archivingMailboxPlanSelector.MailboxPlanId),
                                    subscriberNumber, enableArchive);


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

                if (sendToControl.SendEmail && IsNewUser)
                {
                    ES.Services.Organizations.SendUserPasswordRequestEmail(PanelRequest.ItemID, accountId, "User creation", sendToControl.Email, true);
                }
                else if (sendToControl.SendMobile && IsNewUser)
                {
                    ES.Services.Organizations.SendUserPasswordRequestSms(PanelRequest.ItemID, accountId, "User creation", sendToControl.Mobile);
                }

                Response.Redirect(EditUrl("AccountID", accountId.ToString(), "mailbox_settings",
                    "SpaceID=" + PanelSecurity.PackageId.ToString(),
                    "ItemID=" + PanelRequest.ItemID.ToString(),
                    "Context=Mailbox"));
            }
            catch (Exception ex)
            {
                messageBox.ShowErrorMessage("EXCHANGE_CREATE_MAILBOX", ex);
            }
        }

        private void SetUserAttributes(int accountId)
        {
            OrganizationUser user = ES.Services.Organizations.GetUserGeneralSettings(PanelRequest.ItemID, accountId);

            txtDisplayName.Text = PortalAntiXSS.CheckExchangeRecipientName(txtDisplayName.Text.Trim());
            txtFirstName.Text = PortalAntiXSS.CheckExchangeRecipientName(txtFirstName.Text.Trim());
            txtInitials.Text = PortalAntiXSS.CheckExchangeRecipientName(txtInitials.Text.Trim());
            txtLastName.Text = PortalAntiXSS.CheckExchangeRecipientName(txtLastName.Text.Trim());

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



        protected void rbtnUserExistingUser_CheckedChanged(object sender, EventArgs e)
        {
            ExistingUserDiv.Visible = true;
            NewUserDiv.Visible = false;
        }

        protected void rbtnCreateNewMailbox_CheckedChanged(object sender, EventArgs e)
        {
            NewUserDiv.Visible = true;
            ExistingUserDiv.Visible = false;

        }

        protected void mailboxPlanSelector_Change(object sender, EventArgs e)
        {
        }

    }
}
