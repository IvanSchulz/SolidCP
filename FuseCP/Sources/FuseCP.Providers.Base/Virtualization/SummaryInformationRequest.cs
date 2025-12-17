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
    public enum SummaryInformationRequest
    {
        Name = 0,
        ElementName = 1,
        CreationTime = 2,
        Notes = 3,
        NumberOfProcessors = 4,
        SmallThumbnailImage = 5, // (80x60)
        MediumThumbnailImage = 6, // (160x120)
        LargeThumbnailImage = 7, // (320x240)
        EnabledState = 100,
        ProcessorLoad = 101,
        ProcessorLoadHistory = 102,
        MemoryUsage = 103,
        Heartbeat = 104,
        Uptime = 105,
        GuestOperatingSystem = 106,
        Snapshots = 107,
        AsynchronousTasks = 108,
        HealthState  = 109,
        VirtualSystemSubType = 135 //VM Generation
    }
}
