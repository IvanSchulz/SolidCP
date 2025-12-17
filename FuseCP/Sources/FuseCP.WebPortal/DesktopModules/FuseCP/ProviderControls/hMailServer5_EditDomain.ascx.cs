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
	public partial class hMailServer5_EditDomain : FuseCPControlBase, IMailEditDomainControl
	{
		protected void Page_Load(object sender, EventArgs e)
		{

		}

		public void BindItem(MailDomain item)
		{
			BindMailboxes(item);
		}

		public void SaveItem(MailDomain item)
		{
			item.CatchAllAccount = ddlCatchAllAccount.SelectedValue;
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
	}
}
