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
using System.Data;

namespace FuseCP.Portal
{
    /// <summary>
    /// Summary description for ReportsHelper
    /// </summary>
    public class ReportsHelper
    {
        #region Bandwidth Report
        DataSet dsBandwidthReport;

        public int GetPackagesBandwidthPagedCount(int packageId, string sStartDate, string sEndDate)
        {
            return (int)dsBandwidthReport.Tables[0].Rows[0][0];
        }

        public DataTable GetPackagesBandwidthPaged(int packageId, int maximumRows, int startRowIndex, string sortColumn,
            string sStartDate, string sEndDate)
        {

            dsBandwidthReport = ES.Services.Packages.GetPackagesBandwidthPaged(PanelSecurity.SelectedUserId,
                packageId, DateTime.Parse(sStartDate), DateTime.Parse(sEndDate),
                sortColumn, startRowIndex, maximumRows);
            return dsBandwidthReport.Tables[1];
        }
        #endregion

        #region DiskSpace Report
        DataSet dsDiskspaceReport;

        public int GetPackagesDiskspacePagedCount(int packageId)
        {
            return (int)dsDiskspaceReport.Tables[0].Rows[0][0];
        }

        public DataTable GetPackagesDiskspacePaged(int packageId, int maximumRows, int startRowIndex, string sortColumn)
        {

            dsDiskspaceReport = ES.Services.Packages.GetPackagesDiskspacePaged(
                PanelSecurity.SelectedUserId, packageId,
                sortColumn, startRowIndex, maximumRows);
            return dsDiskspaceReport.Tables[1];
        }
        #endregion
    }
}
