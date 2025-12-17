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
using System.Web.UI.WebControls;
using FuseCP.EnterpriseServer;
using FuseCP.Portal.UserControls.ScheduleTaskView;
using FCP = FuseCP.EnterpriseServer;

namespace FuseCP.Portal.ScheduleTaskControls
{
    public partial class AuditLogReportView : EmptyView
    {
        private static readonly string MailToParameter = "MAIL_TO";
        private static readonly string AuditLogSeverityParameter = "AUDIT_LOG_SEVERITY";
        private static readonly string AuditLogSourceParameter = "AUDIT_LOG_SOURCE";
        private static readonly string AuditLogTaskParameter = "AUDIT_LOG_TASK";
        private static readonly string AuditLogDateParameter = "AUDIT_LOG_DATE";
        private static readonly string ShowExecutionLogParameter = "SHOW_EXECUTION_LOG";

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

            FCP.SystemSettings settings = ES.Services.System.GetSystemSettingsActive(FCP.SystemSettings.SMTP_SETTINGS, false);
            if (settings != null)
            {
                txtMailFrom.Text = settings["SmtpUsername"];
            }
            if (String.IsNullOrEmpty(txtMailFrom.Text))
            {
                txtMailFrom.Text = GetLocalizedString("SMTPWarning.Text");
            }
            
            SetParameter(txtMailTo, MailToParameter);
            BindSources();
            SetParameter(ddlAuditLogSource, AuditLogSourceParameter);
            BindSourceTasks();
            SetParameter(ddlAuditLogTask, AuditLogTaskParameter);
            base.SetParameter(ddlAuditLogDate, AuditLogDateParameter);
            base.SetParameter(ddlAuditLogSeverity, AuditLogSeverityParameter);
            base.SetParameter(ddlExecutionLog, ShowExecutionLogParameter);
            foreach (ListItem item in ddlAuditLogDate.Items)
            {
                item.Text = GetLocalizedString(item.Value + ".Text");
            }
            foreach (ListItem item in ddlAuditLogSeverity.Items)
            {
                item.Text = GetLocalizedString(item.Value + ".Severity");
            }
            foreach (ListItem item in ddlExecutionLog.Items)
            {
                item.Text = GetLocalizedString(item.Value + ".ShowExecutionLog");
            }
        }

        private new void SetParameter(DropDownList control, string parameterName)
        {
            ScheduleTaskParameterInfo parameter = FindParameterById(parameterName);
            Utils.SelectListItem(control, parameter.ParameterValue);
        }

        /// <summary>
        /// Gets scheduler task parameters from view.
        /// </summary>
        /// <returns>Parameters list filled  from view.</returns>
        public override ScheduleTaskParameterInfo[] GetParameters()
        {
            ScheduleTaskParameterInfo mailTo = GetParameter(txtMailTo, MailToParameter);
            ScheduleTaskParameterInfo auditLogSeverity = GetParameter(ddlAuditLogSeverity, AuditLogSeverityParameter);
            ScheduleTaskParameterInfo auditLogSource = GetParameter(ddlAuditLogSource, AuditLogSourceParameter);
            ScheduleTaskParameterInfo auditLogTask = GetParameter(ddlAuditLogTask, AuditLogTaskParameter);
            ScheduleTaskParameterInfo auditLogDate = GetParameter(ddlAuditLogDate, AuditLogDateParameter);
            ScheduleTaskParameterInfo showExecutionLog = GetParameter(ddlExecutionLog, ShowExecutionLogParameter);

            return new ScheduleTaskParameterInfo[6] { mailTo, auditLogSeverity, auditLogSource, auditLogTask, auditLogDate, showExecutionLog };

        }

        protected void ddlSource_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindSourceTasks();
        }

        private string GetAuditLogTaskName(string sourceName, string taskName)
        {
            string localizedText = PortalUtils.GetSharedLocalizedString(Utils.ModuleName,
                "AuditLogTask." + sourceName + "_" + taskName);
            return (localizedText != null) ? localizedText : taskName;
        }

        private string GetAuditLogSourceName(string sourceName)
        {
            string localizedText = PortalUtils.GetSharedLocalizedString(Utils.ModuleName, "AuditLogSource." + sourceName);
            return (localizedText != null) ? localizedText : sourceName;
        }

        private string GetLocalizedString(string resourceKey)
        {
            return (string)GetLocalResourceObject(resourceKey);
        }

        private void BindSourceTasks()
        {
            string sourceName = ddlAuditLogSource.SelectedValue;

            ddlAuditLogTask.Items.Clear();
            ddlAuditLogTask.Items.Add(new ListItem(GetLocalizedString("All.Text"), ""));
            DataTable dt = ES.Services.AuditLog.GetAuditLogTasks(sourceName).Tables[0];
            foreach (DataRow dr in dt.Rows)
            {
                string taskName = dr["TaskName"].ToString();
                ddlAuditLogTask.Items.Add(new ListItem(GetAuditLogTaskName(sourceName, taskName), taskName));
            }
        }

        private void BindSources()
        {
            ddlAuditLogSource.Items.Clear();
            ddlAuditLogSource.Items.Add(new ListItem(GetLocalizedString("All.Text"), ""));
            DataTable dt = ES.Services.AuditLog.GetAuditLogSources().Tables[0];
            foreach (DataRow dr in dt.Rows)
            {
                string sourceName = dr["SourceName"].ToString();
                ddlAuditLogSource.Items.Add(new ListItem(GetAuditLogSourceName(sourceName), sourceName));
            }
        }
    }
}
