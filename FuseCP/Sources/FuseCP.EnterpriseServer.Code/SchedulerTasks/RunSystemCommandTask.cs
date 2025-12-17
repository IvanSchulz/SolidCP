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

using FuseCP.Server.Client;

namespace FuseCP.EnterpriseServer
{
    public class RunSystemCommandTask : SchedulerTask
    {
        public override void DoWork()
        {
            // Input parameters:
            //  - SERVER_NAME
            //  - EXECUTABLE_PATH

            BackgroundTask topTask = TaskManager.TopTask;

            // get input parameters
            string serverName = (string)topTask.GetParamValue("SERVER_NAME");
            string username = (string)topTask.GetParamValue("USERNAME");
            string password = (string)topTask.GetParamValue("PASSWORD");
            string execPath = (string)topTask.GetParamValue("EXECUTABLE_PATH");
            string execParams = (string)topTask.GetParamValue("EXECUTABLE_PARAMS");

            if (execParams == null)
                execParams = "";

            // check input parameters
            if (String.IsNullOrEmpty(serverName))
            {
                TaskManager.WriteWarning("Specify 'Server Name' task parameter");
                return;
            }

            if (String.IsNullOrEmpty(execPath))
            {
                TaskManager.WriteWarning("Specify 'Executable Path' task parameter");
                return;
            }

            // find server by name
            ServerInfo server = ServerController.GetServerByName(serverName);
            if (server == null)
            {
                TaskManager.WriteWarning(String.Format("Server with the name '{0}' was not found", serverName));
                return;
            }

            // execute system command
            Server.Client.OperatingSystem winServer = new Server.Client.OperatingSystem();
            ServiceProviderProxy.ServerInit(winServer, server.ServerId);
            TaskManager.Write(winServer.ExecuteSystemCommand(username, password, execPath, execParams));
        }
    }
}
