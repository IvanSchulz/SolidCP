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

using FuseCP.Providers.OS;
using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace FuseCP.UniversalInstaller
{
	public class ArchInstaller : UnixInstaller
	{

		public override void InstallNet10Runtime()
		{
			if (CheckNet10RuntimeInstalled()) return;

			Info("Installing .NET 10 Runtime...");

			OSInstaller.Install("aspnetcore-runtime-10.0;dotnet-runtime-10.0");

			Net10RuntimeInstalled = true;

			InstallLog("Installed .NET 10 Runtime.");

			ResetHasDotnet();
		}

		public override void RemoveNet10NetRuntime()
		{
			OSInstaller.Remove("dotnet-runtime-8.0");

			ResetHasDotnet();
		}
		public override void RemoveNet10AspRuntime()
		{
			OSInstaller.Remove("aspnetcore-runtime-8.0");

			ResetHasDotnet();
		}
	}
}

