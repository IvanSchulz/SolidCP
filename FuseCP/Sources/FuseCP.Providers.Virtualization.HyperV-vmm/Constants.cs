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

namespace FuseCP.Providers.Virtualization
{
    public static class Constants
    {
        public const string CONFIG_USE_DISKPART_TO_CLEAR_READONLY_FLAG = "FuseCP.HyperV.UseDiskPartClearReadOnlyFlag";
        public const string WMI_VIRTUALIZATION_NAMESPACE = @"root\scvmm";
        public const string WMI_CIMV2_NAMESPACE = @"root\cimv2";

        public const string LIBRARY_INDEX_FILE_NAME = "index.xml";

        public const string EXTERNAL_NETWORK_ADAPTER_NAME = "External Network Adapter";
        public const string PRIVATE_NETWORK_ADAPTER_NAME = "Private Network Adapter";
        public const string MANAGEMENT_NETWORK_ADAPTER_NAME = "Management Network Adapter";

        public const Int64 Size1G = 0x40000000;
        public const Int64 Size1M = 0x100000;
        public const Int64 Size1K = 1024;

        public const string KVP_RAM_SUMMARY_KEY = "VM-RAM-Summary";
        public const string KVP_HDD_SUMMARY_KEY = "VM-HDD-Summary";
    }
}
