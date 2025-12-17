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

namespace FuseCP.Providers.Virtualization.Proxmox
{
    public class VMStatusInfo
    {
        public string data { get; set; }
        public string status { get; set; }
        public long maxdisk { get; set; }
        public string ballooninfo { get; set; }
        public int cpus { get; set; }
        public long uptime { get; set; }
        public long mem { get; set; }
        public string name { get; set; }
        public float cpu { get; set; }
        public long total_mem { get; set; }
        public long free_mem { get; set; }
        public long maxmem { get; set; }
        public string qmpstatus { get; set; }
    }
}
