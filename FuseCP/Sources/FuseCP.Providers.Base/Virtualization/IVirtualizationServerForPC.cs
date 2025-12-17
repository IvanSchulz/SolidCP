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

﻿using System;
using System.Collections.Generic;
using System.Text;
using FuseCP.Providers.Virtualization;

namespace FuseCP.Providers.VirtualizationForPC
{
    public interface IVirtualizationServerForPC
    {
        // Virtual Machines
        VMInfo GetVirtualMachine(string vmId);
        VirtualMachine GetVirtualMachineEx(string vmId);
        List<VirtualMachine> GetVirtualMachines();
        byte[] GetVirtualMachineThumbnailImage(string vmId, ThumbnailSize size);
        VMInfo CreateVirtualMachine(VMInfo vm);
        VMInfo CreateVMFromVM(string sourceName, VMInfo vmTemplate, Guid taskGuid);
        VMInfo UpdateVirtualMachine(VMInfo vm);
        JobResult ChangeVirtualMachineState(string vmId, VirtualMachineRequestedState newState);
        ReturnCode ShutDownVirtualMachine(string vmId, bool force, string reason);
        List<ConcreteJob> GetVirtualMachineJobs(string vmId);
        JobResult RenameVirtualMachine(string vmId, string name);
        //JobResult ExportVirtualMachine(string vmId, string exportPath);
        JobResult DeleteVirtualMachine(string vmId);
        VMInfo MoveVM(VMInfo vmForMove);

        //TODO: Может быть прийдется менять возвращаемый результат
        void ConfigureCreatedVMNetworkAdapters(VMInfo vmInfo);

        // Snapshots
        List<VirtualMachineSnapshot> GetVirtualMachineSnapshots(string vmId);
        VirtualMachineSnapshot GetSnapshot(string snapshotId);
        JobResult CreateSnapshot(string vmId);
        JobResult RenameSnapshot(string vmId, string snapshotId, string name);
        JobResult ApplySnapshot(string vmId, string snapshotId);
        JobResult DeleteSnapshot(string vmId, string snapshotId);
        JobResult DeleteSnapshotSubtree(string snapshotId);
        byte[] GetSnapshotThumbnailImage(string snapshotId, ThumbnailSize size);

        // Virtual Switches
        List<VirtualSwitch> GetExternalSwitches(string computerName);
        List<VirtualSwitch> GetSwitches();
        bool SwitchExists(string switchId);
        VirtualSwitch CreateSwitch(string name);
        ReturnCode DeleteSwitch(string switchId);

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

        // Library
        LibraryItem[] GetLibraryItems(string path);
        LibraryItem[] GetOSLibraryItems();
        LibraryItem[] GetHosts();
        LibraryItem[] GetClusters();

        // Storage
        VirtualHardDiskInfo GetVirtualHardDiskInfo(string vhdPath);
        MountedDiskInfo MountVirtualHardDisk(string vhdPath);
        ReturnCode UnmountVirtualHardDisk(string vhdPath);
        JobResult ExpandVirtualHardDisk(string vhdPath, UInt64 sizeGB);
        JobResult ConvertVirtualHardDisk(string sourcePath, string destinationPath, VirtualHardDiskType diskType);
        void ExpandDiskVolume(string diskAddress, string volumeName);
        void DeleteRemoteFile(string path);
        string ReadRemoteFile(string path);
        void WriteRemoteFile(string path, string content);

        // Jobs
        ConcreteJob GetJob(string jobId);
        List<ConcreteJob> GetAllJobs();
        ChangeJobStateReturnCode ChangeJobState(string jobId, ConcreteJobRequestedState newState);

        // Configuration
        int GetProcessorCoresNumber(string templateId);

        // Monitoring
        List<MonitoredObjectEvent> GetDeviceEvents(string serviceName, string displayName);
        List<MonitoredObjectAlert> GetMonitoringAlerts(string serviceName, string virtualMachineName);
        List<PerformanceDataValue> GetPerfomanceValue(string VmName, PerformanceType perf, DateTime startPeriod, DateTime endPeriod);

        // Networks
        VirtualNetworkInfo[] GetVirtualNetworkByHostName(string hostName);

        bool CheckServerState(VMForPCSettingsName control, string connString, string connName);
    }
}
