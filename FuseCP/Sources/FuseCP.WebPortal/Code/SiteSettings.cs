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
using System.Collections.Specialized;
using System.Text;

namespace FuseCP.WebPortal
{
    public class SiteSettings
    {
		private NameValueCollection _settings;

		public string this[string settingName]
		{
			get { return _settings[settingName];  }
			set { _settings[settingName] = value; }
		}

		public string[] AllKeys
		{
			get { return _settings.AllKeys; }
		}

		public SiteSettings()
		{
			_settings = new NameValueCollection();
		}
    }
}
