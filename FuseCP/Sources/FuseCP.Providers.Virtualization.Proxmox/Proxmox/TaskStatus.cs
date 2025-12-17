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
    public class ProxmoxTaskStatus
    {
        public string exitstatus { get; set; }
        public string status { get; set; }
        public string upid { get; set; }
        public string node { get; set; }
        public long pid { get; set; }
        public long starttime { get; set; }
        public string user { get; set; }
        public string type { get; set; }
        public string id { get; set; }
        public long pstart { get; set; }
    }
}
