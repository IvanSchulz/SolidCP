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

namespace FuseCP.WebDavPortal.WebConfigSections
{
    public class OfficeOnlineElement : ConfigurationElement
    {
        private const string ExtensionKey = "extension";
        private const string OwaViewKey = "OwaView";
        private const string OwaEditorKey = "OwaEditor";
        private const string OwaMobileViewKey = "OwaMobileView";
        private const string OwaNewFileViewKey = "OwaNewFileView";

        [ConfigurationProperty(ExtensionKey, IsKey = true, IsRequired = true)]
        public string Extension
        {
            get { return this[ExtensionKey].ToString(); }
            set { this[ExtensionKey] = value; }
        }

        [ConfigurationProperty(OwaViewKey, IsKey = true, IsRequired = true)]
        public string OwaView
        {
            get { return this[OwaViewKey].ToString(); }
            set { this[OwaViewKey] = value; }
        }

        [ConfigurationProperty(OwaEditorKey, IsKey = true, IsRequired = true)]
        public string OwaEditor
        {
            get { return this[OwaEditorKey].ToString(); }
            set { this[OwaEditorKey] = value; }
        }


        [ConfigurationProperty(OwaMobileViewKey, IsKey = true, IsRequired = true)]
        public string OwaMobileViev
        {
            get { return this[OwaMobileViewKey].ToString(); }
            set { this[OwaMobileViewKey] = value; }
        }

        [ConfigurationProperty(OwaNewFileViewKey, IsKey = true, IsRequired = true)]
        public string OwaNewFileView
        {
            get { return this[OwaNewFileViewKey].ToString(); }
            set { this[OwaNewFileViewKey] = value; }
        }
    }
}
