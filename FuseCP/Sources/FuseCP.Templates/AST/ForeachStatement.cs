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
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace FuseCP.Templates.AST
{
    internal class ForeachStatement : Statement
    {
        string elementIdentifier;
        string indexIdentifier;
        Expression collection;
        List<Statement> statements = new List<Statement>();

        public ForeachStatement(int line, int column)
            : base(line, column)
        {
        }

        public string ElementIdentifier
        {
            get { return elementIdentifier; }
            set { elementIdentifier = value; }
        }

        public string IndexIdentifier
        {
            get { return indexIdentifier; }
            set { indexIdentifier = value; }
        }

        public Expression Collection
        {
            get { return collection; }
            set { collection = value; }
        }

        public List<Statement> Statements
        {
            get { return statements; }
        }

        public override void Eval(TemplateContext context, System.IO.StringWriter writer)
        {
            // evaluate collection expression
            object coll = Collection.Eval(context);
            if(!(coll is IEnumerable))
                throw new ParserException("Collection expression should evaluate into the value implementing IEnumerable interface.",
                    Collection.Line, Collection.Column);

            int idx = 0;
            foreach(object obj in ((IEnumerable)coll))
            {
                context.Variables[ElementIdentifier] = obj;
                if(IndexIdentifier != null)
                    context.Variables[IndexIdentifier] = idx;
                idx++;

                // evaluate statements
                foreach(Statement stm in Statements)
                    stm.Eval(context, writer);
            }

            // cleanup context
            context.Variables.Remove(ElementIdentifier);
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("{foreach ")
                .Append(ElementIdentifier).Append(" in ").Append(Collection);
            if (IndexIdentifier != null)
                sb.Append(" index ").Append(IndexIdentifier);
            sb.Append("}");
            foreach (Statement stm in Statements)
            {
                sb.Append(stm);
            }
            sb.Append("{/foreach}");
            return sb.ToString();
        }
    }
}
