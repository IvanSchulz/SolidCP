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

﻿using System;
using System.Collections.Generic;
using System.Text;

namespace FuseCP.Templates.AST
{
    internal class TemplateStatement : Statement
    {
        public string name;
        List<Statement> statements = new List<Statement>();

        public TemplateStatement(int line, int column)
            : base(line, column)
        {
        }

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public List<Statement> Statements
        {
            get { return statements; }
        }

        public override void Eval(TemplateContext context, System.IO.StringWriter writer)
        {
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("{template ")
                .Append("name=\"").Append(Name).Append("\"}");
            foreach (Statement stm in Statements)
            {
                sb.Append(stm);
            }
            sb.Append("{/template}");
            return sb.ToString();
        }
    }
}
