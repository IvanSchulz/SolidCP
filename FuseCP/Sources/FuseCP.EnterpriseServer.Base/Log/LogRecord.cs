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
    public class LogRecord
    {
	    string recordID;
	    int severityID;
	    int userID;
	    string username;
	    int itemID;
	    string sourceName;
	    DateTime startDate;
	    DateTime finishDate;
	    string taskName;
	    string itemName;
	    string executionLog;

        public string RecordID
        {
            get { return this.recordID; }
            set { this.recordID = value; }
        }

        public int SeverityID
        {
            get { return this.severityID; }
            set { this.severityID = value; }
        }

        public int UserID
        {
            get { return this.userID; }
            set { this.userID = value; }
        }

        public string Username
        {
            get { return this.username; }
            set { this.username = value; }
        }

        public int ItemID
        {
            get { return this.itemID; }
            set { this.itemID = value; }
        }

        public string SourceName
        {
            get { return this.sourceName; }
            set { this.sourceName = value; }
        }

        public System.DateTime StartDate
        {
            get { return this.startDate; }
            set { this.startDate = value; }
        }

        public System.DateTime FinishDate
        {
            get { return this.finishDate; }
            set { this.finishDate = value; }
        }

        public string TaskName
        {
            get { return this.taskName; }
            set { this.taskName = value; }
        }

        public string ItemName
        {
            get { return this.itemName; }
            set { this.itemName = value; }
        }

        public string ExecutionLog
        {
            get { return this.executionLog; }
            set { this.executionLog = value; }
        }
    }
}
