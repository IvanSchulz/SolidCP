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
    public class ElementsRenderingElement : ConfigurationElement
    {
        private const string DefaultCountKey = "defaultCount";
        private const string AddElementsCountKey = "addElementsCount";

        [ConfigurationProperty(DefaultCountKey, IsKey = true, IsRequired = true, DefaultValue = 30)]
        public int DefaultCount
        {
            get { return (int)this[DefaultCountKey]; }
            set { this[DefaultCountKey] = value; }
        }

        [ConfigurationProperty(AddElementsCountKey, IsKey = true, IsRequired = true, DefaultValue = 20)]
        public int AddElementsCount
        {
            get { return (int)this[AddElementsCountKey]; }
            set { this[AddElementsCountKey] = value; }
        }
    }
}
