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
using System.Collections.Generic;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using FuseCP.EnterpriseServer;
using FuseCP.Portal.Code.Framework;
using FuseCP.Portal.UserControls.ScheduleTaskView;

namespace FuseCP.Portal.ScheduleTaskControls
{
	public partial class CheckWebsite : EmptyView
	{
		private static readonly string UrlParameter = "URL";
		private static readonly string AccessUsernameParameter = "USERNAME";
		private static readonly string AccessPasswordParameter = "PASSWORD";
		private static readonly string UseSendMessageIfResponseStatusParameter = "USE_RESPONSE_STATUS";
		private static readonly string UseSendMessageIfResponseContainsParameter = "USE_RESPONSE_CONTAIN";
		private static readonly string UseSendMessageIfResponseDoesntContainParameter = "USE_RESPONSE_DOESNT_CONTAIN";
		private static readonly string SendMessageIfResponseStatusParameter = "RESPONSE_STATUS";
		private static readonly string SendMessageIfResponseContainsParameter = "RESPONSE_CONTAIN";
		private static readonly string SendMessageIfResponseDoesntContainParameter = "RESPONSE_DOESNT_CONTAIN";
		private static readonly string MailFromParameter = "MAIL_FROM";
		private static readonly string MailToParameter = "MAIL_TO";
		private static readonly string MailSubjectParameter = "MAIL_SUBJECT";
		private static readonly string MailBodyParameter = "MAIL_BODY";

		protected void Page_Load(object sender, EventArgs e)
		{
		}

		/// <summary>
		/// Sets scheduler task parameters on view.
		/// </summary>
		/// <param name="parameters">Parameters list to be set on view.</param>
		public override void SetParameters(ScheduleTaskParameterInfo[] parameters)
		{
			base.SetParameters(parameters);

			this.SetParameter(this.txtUrl, UrlParameter);
			this.SetParameter(this.txtAccessUsername, AccessUsernameParameter);
			this.SetParameter(this.txtAccessPassword, AccessPasswordParameter);
			this.SetParameter(this.cbxResponseStatus, UseSendMessageIfResponseStatusParameter);
			this.SetParameter(this.cbxResponseContains, UseSendMessageIfResponseContainsParameter);
			this.SetParameter(this.cbxResponseDoesntContain, UseSendMessageIfResponseDoesntContainParameter);
			this.SetParameter(this.txtResponseStatus, SendMessageIfResponseStatusParameter);
			this.SetParameter(this.txtResponseContains, SendMessageIfResponseContainsParameter);
			this.SetParameter(this.txtResponseDoesntContain, SendMessageIfResponseDoesntContainParameter);
			this.SetParameter(this.txtMailFrom, MailFromParameter);
			this.SetParameter(this.txtMailTo, MailToParameter);
			this.SetParameter(this.txtMailSubject, MailSubjectParameter);
			this.SetParameter(this.txtMailBody, MailBodyParameter);
		}

		/// <summary>
		/// Gets scheduler task parameters from view.
		/// </summary>
		/// <returns>Parameters list filled  from view.</returns>
		public override ScheduleTaskParameterInfo[] GetParameters()
		{
			ScheduleTaskParameterInfo url = this.GetParameter(this.txtUrl, UrlParameter);
			ScheduleTaskParameterInfo accessUsername = this.GetParameter(this.txtAccessUsername, AccessUsernameParameter);
			ScheduleTaskParameterInfo accessPassword = this.GetParameter(this.txtAccessPassword, AccessPasswordParameter);
			ScheduleTaskParameterInfo useSendMessageIfResponseStatus = this.GetParameter(this.cbxResponseStatus, UseSendMessageIfResponseStatusParameter);
			ScheduleTaskParameterInfo useSendMessageIfResponseContains = this.GetParameter(this.cbxResponseContains, UseSendMessageIfResponseContainsParameter);
			ScheduleTaskParameterInfo useSendMessageIfResponseDoesntContain = this.GetParameter(this.cbxResponseDoesntContain, UseSendMessageIfResponseDoesntContainParameter);
			ScheduleTaskParameterInfo sendMessageIfResponseStatus = this.GetParameter(this.txtResponseStatus, SendMessageIfResponseStatusParameter);
			ScheduleTaskParameterInfo sendMessageIfResponseContains = this.GetParameter(this.txtResponseContains, SendMessageIfResponseContainsParameter);
			ScheduleTaskParameterInfo sendMessageIfResponseDoesntContain = this.GetParameter(this.txtResponseDoesntContain, SendMessageIfResponseDoesntContainParameter);
			ScheduleTaskParameterInfo mailFrom = this.GetParameter(this.txtMailFrom, MailFromParameter);
			ScheduleTaskParameterInfo mailTo = this.GetParameter(this.txtMailTo, MailToParameter);
			ScheduleTaskParameterInfo mailSubject = this.GetParameter(this.txtMailSubject, MailSubjectParameter);
			ScheduleTaskParameterInfo mailBody = this.GetParameter(this.txtMailBody, MailBodyParameter);

			return new ScheduleTaskParameterInfo[13] { url, accessUsername, accessPassword, sendMessageIfResponseStatus, sendMessageIfResponseContains, sendMessageIfResponseDoesntContain, useSendMessageIfResponseStatus, useSendMessageIfResponseContains, useSendMessageIfResponseDoesntContain, mailFrom, mailTo, mailSubject, mailBody };

		}
	}
}
