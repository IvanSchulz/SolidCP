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
using System.Collections.ObjectModel;
using System.Linq;
using System.Management.Automation;
using System.Management.Automation.Runspaces;
using System.Text;
using System.Threading.Tasks;

namespace FuseCP.Providers.Virtualization
{
    public static class DvdDriveHelper
    {
        public static DvdDriveInfo Get(PowerShellManager powerShell, string vmName)
        {
            DvdDriveInfo info = null;

            PSObject result = GetPS(powerShell, vmName);

            if (result != null)
            {
                info = new DvdDriveInfo();
                info.Id = result.GetString("Id");
                info.Name = result.GetString("Name");
                info.ControllerType = result.GetEnum<ControllerType>("ControllerType");
                info.ControllerNumber = result.GetInt("ControllerNumber");
                info.ControllerLocation = result.GetInt("ControllerLocation");
                info.Path = result.GetString("Path");
            }
            return info;
        }

        public static PSObject GetPS(PowerShellManager powerShell, string vmName)
        {
            Command cmd = new Command("Get-VMDvdDrive");

            cmd.Parameters.Add("VMName", vmName);

            Collection<PSObject> result = powerShell.Execute(cmd, true);

            if (result != null && result.Count > 0)
            {
                return result[0];
            }
            
            return null;
        }

        public static void Set(PowerShellManager powerShell, string vmName, string path)
        {
            var dvd = Get(powerShell, vmName);
 
            Command cmd = new Command("Set-VMDvdDrive");

            cmd.Parameters.Add("VMName", vmName);
            cmd.Parameters.Add("Path", path);
            cmd.Parameters.Add("ControllerNumber", dvd.ControllerNumber);
            cmd.Parameters.Add("ControllerLocation", dvd.ControllerLocation);

            powerShell.Execute(cmd, true);
        }

        public static void Update(PowerShellManager powerShell, VirtualMachine vm, bool dvdDriveShouldBeInstalled)
        {
            if (!vm.DvdDriveInstalled && dvdDriveShouldBeInstalled)
                Add(powerShell, vm.Name);
            else if (vm.DvdDriveInstalled && !dvdDriveShouldBeInstalled)
                Remove(powerShell, vm.Name);
        }

        public static void Add(PowerShellManager powerShell, string vmName)
        {
            Command cmd = new Command("Add-VMDvdDrive");

            cmd.Parameters.Add("VMName", vmName);

            powerShell.Execute(cmd, true);
        }

        public static void Remove(PowerShellManager powerShell, string vmName)
        {
            var dvd = Get(powerShell, vmName);

            Command cmd = new Command("Remove-VMDvdDrive");

            cmd.Parameters.Add("VMName", vmName);
            cmd.Parameters.Add("ControllerNumber", dvd.ControllerNumber);
            cmd.Parameters.Add("ControllerLocation", dvd.ControllerLocation);

            powerShell.Execute(cmd, true);
        }
    }
}
