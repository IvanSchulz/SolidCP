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
using System.Web;
using System.Collections;
using System.Collections.Generic;
using FuseCP.Web.Services;
using System.ComponentModel;

namespace FuseCP.EnterpriseServer
{
    /// <summary>
    /// Summary description for esTasks
    /// </summary>
    [WebService(Namespace = "http://smbsaas/fusecp/enterpriseserver")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [Policy("EnterpriseServerPolicy")]
    [ToolboxItem(false)]
    public class esTasks: WebService
    {
        [WebMethod]
        public BackgroundTask GetTask(string taskId)
        {
            return TaskManager.GetTask(taskId);
        }

        [WebMethod]
        public BackgroundTask GetTaskWithLogRecords(string taskId, DateTime startLogTime)
        {
            return TaskManager.GetTaskWithLogRecords(taskId, startLogTime);
        }

        [WebMethod]
        public int GetTasksNumber()
        {
            return TaskManager.GetTasksNumber();
        }

        [WebMethod]
        public List<BackgroundTask> GetUserTasks(int userId)
        {
            return TaskManager.GetUserTasks(userId);
        }

        [WebMethod]
        public List<BackgroundTask> GetUserCompletedTasks(int userId)
        {
            return TaskManager.GetUserCompletedTasks(userId);
        }

        [WebMethod]
        public void SetTaskNotifyOnComplete(string taskId)
        {
            TaskManager.SetTaskNotifyOnComplete(taskId);
        }

        [WebMethod]
        public void StopTask(string taskId)
        {
            TaskManager.StopTask(taskId);
        }
    }
}
