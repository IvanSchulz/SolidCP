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
    public enum ChangeJobStateReturnCode
    {
        OK = 0,
        NotSupported = 1,
        UnspecifiedError = 2,
        CannotCompleteWithinTimeoutPeriod = 3,
        Failed = 4,
        InvalidParameter = 5,
        InUse = 6,
        TransitionStarted = 4096,
        InvalidStateTransition = 4097,
        UseofTimeoutParameterNotSupported = 4098,
        Busy = 4099
    }
}
