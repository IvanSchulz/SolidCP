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
using System.Net;
using System.ComponentModel;

using System.Collections;
using System.Collections.Specialized;
using System.Text;

namespace FuseCP.Providers.FTP
{
	[Serializable]
	public class FtpSite : ServiceProviderItem
	{
		private string siteId;
        private ServerBinding[] bindings = new ServerBinding[0];

		private bool allowAnonymous;
		private bool allowExecuteAccess;
		private bool allowScriptAccess;
		private bool allowSourceAccess;
		private bool allowReadAccess;
		private bool allowWriteAccess;
		private string anonymousUsername;
		private string anonymousUserPassword;
		private string contentPath;
		private string logFileDirectory;
		private bool anonymousOnly;

		public const string MSFTP7_SITE_ID = "MsFtp7_SiteId";
		public const string MSFTP7_LOG_EXT_FILE_FIELDS = "MsFtp7_LogExtFileFields";

		public FtpSite()
		{
		}

		[Persistent]
		public string SiteId
		{
			set { siteId = value; }
			get { return siteId; }
		}

        public ServerBinding[] Bindings
		{
			set { bindings = value; }
			get { return bindings; }
		}
			
		public bool AllowScriptAccess
		{
			get { return allowScriptAccess; }
			set { allowScriptAccess = value; }
		}

		public bool AllowSourceAccess
		{
			get { return allowSourceAccess; }
			set { allowSourceAccess = value; }
		}

	
		public bool AllowReadAccess
		{
			get { return allowReadAccess; }
			set { allowReadAccess = value; }
		}

		public bool AllowWriteAccess
		{
			get { return allowWriteAccess; }
			set { allowWriteAccess = value; }
		}

		public string LogFileDirectory
		{
			get { return logFileDirectory; }
			set { logFileDirectory = value; }
		}
		
		public string AnonymousUsername
		{
			get { return anonymousUsername; }
			set { anonymousUsername = value; }
		}

		public string AnonymousUserPassword
		{
			get { return anonymousUserPassword; }
			set { anonymousUserPassword = value; }
		}
	
		public bool AllowAnonymous
		{
			get { return allowAnonymous; }
			set { allowAnonymous = value; }
		}
		
		public bool AllowExecuteAccess
		{
			get { return allowExecuteAccess; }
			set { allowExecuteAccess = value; }
		}

		public string ContentPath
		{
			get { return contentPath; }
			set { contentPath = value; }
		}

		public bool AnonymousOnly
		{
			set { anonymousOnly = value; }
			get { return anonymousOnly; }
		}
	}
}
