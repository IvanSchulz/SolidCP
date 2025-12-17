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

﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Mail;
using System.Net.Mime;
using System.Text;
using FuseCP.EnterpriseServer;
using FuseCP.EnterpriseServer.Code.HostedSolution;
using FuseCP.Providers.HostedSolution;

namespace FuseCP.EnterpriseServer
{
    public class HostedSolutionReportTask : SchedulerTask
    {
        private static readonly string EXCHANGE_REPORT = "EXCHANGE_REPORT";
        private static readonly string ORGANIZATION_REPORT = "ORGANIZATION_REPORT";
        private static readonly string SHAREPOINT_REPORT = "SHAREPOINT_REPORT";
        private static readonly string LYNC_REPORT = "LYNC_REPORT";
        private static readonly string SFB_REPORT = "SFB_REPORT";
        private static readonly string CRM_REPORT = "CRM_REPORT";
        private static readonly string EMAIL = "EMAIL";
        
        
        public override void DoWork()
        {
            try
            {
                TaskManager.Write("Start HostedSolutionReportTask");

                BackgroundTask topTask = TaskManager.TopTask;

                bool isExchange = Utils.ParseBool(topTask.GetParamValue(EXCHANGE_REPORT), false);
                bool isSharePoint = Utils.ParseBool(topTask.GetParamValue(SHAREPOINT_REPORT), false);
                bool isLync = Utils.ParseBool(topTask.GetParamValue(LYNC_REPORT), false);
                bool isSfB = Utils.ParseBool(topTask.GetParamValue(SFB_REPORT), false);
                bool isCRM = Utils.ParseBool(topTask.GetParamValue(CRM_REPORT), false);
                bool isOrganization = Utils.ParseBool(topTask.GetParamValue(ORGANIZATION_REPORT), false);

                string email = topTask.GetParamValue(EMAIL).ToString();

                TaskManager.WriteParameter("isExchange",isExchange);
                TaskManager.WriteParameter("isSharePoint",isSharePoint);
                TaskManager.WriteParameter("isLync", isLync);
                TaskManager.WriteParameter("isSfB", isSfB);
                TaskManager.WriteParameter("isCRM", isCRM);
                TaskManager.WriteParameter("isOrganization", isOrganization);
                TaskManager.WriteParameter("email", email);

                UserInfo user = PackageController.GetPackageOwner(topTask.PackageId);

                TaskManager.WriteParameter("user", user.Username);

                EnterpriseSolutionStatisticsReport report =
                ReportController.GetEnterpriseSolutionStatisticsReport(user.UserId, isExchange, isSharePoint, isCRM,isOrganization, isLync, isSfB);

                TaskManager.WriteParameter("report.ExchangeReport.Items.Count", report.ExchangeReport.Items.Count);
                TaskManager.WriteParameter("report.SharePointReport.Items.Count", report.SharePointReport.Items.Count);
                TaskManager.WriteParameter("report.CRMReport.Items.Count", report.CRMReport.Items.Count);
                TaskManager.WriteParameter("report.OrganizationReport.Items.Count", report.OrganizationReport.Items.Count);
                TaskManager.WriteParameter("report.LyncReport.Items.Count", report.LyncReport.Items.Count);
                TaskManager.WriteParameter("report.SfBReport.Items.Count", report.SfBReport.Items.Count);

                SendMessage(user, email, isExchange && report.ExchangeReport != null ? report.ExchangeReport.ToCSV() : string.Empty,
                            isSharePoint && report.SharePointReport != null ? report.SharePointReport.ToCSV() : string.Empty,
                            isCRM && report.CRMReport != null ? report.CRMReport.ToCSV() : string.Empty,
                            isOrganization && report.OrganizationReport != null ? report.OrganizationReport.ToCSV() : string.Empty,
                            isLync && report.LyncReport != null ? report.LyncReport.ToCSV() : string.Empty,
                            isSfB && report.SfBReport != null ? report.SfBReport.ToCSV() : string.Empty);

            }
            catch(Exception ex)
            {
                TaskManager.WriteError(ex);
            }

            TaskManager.Write("End HostedSolutionReportTask");

        }

        
        private static void PrepareAttament(string name, string csv, List<Attachment> attacments)
        {
            if (!string.IsNullOrEmpty(csv))
            {
                UTF8Encoding encoding = new UTF8Encoding();
                
                byte[] buffer = encoding.GetBytes(csv);
                MemoryStream stream = new MemoryStream(buffer);
                Attachment attachment = new Attachment(stream, name, MediaTypeNames.Text.Plain);

                attacments.Add(attachment);
            }
        }
        
        private void SendMessage(UserInfo user,string email, string exchange_csv, string sharepoint_csv, string crm_csv, string organization_csv, string lync_csv, string sfb_csv)
        {
            TaskManager.Write("SendMessage");

            List<Attachment> attacments = new List<Attachment>();
            PrepareAttament("exchange.csv", exchange_csv, attacments);
            PrepareAttament("sharepoint.csv", sharepoint_csv, attacments);
            PrepareAttament("lync.csv", lync_csv, attacments);
            PrepareAttament("sfb.csv", sfb_csv, attacments);
            PrepareAttament("crm.csv", crm_csv, attacments);
            PrepareAttament("organization.csv", organization_csv, attacments);
            
            // get letter settings
            UserSettings settings = UserController.GetUserSettings(user.UserId, UserSettings.HOSTED_SOLUTION_REPORT);

            string from = settings["From"];
            string cc = settings["CC"];
            string subject = settings["Subject"];
            string body = user.HtmlMail ? settings["HtmlBody"] : settings["TextBody"];
            bool isHtml = user.HtmlMail;

            MailPriority priority = MailPriority.Normal;

            TaskManager.WriteParameter("from", from);
            TaskManager.WriteParameter("email", email);
            TaskManager.WriteParameter("subject", subject);
            TaskManager.WriteParameter("body", body);
          
            
            int res = MailHelper.SendMessage(from, email, cc,  subject, body, priority, isHtml, attacments.ToArray());

            if (res==0)
            {
                TaskManager.Write("SendMessage OK");
            }
            else
            {
                TaskManager.WriteError("SendMessage error ", "error code", res.ToString());
            }

            TaskManager.WriteParameter("", res);

            TaskManager.Write("End SendMessage");

            
        }
    }
}
