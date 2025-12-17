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

#if NETCOREAPP
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Hosting.Systemd;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace FuseCP.Web.Services;

public class IdleShutdownService : BackgroundService
{
	private readonly IHostApplicationLifetime Lifetime;
	private static DateTime LastRequestTime = DateTime.UtcNow;
	public static readonly TimeSpan DefaultIdleTimeout = TimeSpan.FromMinutes(5); // Set your idle timeout
	public static TimeSpan IdleTimeout = DefaultIdleTimeout; // Set your idle timeout
	public ISystemdNotifier SystemdNotifier { get; set; }

	public IdleShutdownService(IHostApplicationLifetime lifetime, ISystemdNotifier notifier)
	{
		Lifetime = lifetime;
		SystemdNotifier = notifier;
		Lifetime.ApplicationStarted.Register(() => SystemdNotifier.Notify(ServiceState.Ready));
	}

	public static void UpdateLastRequestTime()
	{
		LastRequestTime = DateTime.UtcNow;
	}

	protected override async Task ExecuteAsync(CancellationToken stoppingToken)
	{
		while (!stoppingToken.IsCancellationRequested)
		{
			var idleTime = DateTime.UtcNow - LastRequestTime;
			if (idleTime > IdleTimeout)
			{
				Console.WriteLine("Idle timeout reached. Shutting down.");
				SystemdNotifier.Notify(ServiceState.Stopping);
				Lifetime.StopApplication();
				break;
			}

			await Task.Delay(TimeSpan.FromSeconds(30), stoppingToken);
		}
	}
}

public class ActivityTrackingMiddleware
{
	private readonly RequestDelegate Next;

	public ActivityTrackingMiddleware(RequestDelegate next)
	{
		Next = next;
	}

	public async Task InvokeAsync(HttpContext context)
	{
		IdleShutdownService.UpdateLastRequestTime();
		await Next(context);
		IdleShutdownService.UpdateLastRequestTime();
	}
}

public static class ApplicationBuilderExtensions
{
	public static void UseIdleTimeout(this IApplicationBuilder app, TimeSpan idleTimeout = default)
	{
		if (idleTimeout != default) IdleShutdownService.IdleTimeout = idleTimeout;
		app.UseMiddleware<ActivityTrackingMiddleware>();
	}
}
#endif
