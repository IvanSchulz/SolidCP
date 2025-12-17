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
using System.Diagnostics;
using System.IO;
using System.Configuration;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

using FuseCP.Installer.Common;
using FuseCP.Installer.Core;
using FuseCP.Installer.Configuration;

namespace FuseCP.Installer.Controls
{
	/// <summary>
	/// Settings control
	/// </summary>
	internal partial class SettingsControl : ResultViewControl
	{
		/// <summary>
		/// Initializes a new instance of the SettingsControl class.
		/// </summary>
		public SettingsControl()
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
				AppContext = context;
				LoadSettings();
				IsInitialized = true;
			}
		}

		/// <summary>
		/// Loads application settings
		/// </summary>
		private void LoadSettings()
		{
			InstallerSection appConfig = AppConfigManager.AppConfiguration;
			chkAutoUpdate.Checked = appConfig.GetBooleanSetting(ConfigKeys.Web_AutoCheck);
			chkUseHTTPProxy.Checked = appConfig.GetBooleanSetting(ConfigKeys.Web_Proxy_UseProxy);
			txtAddress.Text = appConfig.GetStringSetting(ConfigKeys.Web_Proxy_Address);
			txtUserName.Text = appConfig.GetStringSetting(ConfigKeys.Web_Proxy_UserName);
			txtPassword.Text = appConfig.GetStringSetting(ConfigKeys.Web_Proxy_Password);
		}

		private void OnUseHTTPProxyCheckedChanged(object sender, EventArgs e)
		{
			txtAddress.Enabled = chkUseHTTPProxy.Checked;
			txtUserName.Enabled = chkUseHTTPProxy.Checked;
			txtPassword.Enabled = chkUseHTTPProxy.Checked;
		}

		/// <summary>
		/// Save application configuration
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void OnUpdateClick(object sender, EventArgs e)
		{
			KeyValueConfigurationCollection settings = AppConfigManager.AppConfiguration.Settings;
			settings[ConfigKeys.Web_AutoCheck].Value = chkAutoUpdate.Checked.ToString();
			settings[ConfigKeys.Web_Proxy_UseProxy].Value = chkUseHTTPProxy.Checked.ToString();
			settings[ConfigKeys.Web_Proxy_Address].Value = txtAddress.Text;
			settings[ConfigKeys.Web_Proxy_UserName].Value = txtUserName.Text;
			settings[ConfigKeys.Web_Proxy_Password].Value = txtPassword.Text;
			//
			AppConfigManager.SaveConfiguration(true);
		}

		/// <summary>
		/// Checks for updates
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void OnCheckClick(object sender, EventArgs e)
		{
			//start check in the separated thread
			AppContext.AppForm.StartAsyncProgress("Connecting...", true);
			ThreadStart threadDelegate = new ThreadStart(StartCheck);
			Thread newThread = new Thread(threadDelegate);
			newThread.Start();
		}

		/// <summary>
		/// Starts check
		/// </summary>
		private void StartCheck()
		{
			bool startUpdate = CheckForUpdate();
			if (startUpdate)
			{
				AppContext.AppForm.Close();
			}
		}

		/// <summary>
		/// Checks for update
		/// </summary>
		/// <returns></returns>
		private bool CheckForUpdate()
		{
			string fileName;
			bool updateAvailable = false;
			try
			{
				updateAvailable = AppContext.AppForm.CheckForUpdate(out fileName);
				AppContext.AppForm.FinishProgress();
			}
			catch (Exception ex)
			{
				Log.WriteError("Service error", ex);
				AppContext.AppForm.FinishProgress();
				AppContext.AppForm.ShowServerError();
				return false;
			}
			
			string appName = AppContext.AppForm.Text;
			if (updateAvailable)
			{
				string message = string.Format("This version of {0} is out of date.\nWould you like to download the latest version?", appName);
				if (MessageBox.Show(AppContext.AppForm, message, appName, MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
				{
					return AppContext.AppForm.StartUpdateProcess(fileName);
				}
			}
			else
			{
				string message = string.Format("This version of {0} is up to date.", appName);
				Log.WriteInfo(message);
				AppContext.AppForm.ShowInfo(message);
			}
			return false;
		}

		private void OnViewLogClick(object sender, EventArgs e)
		{
			Log.ShowLogFile();
		}
	}
}
