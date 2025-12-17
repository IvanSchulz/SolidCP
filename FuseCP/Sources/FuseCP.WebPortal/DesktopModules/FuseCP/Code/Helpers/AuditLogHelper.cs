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
    /// Summary description for AuditLogHelper
    /// </summary>
    public class AuditLogHelper
    {
        DataSet dsLog;

        public int GetAuditLogRecordsPagedCount(string sStartDate, string sEndDate, int packageId, int itemId, string itemName,
            int severityId, string sourceName, string taskName)
        {
            return (int)dsLog.Tables[0].Rows[0][0];
        }

        public DataTable GetAuditLogRecordsPaged(int maximumRows, int startRowIndex, string sortColumn,
            string sStartDate, string sEndDate, int packageId, int itemId, string itemName, int severityId, string sourceName, string taskName)
        {
            dsLog = ES.Services.AuditLog.GetAuditLogRecordsPaged(
                PanelSecurity.SelectedUserId, packageId, itemId, itemName, DateTime.Parse(sStartDate), DateTime.Parse(sEndDate),
                severityId, sourceName, taskName, sortColumn, startRowIndex, maximumRows);
            return dsLog.Tables[1];
        }
    }
}
