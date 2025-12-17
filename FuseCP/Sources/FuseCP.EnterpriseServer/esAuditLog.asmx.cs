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
using System.Web;
using System.Collections;
using System.Collections.Generic;
using FuseCP.Web.Services;
using System.ComponentModel;

namespace FuseCP.EnterpriseServer
{
    /// <summary>
    /// Summary description for esApplicationsInstaller
    /// </summary>
    [WebService(Namespace = "http://smbsaas/fusecp/enterpriseserver")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [Policy("EnterpriseServerPolicy")]
    [ToolboxItem(false)]
    public class esAuditLog: WebService
    {
        [WebMethod]
        public DataSet GetAuditLogRecordsPaged(int userId, int packageId, int itemId, string itemName, DateTime startDate, DateTime endDate,
            int severityId, string sourceName, string taskName, string sortColumn, int startRow, int maximumRows)
        {
            return AuditLog.GetAuditLogRecordsPaged(userId, packageId, itemId, itemName, startDate, endDate,
                severityId, sourceName, taskName, sortColumn, startRow, maximumRows);
        }

        [WebMethod]
        public DataSet GetAuditLogSources()
        {
            return AuditLog.GetAuditLogSources();
        }

        [WebMethod]
        public DataSet GetAuditLogTasks(string sourceName)
        {
            return AuditLog.GetAuditLogTasks(sourceName);
        }

        [WebMethod]
        public LogRecord GetAuditLogRecord(string recordId)
        {
            return AuditLog.GetAuditLogRecord(recordId);
        }

        [WebMethod]
        public int DeleteAuditLogRecords(int userId, int itemId, string itemName,
            DateTime startDate, DateTime endDate, int severityId, string sourceName, string taskName)
        {
            return AuditLog.DeleteAuditLogRecords(userId, itemId, itemName, startDate, endDate, severityId, sourceName, taskName);
        }

        [WebMethod]
        public int DeleteAuditLogRecordsComplete()
        {
            return AuditLog.DeleteAuditLogRecordsComplete();
        }
    }
}
