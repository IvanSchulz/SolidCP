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

namespace FuseCP.UniversalInstaller.Web
{
	/// <summary>
	/// Virtual directory item.
	/// </summary>
	[Serializable]
	public class WebVirtualDirectoryItem
	{
		private bool allowExecuteAccess;
		private bool allowScriptAccess;
		private bool allowSourceAccess;
		private bool allowReadAccess;
		private bool allowWriteAccess;
		private string anonymousUsername;
		private string anonymousUserPassword;
		private string contentPath;
		private bool allowDirectoryBrowsingAccess;	
		private bool authAnonymous;
		private bool authWindows;
		private bool authBasic;
		private string defaultDocs;
		private string httpRedirect;
		private string name;
        private AspNetVersion installedDotNetFramework;
		private string applicationPool;

		/// <summary>
		/// Initializes a new instance of the class.
		/// </summary>
		public WebVirtualDirectoryItem()
		{
		}

		/// <summary>
		/// Name
		/// </summary>
		public string Name
		{
			get { return name; }
			set { name = value; }
		}
			
		/// <summary>
		/// Allow script access
		/// </summary>
		public bool AllowScriptAccess
		{
			get { return allowScriptAccess; }
			set { allowScriptAccess = value; }
		}

		/// <summary>
		/// Allow source access
		/// </summary>
		public bool AllowSourceAccess
		{
			get { return allowSourceAccess; }
			set { allowSourceAccess = value; }
		}

	
		/// <summary>
		/// Allow read access
		/// </summary>
		public bool AllowReadAccess
		{
			get { return allowReadAccess; }
			set { allowReadAccess = value; }
		}

		/// <summary>
		/// Allow write access
		/// </summary>
		public bool AllowWriteAccess
		{
			get { return allowWriteAccess; }
			set { allowWriteAccess = value; }
		}
		
		/// <summary>
		/// Anonymous user name
		/// </summary>
		public string AnonymousUsername
		{
			get { return anonymousUsername; }
			set { anonymousUsername = value; }
		}

		/// <summary>
		/// Anonymous user password
		/// </summary>
		public string AnonymousUserPassword
		{
			get { return anonymousUserPassword; }
			set { anonymousUserPassword = value; }
		}
		
		/// <summary>
		/// Allow execute access
		/// </summary>
		public bool AllowExecuteAccess
		{
			get { return allowExecuteAccess; }
			set { allowExecuteAccess = value; }
		}

		/// <summary>
		/// Content path
		/// </summary>
		public string ContentPath
		{
			get { return contentPath; }
			set { contentPath = value; }
		}
	
		/// <summary>
		/// Http redirect
		/// </summary>
		public string HttpRedirect
		{
			get { return httpRedirect; }
			set { httpRedirect = value; }
		}

		/// <summary>
		/// Default documents
		/// </summary>
		public string DefaultDocs
		{
			get { return defaultDocs; }
			set { defaultDocs = value; }
		}

		/// <summary>
		/// Allow directory browsing access
		/// </summary>
		public bool AllowDirectoryBrowsingAccess
		{
			get { return allowDirectoryBrowsingAccess; }
			set { allowDirectoryBrowsingAccess = value; }
		}

		/// <summary>
		/// Anonymous access.
		/// </summary>
		public bool AuthAnonymous
		{
			get { return this.authAnonymous; }
			set { this.authAnonymous = value; }
		}

		/// <summary>
		/// Basic authentication.
		/// </summary>
		public bool AuthBasic
		{
			get { return this.authBasic; }
			set { this.authBasic = value; }
		}

		/// <summary>
		/// Integrated Windows authentication.
		/// </summary>
		public bool AuthWindows
		{
			get { return this.authWindows; }
			set { this.authWindows = value; }
		}

        /// <summary>
        /// Installed ASP.NET version
        /// </summary>
        public AspNetVersion InstalledDotNetFramework
        {
            get { return this.installedDotNetFramework; }
            set { this.installedDotNetFramework = value; }
        }

		/// <summary>
		/// Application pool
		/// </summary>
		public string ApplicationPool
		{
			get { return applicationPool; }
			set { applicationPool = value; }
		}
	}
}

