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

using FuseCP.EnterpriseServer;
using FuseCP.EnterpriseServer.Client;
using FuseCP.Web.Clients;

namespace FuseCP.Setup
{
	public class ServerContext
	{
		public string Server { get; set; }
		public string Username { get; set; }
		public string Password { get; set; }
	}
	/// <summary>
	/// ES Proxy class
	/// </summary>
	public class ES
	{
		public const int ERROR_USER_WRONG_PASSWORD = -110;
		public const int ERROR_USER_WRONG_USERNAME = -109;
		public const int ERROR_USER_ACCOUNT_CANCELLED = -105;
		public const int ERROR_USER_ACCOUNT_DEMO = -106;
		public const int ERROR_USER_ACCOUNT_PENDING = -103;
		public const int ERROR_USER_ACCOUNT_SHOULD_BE_ADMINISTRATOR = -107;
		public const int ERROR_USER_ACCOUNT_SHOULD_BE_RESELLER = -108;
		public const int ERROR_USER_ACCOUNT_SUSPENDED = -104;

		private static ServerContext serverContext = null;

		private static void InitializeServices(ServerContext context)
		{
			serverContext = context;
		}
		
		public static bool Connect(string server, string username, string password)
		{
			bool ret = true;
			ServerContext serverContext = new ServerContext();
			serverContext.Server = server;
			serverContext.Username = username;
			serverContext.Password = password;

			InitializeServices(serverContext);
			int status = -1;
			try
			{
				status = ES.Services.Authentication.AuthenticateUser(serverContext.Username, serverContext.Password, null);
			}
			catch (Exception ex)
			{
				Log.WriteError("Authentication error", ex);
				return false;
			}

			string errorMessage = "Check your internet connection or server URL.";
			if (status != 0)
			{
				switch (status)
				{
					case ERROR_USER_WRONG_USERNAME:
						errorMessage = "Wrong username.";
						break;
					case ERROR_USER_WRONG_PASSWORD:
						errorMessage = "Wrong password.";
						break;
					case ERROR_USER_ACCOUNT_CANCELLED:
						errorMessage = "Account cancelled.";
						break;
					case ERROR_USER_ACCOUNT_PENDING:
						errorMessage = "Account pending.";
						break;
				}
				Log.WriteError(
					string.Format("Cannot connect to the remote server. {0}", errorMessage));
				ret = false;
			}
			return ret;
		}

		public static ES Services
		{
			get
			{
				return new ES();
			}
		}

		public esSystem System
		{
			get { return GetCachedProxy<esSystem>(); }
		}

		public esApplicationsInstaller ApplicationsInstaller
		{
			get { return GetCachedProxy<esApplicationsInstaller>(); }
		}

		public esAuditLog AuditLog
		{
			get { return GetCachedProxy<esAuditLog>(); }
		}

		public esAuthentication Authentication
		{
			get { return GetCachedProxy<esAuthentication>(false); }
		}

		public esComments Comments
		{
			get { return GetCachedProxy<esComments>(); }
		}

		public esDatabaseServers DatabaseServers
		{
			get { return GetCachedProxy<esDatabaseServers>(); }
		}

		public esFiles Files
		{
			get { return GetCachedProxy<esFiles>(); }
		}

		public esFtpServers FtpServers
		{
			get { return GetCachedProxy<esFtpServers>(); }
		}

		public esMailServers MailServers
		{
			get { return GetCachedProxy<esMailServers>(); }
		}

		public esOperatingSystems OperatingSystems
		{
			get { return GetCachedProxy<esOperatingSystems>(); }
		}

		public esPackages Packages
		{
			get { return GetCachedProxy<esPackages>(); }
		}

		public esScheduler Scheduler
		{
			get { return GetCachedProxy<esScheduler>(); }
		}

		public esTasks Tasks
		{
			get { return GetCachedProxy<esTasks>(); }
		}

		public esServers Servers
		{
			get { return GetCachedProxy<esServers>(); }
		}

		public esStatisticsServers StatisticsServers
		{
			get { return GetCachedProxy<esStatisticsServers>(); }
		}

		public esUsers Users
		{
			get { return GetCachedProxy<esUsers>(); }
		}

		public esWebServers WebServers
		{
			get { return GetCachedProxy<esWebServers>(); }
		}

		public esSharePointServers SharePointServers
		{
			get { return GetCachedProxy<esSharePointServers>(); }
		}

		public esImport Import
		{
			get { return GetCachedProxy<esImport>(); }
		}

		public esBackup Backup
		{
			get { return GetCachedProxy<esBackup>(); }
		}

		public esExchangeServer ExchangeServer
		{
			get { return GetCachedProxy<esExchangeServer>(); }
		}

		public esOrganizations Organizations
		{
			get { return GetCachedProxy<esOrganizations>(); }
		}

		public esTest Test
		{
			get { return GetCachedProxy<esTest>(); }
		}

		protected ES()
		{
		}

		protected virtual T GetCachedProxy<T>()
		{
			return GetCachedProxy<T>(true);
		}

		protected virtual T GetCachedProxy<T>(bool secureCalls)
		{
			if (serverContext == null)
			{
				throw new Exception("Server context is not specified");
			}

			Type t = typeof(T);
			string key = t.FullName + ".ServiceProxy";
			T proxy = (T)Activator.CreateInstance(t);

			ClientBase p = proxy as ClientBase;

			// configure proxy
			EnterpriseServerProxyConfigurator cnfg = new EnterpriseServerProxyConfigurator();
			cnfg.EnterpriseServerUrl = serverContext.Server;
			if (secureCalls)
			{
				cnfg.Username = serverContext.Username;
				cnfg.Password = serverContext.Password;
			}

			cnfg.Configure(p);

			return proxy;
		}
	}
}

