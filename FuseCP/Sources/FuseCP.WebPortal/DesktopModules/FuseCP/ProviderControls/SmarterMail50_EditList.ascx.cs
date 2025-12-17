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
using FuseCP.Providers.Mail;
using FuseCP.WebPortal.Code.Controls;


namespace FuseCP.Portal.ProviderControls
{
	public partial class SmarterMail50_EditList : FuseCPControlBase, IMailEditListControl
	{
		private string selectedModerator = null;
		private string itemName = null;
		private MailEditAddress ctrl = null;
		
		protected void Page_Load(object sender, EventArgs e)
		{
			txtPassword.Attributes["value"] =txtPassword.Text;
			BindListModerators();
		}

		public void BindItem(MailList item)
		{
			itemName = item.Name;
			txtDescription.Text = item.Description;
			if (String.IsNullOrEmpty(item.ModeratorAddress))
			{
				Utils.SelectListItem(ddlListModerators, GetLocalizedString("Text.SelectModerator"));
				selectedModerator = GetLocalizedString("Text.SelectModerator");
			}
			else
			{
				Utils.SelectListItem(ddlListModerators, item.ModeratorAddress);
				selectedModerator = item.ModeratorAddress;
			}

			chkReplyToList.Checked = (item.ReplyToMode == ReplyTo.RepliesToList);
			Utils.SelectListItem(ddlPostingMode, item.PostingMode);
			txtPassword.Text = item.Password;
			chkPasswordEnabled.Checked = item.RequirePassword;
			txtSubjectPrefix.Text = item.SubjectPrefix;
			chkSubjectPrefixEnabled.Checked = item.EnableSubjectPrefix;
			txtMaxMessageSize.Text = item.MaxMessageSize.ToString();
			txtMaxRecipients.Text = item.MaxRecipientsPerMessage.ToString();

			// members
			mailEditItems.Items = item.Members;
		}

		public void SaveItem(MailList item)
		{
			item.Description = txtDescription.Text;
			if (ddlListModerators.SelectedValue == GetLocalizedString("Text.SelectModerator"))
			{
				item.ModeratorAddress = null;
			}
			else
			{
				item.ModeratorAddress = ddlListModerators.SelectedValue;
			}

			item.ReplyToMode = chkReplyToList.Checked ? ReplyTo.RepliesToList : ReplyTo.RepliesToSender;
			item.PostingMode = (PostingMode)Enum.Parse(typeof(PostingMode), ddlPostingMode.SelectedValue, true);
			item.Password = txtPassword.Text;
			item.RequirePassword = chkPasswordEnabled.Checked;
			item.SubjectPrefix = txtSubjectPrefix.Text;
			item.EnableSubjectPrefix = chkSubjectPrefixEnabled.Checked;
			item.MaxMessageSize = Int32.Parse(txtMaxMessageSize.Text);
			item.MaxRecipientsPerMessage = Int32.Parse(txtMaxRecipients.Text);
			item.Members = mailEditItems.Items;
			ctrl = null;
		}

		public void BindListModerators()
		{

			string domainName = null;
			if (!String.IsNullOrEmpty(itemName))
			{
				domainName = GetDomainName(itemName);


				MailAccount[] moderators = ES.Services.MailServers.GetMailAccounts(PanelSecurity.PackageId, true);
				ddlListModerators.Items.Clear();
				ddlListModerators.Items.Insert(0, new ListItem(GetLocalizedString("Text.SelectModerator"), ""));

				if (moderators != null)
					foreach (MailAccount account in moderators)
					{
						if (GetDomainName(account.Name) == domainName)
						{
							if (ddlListModerators != null) ddlListModerators.Items.Add(new ListItem(account.Name));
						}
					}

				Utils.SelectListItem(ddlListModerators, selectedModerator);
			}
			else
			{

				MailAccount[] moderators = ES.Services.MailServers.GetMailAccounts(PanelSecurity.PackageId, true);
				ddlListModerators.Items.Clear();
				ddlListModerators.Items.Insert(0, new ListItem(GetLocalizedString("Text.SelectModerator"), ""));

				if (moderators != null)
					foreach (MailAccount account in moderators)
					{
						if (ddlListModerators != null) ddlListModerators.Items.Add(new ListItem(account.Name));
					}

				Utils.SelectListItem(ddlListModerators, selectedModerator);
			}
		}

		private string GetDomainName(string email)
		{
			return email.Substring(email.IndexOf("@") + 1);
		}

		protected void ctxValDomain_EvaluatingContext(object sender, DesktopValidationEventArgs e)
		{
			if (Parent != null) ctrl = (MailEditAddress)Parent.Parent.FindControl("emailAddress");

			string moderator = ddlListModerators.SelectedValue;

			if (ctrl != null)
			{
				if (String.Equals(GetDomainName(moderator),GetDomainName(ctrl.Email), StringComparison.InvariantCultureIgnoreCase))
				{
					e.ContextIsValid = true;
					return;
				}
			}
			e.ContextIsValid = false;
		}
			
	}

}

