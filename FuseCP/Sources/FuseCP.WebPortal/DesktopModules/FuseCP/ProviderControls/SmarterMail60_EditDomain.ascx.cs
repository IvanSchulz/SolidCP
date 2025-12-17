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
using System.Web.UI.WebControls;
using FuseCP.EnterpriseServer;
using FuseCP.Providers.Mail;

namespace FuseCP.Portal.ProviderControls
{
    public partial class SmarterMail60_EditDomain : FuseCPControlBase, IMailEditDomainControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            PackageInfo info = ES.Services.Packages.GetPackage(PanelSecurity.PackageId);

            AdvancedSettingsPanel.Visible = PanelSecurity.EffectiveUser.Role == UserRole.Administrator;
            InitValidators();
        }

        public void BindItem(MailDomain item)
        {
            BindMailboxes(item);
            BindQuotas(item);
            
            featuresSection.BindItem(item);
            sharingSection.BindItem(item);
            throttlingSection.BindItem(item);

            
            if (item[MailDomain.SMARTERMAIL_LICENSE_TYPE] == "PRO")
            {
                secSharing.Visible = false;
                sharingSection.Visible = false;
                secThrottling.Visible = false;
                throttlingSection.Visible = false;
            }
            else
            {
                sharingSection.BindItem(item);
                throttlingSection.BindItem(item);
            }
            
        }

        public void SaveItem(MailDomain item)
        {
            item.CatchAllAccount = ddlCatchAllAccount.SelectedValue;
            SaveQuotas(item);

            featuresSection.SaveItem(item);
            sharingSection.SaveItem(item);
            throttlingSection.SaveItem(item);

            
            if (item[MailDomain.SMARTERMAIL_LICENSE_TYPE] == "PRO")
            {
                secSharing.Visible = false;
                sharingSection.Visible = false;
                secThrottling.Visible = false;
                throttlingSection.Visible = false;
            }
            else
            {
                sharingSection.SaveItem(item);
                throttlingSection.SaveItem(item);
            }
        }

        private void SaveQuotas(MailDomain item)
        {
            item.MaxDomainSizeInMB = Utils.ParseInt(txtSize.Text);
            item.MaxDomainAliases = Utils.ParseInt(txtDomainAliases.Text);
            item.MaxDomainUsers = Utils.ParseInt(txtUser.Text);
            item.MaxAliases = Utils.ParseInt(txtUserAliases.Text);
            item.MaxLists = Utils.ParseInt(txtMailingLists.Text);
            item[MailDomain.SMARTERMAIL5_POP_RETREIVAL_ACCOUNTS] = txtPopRetreivalAccounts.Text;
            item.MaxRecipients = Utils.ParseInt(txtRecipientsPerMessage.Text);
            item.MaxMessageSize = Utils.ParseInt(txtMessageSize.Text);
        }

        private void BindQuotas(MailDomain item)
        {
            txtSize.Text = item.MaxDomainSizeInMB.ToString();
            txtDomainAliases.Text = item.MaxDomainAliases.ToString();
            txtUser.Text = item.MaxDomainUsers.ToString();
            txtUserAliases.Text = item.MaxAliases.ToString();
            txtMailingLists.Text = item.MaxLists.ToString();
            txtPopRetreivalAccounts.Text = item[MailDomain.SMARTERMAIL5_POP_RETREIVAL_ACCOUNTS];
            txtRecipientsPerMessage.Text = item.MaxRecipients.ToString();
            txtMessageSize.Text = item.MaxMessageSize.ToString();
        }

        private void BindMailboxes(MailDomain item)
        {
            MailAccount[] accounts = ES.Services.MailServers.GetMailAccounts(item.PackageId, false);
            MailAlias[] forwardings = ES.Services.MailServers.GetMailForwardings(item.PackageId, false);

            BindAccounts(item, ddlCatchAllAccount, accounts);
            BindAccounts(item, ddlCatchAllAccount, forwardings);
            Utils.SelectListItem(ddlCatchAllAccount, item.CatchAllAccount);

        }

        private void BindAccounts(MailDomain item, DropDownList ddl, MailAccount[] accounts)
        {
            if (ddl.Items.Count == 0)
                ddl.Items.Add(new ListItem(GetLocalizedString("Text.NotSelected"), ""));

            foreach (MailAccount account in accounts)
            {
                int idx = account.Name.IndexOf("@");
                string accountName = account.Name.Substring(0, idx);
                string accountDomain = account.Name.Substring(idx + 1);

                if (String.Compare(accountDomain, item.Name, true) == 0)
                    ddl.Items.Add(new ListItem(account.Name, accountName));
            }
        }

        private void InitValidators()
        {
            string message = "*";
            reqValRecipientsPerMessage.ErrorMessage = message;
            valRecipientsPerMessage.ErrorMessage = message;
            valRecipientsPerMessage.MaximumValue = int.MaxValue.ToString();

            reqValMessageSize.ErrorMessage = message;
            valMessageSize.ErrorMessage = message;
            valMessageSize.MaximumValue = int.MaxValue.ToString();

            reqValMailingLists.ErrorMessage = message;
            valMailingLists.ErrorMessage = message;
            valMailingLists.MaximumValue = int.MaxValue.ToString();

            reqPopRetreivalAccounts.ErrorMessage = message;
            valPopRetreivalAccounts.ErrorMessage = message;
            valPopRetreivalAccounts.MaximumValue = int.MaxValue.ToString();

            reqValUser.ErrorMessage = message;
            valUser.ErrorMessage = message;
            valUser.MaximumValue = int.MaxValue.ToString();

            reqValUserAliases.ErrorMessage = message;
            valUserAliases.ErrorMessage = message;
            valUserAliases.MaximumValue = int.MaxValue.ToString();

            reqValDomainAliases.ErrorMessage = message;
            valDomainAliases.ErrorMessage = message;
            valDomainAliases.MaximumValue = int.MaxValue.ToString();

            reqValDiskSpace.ErrorMessage = message;
            valDomainDiskSpace.ErrorMessage = message;
            valDomainDiskSpace.MaximumValue = int.MaxValue.ToString();

        }
    }
}
