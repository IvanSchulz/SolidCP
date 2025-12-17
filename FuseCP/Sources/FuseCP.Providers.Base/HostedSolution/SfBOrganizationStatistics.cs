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
using System.Linq;
using System.Text;

namespace FuseCP.Providers.HostedSolution
{
    public class SfBOrganizationStatistics
    {
        private int allocatedSfBUsers;
        private int createdSfBUsers;

        private int allocatedSfBEVUsers;
        private int createdSfBEVUsers;


        public int AllocatedSfBUsers
        {
            get { return this.allocatedSfBUsers; }
            set { this.allocatedSfBUsers = value; }
        }

        public int CreatedSfBUsers
        {
            get { return this.createdSfBUsers; }
            set { this.createdSfBUsers = value; }
        }


        public int AllocatedSfBEVUsers
        {
            get { return this.allocatedSfBEVUsers; }
            set { this.allocatedSfBEVUsers = value; }
        }

        public int CreatedSfBEVUsers
        {
            get { return this.createdSfBEVUsers; }
            set { this.createdSfBEVUsers = value; }
        }


    }
}
