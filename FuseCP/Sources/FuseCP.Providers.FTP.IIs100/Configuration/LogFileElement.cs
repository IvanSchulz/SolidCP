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

    internal class LogFileElement : ConfigurationElement
    {
        public string CustomLogPluginClsid
        {
            get
            {
                return (string) base["customLogPluginClsid"];
            }
            set
            {
                base["customLogPluginClsid"] = value;
            }
        }

        public string Directory
        {
            get
            {
                return (string) base["directory"];
            }
            set
            {
                base["directory"] = value;
            }
        }

        public bool Enabled
        {
            get
            {
                return (bool) base["enabled"];
            }
            set
            {
                base["enabled"] = value;
            }
        }

        public bool LocalTimeRollover
        {
            get
            {
                return (bool) base["localTimeRollover"];
            }
            set
            {
                base["localTimeRollover"] = value;
            }
        }

        public FtpLogExtFileFlags LogExtFileFlags
        {
            get
            {
                return (FtpLogExtFileFlags) base["logExtFileFlags"];
            }
            set
            {
                base["logExtFileFlags"] = (int) value;
            }
        }

        public LoggingRolloverPeriod Period
        {
            get
            {
                return (LoggingRolloverPeriod) base["period"];
            }
            set
            {
                base["period"] = (int) value;
            }
        }

        public long TruncateSize
        {
            get
            {
                return (long) base["truncateSize"];
            }
            set
            {
                base["truncateSize"] = value;
            }
        }
    }
}

