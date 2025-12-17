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

namespace FuseCP.EnterpriseServer
{
	public class EnterpriseServerProxyConfigurator
	{
		public const bool UseNetHttpAsDefaultProtocol = true;
		public const bool UseMessageSecurityOverHttp = true;
		public const bool UseMessageSecurityOnCore = false;

		private string enterpriseServerUrl;
		private string username;
		private string password;

		static bool? isCore = null;
		public bool IsCore
		{
			get
			{
				if (isCore == null)
				{
					if (!string.IsNullOrEmpty(enterpriseServerUrl))
					{
						var test = new Client.esTest();
						test.Url = enterpriseServerUrl;
						isCore = test.OSPlatform().IsCore;
					}
					else throw new ArgumentNullException("EnterpriseServerUrl not set.");
				}
				return isCore.Value;
			}
		}
		public string EnterpriseServerUrl
		{
			get { return this.enterpriseServerUrl; }
			set { this.enterpriseServerUrl = value; }
		}

		public string Username
		{
			get { return this.username; }
			set { this.username = value; }
		}

		public string Password
		{
			get { return this.password; }
			set { this.password = value; }
		}

		public void Configure(FuseCP.Web.Clients.ClientBase proxy)
		{
			// set proxy URL
			string serverUrl = enterpriseServerUrl.Trim();
			if (serverUrl.Length == 0)
				throw new Exception("Enterprise Server URL could not be empty");

			proxy.Url = serverUrl;

			// set timeout
			proxy.Timeout = TimeSpan.FromMinutes(15); //15 minutes // System.Threading.Timeout.Infinite;

			if (proxy.IsDefaultApi)
			{
				if (UseMessageSecurityOverHttp && proxy.IsHttp && proxy.IsEncrypted && !proxy.IsLocal &&
					 (UseMessageSecurityOnCore || !IsCore))
				{
					proxy.Protocol = Web.Clients.Protocols.WSHttp;
				}
				else if (UseNetHttpAsDefaultProtocol)
				{
					// use NetHttp protocol as default
					if (proxy.IsHttp) proxy.Protocol = Web.Clients.Protocols.NetHttp;
					else if (proxy.IsHttps) proxy.Protocol = Web.Clients.Protocols.NetHttps;
				}
			} else if (proxy.IsSsh && proxy.Protocol == Web.Clients.Protocols.NetTcpSsl) proxy.Protocol = Web.Clients.Protocols.NetTcp;


			if (!String.IsNullOrEmpty(username) && proxy.IsAuthenticated)
			{
				proxy.Credentials.UserName = username;
				proxy.Credentials.Password = password;
			}
		}
	}
}
