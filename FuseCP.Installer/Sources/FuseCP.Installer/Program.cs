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
using System.Text;
using System.Threading;
using System.Diagnostics;
using System.Collections.Generic;
using System.Windows.Forms;

using System.Security;
using System.Security.Permissions;

using FuseCP.Installer.Common;
using FuseCP.Installer.Services;
using System.Xml;
using System.Runtime.Remoting.Lifetime;
using System.Security.Principal;
using FuseCP.Installer.Core;
using FuseCP.Installer.Configuration;
using FuseCP.Providers.OS;
using System.Reflection;

namespace FuseCP.Installer
{
	/// <summary>
	/// Entry point class 
	/// </summary>
	static class Program
	{
		public const string SetupFromXmlFileParam = "setupxml";

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main()
		{
#if Costura
			CosturaUtility.Initialize();
#endif
			Start();
		}

		static void Start() {

			ResourceUtils.CreateDefaultAppConfig();
			//
			Utils.FixConfigurationSectionDefinition();

			//check security permissions
			if (!Utils.CheckSecurity())
			{
				//ShowSecurityError();
				Utils.RestartAsAdmin();
				return;
			}

			//check administrator permissions
			if (!Utils.IsAdministrator())
			{
				//ShowSecurityError();
				UniversalInstaller.Installer.Current.RestartAsAdmin();
				return;
			}

			//check for running instance
			if ( !Utils.IsNewInstance())
			{
				UiUtils.ShowRunningInstance();
				return;
			}

			Log.WriteApplicationStart();
			//AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(OnUnhandledException);
			Application.ApplicationExit += new EventHandler(OnApplicationExit);
			Application.ThreadException += new ThreadExceptionEventHandler(OnThreadException);
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);

			//check OS version
			Log.WriteInfo("{0} detected", Global.OSVersionWindows != WindowsVersion.NonWindows ? Global.OSVersionWindows.ToString() : Global.OSFlavor.ToString());

			//check IIS version
			if (Global.IISVersion.Major == 0)
				Log.WriteError("IIS not found.");
			else
				Log.WriteInfo("IIS {0} detected", Global.IISVersion);

			ApplicationForm mainForm = new ApplicationForm();

			if (!CheckCommandLineArgument("/nocheck"))
			{
				//Check for new versions
				if (CheckForUpdate(mainForm))
				{
					return;
				}
			}

			if (CheckCommandLineArgument("/uselocalsetupdll"))
			{

			}
			// Load setup parameters from an XML file
			LoadSetupXmlFile();
			//start application
			mainForm.InitializeApplication();
			Application.Run(mainForm);
			//
			Utils.SaveMutex();
		}

		private static void LoadSetupXmlFile()
		{
			string file = GetCommandLineArgumentValue(SetupFromXmlFileParam);
			if (!string.IsNullOrEmpty(file))
			{
				if (FileUtils.FileExists(file))
				{
					try
					{
						XmlDocument doc = new XmlDocument();
						doc.Load(file);
						Global.SetupXmlDocument = doc;
					}
					catch (Exception ex)
					{
						Log.WriteError("I/O error", ex);
					}
				}
			}
		}

		/// <summary>
		/// Application thread exception handler 
		/// </summary>
		static void OnThreadException(object sender, ThreadExceptionEventArgs e)
		{
			Log.WriteError("Fatal error occured.", e.Exception);
			string message = "A fatal error has occurred. We apologize for this inconvenience.\n" +
				"Please contact Technical Support at support@fusecp.com.\n\n" +
				"Make sure you include a copy of the Installer.log file from the\n" +
				"FuseCP Installer home directory.";
			MessageBox.Show(message, "FuseCP Installer", MessageBoxButtons.OK, MessageBoxIcon.Error);
			Application.Exit();
		}

		/// <summary>
		/// Application exception handler
		/// </summary>
		static void OnUnhandledException(object sender, UnhandledExceptionEventArgs e)
		{
			Log.WriteError("Fatal error occured.", (Exception)e.ExceptionObject);
			string message = "A fatal error has occurred. We apologize for this inconvenience.\n" +
				"Please contact Technical Support at support@fusecp.com.\n\n" +
				"Make sure you include a copy of the Installer.log file from the\n" +
				"FuseCP Installer home directory.";
			MessageBox.Show(message, "FuseCP Installer", MessageBoxButtons.OK, MessageBoxIcon.Error);
			Process.GetCurrentProcess().Kill();
		}

		private static void ShowSecurityError()
		{
			string message = "You do not have the appropriate permissions to perform this operation. Make sure you are running the application from the local disk and you have local system administrator privileges.";
			MessageBox.Show(message, "FuseCP Installer", MessageBoxButtons.OK, MessageBoxIcon.Error);
		}

		/// <summary>
		/// Writes to log on application exit
		/// </summary>
		private static void OnApplicationExit(object sender, EventArgs e)
		{
			Log.WriteApplicationEnd();
		}

		/// <summary>
		/// Check whether application is up-to-date
		/// </summary>
		private static bool CheckForUpdate(ApplicationForm mainForm)
		{
			if (!AppConfigManager.AppConfiguration.GetBooleanSetting(ConfigKeys.Web_AutoCheck))
				return false;

			string appName = mainForm.Text;
			string fileName;
			bool updateAvailable;
			try
			{
				updateAvailable = mainForm.CheckForUpdate(out fileName);
			}
			catch (Exception ex)
			{
				Log.WriteError("Update error", ex);
				mainForm.ShowServerError();
				return false;
			}

			if (updateAvailable)
			{
				string message = string.Format("An updated version of {0} is available now.\nWould you like to download and install the latest version?", appName);
				if (MessageBox.Show(message, appName, MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
				{
					return mainForm.StartUpdateProcess(fileName);
				}
			}
			return false;
		}

		/// <summary>
		/// Check for existing command line argument
		/// </summary>
		private static bool CheckCommandLineArgument(string argName)
		{
			string[] args = Environment.GetCommandLineArgs();
			for (int i = 1; i < args.Length; i++)
			{
				string arg = args[i];
				if (string.Equals(arg, argName, StringComparison.InvariantCultureIgnoreCase))
				{
					return true;
				}
			}
			return false;
		}

		/// <summary>
		/// Check for existing command line argument
		/// </summary>
		private static string GetCommandLineArgumentValue(string argName)
		{
			string key = "/"+argName.ToLower()+":";
			string[] args = Environment.GetCommandLineArgs();
			for (int i = 1; i < args.Length; i++)
			{
				string arg = args[i].ToLower();
				if (arg.StartsWith(key))
				{
					return arg.Substring(key.Length);
				}
			}
			return null;
		}

	}
}
