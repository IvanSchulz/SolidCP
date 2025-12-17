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
	using System.Runtime.InteropServices;
	using System.Threading;

    internal class FtpSite : ConfigurationElement
    {
        private ConnectionsElement _connections;
        private DataChannelSecurityElement _dataChannelSecurity;
        private DirectoryBrowseElement _directoryBrowse;
        private FileHandlingElement _fileHandling;
        private FirewallElement _firewall;
        private ConfigurationMethod _flushLogMethod;
		private LogFileElement _logFile;
        private MessagesElement _messages;
        private SecurityElement _security;
        private SessionCollection _sessions;
        private ConfigurationMethod _startMethod;
        private ConfigurationMethod _stopMethod;
        private UserIsolationElement _userIsolation;
		public const int E_NOT_FOUND = -2147023728;
		public const int E_OBJECT_NOT_EXIST = -2147020584;
		private const uint ERROR_ALREADY_EXISTS = 2147942583;
		private string siteServiceId;

		public string SiteServiceId
		{
			get { return siteServiceId; }
			set { siteServiceId = value; }
		}

        public void FlushLog()
        {
            if (this._flushLogMethod == null)
            {
                this._flushLogMethod = base.Methods["FlushLog"];
            }
            this._flushLogMethod.CreateInstance().Execute();
        }

        public void Start()
        {
            if (this._startMethod == null)
            {
                this._startMethod = base.Methods["Start"];
            }
            this._startMethod.CreateInstance().Execute();
        }

        public void Stop()
        {
            if (this._stopMethod == null)
            {
                this._stopMethod = base.Methods["Stop"];
            }
            this._stopMethod.CreateInstance().Execute();
        }

        public bool AllowUTF8
        {
            get
            {
                return (bool) base["allowUTF8"];
            }
            set
            {
                base["allowUTF8"] = value;
            }
        }

        public ConnectionsElement Connections
        {
            get
            {
                if (this._connections == null)
                {
                    this._connections = (ConnectionsElement) base.GetChildElement("connections", typeof(ConnectionsElement));
                }
                return this._connections;
            }
        }

        public DataChannelSecurityElement DataChannelSecurity
        {
            get
            {
                if (this._dataChannelSecurity == null)
                {
                    this._dataChannelSecurity = (DataChannelSecurityElement) base.GetChildElement("dataChannelSecurity", typeof(DataChannelSecurityElement));
                }
                return this._dataChannelSecurity;
            }
        }

        public DirectoryBrowseElement DirectoryBrowse
        {
            get
            {
                if (this._directoryBrowse == null)
                {
                    this._directoryBrowse = (DirectoryBrowseElement) base.GetChildElement("directoryBrowse", typeof(DirectoryBrowseElement));
                }
                return this._directoryBrowse;
            }
        }

        public FileHandlingElement FileHandling
        {
            get
            {
                if (this._fileHandling == null)
                {
                    this._fileHandling = (FileHandlingElement) base.GetChildElement("fileHandling", typeof(FileHandlingElement));
                }
                return this._fileHandling;
            }
        }

		public FirewallElement FirewallSupport
        {
            get
            {
                if (this._firewall == null)
                {
					this._firewall = (FirewallElement)base.GetChildElement("firewallSupport", typeof(FirewallElement));
                }
                return this._firewall;
            }
        }

        public uint LastStartupStatus
        {
            get
            {
                return (uint) base["lastStartupStatus"];
            }
            set
            {
                base["lastStartupStatus"] = value;
            }
        }

		public LogFileElement LogFile
		{
			get
			{
				if (this._logFile == null)
				{
					this._logFile = (LogFileElement)base.GetChildElement("logFile", typeof(LogFileElement));
				}
				return this._logFile;
			}
		}

        public MessagesElement Messages
        {
            get
            {
                if (this._messages == null)
                {
                    this._messages = (MessagesElement) base.GetChildElement("messages", typeof(MessagesElement));
                }
                return this._messages;
            }
        }

        public SecurityElement Security
        {
            get
            {
                if (this._security == null)
                {
                    this._security = (SecurityElement) base.GetChildElement("security", typeof(SecurityElement));
                }
                return this._security;
            }
        }

        public bool ServerAutoStart
        {
            get
            {
                return (bool) base["serverAutoStart"];
            }
            set
            {
                base["serverAutoStart"] = value;
            }
        }

        public SessionCollection Sessions
        {
            get
            {
                if (this._sessions == null)
                {
                    this._sessions = (SessionCollection) base.GetCollection("sessions", typeof(SessionCollection));
                }
                return this._sessions;
            }
        }

        public SiteState State
        {
            get
            {
				SiteState unknown = SiteState.Unknown;
				int num = 0;
				bool flag = false;
				while (!flag && (++num < 10))
				{
					try
					{
						unknown = (SiteState)base["state"];
						flag = true;
						continue;
					}
					catch (COMException exception)
					{
						if (exception.ErrorCode != -2147020584)
						{
							return unknown;
						}
						Thread.Sleep(100);
						continue;
					}
				}
				return unknown;
            }
            set
            {
                base["state"] = (int) value;
            }
        }

        public UserIsolationElement UserIsolation
        {
            get
            {
                if (this._userIsolation == null)
                {
                    this._userIsolation = (UserIsolationElement) base.GetChildElement("userIsolation", typeof(UserIsolationElement));
                }
                return this._userIsolation;
            }
        }
    }
}

