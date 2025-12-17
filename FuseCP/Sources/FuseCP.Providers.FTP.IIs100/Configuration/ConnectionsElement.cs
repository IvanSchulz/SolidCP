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

    internal class ConnectionsElement : ConfigurationElement
    {
        public int ControlChannelTimeout
        {
            get
            {
                return (int) base["controlChannelTimeout"];
            }
            set
            {
                base["controlChannelTimeout"] = value;
            }
        }

        public int DataChannelTimeout
        {
            get
            {
                return (int) base["dataChannelTimeout"];
            }
            set
            {
                base["dataChannelTimeout"] = value;
            }
        }

        public bool DisableSocketPooling
        {
            get
            {
                return (bool) base["disableSocketPooling"];
            }
            set
            {
                base["disableSocketPooling"] = value;
            }
        }

        public long MaxConnections
        {
            get
            {
                return (long) base["maxConnections"];
            }
            set
            {
                base["maxConnections"] = value;
            }
        }

        public int MinBytesPerSecond
        {
            get
            {
                return (int) base["minBytesPerSecond"];
            }
            set
            {
                base["minBytesPerSecond"] = value;
            }
        }

        public bool ResetOnMaxConnections
        {
            get
            {
                return (bool) base["resetOnMaxConnections"];
            }
            set
            {
                base["resetOnMaxConnections"] = value;
            }
        }

        public int ServerListenBacklog
        {
            get
            {
                return (int) base["serverListenBacklog"];
            }
            set
            {
                base["serverListenBacklog"] = value;
            }
        }

        public int UnauthenticatedTimeout
        {
            get
            {
                return (int) base["unauthenticatedTimeout"];
            }
            set
            {
                base["unauthenticatedTimeout"] = value;
            }
        }
    }
}

