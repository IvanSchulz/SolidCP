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
using System.Text;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Text.RegularExpressions;
using System.Web.Services.Protocols;
using System.IO;
using FuseCP.Providers.OS;

namespace FuseCP.Portal
{
	public partial class MessageBox : FuseCPControlBase, IMessageBoxControl, INamingContainer
	{
		const string FuseCPGithubUrl = "https://github.com/FuseCP/FuseCP";
		protected void Page_Load(object sender, EventArgs e)
		{
			//this.Visible = false;
			if (ViewState["ShowNextTime"] != null)
			{
				this.Visible = true;
				ViewState["ShowNextTime"] = null;
			}
		}

		string emailMessage = null;

		public void RenderMessage(MessageBoxType messageType, string message, string description,
			 Exception ex, params string[] additionalParameters)
		{
			this.Visible = true; // show message

			// set icon and styles
			string boxStyle = "MessageBox Green";
			if (messageType == MessageBoxType.Warning)
				boxStyle = "MessageBox Yellow";
			else if (messageType == MessageBoxType.Error)
				boxStyle = "MessageBox Red";

			tblMessageBox.Attributes["class"] = boxStyle;

			// set texts
			litMessage.Text = message;
			litDescription.Text = !String.IsNullOrEmpty(description)
				 ? String.Format("<br/><span class=\"description\">{0}</span>", description) : "";

			// show exception
			if (ex != null)
			{
				// show error
				try
				{
					// technical details
					litPageUrl.Text = PortalAntiXSS.Encode(Request.Url.ToString());
					litLoggedUser.Text = PanelSecurity.LoggedUser.Username;
					litSelectedUser.Text = PanelSecurity.SelectedUser.Username;
					litPackageName.Text = PanelSecurity.PackageId.ToString();
					var stacktxt = ex.ToString().Trim();
					var stackhtml = stacktxt;
					var fileVersion = OSInfo.FuseCPVersion;
					stackhtml = Regex.Replace(stackhtml, @"(?<=\n\s*at\s+.+?\)\s+in\s+)(?:[A-Za-z]:\\|/)[^:]+(?=:line\s+[0-9]+(?:\r?\n|$))", match =>
					{
						var file = match.Value.Replace(Path.DirectorySeparatorChar, '/');
						file = Regex.Replace(file, @"^.*?(?=/FuseCP/(?:Sources|Lib)/)", "");
						return $@"<a href=""{FuseCPGithubUrl}/tree/v{fileVersion}{file}"">{file}</a>";
					}, RegexOptions.Multiline);
					stackhtml = stackhtml.Replace("\n", "<br/>");
					litStackTrace.Text = stackhtml;

					// send form
					litSendFrom.Text = PanelSecurity.LoggedUser.Email;

					if (!String.IsNullOrEmpty(PortalUtils.FromEmail))
						litSendFrom.Text = PortalUtils.FromEmail;

					litSendTo.Text = PortalUtils.AdminEmail;
					litSendCC.Text = PanelSecurity.LoggedUser.Email;
					litSendSubject.Text = GetLocalizedString("Text.Subject");

					// compose email message
					StringBuilder sb = new StringBuilder();
					sb.Append("Page URL: ").Append(litPageUrl.Text).Append("\n\n");
					sb.Append("Logged User: ").Append(litLoggedUser.Text).Append("\n\n");
					sb.Append("Selected User: ").Append(litSelectedUser.Text).Append("\n\n");
					sb.Append("Package ID: ").Append(litPackageName.Text).Append("\n\n");
					sb.Append("Stack Trace: ").Append(stackhtml).Append("\n\n");
					sb.Append("Personal Comments: ").Append("%Comments%").Append("\n\n");
					emailMessage = $@"
<html>
	<head>
		<title>FuseCP Error User Report</title>
	</head>
	<body>

		<h1>FuseCP Error User Report</h1>

		<p>
			{sb.ToString().Replace("\n", "<br/>\n")}
		</p>

	</body>
</html>";
				}
				catch { /* skip */ }
			}
			else
			{
				rowTechnicalDetails.Visible = false;
			}
		}

		protected void btnSend_Click(object sender, EventArgs e)
		{
			EnableViewState = true;
			ViewState["ShowNextTime"] = true;

			try
			{
				btnSend.Visible = false;
				lblSentMessage.Visible = true;

				var from = PanelSecurity.LoggedUser.Email;
				var to = PortalUtils.AdminEmail;
				var subject = GetLocalizedString("Text.Subject");
				emailMessage = emailMessage.Replace("%Comments%", $"<p>{txtSendComments.Text}</p>".Replace("\n", "<br/>\n"));

				// send mail
				PortalUtils.SendMail(from, to, from, subject, emailMessage, true);

				lblSentMessage.Text = GetLocalizedString("Text.MessageSent");
			}
			catch
			{
				lblSentMessage.Text = GetLocalizedString("Text.MessageSentError");
			}
		}

		// Use control state for emailMessage because ViewState won't always work
		protected override void OnInit(EventArgs e)
		{
			base.OnInit(e);
			Page.RegisterRequiresControlState(this);
		}

		protected override object SaveControlState()
		{
			object obj = base.SaveControlState();

			if (emailMessage != null)
			{
				if (obj != null)
				{
					return new Pair(obj, emailMessage);
				}
				else
				{
					return emailMessage;
				}
			}
			else
			{
				return obj;
			}
		}

		protected override void LoadControlState(object state)
		{
			if (state != null)
			{
				Pair p = state as Pair;
				if (p != null)
				{
					base.LoadControlState(p.First);
					emailMessage = (string)p.Second;
				}
				else
				{
					if (state is string)
					{
						emailMessage = (string)state;
					}
					else
					{
						base.LoadControlState(state);
					}
				}
			}
		}

	}
}
