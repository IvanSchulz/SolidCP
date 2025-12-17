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

#if NETFRAMEWORK
using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;
using System.Net;
using System.Timers;
using System.Diagnostics;
using System.Threading;
using System.Linq;
using FuseCP.Web.Clients;

namespace FuseCP.EnterpriseServer;

public class Global : System.Web.HttpApplication
{
	private int keepAliveMinutes = 10;
	private static string keepAliveUrl = "";
	private static System.Timers.Timer timer = null;

	protected void Application_Start(object sender, EventArgs e)
	{
		//if (!Debugger.IsAttached) Debugger.Launch();
		UsernamePasswordValidator.Init();
		//Web.Clients.CertificateValidator.Init();
		Web.Services.StartupNetFX.Start();
		Web.Clients.AssemblyLoader.Init(null, null, false);
	}

	protected void Application_End(object sender, EventArgs e)
	{
		ClientBase.DisposeAllSshTunnels();
		Web.Clients.AssemblyLoader.Dispose();
	}

	protected void Application_BeginRequest(object sender, EventArgs e)
	{
		// ASP.NET Integration Mode workaround
		if (String.IsNullOrEmpty(keepAliveUrl))
		{
			// init keep-alive
			keepAliveUrl = HttpContext.Current.Request.Url.ToString();
			if (this.keepAliveMinutes > 0)
			{
				timer = new System.Timers.Timer(60000 * this.keepAliveMinutes);
				timer.Elapsed += new ElapsedEventHandler(KeepAlive);
				timer.AutoReset = true;
				timer.Start();
			}
		}
	}

	public override void Init()
	{

	}

	private void KeepAlive(Object sender, System.Timers.ElapsedEventArgs e)
	{
		try
		{
			using (HttpWebRequest.Create(keepAliveUrl).GetResponse()) { }
		}
		catch { }
	}
}
#endif
