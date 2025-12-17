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
        public class Property
        {
            public readonly PropertyName Name;
            private string _value = "";

            public Property(PropertyName name, string value)
            {
                Name = name;
                StringValue = value;
            }

            public Property(string name, string nameSpace, string value)
            {
                Name = new PropertyName(name, nameSpace);
                StringValue = value;
            }

            public string StringValue
            {
                get { return _value; }
                set { _value = value; }
            }

            public override string ToString()
            {
                return StringValue;
            }
        }
    }
}
