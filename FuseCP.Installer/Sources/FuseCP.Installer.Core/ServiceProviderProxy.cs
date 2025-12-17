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
using FuseCP.Installer.Configuration;
using System.Net;
using FuseCP.Installer.Services;

namespace FuseCP.Installer.Core
{
	public class ServiceProviderProxy
	{
		public static InstallerService GetInstallerWebService()
		{
			var webService = new InstallerService();

			string url = AppConfigManager.AppConfiguration.GetStringSetting(ConfigKeys.Web_Service);
			if (!String.IsNullOrEmpty(url))
			{
				webService.Url = url;
			}
			else
			{
				webService.Url = "http://installer.fusecp.com/Services/InstallerService-1.0.asmx";
			}

			// check if we need to add a proxy to access Internet
			bool useProxy = AppConfigManager.AppConfiguration.GetBooleanSetting(ConfigKeys.Web_Proxy_UseProxy);
			if (useProxy)
			{
				string proxyServer = AppConfigManager.AppConfiguration.Settings[ConfigKeys.Web_Proxy_Address].Value;
				if (!String.IsNullOrEmpty(proxyServer))
				{
					IWebProxy proxy = new WebProxy(proxyServer);
					string proxyUsername = AppConfigManager.AppConfiguration.Settings[ConfigKeys.Web_Proxy_UserName].Value;
					string proxyPassword = AppConfigManager.AppConfiguration.Settings[ConfigKeys.Web_Proxy_Password].Value;
					if (!String.IsNullOrEmpty(proxyUsername))
						proxy.Credentials = new NetworkCredential(proxyUsername, proxyPassword);
					webService.Proxy = proxy;
				}
			}

			return webService;
		}
	}
}
