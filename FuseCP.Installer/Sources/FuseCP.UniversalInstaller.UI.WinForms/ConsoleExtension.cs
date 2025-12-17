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

namespace FuseCP.UniversalInstaller;

internal static class ConsoleExtension
{
	const int SW_HIDE = 0;
	const int SW_SHOW = 5;
	readonly static IntPtr handle = GetConsoleWindow();
	[DllImport("kernel32.dll")] static extern IntPtr GetConsoleWindow();
	[DllImport("user32.dll")] static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

	public static void Hide()
	{
		ShowWindow(handle, SW_HIDE); //hide the console
	}
	public static void Show()
	{
		ShowWindow(handle, SW_SHOW); //show the console
	}
}
