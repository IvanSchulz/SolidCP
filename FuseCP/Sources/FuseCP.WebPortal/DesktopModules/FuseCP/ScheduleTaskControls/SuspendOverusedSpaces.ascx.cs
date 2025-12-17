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
	public partial class SuspendOverusedSpaces : EmptyView
	{
		private static readonly string SuspendOverusedParameter = "SUSPEND_OVERUSED";
		private static readonly string SuspendWhenDiskSpaceOverusedParameter = "DISKSPACE_OVERUSED";
		private static readonly string SuspendWhenBandwidthOverusedParameter = "BANDWIDTH_OVERUSED";
		private static readonly string SendWarningEmailParameter = "SEND_WARNING_EMAIL";
		private static readonly string WarningUsageThresholdParameter = "WARNING_USAGE_THRESHOLD";
		private static readonly string SendSuspensionEmailParameter = "SEND_SUSPENSION_EMAIL";
		private static readonly string SuspensionUsageThresholdParameter = "SUSPENSION_USAGE_THRESHOLD";
		private static readonly string WarningMailFromParameter = "WARNING_MAIL_FROM";
		private static readonly string WarningMailBccParameter = "WARNING_MAIL_BCC";
		private static readonly string WarningMailSubjectParameter = "WARNING_MAIL_SUBJECT";
		private static readonly string WarningMailBodyParameter = "WARNING_MAIL_BODY";
		private static readonly string SuspensionMailBodyParameter = "SUSPENSION_MAIL_BODY";
		private static readonly string SuspensionMailFromParameter = "SUSPENSION_MAIL_FROM";
		private static readonly string SuspensionMailBccParameter = "SUSPENSION_MAIL_BCC";
		private static readonly string SuspensionMailSubjectParameter = "SUSPENSION_MAIL_SUBJECT";


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

			this.SetParameter(this.cbxDoSuspend, SuspendOverusedParameter);
			this.SetParameter(this.cbxSuspendWhenDiskSpaceOverused, SuspendWhenDiskSpaceOverusedParameter);
			this.SetParameter(this.cbxSuspendWhenBandwidthOverused, SuspendWhenBandwidthOverusedParameter);
			this.SetParameter(this.cbxDoSendWarning, SendWarningEmailParameter);
			this.SetParameter(this.txtWarningThreshold, WarningUsageThresholdParameter);
			this.SetParameter(this.cbxDoSendSuspension, SendSuspensionEmailParameter);
			this.SetParameter(this.txtSuspensionThreshold, SuspensionUsageThresholdParameter);
			this.SetParameter(this.txtWarningMailFrom, WarningMailFromParameter);
			this.SetParameter(this.txtWarningMailBcc, WarningMailBccParameter);
			this.SetParameter(this.txtWarningMailSubject, WarningMailSubjectParameter);
			this.SetParameter(this.txtWarningMailBody, WarningMailBodyParameter);
			this.SetParameter(this.txtSuspensionMailBody, SuspensionMailBodyParameter);
			this.SetParameter(this.txtSuspensionMailFrom, SuspensionMailFromParameter);
			this.SetParameter(this.txtSuspensionMailBcc, SuspensionMailBccParameter);
			this.SetParameter(this.txtSuspensionMailSubject, SuspensionMailSubjectParameter);
		}

		/// <summary>
		/// Gets scheduler task parameters from view.
		/// </summary>
		/// <returns>Parameters list filled  from view.</returns>
		public override ScheduleTaskParameterInfo[] GetParameters()
		{
			ScheduleTaskParameterInfo suspendOverused = this.GetParameter(this.cbxDoSuspend, SuspendOverusedParameter);
			ScheduleTaskParameterInfo suspendWhenDiskSpaceOverused = this.GetParameter(this.cbxSuspendWhenDiskSpaceOverused, SuspendWhenDiskSpaceOverusedParameter);
			ScheduleTaskParameterInfo suspendWhenBandwidthOverused = this.GetParameter(this.cbxSuspendWhenBandwidthOverused, SuspendWhenBandwidthOverusedParameter);
			ScheduleTaskParameterInfo sendWarningEmail = this.GetParameter(this.cbxDoSendWarning, SendWarningEmailParameter);
			ScheduleTaskParameterInfo warningUsageThreshold = this.GetParameter(this.txtWarningThreshold, WarningUsageThresholdParameter);
			ScheduleTaskParameterInfo sendSuspensionEmail = this.GetParameter(this.cbxDoSendSuspension, SendSuspensionEmailParameter);
			ScheduleTaskParameterInfo suspensionUsageThreshold = this.GetParameter(this.txtSuspensionThreshold, SuspensionUsageThresholdParameter);
			ScheduleTaskParameterInfo warningMailFrom = this.GetParameter(this.txtWarningMailFrom, WarningMailFromParameter);
			ScheduleTaskParameterInfo warningMailBcc = this.GetParameter(this.txtWarningMailBcc, WarningMailBccParameter);
			ScheduleTaskParameterInfo warningMailSubject = this.GetParameter(this.txtWarningMailSubject, WarningMailSubjectParameter);
			ScheduleTaskParameterInfo warningMailBody = this.GetParameter(this.txtWarningMailBody, WarningMailBodyParameter);
			ScheduleTaskParameterInfo suspensionMailFrom = this.GetParameter(this.txtSuspensionMailFrom, SuspensionMailFromParameter);
			ScheduleTaskParameterInfo suspensionMailBcc = this.GetParameter(this.txtSuspensionMailBcc, SuspensionMailBccParameter);
			ScheduleTaskParameterInfo suspensionMailSubject = this.GetParameter(this.txtSuspensionMailSubject, SuspensionMailSubjectParameter);
			ScheduleTaskParameterInfo suspensionMailBody = this.GetParameter(this.txtSuspensionMailBody, SuspensionMailBodyParameter);

			return new ScheduleTaskParameterInfo[15] { suspendOverused, suspendWhenDiskSpaceOverused, suspendWhenBandwidthOverused, sendWarningEmail, warningUsageThreshold, sendSuspensionEmail, suspensionUsageThreshold, warningMailFrom, warningMailBcc, warningMailSubject, warningMailBody, suspensionMailFrom, suspensionMailBcc, suspensionMailSubject, suspensionMailBody };
		}
	}
}
