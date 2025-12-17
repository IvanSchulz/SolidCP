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
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FuseCP.Providers.OS
{
    public class SystemResourceUsageInfo
    {
        public short ProcessorTimeUsagePercent { get; set; } = -1; //Non Hyper-V Processor Information(_Total)\% Processor Time (useless for Hyper-V hosts)
        public short LogicalProcessorUsagePercent { get; set; } = -1; //Hyper-V Hypervisor Logical Processor(_Total)\% Total Run Time
        public SystemMemoryInfo SystemMemoryInfo { get; set; }

        //We cannot collect "Hyper-V Hypervisor Virtual Processor(*)\% Total Run Time" for all VMs, as it causes excessive load on the server, with memory leacking
    }
}
