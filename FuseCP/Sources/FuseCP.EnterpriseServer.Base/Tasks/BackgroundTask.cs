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
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace FuseCP.EnterpriseServer
{

    public class BackgroundTask
    {
        #region Fields

        public List<BackgroundTaskParameter> Params = new List<BackgroundTaskParameter>();
        
        public List<BackgroundTaskLogRecord> Logs = new List<BackgroundTaskLogRecord>();

        #endregion

        #region Properties

        public int Id { get; set; }

        public Guid Guid { get; set; }

        public string TaskId { get; set; }

        public int ScheduleId { get; set; }

        public int PackageId { get; set; }

        public int UserId { get; set; }

        public int EffectiveUserId { get; set; }

        public string TaskName { get; set; }

        public int ItemId { get; set; }

        public string ItemName { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime FinishDate { get; set; }

        public int IndicatorCurrent { get; set; }

        public int IndicatorMaximum { get; set; }

        public int MaximumExecutionTime { get; set; }

        public string Source { get; set; }

        public int Severity { get; set; }

        public bool Completed { get; set; }

        public bool NotifyOnComplete { get; set; }

        public BackgroundTaskStatus Status { get; set; }

        #endregion

        #region Constructors

        public BackgroundTask()
        {
            StartDate = DateTime.Now;
            Severity = 0;
            IndicatorCurrent = 0;
            IndicatorMaximum = 0;
            Status = BackgroundTaskStatus.Run;

            Completed = false;
            NotifyOnComplete = false;
        }

        public BackgroundTask(Guid guid, String taskId, int userId, int effectiveUserId, String source, String taskName, String itemName,
            int itemId, int scheduleId, int packageId, int maximumExecutionTime, List<BackgroundTaskParameter> parameters)
            : this()
        {
            Guid = guid;
            TaskId = taskId;
            UserId = userId;
            EffectiveUserId = effectiveUserId;
            Source = source;
            TaskName = taskName;
            ItemName = itemName;
            ItemId = itemId;
            ScheduleId = scheduleId;
            PackageId = packageId;
            MaximumExecutionTime = maximumExecutionTime;
            Params = parameters;
        }

        #endregion

        #region Methods

        public List<BackgroundTaskLogRecord> GetLogs()
        {
            return Logs;
        }

        public Object GetParamValue(String name)
        {
            foreach(BackgroundTaskParameter param in Params)
            {
                if (param.Name == name)
                    return param.Value;
            }

            return null;
        }

        public void UpdateParamValue(String name, object value)
        {
            foreach (BackgroundTaskParameter param in Params)
            {
                if (param.Name == name)
                {
                    param.Value = value;

                    return;

                }
            }

            Params.Add(new BackgroundTaskParameter(name, value));
        }

        public bool ContainsParam(String name)
        {
            foreach (BackgroundTaskParameter param in Params)
            {
                if (param.Name == name)
                    return true;
            }

            return false;
        }

        #endregion
    }

    public class BackgroundTaskParameter
    {
        #region Properties

        public int ParameterId { get; set; }

        public int TaskId { get; set; }

        public String Name { get; set; }

        public Object Value { get; set; }

        public String TypeName { get; set; }

        public String SerializerValue { get; set; }

        #endregion

        #region Constructors

        public BackgroundTaskParameter() { }

        public BackgroundTaskParameter(String name, Object value)
        {
            Name = name;
            Value = value;
        }

        #endregion
    }
}
