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
    internal class ElseIfStatement : Statement
    {
        Expression condition;
        List<Statement> trueStatements = new List<Statement>();

        public ElseIfStatement(int line, int column)
            : base(line, column)
        {
        }

        public Expression Condition
        {
            get { return condition; }
            set { condition = value; }
        }

        public List<Statement> TrueStatements
        {
            get { return trueStatements; }
        }

        public override void Eval(TemplateContext context, System.IO.StringWriter writer)
        {
            foreach (Statement stm in TrueStatements)
                stm.Eval(context, writer);
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("{elseif ")
                .Append(Condition.ToString()).Append("}");
            foreach (Statement stm in TrueStatements)
            {
                sb.Append(stm);
            }
            return sb.ToString();
        }
    }
}
