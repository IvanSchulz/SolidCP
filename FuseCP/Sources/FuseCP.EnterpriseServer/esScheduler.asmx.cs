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
using FuseCP.EnterpriseServer.Base.Scheduling;

namespace FuseCP.EnterpriseServer
{
    /// <summary>
    /// Summary description for esApplicationsInstaller
    /// </summary>
    [WebService(Namespace = "http://smbsaas/fusecp/enterpriseserver")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [Policy("EnterpriseServerPolicy")]
    [ToolboxItem(false)]
    public class esScheduler: WebService
    {
        [WebMethod]
        public DateTime GetSchedulerTime()
        {
            return SchedulerController.GetSchedulerTime();
        }

        [WebMethod]
        public List<ScheduleTaskInfo> GetScheduleTasks()
        {
            return SchedulerController.GetScheduleTasks();
        }

		//[WebMethod]
		//public ScheduleTaskInfo GetScheduleTask(string taskId)
		//{
		//    return SchedulerController.GetScheduleTask(taskId);
		//}

    	[WebMethod]
        public DataSet GetSchedules(int userId)
        {
            return SchedulerController.GetSchedules(userId);
        }

        [WebMethod]
        public DataSet GetSchedulesPaged(int packageId, bool recursive,
            string filterColumn, string filterValue, string sortColumn, int startRow, int maximumRows)
        {
            return SchedulerController.GetSchedulesPaged(packageId,
                recursive, filterColumn, filterValue, sortColumn, startRow, maximumRows);
        }

        [WebMethod]
        public ScheduleInfo GetSchedule(int scheduleId)
        {
            return SchedulerController.GetSchedule(scheduleId);
        }

        [WebMethod]
        public List<ScheduleTaskParameterInfo> GetScheduleParameters(string taskId, int scheduleId)
        {
            return SchedulerController.GetScheduleParameters(taskId, scheduleId);
        }

		[WebMethod]
		public List<ScheduleTaskViewConfiguration> GetScheduleTaskViewConfigurations(string taskId)
		{
			return SchedulerController.GetScheduleTaskViewConfigurations(taskId);
		}

		[WebMethod]
		public ScheduleTaskViewConfiguration GetScheduleTaskViewConfiguration(string taskId, string environment)
		{
			List<ScheduleTaskViewConfiguration> configurations = SchedulerController.GetScheduleTaskViewConfigurations(taskId);
			return configurations.Find(delegate(ScheduleTaskViewConfiguration configuration)
			                           	{
			                           		return configuration.Environment == environment;
			                           	});
		}


    	[WebMethod]
        public int StartSchedule(int scheduleId)
        {
            return SchedulerController.StartSchedule(scheduleId);
        }

        [WebMethod]
        public int StopSchedule(int scheduleId)
        {
            return SchedulerController.StopSchedule(scheduleId);
        }

        [WebMethod]
        public int AddSchedule(ScheduleInfo schedule)
        {
            return SchedulerController.AddSchedule(schedule);
        }

        [WebMethod]
        public int UpdateSchedule(ScheduleInfo schedule)
        {
            return SchedulerController.UpdateSchedule(schedule);
        }

        [WebMethod]
        public int DeleteSchedule(int scheduleId)
        {
            return SchedulerController.DeleteSchedule(scheduleId);
        }
    }
}
