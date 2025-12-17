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
using System.Windows.Forms;
using System.Threading;

namespace FuseCP.Import.Enterprise
{
	static class Program
	{
		private static ApplicationForm appForm;
		private delegate void ExceptionDelegate(Exception ex);
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main()
		{
			Log.WriteApplicationStart();
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
			Application.ApplicationExit += new EventHandler(OnApplicationExit);
			appForm = new ApplicationForm();
			Application.ThreadException += new ThreadExceptionEventHandler(OnThreadException);
			AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(OnApplicationException);
			
			ConnectForm connectForm = new ConnectForm();
			if (connectForm.ShowDialog() == DialogResult.OK)
			{
				appForm.InitializeForm(connectForm.Username, connectForm.Password);
				Application.Run(appForm);
			}
		}

		/// <summary>
		/// Writes to log on application exit
		/// </summary>
		private static void OnApplicationExit(object sender, EventArgs e)
		{
			Log.WriteApplicationEnd();
		}

		/// <summary>
		/// Thread exception handler 
		/// </summary>
		private static void OnThreadException(object sender, ThreadExceptionEventArgs e)
		{
			Exception ex = e.Exception;
			PublishOnMainThread(ex);
		}

		/// <summary>
		/// Application domain exception handler 
		/// </summary>
		private static void OnApplicationException(object sender, UnhandledExceptionEventArgs e)
		{
			Exception ex = e.ExceptionObject as Exception;
			PublishOnMainThread(ex);
		}

		private static void PublishOnMainThread(Exception exception)
		{
			if (appForm.InvokeRequired)
			{
				appForm.Invoke(new ExceptionDelegate(HandleException), new object[] { exception });
			}
			else
			{
				HandleException(exception);
			}
		}

		private static void HandleException(Exception exception)
		{
			Log.WriteError("Fatal error occured.", exception);
			string message = "A fatal error has occurred. We apologize for this inconvenience.\n" +
				"Please contact Technical Support at support@FuseCP.net.\n\n" +
				"Make sure you include a copy of the log file from the \n" +
				"application home directory.";
			MessageBox.Show(message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

			Application.Exit();
			Environment.Exit(0);
		}

	}
}
