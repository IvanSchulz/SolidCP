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
using FuseCP.Providers.HostedSolution;
using FuseCP.EnterpriseServer;
using System.Web.UI.WebControls;

namespace FuseCP.Portal.ExchangeServer
{
    public partial class ExchangeJournalingMailboxGeneralSettings : FuseCPModuleBase
    {

        private PackageContext cntx = null;
        private PackageContext Cntx
        {
            get 
            {
                if (cntx == null)
                    cntx = PackagesHelper.GetCachedPackageContext(PanelSecurity.PackageId);

                return cntx;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindJournalingRecipients();

                BindSettings();

                UserInfo user = UsersHelper.GetUser(PanelSecurity.EffectiveUserId);

                if (user != null)
                {
                    
                    if ((user.Role == UserRole.User) & (Utils.CheckQouta(Quotas.EXCHANGE2007_ISCONSUMER, Cntx)))
                    {
                        chkHideAddressBook.Visible = false;
                        chkDisable.Visible = false;
                    }

                }

                if (GetLocalizedString("buttonPanel.OnSaveClientClick") != null)
                    buttonPanel.OnSaveClientClick = GetLocalizedString("buttonPanel.OnSaveClientClick");
            }
        }

        private void BindJournalingRecipients()
        {
            Organization org = ES.Services.Organizations.GetOrganization(PanelRequest.ItemID);

            string path = org.SecurityGroup;
            string[] parts = path.Substring(path.ToUpper().IndexOf("DC=")).Split(',');
            string domain = "";
            for (int i = 0; i < parts.Length; i++)
            {
                domain += parts[i].Substring(3) + (i < parts.Length - 1 ? "." : "");
            }

            ddlRecipient.Items.Clear();
            if (!String.IsNullOrEmpty(domain))
            {
                string mail = org.OrganizationId + "@" + domain;
                ddlRecipient.Items.Add(new ListItem(GetLocalizedString("OrganizationGroup"), mail, true));
            }

            ExchangeAccount[] accounts = ES.Services.ExchangeServer.GetAccounts(PanelRequest.ItemID, ExchangeAccountType.DistributionList);
            foreach (ExchangeAccount account in accounts)
            {
                ddlRecipient.Items.Add(new ListItem(account.DisplayName + " - " + account.PrimaryEmailAddress, account.PrimaryEmailAddress));
            }
            accounts = ES.Services.ExchangeServer.GetAccounts(PanelRequest.ItemID, ExchangeAccountType.Mailbox);
            foreach (ExchangeAccount account in accounts)
            {
                ddlRecipient.Items.Add(new ListItem(account.DisplayName + " - " + account.PrimaryEmailAddress, account.PrimaryEmailAddress));
            }
            accounts = ES.Services.ExchangeServer.GetAccounts(PanelRequest.ItemID, ExchangeAccountType.SharedMailbox);
            foreach (ExchangeAccount account in accounts)
            {
                ddlRecipient.Items.Add(new ListItem(account.DisplayName + " - " + account.PrimaryEmailAddress, account.PrimaryEmailAddress));
            }
        }

        private void BindSettings()
        {
            try
            {
                // get settings
                ExchangeMailbox mailbox = ES.Services.ExchangeServer.GetMailboxGeneralSettings(PanelRequest.ItemID, PanelRequest.AccountID);

                //get statistics
                ExchangeMailboxStatistics stats = ES.Services.ExchangeServer.GetMailboxStatistics(PanelRequest.ItemID, PanelRequest.AccountID);

                // title
                litDisplayName.Text = mailbox.DisplayName;

                // bind form
                chkHideAddressBook.Checked = mailbox.HideFromAddressBook;
                chkDisable.Checked = mailbox.Disabled;

                lblExchangeGuid.Text = string.IsNullOrEmpty(mailbox.ExchangeGuid) ? "<>" : mailbox.ExchangeGuid ;

                // get account meta
                ExchangeAccount account = ES.Services.ExchangeServer.GetAccount(PanelRequest.ItemID, PanelRequest.AccountID);

                // get mailbox plan
                ExchangeMailboxPlan plan = ES.Services.ExchangeServer.GetExchangeMailboxPlan(PanelRequest.ItemID, account.MailboxPlanId);

                ExchangeJournalRule rule = ES.Services.ExchangeServer.GetJournalRule(PanelRequest.ItemID, account.PrimaryEmailAddress);

                cbEnabled.Checked = rule.Enabled;
                ddlScope.SelectedValue = rule.Scope;
                ddlRecipient.SelectedValue = rule.Recipient;

                if (account.MailboxPlanId == 0)
                {
                    mailboxPlanSelector.AddNone = true;
                    mailboxPlanSelector.MailboxPlanId = "-1";
                }
                else
                {
                    mailboxPlanSelector.MailboxPlanId = account.MailboxPlanId.ToString();
                }

                mailboxSize.QuotaUsedValue = Convert.ToInt32(stats.TotalSize / 1024 / 1024);
                mailboxSize.QuotaValue = (stats.MaxSize == -1) ? -1 : (int)Math.Round((double)(stats.MaxSize / 1024 / 1024));

                if (account.LevelId > 0 && Cntx.Groups.ContainsKey(ResourceGroups.ServiceLevels))
                {
                    EnterpriseServer.Base.HostedSolution.ServiceLevel serviceLevel = ES.Services.Organizations.GetSupportServiceLevel(account.LevelId);

                    litServiceLevel.Visible = true;
                    litServiceLevel.Text = serviceLevel.LevelName;
                    litServiceLevel.ToolTip = serviceLevel.LevelDescription;

                }
                imgVipUser.Visible = account.IsVIP && Cntx.Groups.ContainsKey(ResourceGroups.ServiceLevels);
            }
            catch (Exception ex)
            {
                messageBox.ShowErrorMessage("EXCHANGE_GET_JOURNALING_MAILBOX_SETTINGS", ex);
            }
        }

        private bool SaveSettings()
        {
            if (!Page.IsValid)
                return false;

            try
            {
                if (mailboxPlanSelector.MailboxPlanId == "-1")
                {
                    messageBox.ShowErrorMessage("EXCHANGE_SPECIFY_PLAN");
                    return false;
                }

                int result = ES.Services.ExchangeServer.SetMailboxGeneralSettings(
                    PanelRequest.ItemID, PanelRequest.AccountID,
                    chkHideAddressBook.Checked,
                    chkDisable.Checked);

                if (result < 0)
                {
                    messageBox.ShowResultMessage(result);
                    return false;
                }
                else
                {
                    int planId = Convert.ToInt32(mailboxPlanSelector.MailboxPlanId);

                    result = ES.Services.ExchangeServer.SetExchangeMailboxPlan(PanelRequest.ItemID, PanelRequest.AccountID, planId, -1, false);

                    if (result < 0)
                    {
                        messageBox.ShowResultMessage(result);
                        return false;
                    }
                    else
                    {
                        ExchangeAccount account = ES.Services.ExchangeServer.GetAccount(PanelRequest.ItemID, PanelRequest.AccountID);
                        ExchangeJournalRule rule = ES.Services.ExchangeServer.GetJournalRule(PanelRequest.ItemID, account.PrimaryEmailAddress);
                        rule.Enabled = cbEnabled.Checked;
                        rule.Scope = ddlScope.SelectedValue;
                        rule.Recipient = ddlRecipient.SelectedValue;
                        result = ES.Services.ExchangeServer.SetJournalRule(PanelRequest.ItemID, rule);
                        if (result < 0)
                        {
                            messageBox.ShowResultMessage(result);
                            return false;
                        }
                    }
                }

                messageBox.ShowSuccessMessage("EXCHANGE_UPDATE_JOURNALING_MAILBOX_SETTINGS");
                BindSettings();
                return true;
            }
            catch (Exception ex)
            {
                messageBox.ShowErrorMessage("EXCHANGE_UPDATE_JOURNALING_MAILBOX_SETTINGS", ex);
                return false;
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            SaveSettings();
        }

        protected void btnSaveExit_Click(object sender, EventArgs e)
        {
            if (SaveSettings())
            {
                Response.Redirect(PortalUtils.EditUrl("ItemID", PanelRequest.ItemID.ToString(),
                "journaling_mailboxes",
                "SpaceID=" + PanelSecurity.PackageId));
            }
        }

        public void mailboxPlanSelector_Changed(object sender, EventArgs e)
        {
        }

    }
}
