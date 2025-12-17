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
using FuseCP.Providers.OS;

#if !NETFRAMEWORK
using CoreWCF;
using CoreWCF.Channels;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
#else
using System.Web;
#endif

namespace FuseCP.Web.Services
{
	public static class Server
	{
#if !NETFRAMEWORK
		public static string WebRoot { get; set; } = null;
		public static string ContentRoot { get; set; } = null;
		public static string MapPath(string path) => path.Replace("~", ContentRoot);
		public static string UserHostAddress
		{
			get
			{
				try {
					OperationContext context = OperationContext.Current;
					if (context == null) return "127.0.0.1";
					MessageProperties prop = context.IncomingMessageProperties;
					RemoteEndpointMessageProperty endpoint =
						prop[RemoteEndpointMessageProperty.Name] as RemoteEndpointMessageProperty;
					string ip = endpoint?.Address ?? "127.0.0.1";
					return ip;
				} catch {
					return "127.0.0.1";
				}
			}
		}

		public static Action<WebApplication> ConfigureApp = null;
		public static Action ConfigurationComplete = null;
		public static Action<WebApplicationBuilder> ConfigureBuilder = null;
#else
		public static string UserHostAddress {
			get {
				try {
					return System.Web.HttpContext.Current.Request.UserHostAddress;
				} catch {
					return "127.0.0.1";
				}
			}
		}
		public static string MapPath(string path) => System.Web.Hosting.HostingEnvironment.MapPath(path);
#endif

		public static readonly Dictionary<string, object> Cache = new Dictionary<string, object>();
	}
}
