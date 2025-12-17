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
using System.Collections.ObjectModel;
using System.Linq;
using System.Management.Automation;
using System.Management.Automation.Runspaces;
using System.Text;
using System.Threading.Tasks;

namespace FuseCP.Providers.Virtualization
{
    public static class JobHelper
    {
        public static JobResult CreateSuccessResult(ReturnCode returnCode = ReturnCode.OK)
        {
            return new JobResult
            {
                Job = new ConcreteJob {JobState = ConcreteJobState.Completed},
                ReturnValue = returnCode
            };
        }

        public static JobResult CreateResultFromPSResults(Collection<PSObject> objJob)
        {
            if (objJob == null || objJob.Count == 0)
                return null;

            JobResult result = new JobResult();

            result.Job = CreateFromPSObject(objJob);
            result.ReturnValue = ReturnCode.JobStarted;

            switch (result.Job.JobState)
            {
                case ConcreteJobState.Failed:
                    result.ReturnValue = ReturnCode.Failed;
                    break;
            }

            return result;
        }

        public static ConcreteJob CreateFromPSObject(Collection<PSObject> objJob)
        {
            if (objJob == null || objJob.Count == 0)
                return null;

            ConcreteJob job = new ConcreteJob();
            job.Id = objJob[0].GetProperty<int>("Id").ToString();
            job.JobState = objJob[0].GetEnum<ConcreteJobState>("JobStateInfo");
            job.Caption = objJob[0].GetProperty<string>("Name");
            job.Description = objJob[0].GetProperty<string>("Command");
            job.StartTime = objJob[0].GetProperty<DateTime>("PSBeginTime");
            job.ElapsedTime = objJob[0].GetProperty<DateTime?>("PSEndTime") ?? DateTime.Now;

            // PercentComplete
            job.PercentComplete = 0;
            var progress = (PSDataCollection<ProgressRecord>)objJob[0].GetProperty("Progress");
            if (progress != null && progress.Count > 0)
                job.PercentComplete = progress[0].PercentComplete;

            // Errors
            var errors = (PSDataCollection<ErrorRecord>)objJob[0].GetProperty("Error");
            if (errors != null && errors.Count > 0)
            {
                job.ErrorDescription = errors[0].ErrorDetails.Message + ". " + errors[0].ErrorDetails.RecommendedAction;
                job.ErrorCode = errors[0].Exception != null ? -1 : 0;
            }

            return job;
        }
        
    }
}
