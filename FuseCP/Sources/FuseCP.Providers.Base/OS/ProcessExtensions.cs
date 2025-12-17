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
using System.Text.RegularExpressions;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.IO;
using System.Reflection;

namespace FuseCP.Providers.OS
{
	public static class ProcessExtensions
	{
		// Constants from sys\param.h
		private const int MAXPATHLEN = 1024;
		private const int PROC_PIDPATHINFO_MAXSIZE = 4 * MAXPATHLEN;

		/// <summary>
		/// Gets the full path to the executable file identified by the specified PID
		/// </summary>
		/// <param name="pid">The PID of the running process</param>
		/// <param name="buffer">A pointer to an allocated block of memory that will be filled with the process path</param>
		/// <param name="bufferSize">The size of the buffer, should be PROC_PIDPATHINFO_MAXSIZE</param>
		/// <returns>Returns the length of the path returned on success</returns>
		[DllImport("/usr/lib/libproc.dylib", SetLastError = true)]
		private static extern unsafe int proc_pidpath(
			int pid,
			byte* buffer,
			uint bufferSize);

		/// <summary>
		/// Gets the full path to the executable file identified by the specified PID
		/// </summary>
		/// <param name="pid">The PID of the running process</param>
		/// <returns>Returns the full path to the process executable</returns>
		internal static unsafe string proc_pidpath(int pid)
		{
			// Negative PIDs are invalid
			if (pid < 0) throw new ArgumentException();

			// The path is a fixed buffer size, so use that and trim it after
			int result = 0;
			byte* pBuffer = stackalloc byte[PROC_PIDPATHINFO_MAXSIZE];
			result = proc_pidpath(pid, pBuffer, (uint)(PROC_PIDPATHINFO_MAXSIZE * sizeof(byte)));
			if (result <= 0)
			{
				throw new Exception();
			}

			// OS X uses UTF-8. The conversion may not strip off all trailing \0s so remove them here
			return System.Text.Encoding.UTF8.GetString(pBuffer, result);
		}
		public static string ExecutableFile(this Process process)
		{
			if (OSInfo.IsWindows) return process.MainModule.FileName;
			else if (OSInfo.IsMac) return proc_pidpath(process.Id);
			else
			{
				var procexe = $"/proc/{process.Id}/exe";
				if ((OSInfo.IsLinux || File.Exists(procexe)) && OSInfo.IsCore)
				{
					var m = typeof(Directory).GetMethod("ResolveLinkTarget", BindingFlags.Static | BindingFlags.Public);
					if (m != null) {
						try
						{
							var info = m.Invoke(null, new object[] { procexe, true }) as FileSystemInfo;
							return info.FullName;
						}
						catch { }
					}
				}
				else if (Shell.Default.Find("ps") != null)
				{
					var psout = Shell.Default.Exec("ps -ef").Output().Result;
					var match = Regex.Match(psout, @$"(?<=^\s*[^ \t]*\s+{process.Id}(\s+[^ \t]+){5}\s+)[^ \t]+", RegexOptions.Multiline);
					if (match.Success) return match.Value;
				}
				return null;
			}
		}
	}
}
