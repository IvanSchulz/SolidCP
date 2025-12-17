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


namespace FuseCP.Providers.Virtualization
{
    public class VirtualMachine : ServiceProviderItem
    {
        public VirtualMachine()
        {
        }

        // properties
        [Persistent]
        public string VirtualMachineId { get; set; }
        public string Hostname { get; set; }
        public string Domain { get; set; }
        [Persistent]
        public string Version { get; set; }

        public VirtualMachineState State { get; set; }
        public long Uptime { get; set; }
        public OperationalStatus Heartbeat { get; set; }

        [Persistent]
        public string CreationTime { get; set; }
        [Persistent]
        public string RootFolderPath { get; set; }
        [Persistent]
        public string[] VirtualHardDrivePath { get; set; }
        [Persistent]
        public string OperatingSystemTemplate { get; set; }
        [Persistent]
        public string OperatingSystemTemplatePath { get; set; }
        [Persistent]
        public string OperatingSystemTemplateDeployParams { get; set; }
        [Persistent]
        public string AdministratorPassword { get; set; }

        [Persistent]
        public string CurrentTaskId { get; set; }
        [Persistent]
        public VirtualMachineProvisioningStatus ProvisioningStatus { get; set; }


        [Persistent]
        public int CpuCores { get; set; }
        public int CpuUsage { get; set; }

        [Persistent]
        public int RamSize { get; set; }
        public int RamUsage { get; set; }

        [Persistent]
        public DynamicMemory DynamicMemory { get; set; }

        [Persistent]
        public int[] HddSize { get; set; }
        public LogicalDisk[] HddLogicalDisks { get; set; }
        [Persistent]
        public int HddMaximumIOPS { get; set; }
        [Persistent]
        public int HddMinimumIOPS { get; set; }
        [Persistent]
        public int SnapshotsNumber { get; set; }

        [Persistent]
        public bool DvdDriveInstalled { get; set; }
        [Persistent]
        public bool BootFromCD { get; set; }
        [Persistent]
        public bool NumLockEnabled { get; set; }

        [Persistent]
        public bool StartTurnOffAllowed { get; set; }
        [Persistent]
        public bool PauseResumeAllowed { get; set; }
        [Persistent]
        public bool RebootAllowed { get; set; }
        [Persistent]
        public bool ResetAllowed { get; set; }
        [Persistent]
        public bool ReinstallAllowed { get; set; }

        [Persistent]
        public bool LegacyNetworkAdapter { get; set; }
        [Persistent]
        public bool RemoteDesktopEnabled { get; set; }

        [Persistent]
        public bool ExternalNetworkEnabled { get; set; }
        [Persistent]
        public string ExternalNicMacAddress { get; set; }
        [Persistent]
        public string ExternalSwitchId { get; set; }

        [Persistent]
        public bool PrivateNetworkEnabled { get; set; }
        [Persistent]
        public string PrivateNicMacAddress { get; set; }
        [Persistent]
        public string PrivateSwitchId { get; set; }
        [Persistent]
        public int PrivateNetworkVlan { get; set; }

        [Persistent]
        public bool ManagementNetworkEnabled { get; set; }
        [Persistent]
        public string ManagementNicMacAddress { get; set; }
        [Persistent]
        public string ManagementSwitchId { get; set; }

        [Persistent]
        public bool DmzNetworkEnabled { get; set; }
        [Persistent]
        public string DmzNicMacAddress { get; set; }
        [Persistent]
        public string DmzSwitchId { get; set; }
        [Persistent]
        public int DmzNetworkVlan { get; set; }

        // for GetVirtualMachineEx used in import method
        public VirtualMachineNetworkAdapter[] Adapters { get; set; }

        [Persistent]
        public VirtualHardDiskInfo[] Disks { get; set; }

        [Persistent]
        public string Status { get; set; }

        public ReplicationState ReplicationState { get; set; }

        [Persistent]
        public int Generation { get; set; }
        [Persistent]
        public string SecureBootTemplate { get; set; }
        [Persistent]
        public bool EnableSecureBoot { get; set; }

        [Persistent]
        public int ProcessorCount { get; set; }

        [Persistent]
        public string ParentSnapshotId { get; set; }
        [Persistent]
        public int defaultaccessvlan { get; set; }//external network vlan
        public VirtualMachineIPAddress PrimaryIP { get; set; }
        public bool NeedReboot { get; set; } //give access to force reboot a server.
        [Persistent]
        public string CustomPrivateGateway { get; set; }
        [Persistent]
        public string CustomPrivateDNS1 { get; set; }
        [Persistent]
        public string CustomPrivateDNS2 { get; set; }
        [Persistent]
        public string CustomPrivateMask { get; set; }
        [Persistent]
        public string CustomDmzGateway { get; set; }
        [Persistent]
        public string CustomDmzDNS1 { get; set; }
        [Persistent]
        public string CustomDmzDNS2 { get; set; }
        [Persistent]
        public string CustomDmzMask { get; set; }
        [Persistent]
        public string ClusterName { get; set; }
        public bool IsClustered { get; set; }
    }
}
