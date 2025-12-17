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
using System.Web;
using System.Web.Http;
using FuseCP.WebDavPortal.DependencyInjection;
using FuseCP.WebDavPortal.UI.Routes;

namespace FuseCP.WebDavPortal.App_Start
{
    public class WebApiConfig
    {
        public static void Register(HttpConfiguration configuration)
        {
            #region Owa

            configuration.Routes.MapHttpRoute(
                name: OwaRouteNames.GetFile,
                routeTemplate: "owa/wopi*/files/{accessTokenId}/contents",
                defaults: new {controller = "Owa", action = "GetFile"});

            configuration.Routes.MapHttpRoute(
                name: OwaRouteNames.CheckFileInfo,
                routeTemplate: "owa/wopi*/files/{accessTokenId}",
                defaults: new {controller = "Owa", action = "CheckFileInfo"});

            #endregion



            configuration.Routes.MapHttpRoute("API Default", "api/{controller}/{id}",
                new { id = RouteParameter.Optional });

            configuration.DependencyResolver = new NinjectDependecyResolver();
        }
    }
}
