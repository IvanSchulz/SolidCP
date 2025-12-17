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
    internal class IfStatement : Statement
    {
        Expression condition;
        List<ElseIfStatement> elseIfStatements = new List<ElseIfStatement>();
        List<Statement> trueStatements = new List<Statement>();
        List<Statement> falseStatements = new List<Statement>();

        public IfStatement(int line, int column)
            : base(line, column)
        {
        }

        public Expression Condition
        {
            get { return condition; }
            set { condition = value; }
        }

        public List<ElseIfStatement> ElseIfStatements
        {
            get { return elseIfStatements; }
        }

        public List<Statement> TrueStatements
        {
            get { return trueStatements; }
        }

        public List<Statement> FalseStatements
        {
            get { return falseStatements; }
        }

        public override void Eval(TemplateContext context, System.IO.StringWriter writer)
        {
            // evaluate test condition
            bool result = EvalCondition(Condition, context);

            if (result)
            {
                foreach (Statement stm in TrueStatements)
                    stm.Eval(context, writer);
                return;
            }
            else
            {
                // process else if statements
                foreach (ElseIfStatement stm in ElseIfStatements)
                {
                    if (EvalCondition(stm.Condition, context))
                    {
                        stm.Eval(context, writer);
                        return;
                    }
                }

                // process else
                foreach (Statement stm in FalseStatements)
                    stm.Eval(context, writer);
            }
        }

        private bool EvalCondition(Expression expr, TemplateContext context)
        {
            object val = expr.Eval(context);
            if (val is Boolean)
                return (Boolean)val;
            else if (val is Int32)
                return ((Int32)val) != 0;
            else if (val is Decimal)
                return ((Decimal)val) != 0;
            else if (val != null)
                return true;
            else
                return false;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("{if ")
                .Append(Condition.ToString()).Append("}");

            // true statements
            foreach (Statement stm in TrueStatements)
                sb.Append(stm);

            // elseif statements
            foreach (Statement stm in ElseIfStatements)
                sb.Append(stm);

            // false statements
            if(FalseStatements.Count > 0)
            {
                sb.Append("{else}");
                foreach (Statement stm in FalseStatements)
                    sb.Append(stm);
            }

            sb.Append("{/if}");
            return sb.ToString();
        }
    }
}
