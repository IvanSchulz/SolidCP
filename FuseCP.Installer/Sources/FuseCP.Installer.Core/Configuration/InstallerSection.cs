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
using System.Configuration;

namespace FuseCP.Installer.Configuration
{
	/// <summary>
	/// Provides configuration system support for the <installerSettings> configuration section. 
	/// </summary>
	public class InstallerSection : ConfigurationSection
	{
		/// <summary>
		/// Creates a new instance of the StudioSection class.
		/// </summary>
		public InstallerSection()
		{
		}

		/// <summary>
		/// Declare <connections> collection element represented
		/// in the configuration file by the sub-section.
		/// </summary>
		[ConfigurationProperty("components", IsDefaultCollection = false)]
		public ComponentCollection Components
		{
			get
			{
				ComponentCollection componentCollection = (ComponentCollection)base["components"];
				return componentCollection;
			}
		}

		[ConfigurationProperty("settings", IsDefaultCollection = false)]
		public KeyValueConfigurationCollection Settings
		{
			get
			{
				return (KeyValueConfigurationCollection)base["settings"];
			}
		}
		
		public string GetStringSetting(string key)
		{
			string ret = null;
			if (Settings[key] != null)
			{
				ret = Settings[key].Value;
			}
			return ret;
		}
		
		public int GetInt32Setting(string key)
		{
			int ret = 0;
			if (Settings[key] != null)
			{
				string val = Settings[key].Value;
				Int32.TryParse(val, out ret);
			}
			return ret;
		}

		public bool GetBooleanSetting(string key)
		{
			bool ret = false;
			if (Settings[key] != null)
			{
				string val = Settings[key].Value;
				Boolean.TryParse(val, out ret);
			}
			return ret;
		}
	}
}
