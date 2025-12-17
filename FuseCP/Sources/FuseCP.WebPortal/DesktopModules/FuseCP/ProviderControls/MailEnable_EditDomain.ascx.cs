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

using FuseCP.Providers.Mail;


namespace FuseCP.Portal.ProviderControls
{
    public partial class MailEnable_EditDomain : FuseCPControlBase, IMailEditDomainControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        public void BindItem(MailDomain item)
        {
            BindMailboxes(item);

            chkDomainSmartHostEnabled.Checked = Convert.ToBoolean(item["MailEnable_SmartHostEnabled"]);
            chkDomainSmartHostAuthSenders.Checked = Convert.ToBoolean(item["MailEnable_SmartHostAuth"]);
            txtDestination.Text = item.RedirectionHosts;
        }

        public void SaveItem(MailDomain item)
        {
            item.AbuseAccount = ddlAbuseAccount.SelectedValue;
            item.PostmasterAccount = ddlPostmasterAccount.SelectedValue;

            // if we have a smarthost we need to clear the catchall
            if (chkDomainSmartHostEnabled.Checked)
                item.CatchAllAccount= "";
            else
                item.CatchAllAccount = ddlCatchAllAccount.SelectedValue;

            item["MailEnable_SmartHostEnabled"] = chkDomainSmartHostEnabled.Checked.ToString();
            item["MailEnable_SmartHostAuth"] = chkDomainSmartHostAuthSenders.Checked.ToString(); 
            item.RedirectionHosts = txtDestination.Text;
        }

        private void BindMailboxes(MailDomain item)
        {
            MailAccount[] accounts = ES.Services.MailServers.GetMailAccounts(item.PackageId, false);
            MailAlias[] forwardings = ES.Services.MailServers.GetMailForwardings(item.PackageId, false);

            BindAccounts(item, ddlAbuseAccount, accounts);
            BindAccounts(item, ddlAbuseAccount, forwardings);
            Utils.SelectListItem(ddlAbuseAccount, item.AbuseAccount);

            BindAccounts(item, ddlCatchAllAccount, accounts);
            BindAccounts(item, ddlCatchAllAccount, forwardings);
            Utils.SelectListItem(ddlCatchAllAccount, item.CatchAllAccount);

            BindAccounts(item, ddlPostmasterAccount, accounts);
            BindAccounts(item, ddlPostmasterAccount, forwardings);
            Utils.SelectListItem(ddlPostmasterAccount, item.PostmasterAccount);
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
    }
}
