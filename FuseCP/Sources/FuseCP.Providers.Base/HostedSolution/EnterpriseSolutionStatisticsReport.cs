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

﻿namespace FuseCP.Providers.HostedSolution
{
    public class EnterpriseSolutionStatisticsReport
    {
        public ExchangeStatisticsReport ExchangeReport { get; set; }
        public SharePointStatisticsReport SharePointReport { get; set; }
        public SharePointEnterpriseStatisticsReport SharePointEnterpriseReport { get; set; }
        public CRMStatisticsReport CRMReport { get; set; }
        public OrganizationStatisticsReport OrganizationReport { get; set; }
        public LyncStatisticsReport LyncReport { get; set; }
        public SfBStatisticsReport SfBReport { get; set; }
    }
}
