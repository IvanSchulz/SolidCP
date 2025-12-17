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
using FuseCP.UniversalInstaller;

namespace FuseCP.Setup
{
    /// <summary>
    /// Release 2.0.0
    /// </summary>
    public class Portal200 : Portal
    {
		public override Version MinimalInstallerVersion => new Version("2.0.0");
		public override string VersionsToUpgrade => "1.5.0,1.4.9,1.4.8,1.4.7,1.4.6,1.4.5";
		public override CommonSettings CommonSettings => Settings.WebPortal;
		public Result Install(object args) => base.InstallOrSetup(args, "Install WebPortal",
			Installer.Current.InstallWebPortal, false);

		public Result Update(object args) => base.Update(args, "Update WebPortal",
			Installer.Current.UpdateWebPortal);

		public Result Setup(object args) => base.InstallOrSetup(args, "Setup WebPortal",
			Installer.Current.SetupWebPortal, true);

		public Result Uninstall(object args) => base.Uninstall(args, "Uninstall WebPortal",
			Installer.Current.RemoveWebPortal);
	}
}
