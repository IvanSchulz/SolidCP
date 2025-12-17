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
using System.Linq;
using System.Text;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
#if NETFRAMEWORK
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.Web;
using System.Web.Routing;
using System.Web.Configuration;
using SwaggerWcf;
using SwaggerWcf.Models;
#endif
//using Microsoft.Web.Infrastructure;
using System.ComponentModel;

//[assembly: PreApplicationStartMethod(typeof(FuseCP.Web.Services.StartupFX), "Start")]
namespace FuseCP.Web.Services
{
	public static class StartupNetFX
	{

		static int initialized = 0;
		public static void Start()
		{
#if NETFRAMEWORK
			if (Interlocked.CompareExchange(ref initialized, 1, 0) != 0) return;

			AddServiceRoutes(ServiceTypes.Types.Select(srvc => srvc.Service));
			//SvcVirtualPathProvider.SetupSvcServices(webServices);
			//DictionaryVirtualPathProvider.Startup();

			// set Log trace switch, as it is not working

            // Configure Swagger

            var a = Assembly.GetEntryAssembly();
            var srvcAssemblies = AppDomain.CurrentDomain.GetAssemblies()
                .Select(assembly => assembly.GetName().Name.Replace('.', ' '))
                .Where(name => name == "FuseCP Server" || name == "FuseCP EnterpriseServer" ||
                    name == "FuseCP WebApp");
            var title = $"{string.Join(" & ", srvcAssemblies.ToArray())} API";

			var hasServer = title.Contains("FuseCP Server");
			var hasEnterprise = title.Contains("FuseCP EnterpriseServer");

			var openServices = $"{(hasServer || hasEnterprise ? "but the " : "")}{(hasEnterprise ? $"esAuthentication{(hasServer ? ", " : " & ")}esTest{(hasServer ? ", " : " ")}" : "")}{(hasServer ? "AutoDiscovery & Test " : "")}";
			var clientAssembly = $"{(hasEnterprise ? "FuseCP.EnterpriseServer.Client " : "")}{(hasEnterprise && hasServer ? "& " : "")}{(hasServer ? "FuseCP.Server.Client " : "")}";
			var info = new SwaggerWcf.Models.Info
			{
				Version = "1.0.0",
				Title = title,
				Description = $"This is the REST API of FuseCP. Note that all {openServices}services use Basic Http Authentication. If you use .NET, you might want to access the API over WCF/SOAP, in this case refer to the {clientAssembly}assembly.",
				TermsOfService = "http://fusecp.com/terms/",
				Contact = new SwaggerWcf.Models.InfoContact
				{
					Name = "Support",
					Email = "support@fusecp.com"
				},
				License = new SwaggerWcf.Models.InfoLicense
				{
					Name = "FuseCP License",
					Url = "http://fusecp.com/license/"
				},
			};

            var security = new SecurityDefinitions {
				{
					"basicAuth", new SecurityAuthorization {
						Type = "basic", 
						Name = "basicAuth",
						Description = "Basic HTTP Authentication"
					}
				}
			};

			SwaggerWcfEndpoint.Configure(info, security);
#endif
        }

        static void AddServiceRoutes(IEnumerable<Type> services)
		{
#if NETFRAMEWORK
			foreach (var service in services)
			{
				RouteTable.Routes.Add(new ServiceRoute(service.Name, new ServiceHostFactory(), service));
				RouteTable.Routes.Add(new ServiceRoute($"basic/{service.Name}", new ServiceHostFactory(), service));
				RouteTable.Routes.Add(new ServiceRoute($"ws/{service.Name}", new ServiceHostFactory(), service));
				RouteTable.Routes.Add(new ServiceRoute($"net/{service.Name}", new ServiceHostFactory(), service));
				RouteTable.Routes.Add(new ServiceRoute($"tcp/{service.Name}", new ServiceHostFactory(), service));
                RouteTable.Routes.Add(new ServiceRoute($"pipe/{service.Name}", new ServiceHostFactory(), service));
				RouteTable.Routes.Add(new ServiceRoute($"tcp/ssl/{service.Name}", new ServiceHostFactory(), service));
                RouteTable.Routes.Add(new ServiceRoute($"pipe/ssl/{service.Name}", new ServiceHostFactory(), service));
                RouteTable.Routes.Add(new ServiceRoute($"api/{service.Name}", new ServiceHostFactory(), service));
			}
			RouteTable.Routes.Add(new ServiceRoute("swagger", new WebServiceHostFactory(), typeof(SwaggerWcfEndpoint)));

			var tunnelHandler = new TunnelHandlerNetFX();
			RouteTable.Routes.Add(new Route(tunnelHandler.Route, tunnelHandler));
#endif
		}
	}
}
