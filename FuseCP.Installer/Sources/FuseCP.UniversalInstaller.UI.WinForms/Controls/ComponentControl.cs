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
using System.Threading;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Windows.Forms;

namespace FuseCP.UniversalInstaller.Controls
{
	/// <summary>
	/// Component control
	/// </summary>
	internal partial class ComponentControl : ResultViewControl
	{
		delegate void ReloadApplicationCallback();

		/// <summary>
		/// Initializes a new instance of the ComponentControl class.
		/// </summary>
		public ComponentControl()
		{
			InitializeComponent();
		}

		/// <summary>
		/// Shows control
		/// </summary>
		/// <param name="context"></param>
		public override void ShowControl(FCPAppContext context)
		{
			base.ShowControl(context);
			if (!IsInitialized)
			{
				ComponentInfo element = context.ScopeNode.Tag as ComponentInfo;
				if (element != null)
				{
					txtApplication.Text = element.ApplicationName;
					txtComponent.Text = element.ComponentName;
					txtVersion.Text = element.Version.ToString();
					lblDescription.Text = element.ComponentDescription;

					string installer = element.FullFilePath;// element.Installer;
					string path = element.InstallerPath;
					string type = element.InstallerType;
					if (string.IsNullOrEmpty(installer) ||
						string.IsNullOrEmpty(path) || 
						string.IsNullOrEmpty(type))
					{
						btnRemove.Enabled = false;
						btnSettings.Enabled = false;
					}
				}
				IsInitialized = true;
			}
		}

		/// <summary>
		/// Action on Remove button click
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void OnRemoveClick(object sender, EventArgs e)
		{
			UninstallComponent();
		}

		/// <summary>
		/// Action on Settings button click
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void OnSettingsClick(object sender, EventArgs e)
		{
			SetupComponent();
		}

		/// <summary>
		/// Action on Check For Update button click
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void OnCheckUpdateClick(object sender, EventArgs e)
		{
			//start check in the separate thread
			AppContext.AppForm.StartAsyncProgress("Connecting...", true);
			ThreadStart threadDelegate = new ThreadStart(CheckForUpdate);
			Thread newThread = new Thread(threadDelegate);
			newThread.Start();
		}

		/// <summary>
		/// Checks for component update
		/// </summary>
		private void CheckForUpdate()
		{
			Log.WriteStart("Checking for component update");
			ComponentInfo element = AppContext.ScopeNode.Tag as ComponentInfo;

			string componentName = element.ComponentName;
			string componentCode = element.ComponentCode;
			string release = element.Version.ToString();		
	
			// call web service
			ComponentUpdateInfo info;
			try
			{
				Log.WriteInfo(string.Format("Checking {0} {1}", componentName, release));
				//
				var webService = Installer.Current.InstallerWebService;
				info = webService.GetComponentUpdate(componentCode, release);
				//
				Log.WriteEnd("Component update checked");
				AppContext.AppForm.FinishProgress();
			}
			catch (Exception ex)
			{
				Log.WriteError("Service error", ex);
				AppContext.AppForm.FinishProgress();
				AppContext.AppForm.ShowServerError();
				return;
			}

			string appName = AppContext.AppForm.Text;
			if (info != null)
			{
				string newVersion = info.Version.ToString();
				Log.WriteInfo(string.Format("Version {0} is available for download", newVersion)); 

				string message = string.Format("{0} {1} is available now.\nWould you like to install new version?", componentName, newVersion);
				if (MessageBox.Show(AppContext.AppForm, message, appName, MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
				{
					string fileToDownload = info.UpgradeFilePath;
					string installerPath = info.InstallerPath;
					string installerType = info.InstallerType;
					UpdateComponent(fileToDownload, installerPath, installerType, newVersion);
				}
			}
			else
			{
				string message = string.Format("Current version of {0} is up to date.", componentName);
				Log.WriteInfo(message);
				AppContext.AppForm.ShowInfo(message);
			}
		}

		delegate void UpdateComponentCallback(string fileName, string path, string type, string version);

		/// <summary>
		/// Runs component update
		/// </summary>
		/// <param name="fileName"></param>
		/// <param name="path"></param>
		/// <param name="type"></param>
		private void UpdateComponent(string fileName, string path, string type, string version)
		{
			// InvokeRequired required compares the thread ID of the
			// calling thread to the thread ID of the creating thread.
			// If these threads are different, it returns true.
			if (this.InvokeRequired)
			{
				UpdateComponentCallback callBack = new UpdateComponentCallback(UpdateComponent);
				Invoke(callBack, new object[] { fileName, path, type, version });
			}
			else
			{
				Log.WriteStart("Updating component");

				ComponentInfo element = AppContext.ScopeNode.Tag as ComponentInfo;

				try
				{
					Log.WriteInfo(string.Format("Updating {0}", element.ComponentName));
					//download installer
					var result = Installer.Current.Update(element);
					Log.WriteInfo(string.Format("Installer returned {0}", result));
					Log.WriteEnd("Installer finished");
					Update();
					if (result)
					{
						ReloadApplication();
					}
					FileUtils.DeleteTempDirectory();
					
					Log.WriteEnd("Update completed");
				}
				catch (Exception ex)
				{
					Log.WriteError("Installer error", ex);
					AppContext.AppForm.ShowError(ex);
				}
			}
		}

		/// <summary>
		/// Uninstalls component
		/// </summary>
		private void UninstallComponent()
		{
			Log.WriteStart("Uninstalling component");
		
			try
			{
				ComponentInfo element = AppContext.ScopeNode.Tag as ComponentInfo;

				var result = Installer.Current.Uninstall(element);
			
				Log.WriteInfo(string.Format("Installer returned {0}", result));
				Log.WriteEnd("Installer finished");
				Update();
				ReloadApplication();					
				Log.WriteEnd("Uninstall completed");
			}
			catch (Exception ex)
			{
				Log.WriteError("Installer error", ex);
				AppContext.AppForm.ShowError(ex);
			}
		}

		/// <summary>
		/// Setup component
		/// </summary>
		private void SetupComponent()
		{
			Log.WriteStart("Starting component setup");

			var element = AppContext.ScopeNode.Tag as ComponentInfo;

			try
			{
				Log.WriteInfo(string.Format("Setup {0} {1}", element.ComponentName, element.Version));
				//download installer
				var result = Installer.Current.Setup(element);
				//
				Log.WriteInfo(string.Format("Installer returned {0}", result));
				Log.WriteEnd("Installer finished");

				if (result)
				{
					ReloadApplication();
				}
				FileUtils.DeleteTempDirectory();

				Log.WriteEnd("Component setup completed");
			}
			catch (Exception ex)
			{
				Log.WriteError("Installer error", ex);
				this.AppContext.AppForm.ShowError(ex);
			}
		}

		/// <summary>
		/// Thread safe application reload
		/// </summary>
		private void ReloadApplication()
		{
			// InvokeRequired required compares the thread ID of the
			// calling thread to the thread ID of the creating thread.
			// If these threads are different, it returns true.
			if (this.InvokeRequired)
			{
				ReloadApplicationCallback callback = new ReloadApplicationCallback(ReloadApplication);
				Invoke(callback, null);
			}
			else
			{
				AppContext.AppForm.ReloadApplication();
			}
		}
	}
}
