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

namespace FuseCP.Providers.Web.Iis.Common
{
    using Microsoft.Web.Administration;
    using System;
    using System.Reflection;

    internal sealed class IsapiCgiRestrictionCollection : ConfigurationElementCollectionBase<IsapiCgiRestrictionElement>
    {
        public IsapiCgiRestrictionElement Add(string path, bool allowed)
        {
            IsapiCgiRestrictionElement element = base.CreateElement();
            element.Path = path;
            element.Allowed = allowed;
            return base.Add(element);
        }

        protected override IsapiCgiRestrictionElement CreateNewElement(string elementTagName)
        {
            return new IsapiCgiRestrictionElement();
        }

        public void Remove(string path)
        {
            base.Remove(this[path]);
        }

        public new IsapiCgiRestrictionElement this[string path]
        {
            get
            {
                for (int i = 0; i < base.Count; i++)
                {
                    IsapiCgiRestrictionElement element = base[i];
                    if (string.Equals(Environment.ExpandEnvironmentVariables(element.Path), Environment.ExpandEnvironmentVariables(path), StringComparison.OrdinalIgnoreCase))
                    {
                        return element;
                    }
                }
                return null;
            }
        }
    }
}

