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

namespace FuseCP.Import.CsvBulk 
{
	/// <summary>
	/// Simple log
	/// </summary>
	public sealed class Log
	{
		/// <summary>
		/// Initializes a new instance of the class.
		/// </summary>
		private Log()
		{
		}

		private static string logFile = "FuseCP.Import.CsvBulk.log";

		/// <summary>
		/// Initializes trace listeners.
		/// </summary>
		public static void Initialize(string fileName)
		{
			logFile = fileName;
			FileStream fileLog = new FileStream(logFile, FileMode.Append);
			TextWriterTraceListener fileListener = new TextWriterTraceListener(fileLog);
			fileListener.TraceOutputOptions = TraceOptions.DateTime;
			Trace.UseGlobalLock = true;
			Trace.Listeners.Clear();
			Trace.Listeners.Add(fileListener);
			TextWriterTraceListener consoleListener = new TextWriterTraceListener(System.Console.Out);
			Trace.Listeners.Add(consoleListener);
			Trace.AutoFlush = true;
		}

		/// <summary>
		/// Write error to the log.
		/// </summary>
		/// <param name="message">Error message.</param>
		/// <param name="ex">Exception.</param>
		internal static void WriteError(string message, Exception ex)
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
		internal static void WriteError(string message)
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
		internal static void Write(string message)
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
		internal static void WriteLine(string message)
		{
			try
			{
				string line = string.Format("[{0:G}] {1}", DateTime.Now, message);
				Trace.WriteLine(line);
			}
			catch { }
		}

		/// <summary>
		/// Write info message to log
		/// </summary>
		/// <param name="message"></param>
		internal static void WriteInfo(string message)
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
		internal static void WriteStart(string message)
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
		internal static void WriteEnd(string message)
		{
			try
			{
				string line = string.Format("[{0:G}] END: {1}", DateTime.Now, message);
				Trace.WriteLine(line);
			}
			catch { }
		}

		internal static void WriteApplicationStart()
		{
			try
			{
				string name = typeof(Log).Assembly.GetName().Name;
				string version = typeof(Log).Assembly.GetName().Version.ToString(3);
				string line = string.Format("[{0:G}] ***** {1} {2} Started *****", DateTime.Now, name, version);
				Trace.WriteLine(line);
			}
			catch { }
		}

		internal static void WriteApplicationEnd()
		{
			try
			{
				string name = typeof(Log).Assembly.GetName().Name;
				string line = string.Format("[{0:G}] ***** {1} Ended *****", DateTime.Now, name);
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
				string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, logFile);
				Process.Start("notepad.exe", path);
			}
			catch { }
		}
	}
}
