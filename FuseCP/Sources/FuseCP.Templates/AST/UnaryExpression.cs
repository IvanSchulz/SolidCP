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
    internal class UnaryExpression : Expression
    {
        Expression rhs;
        TokenType op;

        public UnaryExpression(int line, int column, TokenType op, Expression rhs)
            : base(line, column)
        {
            this.op = op;
            this.rhs = rhs;
        }

        public TokenType Operator
        {
            get { return op; }
        }

        Expression RightExpression
        {
            get { return rhs; }
            set { rhs = value; }
        }

        public override object Eval(TemplateContext context)
        {
            // evaluate right side
            object val = rhs.Eval(context);

            if (op == TokenType.Minus && val is Decimal)
                return -(Decimal)val;
            else if (op == TokenType.Minus && val is Int32)
                return -(Int32)val;
            else if (op == TokenType.Not && val is Boolean)
                return !(Boolean)val;

            throw new ParserException("Unary operator can be applied to integer, decimal and boolean expressions.", Line, Column);
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(Operator).Append("(").Append(RightExpression).Append(")");
            
            return sb.ToString();
        }
    }
}
