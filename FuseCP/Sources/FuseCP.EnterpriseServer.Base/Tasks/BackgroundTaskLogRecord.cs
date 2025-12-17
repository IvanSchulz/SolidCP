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
    public class BackgroundTaskLogRecord
    {
        #region Properties

        public int LogId { get; set; }

        public int TaskId { get; set; }

        public DateTime Date { get; set; }

        public String ExceptionStackTrace { get; set; }

        public bool InnerTaskStart { get; set; }

        public int Severity { get; set; }

        public String Text { get; set; }

        public int TextIdent { get; set; }

        public string[] TextParameters { get; set; }

        public string XmlParameters { get; set; }

        #endregion

        #region Constructors

        public BackgroundTaskLogRecord()
        {
            Date = DateTime.Now;
        }

        public BackgroundTaskLogRecord(int taskId, int textIdent, bool innerTaskStart, String text, string[] textParameters)
            : this()
        {
            TaskId = taskId;
            TextIdent = textIdent;
            Text = text;
            InnerTaskStart = innerTaskStart;
            TextParameters = textParameters;
        }

        public BackgroundTaskLogRecord(int taskId, int textIdent, bool innerTaskStart, String text,
                                       String exceptionStackTrace, string[] textParameters)
            : this(taskId, textIdent, innerTaskStart, text, textParameters)
        {
            ExceptionStackTrace = exceptionStackTrace;
        }

        #endregion
    }
}
