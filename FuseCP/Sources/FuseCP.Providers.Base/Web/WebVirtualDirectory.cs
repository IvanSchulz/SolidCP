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
using System.Text;
using System.Xml.Serialization;
using System.Runtime.Serialization;

namespace FuseCP.Providers.Web
{
	public enum WebServerType { Iis, Apache, Nginx };

	public class WebVirtualDirectory : ServiceProviderItem
	{
		#region Web Management Service Constants

		public const string WmSvcAvailable = "WmSvcAvailable";
		public const string WmSvcSiteEnabled = "WmSvcSiteEnabled";
		public const string WmSvcAccountName = "WmSvcAccountName";
		public const string WmSvcAccountPassword = "WmSvcAccountPassword";
		public const string WmSvcServiceUrl = "WmSvcServiceUrl";
		public const string WmSvcServicePort = "WmSvcServicePort";
		public const string WmSvcDefaultPort = "8172";

		#endregion
		private string siteId;
		private string anonymousUsername;
		private string anonymousUserPassword;
		private string contentPath;
		private bool enableWritePermissions;
		private bool enableParentPaths;
		private bool enableDirectoryBrowsing;
		private bool enableAnonymousAccess;
		private bool enableWindowsAuthentication;
		private bool enableBasicAuthentication;
		private bool enableDynamicCompression;
		private bool enableStaticCompression;
		private string parentSiteName;
		private bool iis7;

		[Persistent]
		public string SiteId
		{
			set { siteId = value; }
			get { return siteId; }
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

		public string ContentPath
		{
			get { return contentPath; }
			set { contentPath = value; }
		}


		public bool EnableParentPaths
		{
			get { return this.enableParentPaths; }
			set { this.enableParentPaths = value; }
		}

		public bool EnableWritePermissions
		{
			get { return this.enableWritePermissions; }
			set { this.enableWritePermissions = value; }
		}

		public bool EnableDirectoryBrowsing
		{
			get { return this.enableDirectoryBrowsing; }
			set { this.enableDirectoryBrowsing = value; }
		}

		public bool EnableAnonymousAccess
		{
			get { return this.enableAnonymousAccess; }
			set { this.enableAnonymousAccess = value; }
		}

		public bool EnableWindowsAuthentication
		{
			get { return this.enableWindowsAuthentication; }
			set { this.enableWindowsAuthentication = value; }
		}

		public bool EnableBasicAuthentication
		{
			get { return this.enableBasicAuthentication; }
			set { this.enableBasicAuthentication = value; }
		}

		public bool EnableDynamicCompression
		{
			get { return this.enableDynamicCompression; }
			set { this.enableDynamicCompression = value; }
		}
		public bool EnableStaticCompression
		{
			get { return this.enableStaticCompression; }
			set { this.enableStaticCompression = value; }
		}

		public string ParentSiteName
		{
			get { return this.parentSiteName; }
			set { this.parentSiteName = value; }
		}
		public bool IIs7
		{
			get { return this.iis7; }
			set { this.iis7 = value; }
		}

		/// <summary>
		/// Gets fully qualified name which consists of parent website name if present and virtual directory name.
		/// </summary>
		[XmlIgnore, IgnoreDataMember]
		public string VirtualPath
		{
			get
			{
				return String.Format("/{0}", Name);
			}
		}

		/// <summary>
		/// Gets fully qualified name which consists of parent website name if present and virtual directory name.
		/// </summary>
		[XmlIgnore, IgnoreDataMember]
		public string FullQualifiedPath
		{
			get
			{
				if (String.IsNullOrEmpty(ParentSiteName))
					return Name;
				else if (Name.StartsWith("/"))
					return ParentSiteName + Name;
				else
					return ParentSiteName + "/" + Name;
			}
		}

		[DataMember]
		public WebServerType WebServerType { get; set; } = WebServerType.Iis;
	}
}
