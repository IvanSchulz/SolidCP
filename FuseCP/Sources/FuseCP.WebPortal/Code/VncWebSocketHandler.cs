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
using System.Web.Routing;
using System.Net;
using System.IO;
using FuseCP.Providers.OS;
using FuseCP.Providers.Virtualization;
using FuseCP.EnterpriseServer.Client;
using FuseCP.Portal;

namespace FuseCP.WebPortal
{
    public class VncWebSocketHandler : IHttpHandler, IRouteHandler
    {
        public bool IsReusable => true;

        public IHttpHandler GetHttpHandler(RequestContext requestContext) => this;

        public void ProcessRequest(HttpContext context)
        {
            if (context.IsWebSocketRequest)
            {
                var query = context.Request.QueryString;
                var user = query["user"];
                var password = query["password"];
                var vncpassword = query["vncpassword"];
                if (string.IsNullOrEmpty(password))
                {
                    var authTicket = PortalUtils.AuthTicket;
                    if (authTicket != null)
                    {
                        password = authTicket.UserData.Substring(0, authTicket.UserData.IndexOf(Environment.NewLine));
                    } else
                    {
                        context.Response.StatusCode = (int)HttpStatusCode.Forbidden;
                        return;
                    }
                }
                var item = query["item"];
                int itemId;
                if (string.IsNullOrEmpty(user) || string.IsNullOrEmpty(item) || !int.TryParse(item, out itemId) ||
                    string.IsNullOrEmpty(vncpassword))
                {
                    context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                } else
                {
                    context.AcceptWebSocketRequest(async socketContext =>
                    {
                        TunnelSocket outgoing = null;
                        var incoming = new TunnelSocket(socketContext.WebSocket);
                        var esclient = new EnterpriseServerTunnelClient();
                        esclient.Username = user;
                        esclient.Password = password;
                        esclient.ServerUrl = PortalConfiguration.SiteSettings["EnterpriseServer"];
                        var credentials = new VncCredentials() { Password = vncpassword };
                        try
                        {
                            outgoing = await esclient.GetPveVncWebSocketAsync(itemId, credentials);
                            await incoming.Transmit(outgoing);
                        } catch (Exception ex)
                        {
                            throw new IOException(ex.Message, ex);
                        } finally
                        {
                            outgoing?.Dispose();
                        }
                    });
                }

            } else
            {
                context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            }
        }

        public static void Init()
        {
            RouteTable.Routes.Add(new Route("novnc/websocket", new VncWebSocketHandler()));
        }
    }
}
