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

namespace FuseCP.LinuxVmConfig
{
    [Serializable]
    public sealed class ExecutionContext
    {
        public string ActivityID { get; set; }
        public string ActivityName { get; set; }
        public string ActivityDefinition { get; set; }

        private string activityDescription;
        public string ActivityDescription
        {
            get
            {
                return activityDescription;
            }
            set
            {
                activityDescription = value;
                SaveState();
            }
        }

        private int progress = 0;
        public int Progress
        {
            get
            {
                return progress;
            }
            set
            {
                progress = value;
                if (progress < 0)
                    progress = 0;
                if (progress > 100)
                    progress = 100;
                SaveState();
            }
        }

        private Dictionary<string, string> parameters = new Dictionary<string, string>();
        public Dictionary<string, string> Parameters
        {
            get { return parameters; }
        }

        private void SaveState()
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendFormat("ActivityDefinition={0}|", ActivityDefinition);
            builder.AppendFormat("ActivityDescription={0}|", ActivityDescription);
            builder.AppendFormat("Progress={0}", Progress);
            //SetKvpStringValue(OutputKVP, "FCP-CurrentTask", builder.ToString());
        }
    }
}
