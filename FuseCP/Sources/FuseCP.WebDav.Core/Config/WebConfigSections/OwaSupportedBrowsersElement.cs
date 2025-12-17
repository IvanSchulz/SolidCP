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

using System.Configuration;

namespace FuseCP.WebDav.Core.Config.WebConfigSections
{
    public class OwaSupportedBrowsersElement : ConfigurationElement
    {
        private const string BrowserKey = "browser";
        private const string VersionKey = "version";

        [ConfigurationProperty(BrowserKey, IsKey = true, IsRequired = true)]
        public string Browser
        {
            get { return (string)this[BrowserKey]; }
            set { this[BrowserKey] = value; }
        }

        [ConfigurationProperty(VersionKey, IsKey = true, IsRequired = true)]
        public int Version
        {
            get { return (int)this[VersionKey]; }
            set { this[VersionKey] = value; }
        }
    }
}
