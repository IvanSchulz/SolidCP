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

namespace FuseCP.WebDav.Core
{
    namespace Client
    {
        public sealed class PropertyName
        {
            public readonly string Name;
            public readonly string NamespaceUri;

            public PropertyName(string name, string namespaceUri)
            {
                Name = name;
                NamespaceUri = namespaceUri;
            }

            public override bool Equals(object obj)
            {
                if (obj.GetType() == GetType())
                {
                    if (((PropertyName) obj).Name == Name && ((PropertyName) obj).NamespaceUri == NamespaceUri)
                    {
                        return true;
                    }
                }

                return false;
            }

            public override int GetHashCode()
            {
                return base.GetHashCode();
            }

            public override string ToString()
            {
                return Name;
            }
        }
    }
}
