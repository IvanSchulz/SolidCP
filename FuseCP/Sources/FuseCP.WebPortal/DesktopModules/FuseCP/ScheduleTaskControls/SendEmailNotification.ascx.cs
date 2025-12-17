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
using FuseCP.EnterpriseServer;
using FuseCP.Portal.UserControls.ScheduleTaskView;

namespace FuseCP.Portal.ScheduleTaskControls
{
	public partial class SendEmailNotification : EmptyView
	{
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
			ScheduleTaskParameterInfo mailFrom = this.GetParameter(this.txtMailFrom, MailFromParameter);
			ScheduleTaskParameterInfo mailTo = this.GetParameter(this.txtMailTo, MailToParameter);
			ScheduleTaskParameterInfo mailSubject = this.GetParameter(this.txtMailSubject, MailSubjectParameter);
			ScheduleTaskParameterInfo mailBody = this.GetParameter(this.txtMailBody, MailBodyParameter);

			return new ScheduleTaskParameterInfo[4] { mailFrom, mailTo, mailSubject, mailBody };

		}
	}
}
