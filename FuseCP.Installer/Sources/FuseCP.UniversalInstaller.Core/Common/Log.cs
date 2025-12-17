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

using System.Security.Principal;
using System.Reflection;
using Renci.SshNet.Messages;

namespace FuseCP.UniversalInstaller
{
	/// <summary>
	/// Installer Log.
	/// </summary>
	public class LogWriter
	{
		/// <summary>
		/// Initializes a new instance of the class.
		/// </summary>
		public LogWriter()
		{
			Installer.Current.Shell.Log += WriteLine;
		}

		/// <summary>
		/// Initializes trace listeners.
		/// </summary>
		static LogWriter()
		{
			Initialize();
		}

		static void Initialize()
		{
			string fileName = DefaultFile;
			//
			Trace.Listeners.Clear();
			//
			//FileStream fileLog = new FileStream(fileName, FileMode.Append);
			//
			TextWriterTraceListener fileListener = new TextWriterTraceListener(fileName);
			fileListener.TraceOutputOptions = TraceOptions.DateTime;
			Trace.Listeners.Add(fileListener);
			//
			Trace.AutoFlush = true;
		}

		public static string DefaultFile
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

		string logFile = null;
		public virtual string File { get => logFile ??= DefaultFile; set => logFile = value; }

		public Action OnWrite { get; set; }

		/// <summary>
		/// Write error to the log.
		/// </summary>
		/// <param name="message">Error message.</param>
		/// <param name="ex">Exception.</param>
		public virtual void WriteError(string message, Exception ex)
		{
			try
			{
				OnWrite?.Invoke();
				string line = string.Format("[{0:G}] ERROR: {1}", DateTime.Now, message);
				CloseProgress();
				Trace.WriteLine(line);
				Trace.WriteLine(ex);
			}
			catch { }
		}

		/// <summary>
		/// Write error to the log.
		/// </summary>
		/// <param name="message">Error message.</param>
		public virtual void WriteError(string message)
		{
			try
			{
				OnWrite?.Invoke();
				string line = string.Format("[{0:G}] ERROR: {1}", DateTime.Now, message);
				CloseProgress();
				Trace.WriteLine(line);
			}
			catch { }
		}

		/// <summary>
		/// Write to log
		/// </summary>
		/// <param name="message"></param>
		public virtual void Write(string message)
		{
			try
			{
				OnWrite?.Invoke();
				if (message != ".") message = string.Format("[{0:G}] {1}", DateTime.Now, message);
				CloseProgress();
				Trace.Write(message);
			}
			catch { }
		}

 
		/// <summary>
		/// Write line to log
		/// </summary>
		/// <param name="message"></param>
		public virtual void WriteLine(string message = null)
		{
			if (message == null) message = "";
			try
			{
				OnWrite?.Invoke();
				string line = string.Format("[{0:G}] {1}", DateTime.Now, message);
				CloseProgress();
				Trace.WriteLine(line);
			}
			catch { }
		}

		/// <summary>
		/// Writes formatted informational message into the log
		/// </summary>
		/// <param name="format"></param>
		/// <param name="args"></param>
		public virtual void WriteInfo(string format, params object[] args)
		{
			WriteInfo(String.Format(format, args));
		}

		/// <summary>
		/// Write info message to log
		/// </summary>
		/// <param name="message"></param>
		public virtual void WriteInfo(string message)
		{
			try
			{
				OnWrite?.Invoke();
				string line = string.Format("[{0:G}] INFO: {1}", DateTime.Now, message);
				CloseProgress();
				Trace.WriteLine(line);
			}
			catch { }
		}

		/// <summary>
		/// Write start message to log
		/// </summary>
		/// <param name="message"></param>
		public virtual void WriteStart(string message)
		{
			try
			{
				OnWrite?.Invoke();
				string line = string.Format("[{0:G}] START: {1}", DateTime.Now, message);
				CloseProgress();
				Trace.WriteLine(line);
			}
			catch { }
		}
		
		/// <summary>
		/// Write end message to log
		/// </summary>
		/// <param name="message"></param>
		public virtual void WriteEnd(string message)
		{
			try
			{
				OnWrite?.Invoke();
				string line = string.Format("[{0:G}] END: {1}", DateTime.Now, message);
				CloseProgress();
				Trace.WriteLine(line);
			}
			catch { }
		}

		public virtual void WriteApplicationStart()
		{
			try
			{
				OnWrite?.Invoke();
				string name = Installer.Current.GetEntryAssembly().GetName().Name;
				string version = Installer.Current.GetEntryAssembly().GetName().Version.ToString();
				string identity = WindowsIdentity.GetCurrent().Name;
				string line = string.Format("[{0:G}] {1} {2} Started by {3}", DateTime.Now, name, version, identity);
				CloseProgress();
				Trace.WriteLine(line);
			}
			catch { }
		}

		public virtual void WriteApplicationEnd()
		{
			try
			{
				OnWrite?.Invoke();
				string name = Installer.Current.GetEntryAssembly().GetName().Name;
				string line = string.Format("[{0:G}] {1} Ended", DateTime.Now, name);
				CloseProgress();
				Trace.WriteLine(line);
			}
			catch { }
		}

		/// <summary>
		/// Opens notepad to view log file.
		/// </summary>
		public virtual void ShowLogFile() => Installer.Current.ShowLogFile();

		bool isInLogProgress = false;
		public void ProgressOne()
		{
			isInLogProgress = true;
			OnWrite?.Invoke();
			Trace.Write(".");
		}

		private void CloseProgress()
		{
			if (isInLogProgress)
			{
				isInLogProgress = false;
				Trace.WriteLine("");
			}
		}
	}

	public static class Log
	{
		public static string File => Installer.Current.Log.File;
		public static void WriteError(string message, Exception ex) => Installer.Current.Log.WriteError(message, ex);
		public static void WriteError(string message) => Installer.Current.Log.WriteError(message);
		public static void Write(string message) => Installer.Current.Log.Write(message);
		public static void WriteLine(string message) => Installer.Current.Log.WriteLine(message);
		public static void WriteInfo(string format, params object[] args) => Installer.Current.Log.WriteInfo(format, args);
		public static void WriteInfo(string message) => Installer.Current.Log.WriteInfo(message);
		public static void WriteStart(string message) => Installer.Current.Log.WriteStart(message);
		public static void WriteEnd(string message) => Installer.Current.Log.WriteEnd(message);
		public static void WriteApplicationStart() => Installer.Current.Log.WriteApplicationStart();
		public static void WriteApplicationEnd() => Installer.Current.Log.WriteApplicationEnd();
		public static void ShowLogFile() => Installer.Current.Log.ShowLogFile();
		public static void ProgressOne() => Installer.Current.Log.ProgressOne();
	}
}
