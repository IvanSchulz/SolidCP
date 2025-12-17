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

using System.Management.Automation.Runspaces;

namespace FuseCP.Providers.Virtualization
{
    public static class ReplicaHelper
    {
        public static void SetReplicaServer(PowerShellManager powerShell, bool enabled, string remoteServer, string thumbprint, string storagePath)
        {
            Command cmd = new Command("Set-VMReplicationServer");
            cmd.Parameters.Add("ReplicationEnabled", enabled);

            if (!string.IsNullOrEmpty(remoteServer))
            {
                cmd.Parameters.Add("ComputerName", remoteServer);
            }

            if (!string.IsNullOrEmpty(thumbprint))
            {
                cmd.Parameters.Add("AllowedAuthenticationType", "Certificate");
                cmd.Parameters.Add("CertificateThumbprint", thumbprint);
            }

            if (!string.IsNullOrEmpty(storagePath))
            {
                cmd.Parameters.Add("ReplicationAllowedFromAnyServer", true);
                cmd.Parameters.Add("DefaultStorageLocation", storagePath);
            }

            powerShell.Execute(cmd, false);
        }

        public static void SetFirewallRule(PowerShellManager powerShell, bool enabled)
        {
            Command cmd = new Command("Enable-Netfirewallrule");
            cmd.Parameters.Add("DisplayName", "Hyper-V Replica HTTPS Listener (TCP-In)");

            powerShell.Execute(cmd, false);
        }

        public static void RemoveVmReplication(PowerShellManager powerShell, string vmName, string server)
        {
            Command cmd = new Command("Remove-VMReplication");
            cmd.Parameters.Add("VmName", vmName);
            if (!string.IsNullOrEmpty(server)) cmd.Parameters.Add("ComputerName", server);

            powerShell.Execute(cmd, false);
        }
    }
}
