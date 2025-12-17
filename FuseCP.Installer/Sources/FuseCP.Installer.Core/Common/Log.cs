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
using System.Configuration;
using System.Diagnostics;
using System.IO;

using FuseCP.Installer.Configuration;
using System.Security.Principal;
using FuseCP.Installer.Core;
using System.Reflection;

namespace FuseCP.Installer.Common
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
			Initialize();
		}

		static void Initialize()
		{
			string fileName = LogFile;
			//
			Trace.Listeners.Clear();
			//
			FileStream fileLog = new FileStream(fileName, FileMode.Append);
			//
			TextWriterTraceListener fileListener = new TextWriterTraceListener(fileLog);
			fileListener.TraceOutputOptions = TraceOptions.DateTime;
			Trace.Listeners.Add(fileListener);
			//
			Trace.AutoFlush = true;
		}

		internal static string LogFile
		{
			get
			{
				string fileName = "FuseCP.Installer.log";
				//
				if (string.IsNullOrEmpty(fileName))
				{
					fileName = "Installer.log";
				}
				// Ensure the path is correct
				if (!Path.IsPathRooted(fileName))
				{
					fileName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, fileName);
				}
				//
				return fileName;
			}
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
				Trace.WriteLine(ex);
			}
			catch { }
		}

		/// <summary>
		/// Write error to the log.
		/// </summary>
		/// <param name="message">Error message.</param>
		public static void WriteError(string message)
		{
			try
			{
				string line = string.Format("[{0:G}] ERROR: {1}", DateTime.Now, message);
				Trace.WriteLine(line);
			}
			catch { }
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
			}
			catch { }
		}

		/// <summary>
		/// Writes formatted informational message into the log
		/// </summary>
		/// <param name="format"></param>
		/// <param name="args"></param>
		public static void WriteInfo(string format, params object[] args)
		{
			WriteInfo(String.Format(format, args));
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
			}
			catch { }
		}

		public static void WriteApplicationStart()
		{
			try
			{
				string name = Assembly.GetEntryAssembly().GetName().Name;
				string version = Assembly.GetEntryAssembly().GetName().Version.ToString();
				string identity = WindowsIdentity.GetCurrent().Name;
				string line = string.Format("[{0:G}] {1} {2} Started by {3}", DateTime.Now, name, version, identity);
				Trace.WriteLine(line);
			}
			catch { }
		}

		public static void WriteApplicationEnd()
		{
			try
			{
				string name = Assembly.GetEntryAssembly().GetName().Name;
				string line = string.Format("[{0:G}] {1} Ended", DateTime.Now, name);
				Trace.WriteLine(line);
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
				string path = LogFile;
				path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, path);
				Process.Start("notepad.exe", path);
			}
			catch { }
		}
	}
}
