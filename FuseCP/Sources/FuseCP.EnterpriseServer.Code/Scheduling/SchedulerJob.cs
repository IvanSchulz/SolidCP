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
using System.Threading;
using System.Collections.Generic;
using System.Text;

namespace FuseCP.EnterpriseServer
{
    public class SchedulerJob: ControllerAsyncBase
    {
        private ScheduleInfo scheduleInfo;
        private ScheduleTaskInfo task;

        public ScheduleFinished ScheduleFinishedCallback;

        #region public properties
        public ScheduleInfo ScheduleInfo
        {
            get { return this.scheduleInfo; }
            set { this.scheduleInfo = value; }
        }

        public ScheduleTaskInfo Task
        {
            get { return this.task; }
            set { this.task = value; }
        }
        #endregion

        // Sets the next time this Schedule is kicked off and kicks off events on
        // a seperate thread, freeing the Scheduler to continue
        public void Run()
        {
            // create worker
            Thread worker = new Thread(new ThreadStart(RunSchedule));
            // set worker priority
            switch (scheduleInfo.Priority)
            {
                case SchedulePriority.Highest: worker.Priority = ThreadPriority.Highest; break;
                case SchedulePriority.AboveNormal: worker.Priority = ThreadPriority.AboveNormal; break;
                case SchedulePriority.Normal: worker.Priority = ThreadPriority.Normal; break;
                case SchedulePriority.BelowNormal: worker.Priority = ThreadPriority.BelowNormal; break;
                case SchedulePriority.Lowest: worker.Priority = ThreadPriority.Lowest; break;
            }

            // start worker!
            worker.Start();
        }

        // Implementation of ThreadStart delegate.
        // Used by Scheduler to kick off events on a seperate thread
        private void RunSchedule()
        {
            // impersonate thread
            UserInfo user = PackageController.GetPackageOwner(scheduleInfo.PackageId);
            SecurityContext.SetThreadPrincipal(user.UserId);

            List<BackgroundTaskParameter> parameters = new List<BackgroundTaskParameter>();
            foreach (ScheduleTaskParameterInfo prm in scheduleInfo.Parameters)
            {
                parameters.Add(new BackgroundTaskParameter(prm.ParameterId, prm.ParameterValue));
            }

            TaskManager.StartTask("SCHEDULER", "RUN_SCHEDULE", scheduleInfo.ScheduleName, scheduleInfo.ScheduleId,
                                  scheduleInfo.ScheduleId, scheduleInfo.PackageId, scheduleInfo.MaxExecutionTime,
                                  parameters);

            // run task
            try
            {
                // create scheduled task object
                ISchedulerTask objTask = (ISchedulerTask)Activator.CreateInstance(Type.GetType(task.TaskType));

                if (objTask != null)
                    objTask.DoWork();
                else
                    throw new Exception(String.Format("Could not create scheduled task of '{0}' type",
                        task.TaskType));      
               // Thread.Sleep(40000);
            }
            catch (Exception ex)
            {
                // log error
                TaskManager.WriteError(ex, "Error executing scheduled task");
            }
            finally
            {
                // complete task
                try
                {
                    TaskManager.CompleteTask();
                }
                catch (Exception)
                {
                }
            }
        }
    }
}
