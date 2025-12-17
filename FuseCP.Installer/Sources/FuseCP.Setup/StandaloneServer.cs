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
using System.Xml;
using System.Configuration;
using System.Collections;
using System.Text;
using System.Linq;
using FuseCP.UniversalInstaller;

namespace FuseCP.Setup
{
	public class StandaloneServer : BaseSetup
	{
		public override CommonSettings CommonSettings => null;
		public override ComponentInfo Component => Settings.Installer.InstalledComponents
			.FirstOrDefault(component => component.ComponentCode == Global.StandaloneServer.ComponentCode);
	}
}
 
