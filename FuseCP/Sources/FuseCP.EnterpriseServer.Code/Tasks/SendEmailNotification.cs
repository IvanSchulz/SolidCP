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

namespace FuseCP.EnterpriseServer.Tasks
{
    public class SendEmailNotification : TaskEventHandler
    {
        public override void OnStart()
        {
            // nothing to-do
        }

        public override void OnComplete()
        {
            BackgroundTask topTask = TaskManager.TopTask;

            if (!TaskManager.HasErrors(topTask))
            {
                // Send user add notification
                if (topTask.Source == "USER" &&
                    topTask.TaskName == "ADD" && topTask.ItemId > 0)
                {
                    SendAddUserNotification();
                }
                // Send hosting package add notification
                if (topTask.Source == "HOSTING_SPACE"
                    && topTask.TaskName == "ADD" && topTask.ItemId > 0)
                {
                    SendAddPackageNotification();
                }
                // Send hosting package add notification
                if (topTask.Source == "HOSTING_SPACE_WR"
                    && topTask.TaskName == "ADD" && topTask.ItemId > 0)
                {
                    SendAddPackageWithResourcesNotification();
                }
            }
        }

        private void CheckSmtpResult(int resultCode)
        {
            if (resultCode != 0)
            {
                TaskManager.WriteWarning("Unable to send an e-mail notification");
                TaskManager.WriteParameter("SMTP Result", resultCode);
            }
        }

        protected void SendAddPackageWithResourcesNotification()
        {
            try
            {
                BackgroundTask topTask = TaskManager.TopTask;
                
                bool sendLetter = Utils.ParseBool(topTask.GetParamValue("SendLetter"), false);

                if (sendLetter)
                {
                    int sendResult = PackageController.SendPackageSummaryLetter(topTask.ItemId, null, null, true);
                    CheckSmtpResult(sendResult);
                }
            }
            catch (Exception ex)
            {
                TaskManager.WriteWarning(ex.StackTrace);
            }
        }

        protected void SendAddPackageNotification()
        {
            try
            {
                BackgroundTask topTask = TaskManager.TopTask;
                
                int userId = Utils.ParseInt(topTask.GetParamValue("UserId").ToString(), 0);
                bool sendLetter = Utils.ParseBool(topTask.GetParamValue("SendLetter"), false);
                bool signup = Utils.ParseBool(topTask.GetParamValue("Signup"), false);

                // send space letter if enabled
                UserSettings settings = UserController.GetUserSettings(userId, UserSettings.PACKAGE_SUMMARY_LETTER);
                if (sendLetter
                    && !String.IsNullOrEmpty(settings["EnableLetter"])
                    && Utils.ParseBool(settings["EnableLetter"], false))
                {
                    // send letter
                    int smtpResult = PackageController.SendPackageSummaryLetter(topTask.ItemId, null, null, signup);
                    CheckSmtpResult(smtpResult);
                }
            }
            catch (Exception ex)
            {
                TaskManager.WriteWarning(ex.StackTrace);
            }
        }

        protected void SendAddUserNotification()
        {
            try
            {
                BackgroundTask topTask = TaskManager.TopTask;

                bool sendLetter = Utils.ParseBool(topTask.GetParamValue("SendLetter"), false);

                int userId = topTask.ItemId;
                // send account letter if enabled
                UserSettings settings = UserController.GetUserSettings(userId, UserSettings.ACCOUNT_SUMMARY_LETTER);
                if (sendLetter
                    && !String.IsNullOrEmpty(settings["EnableLetter"])
                    && Utils.ParseBool(settings["EnableLetter"], false))
                {
                    // send letter
                    int smtpResult = PackageController.SendAccountSummaryLetter(userId, null, null, true);
                    CheckSmtpResult(smtpResult);
                }
            }
            catch (Exception ex)
            {
                TaskManager.WriteWarning(ex.StackTrace);
            }
        }
    }
}
