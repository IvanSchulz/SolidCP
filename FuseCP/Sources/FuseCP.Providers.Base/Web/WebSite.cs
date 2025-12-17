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
using System.ComponentModel;
using System.Collections.Generic;
using System.Collections.Specialized;
using FuseCP.Providers.ResultObjects;
using System.Runtime.Serialization;

namespace FuseCP.Providers.Web
{
	/// <summary>
	/// Summary description for WebSiteItem.
	/// </summary>
	[Serializable]
	[DataContract]
	public class WebSite : WebAppVirtualDirectory
	{
		#region String constants
		public const string IIS7_SITE_ID = "WebSiteId_IIS7";
		public const string IIS7_LOG_EXT_FILE_FIELDS = "IIS7_LogExtFileFields";
		#endregion

		private string siteId;
		private string siteIPAddress;
		private int siteIPAddressId;
		private bool isDedicatedIP;
		private string dataPath;
		private ServerBinding[] bindings;
		private bool frontPageAvailable;
		private bool frontPageInstalled;
		private bool coldFusionAvailable;
		private bool createCFAppVirtualDirectories;
		private bool createCFAppVirtualDirectoriesPol;
		private string frontPageAccount;
		private string frontPagePassword;
		private string coldFusionVersion;
		private ServerState siteState;
		private bool securedFoldersInstalled;
		private bool heliconApeInstalled;
		private bool heliconApeEnabled;
		private HeliconApeStatus heliconApeStatus;
		private bool sniEnabled;
		private string siteInternalIPAddress;

		public WebSite()
		{
		}

		[Persistent]
		[DataMember]
		public string SiteId
		{
			get { return siteId; }
			set { siteId = value; }
		}

		[DataMember]
		public string SiteIPAddress
		{
			get { return siteIPAddress; }
			set { siteIPAddress = value; }
		}

		[Persistent]
		[DataMember]
		public int SiteIPAddressId
		{
			get { return siteIPAddressId; }
			set { siteIPAddressId = value; }
		}

		[DataMember]
		public bool IsDedicatedIP
		{
			get { return isDedicatedIP; }
			set { isDedicatedIP = value; }
		}

		/// <summary>
		/// Gets or sets logs path for the web site
		/// </summary>
		[Persistent]
		[DataMember]
		public string LogsPath { get; set; }

		[Persistent]
		[DataMember]
		public string DataPath
		{
			get { return dataPath; }
			set { dataPath = value; }
		}

		[DataMember]
		public ServerBinding[] Bindings
		{
			get { return bindings; }
			set { bindings = value; }
		}

		[Persistent]
		[DataMember]
		public string FrontPageAccount
		{
			get { return this.frontPageAccount; }
			set { this.frontPageAccount = value; }
		}

		[Persistent]
		[DataMember]
		public string FrontPagePassword
		{
			get { return this.frontPagePassword; }
			set { this.frontPagePassword = value; }
		}

		[DataMember]
		public bool FrontPageAvailable
		{
			get { return this.frontPageAvailable; }
			set { this.frontPageAvailable = value; }
		}

		[DataMember]
		public bool FrontPageInstalled
		{
			get { return this.frontPageInstalled; }
			set { this.frontPageInstalled = value; }
		}

		[DataMember]
		public bool ColdFusionAvailable
		{
			get { return this.coldFusionAvailable; }
			set { this.coldFusionAvailable = value; }
		}

		[DataMember]
		public string ColdFusionVersion
		{
			get { return this.coldFusionVersion; }
			set { this.coldFusionVersion = value; }
		}

		[DataMember]
		public bool CreateCFAppVirtualDirectories
		{
			get { return this.createCFAppVirtualDirectories; }
			set { this.createCFAppVirtualDirectories = value; }
		}

		[DataMember]
		public bool CreateCFAppVirtualDirectoriesPol
		{
			get { return this.createCFAppVirtualDirectoriesPol; }
			set { this.createCFAppVirtualDirectoriesPol = value; }
		}

		[DataMember]
		public ServerState SiteState
		{
			get { return this.siteState; }
			set { this.siteState = value; }
		}

		[DataMember]
		public bool SecuredFoldersInstalled
		{
			get { return this.securedFoldersInstalled; }
			set { this.securedFoldersInstalled = value; }
		}

		[DataMember]
		public bool HeliconApeInstalled
		{
			get { return this.heliconApeInstalled; }
			set { this.heliconApeInstalled = value; }
		}

		[DataMember]
		public bool HeliconApeEnabled
		{
			get { return this.heliconApeEnabled; }
			set { this.heliconApeEnabled = value; }
		}

		[DataMember]
		public HeliconApeStatus HeliconApeStatus
		{
			get { return this.heliconApeStatus; }
			set { this.heliconApeStatus = value; }
		}

		[DataMember]
		public bool SniEnabled
		{
			get { return this.sniEnabled; }
			set { this.sniEnabled = value; }
		}

		[DataMember]
		public string SiteInternalIPAddress
		{
			get { return siteInternalIPAddress; }
			set { siteInternalIPAddress = value; }
		}
	}

	[Flags]
	public enum SiteAppPoolMode
	{
		Dedicated = 1,
		Shared = 2,
		Classic = 4,
		Integrated = 8,
		dotNetFramework1 = 16,
		dotNetFramework2 = 32,
		dotNetFramework4 = 64
	};

	public class WSHelper
	{
		public const int IIS6 = 0;
		public const int IIS7 = 1;

		//
		public static Dictionary<SiteAppPoolMode, string[]> MMD = new Dictionary<SiteAppPoolMode, string[]>
		{
			{ SiteAppPoolMode.dotNetFramework1, new string[] {"", "v1.1"} },
			{ SiteAppPoolMode.dotNetFramework2, new string[] {"2.0", "v2.0"} },
			{ SiteAppPoolMode.dotNetFramework4, new string[] {"4.0", "v4.0"} },
		};

		public static string InferAppPoolName(string formatString, string siteName, SiteAppPoolMode NHRT)
		{
			if (String.IsNullOrEmpty(formatString))
				throw new ArgumentNullException("formatString");
			//
			NHRT |= SiteAppPoolMode.Dedicated;
			//
			formatString = formatString.Replace("#SITE-NAME#", siteName);
			//
			foreach (var fwVersionKey in MMD.Keys)
			{
				if ((NHRT & fwVersionKey) == fwVersionKey)
				{
					formatString = formatString.Replace("#IIS6-ASPNET-VERSION#", MMD[fwVersionKey][IIS6]);
					formatString = formatString.Replace("#IIS7-ASPNET-VERSION#", MMD[fwVersionKey][IIS7]);
					//
					break;
				}
			}
			//
			SiteAppPoolMode pipeline = NHRT & (SiteAppPoolMode.Classic | SiteAppPoolMode.Integrated);
			formatString = formatString.Replace("#PIPELINE-MODE#", pipeline.ToString());
			//
			return formatString.Trim();
		}

		public static string WhatFrameworkVersionIs(SiteAppPoolMode value)
		{
			SiteAppPoolMode dotNetVersion = value & (SiteAppPoolMode.dotNetFramework1
				| SiteAppPoolMode.dotNetFramework2 | SiteAppPoolMode.dotNetFramework4);
			//
			return String.Format("v{0}", MMD[dotNetVersion][IIS7]);
		}
	}
}
