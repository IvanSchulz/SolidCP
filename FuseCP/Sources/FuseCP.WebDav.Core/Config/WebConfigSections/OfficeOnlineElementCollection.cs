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

namespace FuseCP.WebDavPortal.WebConfigSections
{
    [ConfigurationCollection(typeof(OfficeOnlineElement))]
    public class OfficeOnlineElementCollection : ConfigurationElementCollection
    {
        private const string CobaltFileTtlKey = "cobaltFileTtl";
        private const string CobaltNewFilePathKey = "cobaltNewFilePath";

        [ConfigurationProperty(CobaltNewFilePathKey, IsKey = true, IsRequired = true)]
        public string CobaltNewFilePath
        {
            get { return this[CobaltNewFilePathKey].ToString(); }
            set { this[CobaltNewFilePathKey] = value; }
        }

        [ConfigurationProperty(CobaltFileTtlKey, IsKey = true, IsRequired = true)]
        public int CobaltFileTtl
        {
            get { return int.Parse(this[CobaltFileTtlKey].ToString()); }
            set { this[CobaltFileTtlKey] = value; }
        }

        protected override ConfigurationElement CreateNewElement()
        {
            return new OfficeOnlineElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((OfficeOnlineElement)element).Extension;
        }
    }
}
