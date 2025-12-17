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
using System.Collections.Generic;
using System.Text;

namespace FuseCP.Templates.AST
{
    internal class IdentifierPart
    {
        string name;
        Expression index;
        bool isMethod;
        List<Expression> methodParameters = new List<Expression>();
        int line;
        int column;

        public IdentifierPart(string name, int line, int column)
        {
            this.name = name;
            this.line = line;
            this.column = column;
        }

        public string Name
        {
            get { return name; }
        }

        public Expression Index
        {
            get { return index; }
            set { index = value; }
        }

        public bool IsMethod
        {
            get { return isMethod; }
            set { isMethod = value; }
        }

        public List<Expression> MethodParameters
        {
            get { return methodParameters; }
        }

        public int Line
        {
            get { return line; }
        }

        public int Column
        {
            get { return column; }
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(name);
            if (Index != null)
                sb.Append("[").Append(Index).Append("]");

            return sb.ToString();
        }
    }
}
