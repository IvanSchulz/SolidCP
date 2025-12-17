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
using System.IO;
using System.Linq;
using System.Net;


namespace FuseCP.Providers.OS
{
    public class Apt: Installer
	{

		public override bool IsInstallerInstalled => Shell.Find("apt-get") != null;
		public override Shell InstallAsync(string apps)
		{
			return Shell.ExecAsync($"apt-get install -y {apps.Replace(',', ' ').Replace(';', ' ')}");
		}

		public override Shell AddSourcesAsync(string sources)
		{
			string list;
			IEnumerable<string> store;

			if (Directory.Exists("/etc/apt/sources.list.d"))
			{
				list = "/etc/apt/source.list.d/fusecp.list";
				store = Directory.EnumerateFiles("/etc/apt/source.list.d/*.list")
					.SelectMany(file => File.ReadAllLines(file))
					.Select(line => line.Trim());
			}
			else
			{
				list = "/etc/apt/source.list";
				store = (File.Exists(list) ? File.ReadAllLines(list) : new string[0])
					.Select(line => line.Trim());
			}
			var lines = sources.Split(';')
				.Select(line => line.Trim())
				.Except(store);

			File.AppendAllLines(list, lines);

			return UpdateAsync();
		}
		public override bool IsInstalled(string apps)
		{
			return !Shell.Exec($"dpkg -l {apps.Replace(';', ' ').Replace(';', ' ')}").Output().Result?.Contains("no packages found") ?? false;
		}

		public override Shell RemoveAsync(string apps)
		{
			return Shell.ExecAsync($"apt-get remove {apps.Replace(';', ' ').Replace(',', ' ')}");
		}

		public override Shell UpdateAsync()
		{
			return Shell.ExecAsync("apt-get update");
		}
	}
}
