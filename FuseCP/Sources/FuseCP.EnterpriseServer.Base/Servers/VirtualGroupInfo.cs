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
    public class VirtualGroupInfo
    {
        private int virtualGroupId;

        public int VirtualGroupId
        {
            get { return virtualGroupId; }
            set { virtualGroupId = value; }
        }
        private int serverId;

        public int ServerId
        {
            get { return serverId; }
            set { serverId = value; }
        }
        private int groupId;

        public int GroupId
        {
            get { return groupId; }
            set { groupId = value; }
        }
        private bool primaryDistribution;

        public bool PrimaryDistribution
        {
            get { return primaryDistribution; }
            set { primaryDistribution = value; }
        }
        private int distributionType;

        public int DistributionType
        {
            get { return distributionType; }
            set { distributionType = value; }
        }
        private bool bindDistributionToPrimary;

        public bool BindDistributionToPrimary
        {
            get { return bindDistributionToPrimary; }
            set { bindDistributionToPrimary = value; }
        }
    }
}
