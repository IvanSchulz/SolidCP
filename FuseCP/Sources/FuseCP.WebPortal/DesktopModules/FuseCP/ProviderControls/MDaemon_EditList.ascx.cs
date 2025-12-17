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
	public partial class MDaemon_EditList : FuseCPControlBase, IMailEditListControl
	{
		protected void Page_Load(object sender, EventArgs e)
		{

		}

		public void BindItem(MailList item)
		{
			Utils.SelectListItem(ddlReplyTo, item.ReplyToMode);
			Utils.SelectListItem(ddlPostingMode, item.PostingMode);

			chkModerationEnabled.Checked = item.Moderated;
			txtModeratorEmail.Text = item.ModeratorAddress;

			mailEditItems.Items = item.Members;
		}

		public void SaveItem(MailList item)
		{
			item.ReplyToMode = (ReplyTo)Enum.Parse(typeof(ReplyTo), ddlReplyTo.SelectedValue, true);
			item.PostingMode = (PostingMode)Enum.Parse(typeof(PostingMode), ddlPostingMode.SelectedValue, true);

			// save password
			item.Password = txtPassword.Text;

			item.Moderated = chkModerationEnabled.Checked;
			item.ModeratorAddress = txtModeratorEmail.Text;

			item.Members = mailEditItems.Items;
		}
	}
}
