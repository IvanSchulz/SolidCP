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
using System.Data;
using System.Web;
using System.Collections;
using FuseCP.Web.Services;
using System.ComponentModel;

namespace FuseCP.EnterpriseServer
{
	/// <summary>
	/// Summary description for esSystem
	/// </summary>
	[WebService(Namespace = "http://tempuri.org/")]
	[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
	[Policy("EnterpriseServerPolicy")]
	[ToolboxItem(false)]
	public class esSystem: WebService
	{
		[WebMethod]
		public SystemSettings GetSystemSettings(string settingsName)
		{
			return SystemController.GetSystemSettings(settingsName);
		}

        [WebMethod]
        public SystemSettings GetSystemSettingsActive(string settingsName, bool decrypt)
        {
            return SystemController.GetSystemSettingsActive(settingsName, decrypt);
        }

        [WebMethod]
        public bool CheckIsTwilioEnabled()
        {
            return SystemController.CheckIsTwilioEnabled();
        }

		[WebMethod]
		public int SetSystemSettings(string settingsName, SystemSettings settings)
		{
			return SystemController.SetSystemSettings(
				settingsName,
				settings
			);
		}

		[WebMethod]
		public DataSet GetThemes()
		{
			return SystemController.GetThemes();
		}

		[WebMethod]
		public DataSet GetThemeSettings(int ThemeID)
		{
			return SystemController.GetThemeSettings(ThemeID);
		}

		[WebMethod]
		public DataSet GetThemeSetting(int ThemeID, string SettingsName)
		{
			return SystemController.GetThemeSetting(ThemeID, SettingsName);
		}

		[WebMethod]
		public string GetCryptoKey() => new EnterpriseServerTunnelService().CryptoKey;

		[WebMethod]
		public Data.DbType GetDatabaseType() => SystemController.GetDatabaseType();

		[WebMethod]
		public bool GetUseEntityFramework() => SystemController.GetUseEntityFramework();

		[WebMethod]
		public int SetUseEntityFramework(bool useEntityFramework) => SystemController.SetUseEntityFramework(useEntityFramework);

	}
}
