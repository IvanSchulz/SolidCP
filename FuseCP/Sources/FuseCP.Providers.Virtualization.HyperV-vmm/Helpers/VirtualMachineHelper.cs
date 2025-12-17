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
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Management.Automation;
using System.Management.Automation.Runspaces;
using System.Text;
using System.Threading.Tasks;

namespace FuseCP.Providers.Virtualization
{
    public static class VirtualMachineHelper
    {
        public static OperationalStatus GetVMHeartBeatStatus(PowerShellManager powerShell, string name)
        {
            OperationalStatus status = OperationalStatus.None;

            Command cmd = new Command("Get-VMIntegrationService");

            cmd.Parameters.Add("VMName", name);
            cmd.Parameters.Add("Name", "HeartBeat");

            Collection<PSObject> result = powerShell.Execute(cmd, true);
            if (result != null && result.Count > 0)
            {
                var statusString = result[0].GetProperty("PrimaryOperationalStatus");

                if (statusString != null)
                    status = (OperationalStatus)Enum.Parse(typeof(OperationalStatus), statusString.ToString());
            }
            return status;
        }

        public static int GetVMProcessors(PowerShellManager powerShell, string name)
        {
            int procs = 0;

            Command cmd = new Command("Get-VMProcessor");

            cmd.Parameters.Add("VMName", name);

            Collection<PSObject> result = powerShell.Execute(cmd, true);
            if (result != null && result.Count > 0)
            {
                procs = Convert.ToInt32(result[0].GetProperty("Count"));

            }
            return procs;
        }

        public static void UpdateProcessors(PowerShellManager powerShell, VirtualMachine vm, int cpuCores, int cpuLimitSettings, int cpuReserveSettings, int cpuWeightSettings)
        {
            Command cmd = new Command("Set-VMProcessor");

            cmd.Parameters.Add("VMName", vm.Name);
            cmd.Parameters.Add("Count", cpuCores);
            cmd.Parameters.Add("Maximum", cpuLimitSettings);
            cmd.Parameters.Add("Reserve", cpuReserveSettings);
            cmd.Parameters.Add("RelativeWeight", cpuWeightSettings);

            powerShell.Execute(cmd, true);
        }

        public static void Delete(PowerShellManager powerShell, string vmName, string vmId, string server, string clusterName)
        {
            if (!String.IsNullOrEmpty(clusterName))
            {
                Command cmdCluster = new Command("Remove-ClusterGroup");
                cmdCluster.Parameters.Add("VMId", vmId);
                cmdCluster.Parameters.Add("RemoveResources");
                cmdCluster.Parameters.Add("Force");
                powerShell.Execute(cmdCluster, false);
            }
            Command cmd = new Command("Remove-VM");
            cmd.Parameters.Add("Name", vmName);
            if (!string.IsNullOrEmpty(server)) cmd.Parameters.Add("ComputerName", server);
            cmd.Parameters.Add("Force");
            powerShell.Execute(cmd, false, true);
        }

        public static void Stop(PowerShellManager powerShell, string vmName, bool force, string server)
        {
            Command cmd = new Command("Stop-VM");

            cmd.Parameters.Add("Name", vmName);
            if (force) cmd.Parameters.Add("Force");
            if (!string.IsNullOrEmpty(server)) cmd.Parameters.Add("ComputerName", server);
            //if (!string.IsNullOrEmpty(reason)) cmd.Parameters.Add("Reason", reason);

            powerShell.Execute(cmd, false);
        }
    }
}
