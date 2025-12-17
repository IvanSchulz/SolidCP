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
using System.Web;
using System.Web.Security;
using System.Collections.Concurrent;
using FuseCP.EnterpriseServer.Client;
using FuseCP.EnterpriseServer;
//using FuseCP.Providers.HostedSolution;
using FuseCP.EnterpriseServer.Base;

#if Modules
namespace FuseCP.Portal
{
#else
namespace FuseCP.WebPortal
{
	using FuseCP.Portal;
#endif

	// ES.Services

	public class ES
	{
		public FormsAuthenticationTicket AuthTicket
		{
			get
			{
				try
				{
					if (HttpContext.Current != null)
					{
						return PortalUtils.AuthTicket;
					}
				}
				catch { }

				return null;
			}
		}
		public ES() { }

		static ES services = null;
		static object Lock = new object();
		public static ES Services
		{
			get
			{
				lock (Lock)
				{
					if (services == null)
					{
						services = new ES();
						//services.AuthTicket = null;
					}
				}
				return services;
			}
		}

		public esCRM CRM
		{
			get
			{
				return GetCachedProxy<esCRM>();
			}
		}


		public esVirtualizationServer VPS
		{
			get { return GetCachedProxy<esVirtualizationServer>(); }
		}

		public esVirtualizationServer2012 VPS2012
		{
			get { return GetCachedProxy<esVirtualizationServer2012>(); }
		}

		public esVirtualizationServerProxmox Proxmox
		{
			get { return GetCachedProxy<esVirtualizationServerProxmox>(); }
		}

		public esVirtualizationServerForPrivateCloud VPSPC
		{
			get { return GetCachedProxy<esVirtualizationServerForPrivateCloud>(); }
		}

		public esBlackBerry BlackBerry
		{
			get { return GetCachedProxy<esBlackBerry>(); }
		}

		public esOCS OCS
		{
			get { return GetCachedProxy<esOCS>(); }
		}


		public esLync Lync
		{
			get { return GetCachedProxy<esLync>(); }
		}

		public esSfB SfB
		{
			get { return GetCachedProxy<esSfB>(); }
		}


		public esOrganizations Organizations
		{
			get
			{
				return GetCachedProxy<esOrganizations>();
			}
		}

		public esSystem System
		{
			get { return GetCachedProxy<esSystem>(); }
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

		public esHostedSharePointServers HostedSharePointServers
		{
			get { return GetCachedProxy<esHostedSharePointServers>(); }
		}

		public esHostedSharePointServersEnt HostedSharePointServersEnt
		{
			get { return GetCachedProxy<esHostedSharePointServersEnt>(); }
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

		public esEnterpriseStorage EnterpriseStorage
		{
			get { return GetCachedProxy<esEnterpriseStorage>(); }
		}

		public esRemoteDesktopServices RDS
		{
			get { return GetCachedProxy<esRemoteDesktopServices>(); }
		}

		public esStorageSpaces StorageSpaces
		{
			get { return GetCachedProxy<esStorageSpaces>(); }
		}

		public esSpamExperts SpamExperts
		{
			get { return GetCachedProxy<esSpamExperts>(); }
		}

		public esTest Test
		{
			get { return GetCachedProxy<esTest>(); }
		}

		public static void Start()
		{
			Services.Test.TouchAsync();
		}
		protected virtual T GetCachedProxy<T>()
			where T: Web.Clients.ClientBase
		{
			return GetCachedProxy<T>(true);
		}

		static ConcurrentDictionary<Type, Web.Clients.ClientBase> cache = new ConcurrentDictionary<Type, Web.Clients.ClientBase>();
		protected virtual T GetCachedProxy<T>(bool secureCalls)
			where T: Web.Clients.ClientBase
		{
			Type t = typeof(T);
			string key = t.FullName + ".ServiceProxy";

			T proxy = null;
			bool useHttpContext = false;

			try
			{
				if (HttpContext.Current != null)
				{
					proxy = (T)HttpContext.Current.Items[key];
					useHttpContext = true;
				}
			}
			catch { }

			if (proxy == null)
			{
				proxy = (T)Activator.CreateInstance(t);
				if (useHttpContext) HttpContext.Current.Items[key] = proxy;
			}

			// configure proxy
			PortalUtils.ConfigureEnterpriseServerProxy(proxy, secureCalls, AuthTicket);

			return proxy;
		}
	}
}
