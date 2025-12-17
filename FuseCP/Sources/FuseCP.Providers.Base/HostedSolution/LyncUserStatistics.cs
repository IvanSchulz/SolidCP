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

namespace FuseCP.Providers.HostedSolution
{
    public class LyncUserStatistics : BaseStatistics
    {
        public string DisplayName { get; set; }
        public DateTime AccountCreated { get; set; }
        public string SipAddress { get; set; }
        public bool InstantMessaing{ get; set; }
        public bool MobileAccess { get; set; }
        public bool Federation { get; set; }
        public bool Conferencing { get; set; }
        public bool EnterpriseVoice { get; set; }
        public string EVPolicy { get; set; }
        public string PhoneNumber { get; set; }
        public string LyncUserPlan { get; set; }
    }
}
