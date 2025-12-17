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
    public partial class NotifyOverusedDatabases : EmptyView
	{
        private static readonly string MSSQLOverusedParameter = "MSSQL_OVERUSED";
        private static readonly string MYSQLOverusedParameter = "MYSQL_OVERUSED";
        private static readonly string MARIADBOverusedParameter = "MARIADB_OVERUSED";
        private static readonly string SendWarningEmailParameter = "SEND_WARNING_EMAIL";
		private static readonly string WarningUsageThresholdParameter = "WARNING_USAGE_THRESHOLD";
        private static readonly string SendOverusedEmailParameter = "SEND_OVERUSED_EMAIL";
        private static readonly string OverusedUsageThresholdParameter = "OVERUSED_USAGE_THRESHOLD";
		private static readonly string WarningMailFromParameter = "WARNING_MAIL_FROM";
		private static readonly string WarningMailBccParameter = "WARNING_MAIL_BCC";
		private static readonly string WarningMailSubjectParameter = "WARNING_MAIL_SUBJECT";
		private static readonly string WarningMailBodyParameter = "WARNING_MAIL_BODY";
        private static readonly string OverusedMailBodyParameter = "OVERUSED_MAIL_BODY";
        private static readonly string OverusedMailFromParameter = "OVERUSED_MAIL_FROM";
        private static readonly string OverusedMailBccParameter = "OVERUSED_MAIL_BCC";
        private static readonly string OverusedMailSubjectParameter = "OVERUSED_MAIL_SUBJECT";


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

			this.SetParameter(this.cbxMSSQLOverused, MSSQLOverusedParameter);
			this.SetParameter(this.cbxMYSQLOverused, MYSQLOverusedParameter);
            this.SetParameter(this.cbxMARIADBOverused, MARIADBOverusedParameter);
            this.SetParameter(this.cbxDoSendWarning, SendWarningEmailParameter);
			this.SetParameter(this.txtWarningThreshold, WarningUsageThresholdParameter);
            this.SetParameter(this.cbxDoSendOverused, SendOverusedEmailParameter);
            this.SetParameter(this.txtOverusedThreshold, OverusedUsageThresholdParameter);
			this.SetParameter(this.txtWarningMailFrom, WarningMailFromParameter);
			this.SetParameter(this.txtWarningMailBcc, WarningMailBccParameter);
			this.SetParameter(this.txtWarningMailSubject, WarningMailSubjectParameter);
			this.SetParameter(this.txtWarningMailBody, WarningMailBodyParameter);
            this.SetParameter(this.txtOverusedMailBody, OverusedMailBodyParameter);
            this.SetParameter(this.txtOverusedMailFrom, OverusedMailFromParameter);
            this.SetParameter(this.txtOverusedMailBcc, OverusedMailBccParameter);
			this.SetParameter(this.txtOverusedMailSubject, OverusedMailSubjectParameter);
		}

		/// <summary>
		/// Gets scheduler task parameters from view.
		/// </summary>
		/// <returns>Parameters list filled  from view.</returns>
		public override ScheduleTaskParameterInfo[] GetParameters()
		{
			ScheduleTaskParameterInfo MSSQLOverused = this.GetParameter(this.cbxMSSQLOverused, MSSQLOverusedParameter);
            ScheduleTaskParameterInfo MYSQLOverused = this.GetParameter(this.cbxMYSQLOverused, MYSQLOverusedParameter);
            ScheduleTaskParameterInfo MARIADBOverused = this.GetParameter(this.cbxMARIADBOverused, MARIADBOverusedParameter);
            ScheduleTaskParameterInfo sendWarningEmail = this.GetParameter(this.cbxDoSendWarning, SendWarningEmailParameter);
			ScheduleTaskParameterInfo warningUsageThreshold = this.GetParameter(this.txtWarningThreshold, WarningUsageThresholdParameter);
			ScheduleTaskParameterInfo sendOverusedEmail = this.GetParameter(this.cbxDoSendOverused, SendOverusedEmailParameter);
			ScheduleTaskParameterInfo OverusedUsageThreshold = this.GetParameter(this.txtOverusedThreshold, OverusedUsageThresholdParameter);
			ScheduleTaskParameterInfo warningMailFrom = this.GetParameter(this.txtWarningMailFrom, WarningMailFromParameter);
			ScheduleTaskParameterInfo warningMailBcc = this.GetParameter(this.txtWarningMailBcc, WarningMailBccParameter);
			ScheduleTaskParameterInfo warningMailSubject = this.GetParameter(this.txtWarningMailSubject, WarningMailSubjectParameter);
			ScheduleTaskParameterInfo warningMailBody = this.GetParameter(this.txtWarningMailBody, WarningMailBodyParameter);
			ScheduleTaskParameterInfo OverusedMailFrom = this.GetParameter(this.txtOverusedMailFrom, OverusedMailFromParameter);
			ScheduleTaskParameterInfo OverusedMailBcc = this.GetParameter(this.txtOverusedMailBcc, OverusedMailBccParameter);
			ScheduleTaskParameterInfo OverusedMailSubject = this.GetParameter(this.txtOverusedMailSubject, OverusedMailSubjectParameter);
			ScheduleTaskParameterInfo OverusedMailBody = this.GetParameter(this.txtOverusedMailBody, OverusedMailBodyParameter);

            return new ScheduleTaskParameterInfo[15] { MSSQLOverused, MYSQLOverused, MARIADBOverused, sendWarningEmail, warningUsageThreshold, sendOverusedEmail, OverusedUsageThreshold, warningMailFrom, warningMailBcc, warningMailSubject, warningMailBody, OverusedMailFrom, OverusedMailBcc, OverusedMailSubject, OverusedMailBody };
		}
	}
}
