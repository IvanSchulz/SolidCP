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
using System.Text;
using FuseCP.Providers.OS;
using FuseCP.Providers.Common;

namespace FuseCP.Providers.Virtualization
{
    public interface IVirtualizationServer2012
    {
        // Virtual Machines
        VirtualMachine GetVirtualMachine(string vmId);
        VirtualMachine GetVirtualMachineEx(string vmId);
        JobResult ExecuteCustomPsScript(string script);
        List<VirtualMachine> GetVirtualMachines();
        byte[] GetVirtualMachineThumbnailImage(string vmId, ThumbnailSize size);
        VirtualMachine CreateVirtualMachine(VirtualMachine vm);
        VirtualMachine UpdateVirtualMachine(VirtualMachine vm);
        JobResult ChangeVirtualMachineState(string vmId, VirtualMachineRequestedState newState, string clusterName);
        ReturnCode ShutDownVirtualMachine(string vmId, bool force, string reason);
        List<ConcreteJob> GetVirtualMachineJobs(string vmId);
        JobResult RenameVirtualMachine(string vmId, string name, string clusterName);
        JobResult ExportVirtualMachine(string vmId, string exportPath);
        JobResult DeleteVirtualMachine(string vmId, string clusterName);
        JobResult DeleteVirtualMachineExtended(string vmId, string clusterName);
        bool IsTryToUpdateVirtualMachineWithoutRebootSuccess(VirtualMachine vm);

        // Snapshots
        List<VirtualMachineSnapshot> GetVirtualMachineSnapshots(string vmId);
        VirtualMachineSnapshot GetSnapshot(string snapshotId);
        JobResult CreateSnapshot(string vmId);
        JobResult RenameSnapshot(string vmId, string snapshotId, string name);
        JobResult ApplySnapshot(string vmId, string snapshotId);
        JobResult DeleteSnapshot(string snapshotId);
        JobResult DeleteSnapshotSubtree(string snapshotId);
        byte[] GetSnapshotThumbnailImage(string snapshotId, ThumbnailSize size);

        // Virtual Switches
        List<VirtualSwitch> GetExternalSwitches(string computerName);
        List<VirtualSwitch> GetExternalSwitchesWMI(string computerName);
        List<VirtualSwitch> GetInternalSwitches(string computerName);
        List<VirtualSwitch> GetSwitches();
        bool SwitchExists(string switchId);
        VirtualSwitch CreateSwitch(string name);
        ReturnCode DeleteSwitch(string switchId);

        // Secure Boot
        List<SecureBootTemplate> GetSecureBootTemplates(string computerName);

        // IP operations
        List<VirtualMachineNetworkAdapter> GetVirtualMachinesNetwordAdapterSettings(string vmId);
        JobResult InjectIPs(string vmId, GuestNetworkAdapterConfiguration guestNetworkAdapterConfiguration);

        // DVD operations
        string GetInsertedDVD(string vmId);
        JobResult InsertDVD(string vmId, string isoPath);
        JobResult EjectDVD(string vmId);

        // KVP items
        List<KvpExchangeDataItem> GetKVPItems(string vmId);
        List<KvpExchangeDataItem> GetStandardKVPItems(string vmId);
        JobResult AddKVPItems(string vmId, KvpExchangeDataItem[] items);
        JobResult RemoveKVPItems(string vmId, string[] itemNames);
        JobResult ModifyKVPItems(string vmId, KvpExchangeDataItem[] items);

        // Storage
        bool IsEmptyFolders(string path);
        bool FileExists(string path);
        VirtualHardDiskInfo GetVirtualHardDiskInfo(string vhdPath);
        MountedDiskInfo MountVirtualHardDisk(string vhdPath);
        ReturnCode UnmountVirtualHardDisk(string vhdPath);
        JobResult ExpandVirtualHardDisk(string vhdPath, UInt64 sizeGB);
        JobResult ConvertVirtualHardDisk(string sourcePath, string destinationPath, VirtualHardDiskType diskType, uint blockSizeBytes);
        JobResult CreateVirtualHardDisk(string destinationPath, VirtualHardDiskType diskType, uint blockSizeBytes, UInt64 sizeGB);
        void ExpandDiskVolume(string diskAddress, string volumeName);
        void DeleteRemoteFile(string path);
        string ReadRemoteFile(string path);
        void WriteRemoteFile(string path, string content);

        // Jobs
        ConcreteJob GetJob(string jobId);
        ConcreteJob GetPsJob(string jobId);
        List<ConcreteJob> GetAllJobs();
        void ClearOldPsJobs();
        ChangeJobStateReturnCode ChangeJobState(string jobId, ConcreteJobRequestedState newState);

        // Server information
        SystemResourceUsageInfo GetSystemResourceUsageInfo();
        SystemMemoryInfo GetSystemMemoryInfo();

        // Configuration
        int GetProcessorCoresNumber();
        List<VMConfigurationVersion> GetVMConfigurationVersionSupportedList();

        // Replication 
        List<CertificateInfo> GetCertificates(string remoteServer);
        void SetReplicaServer(string remoteServer, string thumbprint, string storagePath);
        void UnsetReplicaServer(string remoteServer);
        ReplicationServerInfo GetReplicaServer(string remoteServer);
        void EnableVmReplication(string vmId, string replicaServer, VmReplication replication);
        void SetVmReplication(string vmId, string replicaServer, VmReplication replication);
        void TestReplicationServer(string vmId, string replicaServer, string localThumbprint);
        void StartInitialReplication(string vmId);
        VmReplication GetReplication(string vmId);
        void DisableVmReplication(string vmId);
        ReplicationDetailInfo GetReplicationInfo(string vmId);
        void PauseReplication(string vmId);
        void ResumeReplication(string vmId);
    }
}
