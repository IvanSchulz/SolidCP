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

namespace FuseCP.HyperV.Utils
{
    public enum ReturnCode
    {
        OK = 0,
        JobStarted = 4096,
        Failed = 32768,
        AccessDenied = 32769,
        NotSupported = 32770,
        Unknown = 32771,
        Timeout = 32772,
        InvalidParameter = 32773,
        SystemIsInUse = 32774,
        InvalidStateForThisOperation = 32775,
        IncorrectDataType = 32776,
        SystemIsNotAvailable = 32777,
        OutOfMemory = 32778
    }
}
