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
using System.ServiceProcess;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using FuseCP.EnterpriseServer.Code;
using System.IO;


#if NETCOREAPP
using FuseCP.Providers.OS;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Hosting.WindowsServices;
using Microsoft.Extensions.Hosting.Systemd;
#endif

namespace FuseCP.SchedulerService
{
    static class Program
    {
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		static void Main(string[] args)
        {
#if NETFRAMEWORK
            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[] 
			{ 
				new SchedulerService() 
			};
            ServiceBase.Run(ServicesToRun);
#else
            var builder = Host.CreateDefaultBuilder(args);

			ScheduleWorker.RunAsService = true;
			
			builder.ConfigureServices((hostContext, services) =>
			{
				Web.Services.Configuration.Read(hostContext.Configuration, args);
				services.AddHostedService<ScheduleWorker>();
			});

			if (OSInfo.IsWindows) builder.UseWindowsService();
			else if (OSInfo.IsSystemd) builder.UseSystemd();

			var host = builder.Build();
			host.Run();
#endif
		}
	}
}
