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
    public class VMConfig
    {
        public long baloon { get; set; }
        public string bootdisk { get; set; }
        public long cores { get; set; }
        public string cpu { get; set; }
        public string digest { get; set; }
        public string ide2 { get; set; }
        public long memory { get; set; }
        public string name { get; set; }
        public string net0 { get; set; }
        public string ostype { get; set; }
        public string parent { get; set; }
        public string smbios1 { get; set; }
        public string virtio0 { get; set; }
        public string scsi0 { get; set; }
    }
}
