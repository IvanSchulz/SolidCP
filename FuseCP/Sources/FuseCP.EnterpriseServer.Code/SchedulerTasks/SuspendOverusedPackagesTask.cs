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
    public class SuspendOverusedPackagesTask : SchedulerTask
    {
        public const string DISKSPACE_FORMAT_STRING = "disk space usage - {0}%";
        public const string BANDWIDTH_FORMAT_STRING = "bandwidth usage - {0}%";

        public override void DoWork()
        {
            // Input parameters:
            //  - DISKSPACE_OVERUSED
            //  - BANDWIDTH_OVERUSED

            BackgroundTask topTask = TaskManager.TopTask;

            // get the list of all packages
            List<PackageInfo> packages = PackageController.GetPackagePackages(topTask.PackageId, false);
            TaskManager.Write("Packages to verify: " + packages.Count.ToString());

            bool checkDiskspace = (String.Compare((string)topTask.GetParamValue("DISKSPACE_OVERUSED"), "true", true) == 0);
            bool checkBandwidth = (String.Compare((string)topTask.GetParamValue("BANDWIDTH_OVERUSED"), "true", true) == 0);

			bool suspendOverused = Convert.ToBoolean(topTask.GetParamValue("SUSPEND_OVERUSED"));

			bool sendWarningEmail = Convert.ToBoolean(topTask.GetParamValue("SEND_WARNING_EMAIL"));
			bool sendSuspensionEmail = Convert.ToBoolean(topTask.GetParamValue("SEND_SUSPENSION_EMAIL"));
			int warningUsageThreshold = Convert.ToInt32(topTask.GetParamValue("WARNING_USAGE_THRESHOLD"));
			int suspensionUsageThreshold = Convert.ToInt32(topTask.GetParamValue("SUSPENSION_USAGE_THRESHOLD"));
			string warningMailFrom = Convert.ToString(topTask.GetParamValue("WARNING_MAIL_FROM"));
			string warningMailBcc = Convert.ToString(topTask.GetParamValue("WARNING_MAIL_BCC"));
			string warningMailSubject = Convert.ToString(topTask.GetParamValue("WARNING_MAIL_SUBJECT"));
			string warningMailBody = Convert.ToString(topTask.GetParamValue("WARNING_MAIL_BODY"));
			string suspensionMailFrom = Convert.ToString(topTask.GetParamValue("SUSPENSION_MAIL_FROM"));
			string suspensionMailBcc = Convert.ToString(topTask.GetParamValue("SUSPENSION_MAIL_BCC"));
			string suspensionMailSubject = Convert.ToString(topTask.GetParamValue("SUSPENSION_MAIL_SUBJECT"));
			string suspensionMailBody = Convert.ToString(topTask.GetParamValue("SUSPENSION_MAIL_BODY"));

            int suspendedPackages = 0;

            foreach (PackageInfo package in packages)
            {
				bool isBandwidthBelowWarningThreshold = true;
				bool isBandwidthBelowSuspensionThreshold = true;
				bool isDiskSpaceBelowWarningThreshold = true;
				bool isDiskSpaceBelowSuspensionThreshold = true;
				
				UserInfo userInfo = UserController.GetUser(package.UserId);

            	int diskSpaceUsage = 0;
            	int bandwidthUsage = 0;
                // disk space
                if (checkDiskspace)
                {
                    QuotaValueInfo dsQuota = PackageController.GetPackageQuota(package.PackageId, Quotas.OS_DISKSPACE);
					if (dsQuota.QuotaAllocatedValue > 0)
					{
						diskSpaceUsage = (dsQuota.QuotaUsedValue*100/dsQuota.QuotaAllocatedValue);
						isDiskSpaceBelowWarningThreshold = diskSpaceUsage < warningUsageThreshold;
						isDiskSpaceBelowSuspensionThreshold = diskSpaceUsage < suspensionUsageThreshold;
					}
                }

                // bandwidth
                if (checkBandwidth)
                {
                    QuotaValueInfo bwQuota = PackageController.GetPackageQuota(package.PackageId, Quotas.OS_BANDWIDTH);
					if (bwQuota.QuotaAllocatedValue > 0)
					{
						bandwidthUsage = (bwQuota.QuotaUsedValue*100/bwQuota.QuotaAllocatedValue);
						isBandwidthBelowWarningThreshold = bandwidthUsage < warningUsageThreshold;
						isBandwidthBelowSuspensionThreshold = bandwidthUsage < suspensionUsageThreshold;
					}
                }

            	string userName = String.Format("{0} {1} ({2})/{3}", userInfo.FirstName, userInfo.LastName, userInfo.Username, userInfo.Email);
                //
                List<string> formatItems = new List<string>();
                // add diskspace usage if enabled
                if (checkDiskspace) formatItems.Add(String.Format(DISKSPACE_FORMAT_STRING, diskSpaceUsage));
                // add bandwidth usage if enabled
                if (checkBandwidth) formatItems.Add(String.Format(BANDWIDTH_FORMAT_STRING, bandwidthUsage));
                // build usage string
                string usage = String.Join(", ", formatItems.ToArray());

                // cleanup items
                formatItems.Clear();
                // add diskspace warning max usage
                if (checkDiskspace) formatItems.Add(String.Format(DISKSPACE_FORMAT_STRING, warningUsageThreshold));
                // add bandwidth warning max usage
                if (checkBandwidth) formatItems.Add(String.Format(BANDWIDTH_FORMAT_STRING, warningUsageThreshold));
                // build warning max usage string
                string warningMaxUsage = String.Join(", ", formatItems.ToArray());

                // cleanup items
                formatItems.Clear();
                // add diskspace suspension max usage
                if (checkDiskspace) formatItems.Add(String.Format(DISKSPACE_FORMAT_STRING, suspensionUsageThreshold));
                // add bandwidth suspension max usage
                if (checkBandwidth) formatItems.Add(String.Format(BANDWIDTH_FORMAT_STRING, suspensionUsageThreshold));
                // build suspension max usage string
                string suspensionMaxUsage = String.Join(", ", formatItems.ToArray());

				string warningMailSubjectProcessed = ReplaceVariables(warningMailSubject, warningMaxUsage, usage, package.PackageName, userName);
				string warningMailBodyProcessed = ReplaceVariables(warningMailBody, warningMaxUsage, usage, package.PackageName, userName);

				string suspensionMailSubjectProcessed = ReplaceVariables(suspensionMailSubject, suspensionMaxUsage, usage, package.PackageName, userName);
				string suspensionMailBodyProcessed = ReplaceVariables(suspensionMailBody, suspensionMaxUsage, usage, package.PackageName, userName);


				// Send email notifications
				if (sendWarningEmail && (!isDiskSpaceBelowWarningThreshold || !isBandwidthBelowWarningThreshold))
				{
					// Send warning email.
					this.SendEmail(warningMailFrom, userInfo.Email, warningMailBcc, warningMailSubjectProcessed, warningMailBodyProcessed, false);
				}

				if (sendSuspensionEmail && (!isDiskSpaceBelowSuspensionThreshold || !isBandwidthBelowSuspensionThreshold))
				{
					// Send suspension email.
					this.SendEmail(suspensionMailFrom, userInfo.Email, suspensionMailBcc, suspensionMailSubjectProcessed, suspensionMailBodyProcessed, false);
				}

            	// suspend package if required
				if (suspendOverused && (!isDiskSpaceBelowSuspensionThreshold || !isBandwidthBelowSuspensionThreshold))
                {
                    suspendedPackages++;
                    
                    // load user details
                    UserInfo user = PackageController.GetPackageOwner(package.PackageId);

                    TaskManager.Write(String.Format("Suspend space '{0}' of user '{1}'",
                        package.PackageName, user.Username));

                    try
                    {
                        PackageController.ChangePackageStatus(null, package.PackageId, PackageStatus.Suspended, false);
                    }
                    catch (Exception ex)
                    {
                        TaskManager.WriteError("Error while changing space status: " + ex.ToString());
                    }
                }
            }

            // log results
            TaskManager.Write("Total packages suspended: " + suspendedPackages.ToString());
        }

		private string ReplaceVariables(string content, string threshold, string usage, string spaceName, string customerName)
		{
			if (!String.IsNullOrEmpty(content))
			{
				content = Utils.ReplaceStringVariable(content, "threshold", threshold);
				content = Utils.ReplaceStringVariable(content, "date", DateTime.Now.ToString());
				content = Utils.ReplaceStringVariable(content, "usage", usage);
				content = Utils.ReplaceStringVariable(content, "space", spaceName);
				content = Utils.ReplaceStringVariable(content, "customer", customerName);
			}
			return content;
		}

    	private void SendEmail(string from, string to, string bcc, string subject, string body, bool isHtml)
		{
			// check input parameters
			if (String.IsNullOrEmpty(from))
			{
				TaskManager.WriteWarning("Specify 'Mail From' task parameter");
				return;
			}

			if (String.IsNullOrEmpty(to))
			{
				TaskManager.WriteWarning("Specify 'Mail To' task parameter");
				return;
			}

			// send mail message
			MailHelper.SendMessage(from, to, bcc, subject, body, isHtml);
		}
    }
}
