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
using System.Text;

namespace FuseCP.EnterpriseServer
{
    public class SendMailNotificationTask : SchedulerTask
    {
        public override void DoWork()
        {
            // Input parameters:
            //  - MAIL_FROM
            //  - MAIL_TO
            //  - MAIL_SUBJECT
            //  - MAIL_BODY

            BackgroundTask topTask = TaskManager.TopTask;

            // get input parameters
            string mailFrom = (string)topTask.GetParamValue("MAIL_FROM");
            string mailTo = (string)topTask.GetParamValue("MAIL_TO");
            string mailSubject = (string)topTask.GetParamValue("MAIL_SUBJECT");
            string mailBody = (string)topTask.GetParamValue("MAIL_BODY");

            // check input parameters
            if (String.IsNullOrEmpty(mailFrom))
            {
                TaskManager.WriteWarning("Specify 'Mail From' task parameter");
                return;
            }

            if (String.IsNullOrEmpty(mailTo))
            {
                TaskManager.WriteWarning("Specify 'Mail To' task parameter");
                return;
            }

            if (String.IsNullOrEmpty(mailSubject))
            {
                TaskManager.WriteWarning("Specify 'Mail Subject' task parameter");
                return;
            }

            // send mail message
            MailHelper.SendMessage(mailFrom, mailTo, mailSubject, mailBody, false);
        }
    }
}
