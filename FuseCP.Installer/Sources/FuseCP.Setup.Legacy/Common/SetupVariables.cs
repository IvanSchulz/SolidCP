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
using System.Xml;
using System.Collections.Generic;
using System.Text;
using FuseCP.Setup.Common;
using FuseCP.UniversalInstaller;

namespace FuseCP.Setup
{
	/// <summary>
	/// Variables container.
	/// </summary>
	public sealed class SetupVariables
	{
		//
		public static readonly SetupVariables Empty = new SetupVariables();
		public bool EnableScpaMode { get; set; }
		public string PeerAdminPassword { get; set; }
		public string DatabaseUser { get; set; }
		public string DatabaseUserPassword { get; set; }
		public bool NewDatabaseUser { get; set; }
		/// <summary>
		/// Installation folder
		/// </summary>
		public string InstallationFolder { get; set; }

		public string InstallFolder
		{
			get { return InstallationFolder; }
			set { InstallationFolder = value; }
		}

		/// <summary>
		/// Component id
		/// </summary>
		public string ComponentId { get; set; }

		public string ComponentDescription { get; set; }


		/// <summary>
		/// Component code
		/// </summary>
		public string ComponentCode { get; set; }

		/// <summary>
		/// Component name
		/// </summary>
		public string ComponentName { get; set; }

		/// <summary>
		/// Component name
		/// </summary>
		public string ApplicationName { get; set; }

		public string ComponentFullName
		{
			get { return ApplicationName + " " + ComponentName; }
		}

		public string Instance { get; set; }

		/// <summary>
		/// Install currentScenario
		/// </summary>
		public SetupActions SetupAction { get; set; }

		/// <summary>
		/// Product key
		/// </summary>
		public string ProductKey { get; set; }

		/// <summary>
		/// Release Id
		/// </summary>
		public int ReleaseId { get; set; }

		// Workaround
		public string Release
		{
			get { return Version; }
			set { Version = value; }
		}

		/// <summary>
		/// Release name
		/// </summary>
		public string Version { get; set; }

		/// <summary>
		/// Connection string
		/// </summary>
		public string ConnectionString { get; set; }

		/// <summary>
		/// Database
		/// </summary>
		public string Database { get; set; }

		/// <summary>
		/// DatabaseServer
		/// </summary>
		public string DatabaseServer { get; set; }

		public int DatabasePort { get; set; }

		public FuseCP.EnterpriseServer.Data.DbType DatabaseType { get; set; }
		/// <summary>
		/// Database install connection string
		/// </summary>
		public string DbInstallConnectionString { get; set; }

		public string InstallConnectionString
		{
			get { return DbInstallConnectionString; }
			set { DbInstallConnectionString = value; }
		}

		/// <summary>
		/// Create database
		/// </summary>
		public bool CreateDatabase { get; set; }

		public bool NewVirtualDirectory { get; set; }

		public bool NewWebApplicationPool { get; set; }

		public string WebApplicationPoolName { get; set; }

		public string ApplicationPool
		{
			get { return WebApplicationPoolName; }
			set { WebApplicationPoolName = value; }
		}

		/// <summary>
		/// Virtual directory
		/// </summary>
		public string VirtualDirectory { get; set; }

		public bool NewWebSite { get; set; }
		/// <summary>
		/// Website Id
		/// </summary>
		public string WebSiteId { get; set; }

		/// <summary>
		/// Website IP
		/// </summary>
		public string WebSiteIP { get; set; }

		/// <summary>
		/// Website port
		/// </summary>
		public string WebSitePort { get; set; }

		/// <summary>
		/// Website domain
		/// </summary>
		public string WebSiteDomain { get; set; }

		public string LetsEncryptEmail { get; set; }
		public string CertificateStoreLocation { get; set; }
		public string CertificateStore { get; set; }
		public string CertificateFindType { get; set; }
		public string CertificateFindValue { get; set; }
		public string CertificateFile { get; set; }
		public string CertificatePassword { get; set; }

		/// <summary>
		/// User account
		/// </summary>
		public string UserAccount { get; set; }

		/// <summary>
		/// User password
		/// </summary>
		public string UserPassword { get; set; }

		/// <summary>
		/// User Membership
		/// </summary>
		public string[] UserMembership
		{
			get
			{
				if (ComponentCode.Equals(Global.Server.ComponentCode, StringComparison.OrdinalIgnoreCase))
				{
					return Global.Server.ServiceUserMembership;
				}
				else if (ComponentCode.Equals(Global.EntServer.ComponentCode, StringComparison.OrdinalIgnoreCase))
				{
					return Global.EntServer.ServiceUserMembership;
				}
				else if (ComponentCode.Equals(Global.WebPortal.ComponentCode, StringComparison.OrdinalIgnoreCase))
				{
					return Global.WebPortal.ServiceUserMembership;
				}
				else if (ComponentCode.Equals(Global.WebDavPortal.ComponentCode, StringComparison.OrdinalIgnoreCase))
				{
					return Global.WebDavPortal.ServiceUserMembership;
				}
				else
				{
					return new string[] { };
				}
			}
		}


		/// <summary>
		/// Welcome screen has been skipped
		/// </summary>
		public bool WelcomeScreenSkipped { get; set; }

		public string InstallerFolder { get; set; }

		public string Installer { get; set; }

		public string InstallerType { get; set; }

		public string InstallerPath { get; set; }

		public XmlNode ComponentConfig { get; set; }

		public string ServerPassword { get; set; }

		public string CryptoKey { get; set; }

		public bool UpdateWebSite { get; set; }

		public bool UpdateServerPassword { get; set; }

		public bool UpdateServerAdminPassword { get; set; }

		public string ServerAdminPassword { get; set; }

		public string BaseDirectory { get; set; }

		public string SpecialBaseDirectory { get; set; }
		public IDictionary<string, string> FileNameMap { get; set; }
		public IDictionary<string, string> SessionVariables { get; set; }
		public IDictionary<string, string[]> XmlData { get; set; } // XPath, Value.
		public string[] SysFields; // Fields that saved in sys config.
		public bool ComponentExists { get; set; }

		public string UpdateVersion { get; set; }

        public string EnterpriseServerURL { get; set; }
        public bool EmbedEnterpriseServer { get; set; }
        public string EnterpriseServerPath { get; set; }
		public bool ExposeEnterpriseServerWebservices { get; set; }

        public string UserDomain { get; set; }

		public string Domain
		{
			get { return UserDomain; }
			set { UserDomain = value; }
		}

		public bool NewUserAccount { get; set; }

		public bool NewApplicationPool { get; set; }

		public Common.ServerItem[] SQLServers { get; set; }

		public string Product { get; set; }

		public Version IISVersion { get; set; }

		public string ServiceIP { get; set; }

		public string ServicePort { get; set; }

		public string ServiceName { get; set; }

		public string ConfigurationFile { get; set; }
		private bool m_UseUserCredentials = true;
		public bool UseUserCredentials { get { return m_UseUserCredentials; } set { m_UseUserCredentials = value; } }
		public string ServiceFile { get; set; }

		public string LicenseKey { get; set; }

		public string SetupXml { get; set; }

		public string RemoteServerUrl
		{
			get
			{
				string address = "https://";
				string server = String.Empty;
				string ipPort = String.Empty;
				//server 
				if (String.IsNullOrEmpty(WebSiteDomain) == false
					&& WebSiteDomain.Trim().Length > 0)
				{
					//domain 
					server = WebSiteDomain.Trim();
				}
				else
				{
					//ip
					if (String.IsNullOrEmpty(WebSiteIP) == false
						&& WebSiteIP.Trim().Length > 0)
					{
						server = WebSiteIP.Trim();
					}
				}
				//port
				if (server.Length > 0 &&
					WebSiteIP.Trim().Length > 0 &&
					WebSitePort.Trim() != "80")
				{
					ipPort = ":" + WebSitePort.Trim();
				}

				//address string
				address += server + ipPort;
				//
				return address;
			}
		}

		public string RemoteServerPassword { get; set; }

		public string ServerComponentId { get; set; }

		public string PortalComponentId { get; set; }

		public string EnterpriseServerComponentId { get; set; }

		public SetupVariables Clone()
		{
			return (SetupVariables)this.MemberwiseClone();
		}

		public string InstallerExe { get; set; }

		public bool InstallNet8Runtime { get; set; }
	}
}
