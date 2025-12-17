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
using System.Xml;

namespace FuseCP.EnterpriseServer
{
    public class AuditLogReportTask : SchedulerTask
    {
        public override void DoWork()
        {
            BackgroundTask topTask = TaskManager.TopTask;

            // get input parameters
            string mailTo = (string)topTask.GetParamValue("MAIL_TO");
            int auditLogSeverity = Utils.ParseInt((string)topTask.GetParamValue("AUDIT_LOG_SEVERITY"), -1);
            string auditLogSource = (string)topTask.GetParamValue("AUDIT_LOG_SOURCE");
            string auditLogTask = (string)topTask.GetParamValue("AUDIT_LOG_TASK");
            string auditLogDate = (string)topTask.GetParamValue("AUDIT_LOG_DATE");
            int showExecutionLog = Utils.ParseInt((string)topTask.GetParamValue("SHOW_EXECUTION_LOG"), 0);

            // check input parameters
            if (string.IsNullOrEmpty(mailTo))
            {
                TaskManager.WriteWarning("Specify 'Mail To' task parameter");
                return;
            }

            string mailFrom = null;
            SystemSettings settings = SystemController.GetSystemSettingsInternal(SystemSettings.SMTP_SETTINGS, false);
            if (settings != null)
            {
                mailFrom = settings["SmtpUsername"];
            }
            if (String.IsNullOrEmpty(mailFrom))
            {
                TaskManager.WriteWarning("You need to configure SMTP settings first");
                return;
            }

            DateTime logStart, logEnd;

            switch (auditLogDate)
            {
                case "today":
                    logStart = DateTime.Now;
                    logEnd = DateTime.Now;
                    break;
                case "yesterday":
                    logStart = DateTime.Now.AddDays(-1);
                    logEnd = DateTime.Now.AddDays(-1);
                    break;
                case "schedule":
                default:
                    logEnd = DateTime.Now;
                    ScheduleInfo schedule = SchedulerController.GetSchedule(topTask.ScheduleId);
                    switch (schedule.ScheduleTypeId)
                    {
                        case "Daily":
                            logStart = DateTime.Now.AddDays(-1);
                            break;
                        case "Weekly":
                            logStart = DateTime.Now.AddDays(-7);
                            break;
                        case "Monthly":
                            logStart = DateTime.Now.AddMonths(-1);
                            break;
                        case "Interval":
                            logStart = DateTime.Now.AddSeconds(-schedule.Interval);
                            break;
                        case "OneTime":
                        default:
                            logStart = DateTime.Now;
                            break;
                    }
                    break;
            }

            string mailSubject = "Audit Log Report (" + logStart.ToString("MMM dd, yyyy") + " - " + logEnd.ToString("MMM dd, yyyy") + ")";

            StringBuilder sb = new StringBuilder();
            sb.AppendLine("<html><head><style>");
            sb.AppendLine("table, th, td { border: 1px solid black; border-collapse: collapse; }");
            sb.AppendLine("th, td { padding: 5px; }");
            sb.AppendLine("th { text-align: left; }");
            sb.AppendLine("</style></head><body>");
            sb.AppendLine("<h2>" + mailSubject + "</h2>");
            sb.AppendFormat("<h3>Source: {0}, Task: {1}, Severity: {2}</h3>", String.IsNullOrEmpty(auditLogSource) ? "All" : auditLogSource, 
                String.IsNullOrEmpty(auditLogTask) ? "All" : auditLogTask, GetAuditLogRecordSeverityName(auditLogSeverity));

            DataTable logs = AuditLog.GetAuditLogRecordsPaged(topTask.EffectiveUserId, 0, 0, null, logStart, logEnd,
                auditLogSeverity, auditLogSource, auditLogTask, "", 0, Int32.MaxValue).Tables[1];

            sb.AppendLine("<p>");
            if (logs.Rows.Count == 0)
            {
                sb.AppendLine("Audit Log is empty.");
            }
            else
            {
                sb.AppendLine("<table>");
                sb.Append("<tr><th>Started</th><th>Finished</th><th>Severity</th><th>Username</th><th>Source</th><th>Task</th><th>Item-Name</th>");
                if (showExecutionLog == 1)
                {
                    sb.AppendLine("<th>Execution-Log</th></tr>");
                }
                else
                {
                    sb.AppendLine("</tr>");
                }
                foreach (DataRow log in logs.Rows)
                {
                    sb.AppendLine("<tr>");
                    // Started
                    sb.AppendFormat("<td>{0}</td>", log["StartDate"].ToString());
                    // Finished
                    sb.AppendFormat("<td>{0}</td>", log["FinishDate"].ToString());
                    // Severity
                    sb.AppendFormat("<td>{0}</td>", GetAuditLogRecordSeverityName((int)log["SeverityID"]));
                    // Username
                    sb.AppendFormat("<td>{0}</td>", log["Username"]);
                    // Source
                    sb.AppendFormat("<td>{0}</td>", log["SourceName"]);
                    // Task
                    sb.AppendFormat("<td>{0}</td>", log["TaskName"]);
                    // Item-Name
                    sb.AppendFormat("<td>{0}</td>", log["ItemName"]);
                    // Execution-Log
                    if (showExecutionLog == 1)
                    {
                        string executionLog = FormatPlainTextExecutionLog(log["ExecutionLog"].ToString());
                        sb.AppendFormat("<td>{0}</td>", executionLog);
                    }
                    sb.AppendLine("</tr>");
                }
                sb.AppendLine("</table>");
            }
            sb.AppendLine("</p></body></html>");

            // send mail message
            int res = MailHelper.SendMessage(mailFrom, mailTo, mailSubject, sb.ToString(), true);
            if (res != 0) TaskManager.WriteError("SMTP Error. Code: " + res.ToString());
        }

        private string FormatPlainTextExecutionLog(string xmlLog)
        {
            StringBuilder sb = new StringBuilder();
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xmlLog);

            XmlNodeList nodeRecords = doc.SelectNodes("/log/records/record");

            foreach (XmlNode nodeRecord in nodeRecords)
            {
                // text
                XmlNode nodeText = nodeRecord.SelectSingleNode("text");

                // text parameters
                string[] prms = new string[0];
                XmlNodeList nodePrms = nodeRecord.SelectNodes("textParameters/value");
                if (nodePrms != null)
                {
                    prms = new string[nodePrms.Count];
                    for (int i = 0; i < nodePrms.Count; i++)
                        prms[i] = nodePrms[i].InnerText;
                }

                string text = nodeText.InnerText;

                // format parameters
                if (prms.Length > 0) text = String.Format(text, prms);
                // Record text
                if (!String.IsNullOrEmpty(text))
                {
                    sb.Append(text);
                    sb.AppendLine();
                }
                //
                XmlNode nodeStackTrace = nodeRecord.SelectSingleNode("stackTrace");
                // Record stack trace
                if (!String.IsNullOrEmpty(nodeStackTrace.InnerText))
                {
                    sb.Append(nodeStackTrace.InnerText);
                    sb.AppendLine();
                }
            }
            return sb.ToString().Replace("\"", "\"\"");
        }

        private string GetAuditLogRecordSeverityName(int severityId)
        {
            string name;
            switch (severityId)
            {
                case -1:
                    name = "All";
                    break;
                case 0:
                default:
                    name = "Information";
                    break;
                case 1:
                    name = "Warning";
                    break;
                case 2:
                    name = "Error";
                    break;
            }
            return name;
        }
    }
}
