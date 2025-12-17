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
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Diagnostics;
using FuseCP.Providers.OS;

namespace FuseCP.Providers.Database
{
	public class MsSqlServer2017 : MsSqlServer2016
	{
		protected override bool CheckUnixVersion(string version) {
			var processes = Process.GetProcessesByName("sqlservr")
				.Select(p => p.ExecutableFile())
				.Concat(new string[] { Shell.Default.Find("sqlservr") })
				.Where(exe => exe != null)
				.Distinct();
			foreach (var exe in processes) {
				if (File.Exists(exe))
				{
					try
					{
						var output = Shell.Default.ExecScript($"PAL_PROGRAM_INFO=1 {exe}").Output().Result;
						var match = Regex.Match(output, @"^\s*Version\s+(?<version>[0-9][0-9.]+)", RegexOptions.Multiline);
						if (match.Success)
						{
							var ver = match.Groups["version"].Value;
							if (ver.StartsWith(version)) return true;
						}
					}
					catch { }
				}
			}
			return false;
		}
		public override bool IsInstalled()
		{
			return CheckVersion("14.");
		}
	}
}
