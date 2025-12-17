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

    internal class SessionElement : ConfigurationElement
    {
        private ConfigurationMethod _terminateMethod;

        public void Terminate()
        {
            if (this._terminateMethod == null)
            {
                this._terminateMethod = base.Methods["Terminate"];
            }
            this._terminateMethod.CreateInstance().Execute();
        }

        public long BytesReceived
        {
            get
            {
                return (long) base["bytesReceived"];
            }
            set
            {
                base["bytesReceived"] = value;
            }
        }

        public long BytesSent
        {
            get
            {
                return (long) base["bytesSent"];
            }
            set
            {
                base["bytesSent"] = value;
            }
        }

        public string CommandStartTime
        {
            get
            {
                return (string) base["commandStartTime"];
            }
            set
            {
                base["commandStartTime"] = value;
            }
        }

        public string CurrentCommand
        {
            get
            {
                return (string) base["currentCommand"];
            }
            set
            {
                base["currentCommand"] = value;
            }
        }

        public long LastErrorStatus
        {
            get
            {
                return (long) base["lastErrorStatus"];
            }
            set
            {
                base["lastErrorStatus"] = value;
            }
        }

        public string PreviousCommand
        {
            get
            {
                return (string) base["previousCommand"];
            }
            set
            {
                base["previousCommand"] = value;
            }
        }

        public long SessionId
        {
            get
            {
                return (long) base["sessionId"];
            }
            set
            {
                base["sessionId"] = value;
            }
        }

        public string SessionStartTime
        {
            get
            {
                return (string) base["sessionStartTime"];
            }
            set
            {
                base["sessionStartTime"] = value;
            }
        }

        public string UserName
        {
            get
            {
                return (string) base["userName"];
            }
            set
            {
                base["userName"] = value;
            }
        }
    }
}

