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

using System.ComponentModel;
using System.Configuration.Install;
using System.ServiceProcess;

namespace FuseCP.SchedulerService
{
    [RunInstaller(true)]
    public class SchedulerServiceInstaller : Installer
    {
        #region Constructor

        public SchedulerServiceInstaller()
        {
            var processInstaller = new ServiceProcessInstaller();
            var serviceInstaller = new ServiceInstaller();

            processInstaller.Account = ServiceAccount.LocalSystem;
            serviceInstaller.DisplayName = "FuseCP Scheduler";
            serviceInstaller.StartType = ServiceStartMode.Automatic;
            serviceInstaller.ServiceName = "FuseCP Scheduler";

            Installers.Add(processInstaller);
            Installers.Add(serviceInstaller);
        }

        #endregion
    }
}
#endif
