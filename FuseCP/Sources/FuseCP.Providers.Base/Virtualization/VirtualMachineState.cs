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
    public enum VirtualMachineState
    {
        /*
        Unknown = 0,
        Running = 2, // start
        Off = 3, // turn off
        Reset = 10, // reset
        Paused = 32768, // pause
        Saved = 32769, // save
        Starting = 32770,
        Snapshotting = 32771,
        Migrating = 32772,
        Saving = 32773,
        Stopping = 32774,
        Deleted = 32775,
        Pausing = 32776
        */
        Snapshotting = 32771,
        Migrating = 32772,
        Deleted = 32775,

        Unknown = 0,
        Other = 1,
        Running = 2,
        Off = 3,
        Stopping = 32774, // new 4
        Saved = 32769, // new 6
        Paused = 32768, // new 9
        Starting = 32770, // new 10
        Reset = 10, // new 11
        Saving = 32773, // new 32773
        Pausing = 32776, // new 32776
        Resuming = 32777,
        FastSaved = 32779,
        FastSaving = 32780,
        RunningCritical = 32781,
        OffCritical = 32782,
        StoppingCritical = 32783,
        SavedCritical = 32784,
        PausedCritical = 32785,
        StartingCritical = 32786,
        ResetCritical = 32787,
        SavingCritical = 32788,
        PausingCritical = 32789,
        ResumingCritical = 32790,
        FastSavedCritical = 32791,
        FastSavingCritical = 32792
    }
}
