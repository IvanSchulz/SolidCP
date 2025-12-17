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

namespace FuseCP.EnterpriseServer
{
    [Flags]
    public enum DemandAccount
    {
        NotDemo = 0x1,
        IsActive = 0x2,
        IsAdmin = 0x4,
        IsReseller = 0x8,
        IsPlatformCSR = 0x10,
        IsPlatformHelpdesk = 0x20,
        IsResellerCSR = 0x40,
        IsResellerHelpdesk = 0x80,
    }
}
