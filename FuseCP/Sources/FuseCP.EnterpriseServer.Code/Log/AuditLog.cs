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
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Text;
using System.IO;
using System.Xml;
using FuseCP.EnterpriseServer.Data;

namespace FuseCP.EnterpriseServer
{
    public class AuditLog: ControllerBase
    {
        public AuditLog(ControllerBase provider) : base(provider) { }

        public DataSet GetAuditLogRecordsPaged(int userId, int packageId, int itemId, string itemName, DateTime startDate, DateTime endDate,
            int severityId, string sourceName, string taskName, string sortColumn, int startRow, int maximumRows)
        {
            return Database.GetAuditLogRecordsPaged(SecurityContext.User.UserId,
                userId, packageId, itemId, itemName, GetStartDate(startDate), GetEndDate(endDate),
                severityId, sourceName, taskName, sortColumn, startRow, maximumRows);
        }

        public DataSet GetAuditLogSources()
        {
            return Database.GetAuditLogSources();
        }

        public DataSet GetAuditLogTasks(string sourceName)
        {
            return Database.GetAuditLogTasks(sourceName);
        }

        public LogRecord GetAuditLogRecord(string recordId)
        {
            return ObjectUtils.FillObjectFromDataReader<LogRecord>(
                Database.GetAuditLogRecord(recordId));
        }

        public int DeleteAuditLogRecords(int userId, int itemId, string itemName,
            DateTime startDate, DateTime endDate, int severityId, string sourceName, string taskName)
        {
            // check account
            int accountCheck = SecurityContext.CheckAccount(DemandAccount.NotDemo);
            if (accountCheck < 0) return accountCheck;

            Database.DeleteAuditLogRecords(SecurityContext.User.UserId,
                userId, itemId, itemName, GetStartDate(startDate), GetEndDate(endDate), severityId, sourceName, taskName);

            return 0;
        }

        public int DeleteAuditLogRecordsComplete()
        {
            // check account
            int accountCheck = SecurityContext.CheckAccount(DemandAccount.NotDemo | DemandAccount.IsAdmin);
            if (accountCheck < 0) return accountCheck;

            Database.DeleteAuditLogRecordsComplete();

            return 0;
        }

        public void AddAuditLogInfoRecord(string sourceName, string taskName, string itemName, 
            string[] executionValues, int packageId = 0, int itemId = 0)
        {

            AddAuditLogRecord(0, sourceName, taskName, itemName, executionValues);
        }

        public void AddAuditLogWarningRecord(string sourceName, string taskName, string itemName,
            string[] executionValues, int packageId = 0, int itemId = 0)
        {

            AddAuditLogRecord(1, sourceName, taskName, itemName, executionValues);
        }

        public void AddAuditLogErrorRecord(string sourceName, string taskName, string itemName,
            string[] executionValues, int packageId = 0, int itemId = 0)
        {

            AddAuditLogRecord(2, sourceName, taskName, itemName, executionValues);
        }

        static ConcurrentDictionary<int, string> UsersCache = new ConcurrentDictionary<int, string>(); 
        public void AddAuditLogRecord(int severityId, string sourceName, string taskName, string itemName,
            string[] executionValues, int packageId = 0, int itemId = 0)
        {
            string recordId = Guid.NewGuid().ToString("N");
            DateTime startAndfinishDate = DateTime.Now; //because it is an immediate action.
            var user = SecurityContext.User;

            int userId = user.OwnerId == 0
                             ? user.UserId
                             : user.OwnerId;

            int effectiveUserId = user.UserId;
			string username;
			if (!UsersCache.TryGetValue(effectiveUserId, out username))
			{
				UserInfo userInternal = UserController.GetUserInternally(effectiveUserId);
				username = userInternal != null ? userInternal.Username : null;
				if (username != null) UsersCache.AddOrUpdate(effectiveUserId, username, (key, oldValue) => username);
			}

			AddAuditLogRecord(recordId, severityId, userId, username, packageId, itemId, itemName,
                    startAndfinishDate, startAndfinishDate, sourceName, taskName, DummyFormatExecutionLog(startAndfinishDate, 0, executionValues));
        }

        public void AddAuditLogRecord(string recordId, int severityId, int userId, string username,
            int packageId, int itemId, string itemName, DateTime startDate, DateTime finishDate,
            string sourceName, string taskName, string executionLog)
        {
            try
            {
                using (var database = new DataProvider()) database.AddAuditLogRecord(recordId, severityId, userId, username, packageId, itemId, itemName,
                    startDate, finishDate, sourceName, taskName, executionLog);
            }
            catch { }
        }

        private DateTime GetStartDate(DateTime d)
        {
            return new DateTime(d.Year, d.Month, d.Day, 0, 0, 0);
        }

        private DateTime GetEndDate(DateTime d)
        {
            return new DateTime(d.Year, d.Month, d.Day, 23, 59, 59);
        }

        //extremely simple and dummy creating XML string.
        private string DummyFormatExecutionLog(DateTime startDate, int severityId, string[] values)
        {
            StringWriter sw = new StringWriter();
            XmlWriter writer = new XmlTextWriter(sw);

            writer.WriteStartElement("log");

            // parameters
            writer.WriteStartElement("parameters");
            writer.WriteEndElement(); // parameters

            // records
            writer.WriteStartElement("records");
            foreach (string value in values)
            {
                writer.WriteStartElement("record");
                writer.WriteAttributeString("severity", severityId.ToString());
                writer.WriteAttributeString("date", startDate.ToString(System.Globalization.CultureInfo.InvariantCulture));
                writer.WriteAttributeString("ident", "0"); //because it DB it write only with 0

                // text
                writer.WriteElementString("text", value);

                // stack trace
                writer.WriteElementString("stackTrace", null);
                writer.WriteEndElement();
            }
            writer.WriteEndElement();
            writer.WriteEndElement();

            return sw.ToString();
        }
    }
}
