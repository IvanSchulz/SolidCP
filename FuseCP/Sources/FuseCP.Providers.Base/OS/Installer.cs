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
using System.Linq;
using System.Diagnostics;

namespace FuseCP.Providers.OS
{
	public abstract class Installer
	{
		public virtual Shell Shell { get; set; } = OSInfo.Current.DefaultShell;
		public virtual Shell Install(string apps) => InstallAsync(apps).Task().Result;
		public abstract Shell InstallAsync(string apps);
		public virtual Shell AddSources(string sources) => AddSourcesAsync(sources).Task().Result;
		public abstract Shell AddSourcesAsync(string sources);
		public abstract bool IsInstallerInstalled { get; }
		public abstract bool IsInstalled(string apps);
		public void CheckInstallerInstalled()
		{
			if (!IsInstallerInstalled) throw new PlatformNotSupportedException($"The installer type {this.GetType().Name} is not installed on this system.");
		}
		public virtual Shell Remove(string apps) => RemoveAsync(apps).Task().Result;
		public abstract Shell RemoveAsync(string apps);
		public virtual Shell Update() => UpdateAsync().Task().Result;
		public abstract Shell UpdateAsync();
		public static Installer Default => OSInfo.Current.DefaultInstaller;

	}

}
