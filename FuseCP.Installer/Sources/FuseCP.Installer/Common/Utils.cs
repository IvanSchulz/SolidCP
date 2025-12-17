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
using System.Windows.Forms;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.DirectoryServices;
using System.DirectoryServices.ActiveDirectory;
using System.Reflection;

using System.Security.Principal;
using System.Security;
using System.Security.Permissions;
using System.Runtime.InteropServices;
using System.Threading;
using System.Linq;

namespace FuseCP.Installer.Common
{
	public static class UiUtils
	{
		public static void ShowRunningInstance()
		{
			Process currentProcess = Process.GetCurrentProcess();
			foreach (Process process in Process.GetProcessesByName(currentProcess.ProcessName))
			{
				if (process.Id != currentProcess.Id)
				{
					//set focus
					User32.SetForegroundWindow(process.MainWindowHandle);
					break;
				}
			}
		}
	}

	public class ResourceUtils
	{
		public static void CreateDefaultAppConfig()
		{
			var path = AppDomain.CurrentDomain.BaseDirectory;
			var assembly = Assembly.GetEntryAssembly();
			var file = assembly.Location + ".config";
			if (!File.Exists(file))
			{
				var resources = assembly.GetManifestResourceNames();
				var resource = resources.FirstOrDefault(r => r.EndsWith("App.config") || r.EndsWith("App.Release.config"));
				if (resource != null)
				{
					using (var src = assembly.GetManifestResourceStream(resource))
					using (var reader = new StreamReader(src))
					{
						File.WriteAllText(file, reader.ReadToEnd());
					}
				}
			}
		}
	}
}
