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
using System.Runtime.InteropServices;

namespace FuseCP.Core
{
	public static class Net
	{
		public static bool IsMono => Type.GetType("Mono.Runtime") != null;
		public static bool IsCore => !IsMono && (RuntimeInformation.FrameworkDescription == ".NET" || RuntimeInformation.FrameworkDescription == ".NET Core");
		public static bool IsFramework => !IsMono && RuntimeInformation.FrameworkDescription == ".NET Framework";
		public static bool IsNet7 => !IsMono && RuntimeInformation.FrameworkDescription == ".NET" && Environment.Version.Major >= 7;
		public static bool IsNet6 => !IsMono && RuntimeInformation.FrameworkDescription == ".NET" && Environment.Version.Major >= 6;
		public static bool IsNet5 => !IsMono && RuntimeInformation.FrameworkDescription == ".NET" && Environment.Version.Major >= 5;
		public static bool IsNet4 => IsFramework && Environment.Version.Major == 4;
		public static bool IsNet35 => IsFramework && Environment.Version.Major == 3;
		public static bool IsNet2 => IsFramework && Environment.Version.Major == 2;

	}
}
