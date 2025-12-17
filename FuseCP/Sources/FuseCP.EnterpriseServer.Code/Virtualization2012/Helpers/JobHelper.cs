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

using FuseCP.Providers.Virtualization;
//using FuseCP.Providers.Virtualization2012;
using FuseCP.Server.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FuseCP.EnterpriseServer.Code.Virtualization2012.Helpers
{
    public class JobHelper: ControllerBase
    {
        public JobHelper(ControllerBase provider) : base(provider) { }

        public bool TryJobCompleted(VirtualizationServer2012 vs, ConcreteJob job, bool resetProgressBarIndicatorAfterFinish = true, bool isPowerShellJob = false)
        {
            bool jobCompleted = false;
            short timeout = 5;
            while (timeout > 0)
            {
                timeout--;
                try
                {
                    jobCompleted = JobCompleted(vs, job, resetProgressBarIndicatorAfterFinish, isPowerShellJob);
                }
                catch (ThreadAbortException) //https://github.com/FuseCP/FuseCP/issues/103
                {
                    //maybe there need to use Thread.ResetAbort(); ???

                    TaskManager.Write("VPS_CREATE_TRY_JOB_COMPLETE_ATTEMPTS_LEFT_AFTER_THREAD_ABORT", timeout.ToString());
                    job = GetLastJobUpdate(vs, job, isPowerShellJob); //get the last job state

                    jobCompleted = (job.JobState == ConcreteJobState.Completed); //is job completed?                                      
                }

                if (jobCompleted)
                {
                    timeout = 0;
                }
            }

            return jobCompleted;
        }

        public bool JobCompleted(VirtualizationServer2012 vs, ConcreteJob job, bool resetProgressBarIndicatorAfterFinish = true, bool isPowerShellJob = false)
        {
            TaskManager.IndicatorMaximum = 100;
            bool jobCompleted = true;
            short timeout = 60;
            while (job.JobState == ConcreteJobState.NotStarted && timeout > 0) //Often jobs are only initialized, need to wait a little, that it started.
            {
                timeout--;
                Thread.Sleep(2000);
                job = GetLastJobUpdate(vs, job, isPowerShellJob);
            }

            while (job.JobState == ConcreteJobState.Starting ||
                job.JobState == ConcreteJobState.Running)
            {
                Thread.Sleep(3000);
                job = GetLastJobUpdate(vs, job, isPowerShellJob);
                TaskManager.IndicatorCurrent = job.PercentComplete;
            }

            if (job.JobState != ConcreteJobState.Completed)
            {
                jobCompleted = false;
            }
            if (resetProgressBarIndicatorAfterFinish)
            {
                TaskManager.IndicatorCurrent = 0;   // reset indicator
            }

            if (isPowerShellJob)
            {
                vs.ClearOldPsJobs();
            }

            return jobCompleted;
        }

        public static ConcreteJob GetLastJobUpdate(VirtualizationServer2012 vs, ConcreteJob job, bool isPowerShellJob = false)
        {
            return isPowerShellJob ? vs.GetPsJob(job.Id) : vs.GetJob(job.Id);
        }
    }
}
