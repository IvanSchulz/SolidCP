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

namespace FuseCP.EnterpriseServer
{
    [Serializable]
    public class HostingPlanGroupInfo
    {
        int groupId;
        string groupName;
        bool enabled;
        bool calculateDiskSpace;
        bool calculateBandwidth;

        public int GroupId
        {
            get { return groupId; }
            set { groupId = value; }
        }

        public bool Enabled
        {
            get { return enabled; }
            set { enabled = value; }
        }

        public bool CalculateDiskSpace
        {
            get { return calculateDiskSpace; }
            set { calculateDiskSpace = value; }
        }

        public bool CalculateBandwidth
        {
            get { return calculateBandwidth; }
            set { calculateBandwidth = value; }
        }

        public string GroupName
        {
            get { return this.groupName; }
            set { this.groupName = value; }
        }
    }
}
