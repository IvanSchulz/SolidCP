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
using System.Threading.Tasks;

namespace FuseCP.Providers.OS
{
	public class WindowsServiceController : ServiceController
	{
		public override bool IsInstalled => true;

		public override IEnumerable<OSService> All() => OSInfo.Windows.GetOSServices();

		public override void ChangeStatus(string serviceId, OSServiceStatus status)
		{
			OSInfo.Windows.ChangeOSServiceStatus(serviceId, status);
		}

		public override OSService Info(string serviceId)
			=> All().Where(s => s.Id == serviceId).FirstOrDefault();

		public override ServiceManager Install(ServiceDescription service)
		{
			var winService = service as WindowsServiceDescription;
			var cmd = $"sc.exe create {service.ServiceId} binPath= \"{service.Executable}\"";
			if (winService != null)
			{
				if (!string.IsNullOrEmpty(winService.DisplayName)) cmd += $" DisplayName= \"{winService.DisplayName}\"";
				if (winService.DependsOn != null && winService.DependsOn.Any())
				{
					cmd += $" depend= \"{string.Join("/", winService.DependsOn.Select(dep => dep.Trim()))}";
				}
				if (!string.IsNullOrEmpty(winService.Object))
				{
					cmd += $" obj= \"{winService.Object}\"";
				}
				if (winService.Type != WindowsServiceType.Own) cmd += $" type= {winService.Type.ToString().ToLower()}";
				if (winService.Error != WindowsServiceErrorHandling.Normal) cmd += $" error= {winService.Error.ToString().ToLower()}";
				if (winService.Start != WindowsServiceStartMode.Demand) cmd += $" start= {winService.Start.ToString().ToLower()}";
				if (winService.Tag.HasValue) cmd += $" tag= {(winService.Tag.Value ? "yes" : "no")}";
				if (!string.IsNullOrEmpty(winService.Group)) cmd += $" group= \"{winService.Group}\"";
				if (!string.IsNullOrEmpty(winService.Password)) cmd += $" password= \"{winService.Password}\"";
			}
			Shell.Standard.Exec(cmd);

			return new ServiceManager(this, service.ServiceId);
		}

		public override void Remove(string serviceId)
		{
			Shell.Standard.Exec($"sc.exe delete {serviceId}");
		}

		public override void SystemReboot() => Shell.Standard.Exec("shutdown.exe /r /f /t 0");
	}
}
