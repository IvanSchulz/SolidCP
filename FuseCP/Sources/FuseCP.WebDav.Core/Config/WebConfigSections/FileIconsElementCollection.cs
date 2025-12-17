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
    [ConfigurationCollection(typeof (FileIconsElement))]
    public class FileIconsElementCollection : ConfigurationElementCollection
    {
        private const string DefaultPathKey = "defaultPath";
        private const string FolderPathKey = "folderPath";

        [ConfigurationProperty(DefaultPathKey, IsRequired = false, DefaultValue = "/")]
        public string DefaultPath
        {
            get { return (string) this[DefaultPathKey]; }
            set { this[DefaultPathKey] = value; }
        }

        [ConfigurationProperty(FolderPathKey, IsRequired = false)]
        public string FolderPath
        {
            get { return (string)this[FolderPathKey]; }
            set { this[FolderPathKey] = value; }
        }

        protected override ConfigurationElement CreateNewElement()
        {
            return new FileIconsElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((FileIconsElement) element).Extension;
        }
    }
}
