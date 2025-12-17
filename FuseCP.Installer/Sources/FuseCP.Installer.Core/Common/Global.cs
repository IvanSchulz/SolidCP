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
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.EnterpriseServices;
using FuseCP.Providers.OS;

namespace FuseCP.Installer.Common
{
	public class Global
	{
		public const string VisualInstallerShell = "VisualInstallerShell";
		public const string SilentInstallerShell = "SilentInstallerShell";
		public const string DefaultInstallPathRoot = @"C:\FuseCP";
		public const string LoopbackIPv4 = "127.0.0.1";
		public const string InstallerProductCode = "cfg core";

		public abstract class Parameters
		{
			public const string ComponentId = "ComponentId";
			public const string EnterpriseServerUrl = "EnterpriseServerUrl";
			public const string ShellMode = "ShellMode";
			public const string ShellVersion = "ShellVersion";
			public const string IISVersion = "IISVersion";
			public const string BaseDirectory = "BaseDirectory";
			public const string Installer = "Installer";
			public const string InstallerType = "InstallerType";
			public const string InstallerPath = "InstallerPath";
			public const string InstallerFolder = "InstallerFolder";
			public const string Platforms = "Platforms";
			public const string Version = "Version";
			public const string ComponentDescription = "ComponentDescription";
			public const string ComponentCode = "ComponentCode";
			public const string ApplicationName = "ApplicationName";
			public const string ComponentName = "ComponentName";
			public const string WebSiteIP = "WebSiteIP";
			public const string WebSitePort = "WebSitePort";
			public const string WebSiteDomain = "WebSiteDomain";
			public const string ServerPassword = "ServerPassword";
			public const string UserDomain = "UserDomain";
			public const string UserAccount = "UserAccount";
			public const string UserPassword = "UserPassword";
			public const string CryptoKey = "CryptoKey";
			public const string ServerAdminPassword = "ServerAdminPassword";
			public const string SetupXml = "SetupXml";
			public const string ParentForm = "ParentForm";
			public const string Component = "Component";
			public const string FullFilePath = "FullFilePath";
			public const string DatabaseServer = "DatabaseServer";
			public const string DbServerAdmin = "DbServerAdmin";
			public const string DbServerAdminPassword = "DbServerAdminPassword";
			public const string DatabaseName = "DatabaseName";
			public const string ConnectionString = "ConnectionString";
			public const string InstallConnectionString = "InstallConnectionString";
			public const string Release = "Release";
		}

		public abstract class Messages
		{
			public const string NotEnoughPermissionsError = "You do not have the appropriate permissions to perform this operation. Make sure you are running the application from the local disk and you have local system administrator privileges.";
			public const string InstallerVersionIsObsolete = "FuseCP Installer {0} or higher required.";
			public const string ComponentIsAlreadyInstalled = "Component or its part is already installed.";
			public const string AnotherInstanceIsRunning = "Another instance of the installation process is already running.";
			public const string NoInputParametersSpecified = "No input parameters specified";
			public const int InstallationError = -1000;
			public const int UnknownComponentCodeError = -999;
			public const int SuccessInstallation = 0;
			public const int AnotherInstanceIsRunningError = -998;
			public const int NotEnoughPermissionsErrorCode = -997;
			public const int NoInputParametersSpecifiedError = -996;
			public const int ComponentIsAlreadyInstalledError = -995;
		}

		public abstract class Server
		{
			public abstract class CLI
			{
				public const string ServerPassword = "passw";
			};

			public const string ComponentName = "Server";
			public const string ComponentCode = "server";
			public const string ComponentDescription = "FuseCP Server is a set of services running on the remote server to be controlled. Server application should be reachable from Enterprise Server one.";
			public const string ServiceAccount = "FCPServer";
			public const string DefaultPort = "9003";
			public const string DefaultIP = "127.0.0.1";
			public const string SetupController = "Server";
		}

        public abstract class WebDavPortal
        {
            public const string ComponentName = "WebDavPortal";
            public const string ComponentDescription = "FuseCP WebDav Portal is a client frontend for viewing and editing their WebDav Enterprise storage files.";
            public const string ServiceAccount = "FCPWebDav";
            public const string DefaultPort = "9004";
            public const string DefaultIP = "";
            public const string ComponentCode = "WebDavPortal";
            public const string SetupController = "WebDavPortal";

            public static string[] ServiceUserMembership
            {
                get
                {
                    if (IISVersion.Major >= 7)
                    {
                        return new string[] { "IIS_IUSRS" };
                    }
                    //
                    return new string[] { "IIS_WPG" };
                }
            }

            public abstract class CLI
            {
                
            }
        }

        public abstract class StandaloneServer
		{
			public const string SetupController = "StandaloneServerSetup";
			public const string ComponentCode = "standalone";
			public const string ComponentName = "Standalone Server Setup";
		}

		public abstract class WebPortal
		{
			public const string ComponentName = "Portal";
			public const string ComponentDescription = "FuseCP Portal is a control panel itself with user interface which allows managing user accounts, hosting spaces, web sites, FTP accounts, files, etc.";
			public const string ServiceAccount = "FCPPortal";
			public const string DefaultPort = "9001";
			public const string DefaultIP = "";
			public const string DefaultEntServURL = "http://127.0.0.1:9002";
			public const string ComponentCode = "portal";
			public const string SetupController = "Portal";

			public static string[] ServiceUserMembership
			{
				get
				{
					if (IISVersion.Major >= 7)
					{
						return new string[] { "IIS_IUSRS" };
					}
					//
					return new string[] { "IIS_WPG" };
				}
			}

			public abstract class CLI
			{
				public const string EnterpriseServerUrl = "esurl";
			}
		}

		public abstract class EntServer
		{
			public const string ComponentName = "Enterprise Server";
			public const string ComponentDescription = "Enterprise Server is the heart of FuseCP system. It includes all business logic of the application. Enterprise Server should have access to Server and be accessible from Portal applications.";
			public const string ServiceAccount = "FCPEnterprise";
			public const string DefaultPort = "9002";
			public const string DefaultIP = "127.0.0.1";
			public const string DefaultDomain = "";
			public const string DefaultDbServer = @"localhost\sqlexpress";
			public const string DefaultDatabase = "FuseCP";
			public const string AspNetConnectionStringFormat = "server={0};database={1};uid={2};pwd={3};";
			public const string ComponentCode = "enterprise server";
			public const string SetupController = "EnterpriseServer";

			public static string[] ServiceUserMembership
			{
				get
				{
                    if (IISVersion.Major >= 7)
					{
						return new string[] { "IIS_IUSRS" };
					}
					//
					return new string[] { "IIS_WPG" };
				}
			}

			public abstract class CLI
			{
				public const string ServeradminPassword = "passw";
				public const string DatabaseName = "dbname";
				public const string DatabaseServer = "dbserver";
				public const string DbServerAdmin = "dbadmin";
				public const string DbServerAdminPassword = "dbapassw";
			}
		}

		public abstract class CLI
		{
			public const string WebSiteIP = "webip";
			public const string ServiceAccountPassword = "upassw";
			public const string ServiceAccountDomain = "udomaim";
			public const string ServiceAccountName = "uname";
			public const string WebSitePort = "webport";
			public const string WebSiteDomain = "webdom";
		}

		private Global()
		{
		}

		private static Version iisVersion;
		//
		public static Version IISVersion
		{
			get
			{
				if (OSInfo.IsWindows) {
					if (iisVersion == null)
					{
						iisVersion = RegistryUtils.GetIISVersion();
					} 
				} else iisVersion = new Version();
				//
				return iisVersion;
			}
		}

		//
		private static Providers.OS.WindowsVersion osVersion = Providers.OS.WindowsVersion.Unknown;

		/// <summary>
		/// Represents Setup Control Panel Accounts system settings set (FCPA)
		/// </summary>
		public class FCPA
		{
			public const string SettingsKeyName = "EnabledFCPA";
		}

		public static Providers.OS.WindowsVersion OSVersionWindows
		{
			get
			{
				if (OSInfo.IsWindows)
				{
					if (osVersion == Providers.OS.WindowsVersion.Unknown)
					{
						osVersion = OSInfo.WindowsVersion;
					}
					//
					return osVersion;
				}
				else return Providers.OS.WindowsVersion.NonWindows;
			}
		}

		public static Providers.OS.OSFlavor OSFlavor => OSInfo.OSFlavor;

		public static XmlDocument SetupXmlDocument { get; set; }
	}

	public class SetupEventArgs<T> : EventArgs
	{
		public T EventData { get; set; }
		public string EventMessage { get; set; }
	}
}
