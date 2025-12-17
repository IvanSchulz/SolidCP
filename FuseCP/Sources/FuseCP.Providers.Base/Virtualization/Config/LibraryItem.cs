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

namespace FuseCP.Providers.Virtualization
{
	public class LibraryItem
	{
		public string LibraryID => $"{Name ?? ""}/{Path ?? ""}/{DeployScriptParams ?? ""}";
		public string Path { get; set; }
		public string Name { get; set; }
		public int Generation { get; set; }
		public string SecureBootTemplate { get; set; }
		public bool EnableSecureBoot { get; set; } = true; //by default is true
		public string Description { get; set; }
		public bool LegacyNetworkAdapter { get; set; }
		public bool RemoteDesktop { get; set; }
		public long DiskSize { get; set; }
		public uint VhdBlockSizeBytes { get; set; }
		public int ProcessVolume { get; set; }
		public string[] SysprepFiles { get; set; }
		public bool ProvisionAdministratorPassword { get; set; }
		public bool ProvisionComputerName { get; set; }
		public bool ProvisionNetworkAdapters { get; set; }
		public string DeployScriptParams { get; set; }
		public string CDKey { get; set; }
		public string TimeZoneId { get; set; }

		public VirtualNetworkInfo[] Networks { get; set; }

	}
}
