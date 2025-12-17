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
    public enum VMComputerSystemStateInfo : int
    {
        CheckpointFailed = 213,
        CreatingCheckpoint = 210,
        CreationFailed = 101,
        CustomizationFailed = 105,
        Deleting = 13,
        DeletingCheckpoint = 211,
        DiscardingDrives = 80,
        DiscardSavedState = 10,
        FinishingCheckpointOperation = 215,
        HostNotResponding = 221,
        IncompleteVMConfig = 223,
        InitializingCheckpointOperation = 214,
        MergingDrives = 12,
        MigrationFailed = 201,
        Missing = 220,
        P2VCreationFailed = 240,
        Paused = 6,
        Pausing = 81,
        PoweringOff = 2,
        PowerOff = 1,
        RecoveringCheckpoint = 212,
        Restoring = 5,
        Running = 0,
        Saved = 3,
        Saving = 4,
        Starting = 11,
        Stored = 102,
        TemplateCreationFailed = 104,
        UnderCreation = 100,
        UnderMigration = 200,
        UnderTemplateCreation = 103,
        UnderUpdate = 106,
        Unsupported = 222,
        UnsupportedCluster = 225,
        UnsupportedSharedFiles = 224,
        UpdateFailed = 107,
        V2VCreationFailed = 250
    }
}
