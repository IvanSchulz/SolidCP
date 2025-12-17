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
using System.Xml.Serialization;
using System.Runtime.Serialization;

namespace FuseCP.EnterpriseServer
{
    public class ScheduleInfo
    {
        private int scheduleId;
        private string taskId;
        private int packageId;
        private string scheduleName;
        private string scheduleTypeId;
        private int interval;
        private DateTime fromTime;
        private DateTime toTime;
        private DateTime startTime;
        private DateTime lastRun;
        private DateTime nextRun;
        private bool enabled;
        private string statusId;
        private ScheduleTaskParameterInfo[] parameters;
        private string priorityId;
        private int historiesNumber;
        private int maxExecutionTime;
        private int weekMonthDay;

        public int ScheduleId
        {
            get { return this.scheduleId; }
            set { this.scheduleId = value; }
        }

        public string TaskId
        {
            get { return this.taskId; }
            set { this.taskId = value; }
        }

        public int PackageId
        {
            get { return this.packageId; }
            set { this.packageId = value; }
        }

        public string ScheduleTypeId
        {
            get { return this.scheduleTypeId; }
            set { this.scheduleTypeId = value; }
        }

        public string ScheduleName
        {
            get { return this.scheduleName; }
            set { this.scheduleName = value; }
        }

        [XmlIgnore, IgnoreDataMember]
        public ScheduleType ScheduleType
        {
            get { return (ScheduleType)Enum.Parse(typeof(ScheduleType), scheduleTypeId, true); }
            set { scheduleTypeId = value.ToString(); }
        }

        public int Interval
        {
            get { return this.interval; }
            set { this.interval = value; }
        }

        public System.DateTime FromTime
        {
            get { return this.fromTime; }
            set { this.fromTime = value; }
        }

        public System.DateTime ToTime
        {
            get { return this.toTime; }
            set { this.toTime = value; }
        }

        public System.DateTime StartTime
        {
            get { return this.startTime; }
            set { this.startTime = value; }
        }

        public System.DateTime LastRun
        {
            get { return this.lastRun; }
            set { this.lastRun = value; }
        }

        public System.DateTime NextRun
        {
            get { return this.nextRun; }
            set { this.nextRun = value; }
        }

        public bool Enabled
        {
            get { return this.enabled; }
            set { this.enabled = value; }
        }

        public string StatusId
        {
            get { return this.statusId; }
            set { this.statusId = value; }
        }

        [XmlIgnore, IgnoreDataMember]
        public ScheduleStatus Status
        {
            get { return (ScheduleStatus)Enum.Parse(typeof(ScheduleStatus), statusId, true); }
            set { statusId = value.ToString(); }
        }

        public ScheduleTaskParameterInfo[] Parameters
        {
            get { return this.parameters; }
            set { this.parameters = value; }
        }

        public int WeekMonthDay
        {
            get { return this.weekMonthDay; }
            set { this.weekMonthDay = value; }
        }

        public string PriorityId
        {
            get { return this.priorityId; }
            set { this.priorityId = value; }
        }

        [XmlIgnore, IgnoreDataMember]
        public SchedulePriority Priority
        {
            get { return (SchedulePriority)Enum.Parse(typeof(SchedulePriority), priorityId, true); }
            set { priorityId = value.ToString(); }
        }

        public int HistoriesNumber
        {
            get { return this.historiesNumber; }
            set { this.historiesNumber = value; }
        }

        public int MaxExecutionTime
        {
            get { return this.maxExecutionTime; }
            set { this.maxExecutionTime = value; }
        }
    }
}
