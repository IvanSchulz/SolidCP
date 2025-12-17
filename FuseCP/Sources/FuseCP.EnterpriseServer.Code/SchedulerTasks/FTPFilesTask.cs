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
using System.IO;
using System.Collections.Generic;
using System.Text;

using FuseCP.Server.Client;

namespace FuseCP.EnterpriseServer
{
    public class FTPFilesTask : SchedulerTask
    {
        public override void DoWork()
        {
            // Input parameters:
            //  - FILE_PATH
            //  - FTP_SERVER
            //  - FTP_USERNAME
            //  - FTP_PASSWORD
            //  - FTP_FOLDER

            BackgroundTask topTask = TaskManager.TopTask;

			// get input parameters
			string username = (string)topTask.GetParamValue("USERNAME");
			string password = (string)topTask.GetParamValue("PASSWORD");
			string filePath = (string)topTask.GetParamValue("FILE_PATH");
            string ftpServer = (string)topTask.GetParamValue("FTP_SERVER");
            string ftpUsername = (string)topTask.GetParamValue("FTP_USERNAME");
            string ftpPassword = (string)topTask.GetParamValue("FTP_PASSWORD");
            string ftpFolder = (string)topTask.GetParamValue("FTP_FOLDER");

            // check input parameters
            if (String.IsNullOrEmpty(filePath))
            {
                TaskManager.WriteWarning("Specify 'File' task parameter");
                return;
            }

            if (String.IsNullOrEmpty(ftpServer))
            {
                TaskManager.WriteWarning("Specify 'FTP Server' task parameter");
                return;
            }

            // substitute parameters
            DateTime d = DateTime.Now;
            string date = d.ToString("yyyyMMdd");
            string time = d.ToString("HHmm");

            filePath = Utils.ReplaceStringVariable(filePath, "date", date);
            filePath = Utils.ReplaceStringVariable(filePath, "time", time);

            // build FTP command file
            StringBuilder sb = new StringBuilder();
            StringWriter writer = new StringWriter(sb);

            // FTP server
            writer.WriteLine("open " + ftpServer);

            // check if anonymous mode
            if (String.IsNullOrEmpty(ftpUsername))
            {
                ftpUsername = "anonymous";
                ftpPassword = "anonymous@email.com";
            }

            // FTP username/password
            writer.WriteLine(ftpUsername);
            writer.WriteLine(ftpPassword);

            // check if we need to change remote folder
            if (!String.IsNullOrEmpty(ftpFolder))
            {
                writer.WriteLine("cd " + ftpFolder.Replace("\\", "/"));
            }

            // file to send
            writer.WriteLine("binary");
            writer.WriteLine("put " + FilesController.GetFullPackagePath(topTask.PackageId, filePath));

            // bye
            writer.WriteLine("bye");

            string cmdBatch = sb.ToString();

            // create temp file in user space
            string cmdPath = Utils.GetRandomString(10) + ".txt";
            string fullCmdPath = FilesController.GetFullPackagePath(topTask.PackageId, cmdPath);

            // upload batch
            FilesController.UpdateFileBinaryContent(topTask.PackageId, cmdPath, Encoding.UTF8.GetBytes(cmdBatch));

            // execute system command
            // load OS service
            int serviceId = PackageController.GetPackageServiceId(topTask.PackageId, ResourceGroups.Os);

            // load service
            ServiceInfo service = ServerController.GetServiceInfo(serviceId);
            if (service == null)
                return;

            Server.Client.OperatingSystem winServer = new Server.Client.OperatingSystem();
            ServiceProviderProxy.ServerInit(winServer, service.ServerId);
            //TODO implement this as a method of OperatingSystem
            TaskManager.Write(winServer.ExecuteSystemCommand(username, password, "ftp.exe", "-s:" + fullCmdPath));

            // delete batch file
            FilesController.DeleteFiles(topTask.PackageId, new string[] { cmdPath });
        }
    }
}
