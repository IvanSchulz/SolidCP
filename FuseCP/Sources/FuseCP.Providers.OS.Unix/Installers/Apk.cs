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
    public class Apk: Installer
	{

		public override bool IsInstallerInstalled => Shell.Find("apk") != null;
		public override Shell InstallAsync(string apps)
		{
			throw new NotSupportedException();
		}

		public override Shell AddSourcesAsync(string sources)
		{
			throw new NotSupportedException();
		}
		public override bool IsInstalled(string apps)
		{
			throw new NotSupportedException();
		}

		public override Shell RemoveAsync(string apps)
		{
			throw new NotSupportedException();
		}

		public override Shell UpdateAsync()
		{
			throw new NotSupportedException();
		}
	}
}
