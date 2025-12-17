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

namespace FuseCP.WIXInstaller.Common
{
    internal struct Prop
    {
        public const string REQ_LOG = "PI_PREREQ_LOG";
        public const string REQ_NETFRAMEWORK20 = "NETFRAMEWORK20";
        public const string REQ_NETFRAMEWORK35 = "NETFRAMEWORK35";
        public const string REQ_NETFRAMEWORK40FULL = "NETFRAMEWORK40FULL";
        public const string REQ_OS = "PI_PREREQ_OS";
        public const string REQ_IIS = "PI_PREREQ_IIS";
        public const string REQ_ASPNET = "PI_PREREQ_ASPNET";
    }
}
