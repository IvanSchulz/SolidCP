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

namespace FuseCP.Providers.FTP.IIs100.Config
{
    using Microsoft.Web.Administration;
    using System;

    internal class LoggingElement : ConfigurationElement
    {
        private LogFileElement _logFile;

        public bool CentralLogFile
        {
            get
            {
                return (bool) base["centralLogFile"];
            }
            set
            {
                base["centralLogFile"] = value;
            }
        }

        public LogFileElement LogFile
        {
            get
            {
                if (this._logFile == null)
                {
                    this._logFile = (LogFileElement) base.GetChildElement("logFile", typeof(LogFileElement));
                }
                return this._logFile;
            }
        }

        public bool LogInUTF8
        {
            get
            {
                return (bool) base["logInUTF8"];
            }
            set
            {
                base["logInUTF8"] = value;
            }
        }

        public SelectiveLogging SelectiveLogging
        {
            get
            {
                return (SelectiveLogging) base["selectiveLogging"];
            }
            set
            {
                base["selectiveLogging"] = (int) value;
            }
        }
    }
}

