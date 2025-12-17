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

﻿using System;

namespace FuseCP.Providers.HostedSolution
{
    public class CRMOrganizationStatistics : BaseStatistics
    {
        public Guid CRMOrganizationId { get; set; }
        public string CRMUserName { get; set; }
        public CRMUserAccessMode ClientAccessMode { get; set; }
		public bool CRMDisabled { get; set; }

        public int CRMUsersCount { get; set; }
        public string AccountNumber { get; set; }
        public string СRMOrganizationName { get; set; }
        public int CRMUsersFullLicenceCount { get; set; }
        public int CRMUsersReadOnlyLicenceCount { get; set; }
        public int CRMUsersESSLicenceCount { get; set; }
        public int UsedSpace { get; set; }
        public string UsageMonth { get; set; }

    }
}
