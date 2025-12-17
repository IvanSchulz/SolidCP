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
    public class VirtualHardDiskInfo
    {
        public VirtualHardDiskInfo()
        {

        }

        public VirtualHardDiskInfo(long fileSize, bool inSavedState, bool inUse, long maxInternalSize, string parentPath, VirtualHardDiskType diskType,
            bool supportPersistentReservations, ulong maximumIOPS, ulong minimumIOPS, ControllerType vhdControllerType, int controllerNumber, int controllerLocation,
            string name, string path, VirtualHardDiskFormat diskFormat, bool attached, uint blockSizeBytes)
        {
            FileSize = fileSize;
            InSavedState = inSavedState;
            InUse = inUse;
            MaxInternalSize = maxInternalSize;
            ParentPath = parentPath;
            DiskType = diskType;
            SupportPersistentReservations = supportPersistentReservations;
            MaximumIOPS = maximumIOPS;
            MinimumIOPS = minimumIOPS;
            VHDControllerType = vhdControllerType;
            ControllerNumber = controllerNumber;
            ControllerLocation = controllerLocation;
            Name = name;
            Path = path;
            DiskFormat = diskFormat;
            Attached = attached;
            BlockSizeBytes = blockSizeBytes;
        }

        public VirtualHardDiskInfo Clone()
        {
            return new VirtualHardDiskInfo(FileSize, InSavedState, InUse, MaxInternalSize, ParentPath, DiskType, SupportPersistentReservations, MaximumIOPS,
                MinimumIOPS, VHDControllerType, ControllerNumber, ControllerLocation, Name, Path, DiskFormat, Attached, BlockSizeBytes);
        }

        public long FileSize { get; set; }
        public bool InSavedState { get; set; }
        public bool InUse { get; set; }
        public long MaxInternalSize { get; set; }
        public string ParentPath { get; set; }
        public VirtualHardDiskType DiskType { get; set; }
        public bool SupportPersistentReservations { get; set; }
        public ulong MaximumIOPS { get; set; }
        public ulong MinimumIOPS { get; set; }
        public ControllerType VHDControllerType { get; set; }
        public int ControllerNumber { get; set; }
        public int ControllerLocation { get; set; }
        public string Name { get; set; }
        public string Path { get; set; }
        public VirtualHardDiskFormat DiskFormat { get; set; }
        public bool Attached { get; set; }
        public uint BlockSizeBytes { get; set; }
    }
}
