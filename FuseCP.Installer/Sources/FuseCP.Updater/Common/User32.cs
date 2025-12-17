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
using System.Runtime.InteropServices;
using System.Collections.Generic;
using System.Text;

namespace FuseCP.Updater.Common
{
	internal sealed class User32
	{
		/// <summary>
		/// Win32 API Constants for ShowWindowAsync()
		/// </summary>
		internal const int SW_HIDE = 0;
		internal const int SW_SHOWNORMAL = 1;
		internal const int SW_SHOWMINIMIZED = 2;
		internal const int SW_SHOWMAXIMIZED = 3;
		internal const int SW_SHOWNOACTIVATE = 4;
		internal const int SW_RESTORE = 9;
		internal const int SW_SHOWDEFAULT = 10;

		[DllImport("user32.dll")]
		internal static extern bool SetForegroundWindow(IntPtr hWnd);
		[DllImport("user32.dll")]
		internal static extern bool IsIconic(IntPtr hWnd);
		[DllImport("user32.dll")]
		internal static extern bool ShowWindowAsync(IntPtr hWnd, int nCmdShow);
	}
}
