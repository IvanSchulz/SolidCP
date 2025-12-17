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
using System.Text.RegularExpressions;

namespace FuseCP.Providers.OS
{
    public class Zypper : Installer
    {
        public override bool IsInstallerInstalled => Shell.Find("zypper") != null;

        public override Shell AddSourcesAsync(string sources)
        {
            throw new NotImplementedException("AddSources not supported for Zypper.");
        }

        public override Shell InstallAsync(string apps)
        {
			return Shell.ExecAsync($"zypper -n install {string.Join(" ", apps.Split(' ', ',', ';'))}");
        }

		public override bool IsInstalled(string apps)
		{
			var applist = apps.Split(' ', ',', ';')
				.Select(app => app.Trim())
				.ToArray();
			var output = Shell.Exec($"zypper -n info {string.Join(" ", applist)}").Output().Result;
			var installed = Regex.Matches(output, @"^Name\s*:\s*(?<name>.*?)$(?=\s*(?:^[^:]+:.+$\s*)+^Installed\s*:\s*Yes)", RegexOptions.Multiline)
					.OfType<Match>()
					.Select(m => m.Groups["name"].Value.Trim());
			return applist.Except(installed)
				.Any();
		}

		public override Shell RemoveAsync(string apps)
		{
			return Shell.ExecAsync($"zypper -n remove {string.Join(" ", apps.Split(' ', ',', ';'))}");
		}
		public override Shell UpdateAsync() => Shell;
	}
}
