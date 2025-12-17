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
using System.Diagnostics;
using System.IO;
using FuseCP.Providers.OS;

namespace FuseCP.Setup
{
	/// <summary>
	/// Installer Log.
	/// </summary>
	public sealed class Log
	{
		/// <summary>
		/// Initializes a new instance of the class.
		/// </summary>
		private Log()
		{
		}

		/// <summary>
		/// Initializes trace listeners.
		/// </summary>
		static Log()
		{
			Trace.AutoFlush = true;
		}

		/// <summary>
		/// Write error to the log.
		/// </summary>
		/// <param name="message">Error message.</param>
		/// <param name="ex">Exception.</param>
		public static void WriteError(string message, Exception ex)
		{
			try
			{
				string line = string.Format("[{0:G}] ERROR: {1}", DateTime.Now, message);
				Trace.WriteLine(line);
				if ( ex != null )
					Trace.WriteLine(ex);

				if (OSInfo.IsMono && Debugger.IsAttached) {
					Debugger.Log(1, "", line);
					if (ex != null) Debugger.Log(1, "", ex.ToString());
				}
			}
			catch { }
		}

		/// <summary>
		/// Write error to the log.
		/// </summary>
		/// <param name="message">Error message.</param>
		/// <param name="ex">Exception.</param>
        public static void WriteError(string message)
		{
			WriteError(message, null);
		}

		/// <summary>
		/// Write to log
		/// </summary>
		/// <param name="message"></param>
        public static void Write(string message)
		{
			try
			{
				string line = string.Format("[{0:G}] {1}", DateTime.Now, message);
				Trace.Write(line);
				if (OSInfo.IsMono && Debugger.IsAttached)
				{
					Debugger.Log(1, "", line);
				}
			}
			catch { }
		}

 
		/// <summary>
		/// Write line to log
		/// </summary>
		/// <param name="message"></param>
        public static void WriteLine(string message)
		{
			try
			{
				string line = string.Format("[{0:G}] {1}", DateTime.Now, message);
				Trace.WriteLine(line);
				if (OSInfo.IsMono && Debugger.IsAttached)
				{
					Debugger.Log(1, "", line);
				}
			}
			catch { }
		}

		/// <summary>
		/// Write info message to log
		/// </summary>
		/// <param name="message"></param>
        public static void WriteInfo(string message)
		{
			try
			{
				string line = string.Format("[{0:G}] INFO: {1}", DateTime.Now, message);
				Trace.WriteLine(line);
				if (OSInfo.IsMono && Debugger.IsAttached)
				{
					Debugger.Log(1, "", line);
				}
			}
			catch { }
		}

		/// <summary>
		/// Write start message to log
		/// </summary>
		/// <param name="message"></param>
        public static void WriteStart(string message)
		{
			try
			{
				string line = string.Format("[{0:G}] START: {1}", DateTime.Now, message);
				Trace.WriteLine(line);
				Trace.Flush();
				if (OSInfo.IsMono && Debugger.IsAttached)
				{
					Debugger.Log(1, "", line);
				}
			}
			catch { }
		}
		
		/// <summary>
		/// Write end message to log
		/// </summary>
		/// <param name="message"></param>
        public static void WriteEnd(string message)
		{
			try
			{
				string line = string.Format("[{0:G}] END: {1}", DateTime.Now, message);
				Trace.WriteLine(line);
				Trace.Flush();
				if (OSInfo.IsMono && Debugger.IsAttached)
				{
					Debugger.Log(1, "", line);
				}
			}
			catch { }
		}

		/// <summary>
		/// Opens notepad to view log file.
		/// </summary>
		public static void ShowLogFile()
		{
			try
			{
				string path = AppConfig.GetSettingStringValue("Log.FileName");
				if (string.IsNullOrEmpty(path))
				{
					path = "FuseCP.Installer.log";
				}

				if (OSInfo.IsWindows)
				{
					path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, path);
					Process.Start("notepad.exe", path);
				} else
				{
					path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, path);
					if (Shell.Default.Find("gedit") != null) Process.Start("gedit", path);
					else if (Shell.Default.Find("gnome-text-editor") != null) Process.Start("gnome-text-editor", path);
					else if (Shell.Default.Find("kate") != null) Process.Start("kate", path);
					else if (Shell.Default.Find("mousepad") != null) Process.Start("mousepad", path);
					else if (Shell.Default.Find("leafpad") != null) Process.Start("leafpad", path);
					else if (Shell.Default.Find("pluma") != null) Process.Start("pluma", path);
				}
			}
			catch { }
		}

        public static TraceListenerCollection Listeners { get { return Trace.Listeners; } }
	}
}
