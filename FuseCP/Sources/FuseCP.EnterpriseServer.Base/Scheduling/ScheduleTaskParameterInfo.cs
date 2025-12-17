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
    public class ScheduleTaskParameterInfo
    {
        private string taskId;
        private string parameterId;
        private string dataTypeId;
        private string defaultValue;
        private string parameterValue;

        public string TaskId
        {
            get { return this.taskId; }
            set { this.taskId = value; }
        }

        public string ParameterId
        {
            get { return this.parameterId; }
            set { this.parameterId = value; }
        }

        public string DataTypeId
        {
            get { return this.dataTypeId; }
            set { this.dataTypeId = value; }
        }

        [XmlIgnore, IgnoreDataMember]
        public ScheduleTaskParameterType DataType
        {
            get { return (ScheduleTaskParameterType)Enum.Parse(typeof(ScheduleTaskParameterType), dataTypeId, true); }
            set { dataTypeId = value.ToString(); }
        }

        public string DefaultValue
        {
            get { return this.defaultValue; }
            set { this.defaultValue = value; }
        }

        public string ParameterValue
        {
            get { return this.parameterValue; }
            set { this.parameterValue = value; }
        }
    }
}
