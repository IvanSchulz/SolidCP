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
using FuseCP.Installer.Common;
using System.Configuration;
using FuseCP.Installer.Configuration;
using System.IO;
using System.Reflection;


namespace FuseCP.Installer.Core
{
	public sealed class AppConfigManager
	{
		private static System.Configuration.Configuration appConfig;
		public const string AppConfigFileNameWithoutExtension = "FuseCP.Installer.exe";

		static AppConfigManager()
		{
			LoadConfiguration();
		}

		#region Core.Configuration
		/// <summary>
		/// Loads application configuration
		/// </summary>
		public static void LoadConfiguration()
		{
			Log.WriteStart("Loading application configuration");
			//
			var exePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, AppConfigFileNameWithoutExtension);
			//
			appConfig = ConfigurationManager.OpenExeConfiguration(exePath);
			//
			Log.WriteEnd("Application configuration loaded");
		}

		/// <summary>
		/// Returns application configuration section
		/// </summary>
		public static InstallerSection AppConfiguration
		{
			get
			{
				return appConfig.GetSection("installer") as InstallerSection;
			}
		}

		/// <summary>
		/// Saves application configuration
		/// </summary>
		public static void SaveConfiguration(bool showAlert)
		{
			if (appConfig != null)
			{
				try
				{
					Log.WriteStart("Saving application configuration");
					appConfig.Save();
					Log.WriteEnd("Application configuration saved");
					if (showAlert)
					{
						//ShowInfo("Application settings updated successfully.");
					}
				}
				catch (Exception ex)
				{
					Log.WriteError("Core.Configuration error", ex);
					if (showAlert)
					{
						//ShowError(ex);
					}
				}
			}
		}

		#endregion
	}
}
