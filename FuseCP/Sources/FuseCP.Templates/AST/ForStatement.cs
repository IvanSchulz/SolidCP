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
    internal class ForStatement : Statement
    {
        string indexIdentifier;
        Expression startIndex;
        Expression endIndex;
        List<Statement> statements = new List<Statement>();

        public ForStatement(int line, int column)
            : base(line, column)
        {
        }

        public string IndexIdentifier
        {
            get { return indexIdentifier; }
            set { indexIdentifier = value; }
        }

        public Expression StartIndex
        {
            get { return startIndex; }
            set { startIndex = value; }
        }

        public Expression EndIndex
        {
            get { return endIndex; }
            set { endIndex = value; }
        }

        public List<Statement> Statements
        {
            get { return statements; }
        }

        public override void Eval(TemplateContext context, System.IO.StringWriter writer)
        {
            // evaluate indicies
            object objStartIdx = StartIndex.Eval(context);
            object objEndIdx = EndIndex.Eval(context);

            // check indicies
            if (!(objStartIdx is Int32))
                throw new ParserException("Start index expression should evaluate to integer.", StartIndex.Line, StartIndex.Column);
            if (!(objEndIdx is Int32))
                throw new ParserException("End index expression should evaluate to integer.", StartIndex.Line, StartIndex.Column);

            int startIdx = Convert.ToInt32(objStartIdx);
            int endIdx = Convert.ToInt32(objEndIdx);
            int step = startIdx < endIdx ? 1 : -1;
            endIdx += step;

            int i = startIdx;
            do
            {
                context.Variables[IndexIdentifier] = i;

                // evaluate statements
                foreach (Statement stm in Statements)
                    stm.Eval(context, writer);

                i += step;
            }
            while (i != endIdx);

            // cleanup vars
            context.Variables.Remove(IndexIdentifier);
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("{for ")
                .Append(IndexIdentifier).Append(" = ")
                .Append(StartIndex).Append(" to ").Append(EndIndex).Append("}");
            foreach (Statement stm in Statements)
            {
                sb.Append(stm);
            }
            sb.Append("{/for}");
            return sb.ToString();
        }
    }
}
