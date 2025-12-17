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

namespace FuseCP.Providers.FTP.IIs100.Config
{
    using Microsoft.Web.Administration;
    using System;
    using System.Reflection;

    internal class ProviderCollection : ConfigurationElementCollectionBase<ProviderElement>
    {
        public ProviderElement Add(string name, bool enabled)
        {
            ProviderElement element = base.CreateElement();
            element.Name = name;
            element.Enabled = enabled;
            return base.Add(element);
        }

        protected override ProviderElement CreateNewElement(string elementTagName)
        {
            return new ProviderElement();
        }

        public new ProviderElement this[string name]
        {
            get
            {
                for (int i = 0; i < base.Count; i++)
                {
                    ProviderElement element = base[i];
                    if (string.Equals(element.Name, name, StringComparison.OrdinalIgnoreCase))
                    {
                        return element;
                    }
                }
                return null;
            }
        }
    }
}

