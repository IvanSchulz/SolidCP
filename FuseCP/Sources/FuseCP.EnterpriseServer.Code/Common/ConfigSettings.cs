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
using Microsoft.Win32;
using FuseCP.Providers.OS;
using FuseCP.Web.Services;

namespace FuseCP.EnterpriseServer
{
	/// <summary>
	/// Summary description for ConfigSettings.
	/// </summary>
	public class ConfigSettings
	{

		const string EnterpriseServerRegistryPath = "SOFTWARE\\FuseCP\\EnterpriseServer";

		private static string GetKeyFromRegistry(string Key)
		{
			string value = string.Empty;

			if (!string.IsNullOrEmpty(Key))
			{
				RegistryKey root = Registry.LocalMachine;
				RegistryKey rk = root.OpenSubKey(EnterpriseServerRegistryPath);
				if (rk != null)
				{
					value = (string)rk.GetValue(Key, null);
					rk.Close();
				}
			}
			return value;
		}

		public static string ConnectionString => Data.DbSettings.ConnectionString;
		public static string NativeConnectionString => Data.DbSettings.NativeConnectionString;

		static string cryptoKey = null;
		public static string CryptoKey
		{
			get
			{
				if (cryptoKey == null)
				{
					string key;
					if (OSInfo.IsNetFX)
					{
						key = ConfigurationManager.AppSettings["FuseCP.AltCryptoKey"];
					}
					else
					{
						key = Web.Services.Configuration.AltCryptoKey;
					}

					string value = string.Empty;

					if (OSInfo.IsWindows) value = GetKeyFromRegistry(key);

					if (!string.IsNullOrEmpty(value))
						cryptoKey = value;
					else
					{
						if (OSInfo.IsNetFX)
						{
							cryptoKey = ConfigurationManager.AppSettings["FuseCP.CryptoKey"];
						}
						else
						{
							cryptoKey = Web.Services.Configuration.CryptoKey;
						}
					}
				}
				return cryptoKey;
			}
		}


		static bool? encryptionEnabled = null;
		public static bool EncryptionEnabled
		{
			get
			{
				if (encryptionEnabled == null)
				{
					if (OSInfo.IsNetFX)
					{
						encryptionEnabled = (ConfigurationManager.AppSettings["FuseCP.EncryptionEnabled"] != null)
						? bool.Parse(ConfigurationManager.AppSettings["FuseCP.EncryptionEnabled"]) : true;
					} else
					{
						encryptionEnabled = Web.Services.Configuration.EncryptionEnabled;
					}
				}
				return encryptionEnabled.Value;
			}
		}

		static string webApplicationPath = null;
		public static string WebApplicationsPath
		{
			get
			{
				if (webApplicationPath == null)
				{
					if (OSInfo.IsNetFX)
					{
						webApplicationPath = ConfigurationManager.AppSettings["FuseCP.EnterpriseServer.WebApplicationsPath"];
					}
					else
					{
						webApplicationPath = Web.Services.Configuration.WebApplicationsPath;
					}
				}
				if (webApplicationPath.StartsWith("~")) webApplicationPath = Web.Services.Server.MapPath(webApplicationPath);

				return webApplicationPath;
			}
		}

		public static string BackupsPath
		{
			get
			{
				SystemSettings settings = new SystemController().GetSystemSettingsInternal(
					SystemSettings.BACKUP_SETTINGS,
					false
				);

				return settings["BackupsPath"];
			}
		}

		#region Communication
		static int? serverRequestTimeout = null;
		public static int ServerRequestTimeout
		{
			get
			{
				if (serverRequestTimeout == null)
				{
					if (OSInfo.IsNetFX)
					{
						serverRequestTimeout = Utils.ParseInt(
							ConfigurationManager.AppSettings["FuseCP.EnterpriseServer.ServerRequestTimeout"], -1);
					}
					else
					{
						serverRequestTimeout = Web.Services.Configuration.ServerRequestTimeout ?? -1;
					}
				}
				return serverRequestTimeout.Value;
			}
		}
		#endregion
	}
}
