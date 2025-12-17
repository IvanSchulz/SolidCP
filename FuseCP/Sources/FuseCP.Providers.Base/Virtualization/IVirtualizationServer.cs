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

namespace FuseCP.Providers.Virtualization
{
    public interface IVirtualizationServer
    {
        // Virtual Machines
        VirtualMachine GetVirtualMachine(string vmId);
        VirtualMachine GetVirtualMachineEx(string vmId);
        List<VirtualMachine> GetVirtualMachines();
        byte[] GetVirtualMachineThumbnailImage(string vmId, ThumbnailSize size);
        VirtualMachine CreateVirtualMachine(VirtualMachine vm);
        VirtualMachine UpdateVirtualMachine(VirtualMachine vm);
        JobResult ChangeVirtualMachineState(string vmId, VirtualMachineRequestedState newState);
        ReturnCode ShutDownVirtualMachine(string vmId, bool force, string reason);
        List<ConcreteJob> GetVirtualMachineJobs(string vmId);
        JobResult RenameVirtualMachine(string vmId, string name);
        JobResult ExportVirtualMachine(string vmId, string exportPath);
        JobResult DeleteVirtualMachine(string vmId);

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
        int GetProcessorCoresNumber();

    }
}
