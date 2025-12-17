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
    internal class BinaryExpression : Expression
    {
        Expression lhs;
        Expression rhs;
        TokenType op;

        public BinaryExpression(int line, int column, Expression lhs, TokenType op, Expression rhs)
            : base(line, column)
        {
            this.lhs = lhs;
            this.op = op;
            this.rhs = rhs;
        }

        public TokenType Operator
        {
            get { return op; }
        }

        Expression LeftExpression
        {
            get { return lhs; }
            set { lhs = value; }
        }

        Expression RightExpression
        {
            get { return rhs; }
            set { rhs = value; }
        }

        public override object Eval(TemplateContext context)
        {
            // evaluate both parts
            object lv = LeftExpression.Eval(context);
            object rv = RightExpression.Eval(context);

            // equality/not-equality
            if (op == TokenType.Equal)
            {
                if (lv == null && rv == null)
                    return true;
                else if (lv != null)
                    return lv.Equals(rv);
            }
            else if (op == TokenType.NotEqual)
            {
                if (lv == null && rv == null)
                    return false;
                else if (lv != null)
                    return !lv.Equals(rv);
            }

            // arithmetic operation
            else if (op == TokenType.Mult
                || op == TokenType.Div
                || op == TokenType.Mod
                || op == TokenType.Plus
                || op == TokenType.Minus
                || op == TokenType.Less
                || op == TokenType.LessOrEqual
                || op == TokenType.Greater
                || op == TokenType.GreaterOrEqual)
            {
                if(!((lv is Decimal || lv is Int32) && (rv is Decimal || rv is Int32)))
                    throw new ParserException("Arithmetic and logical operations can be applied to operands or integer and decimal types only.",
                        Line, Column);
                    
                
                bool dec = lv is Decimal || rv is Decimal;
                object val = null;
                if(op == TokenType.Mult)
                    val = dec ? (Decimal)lv * (Decimal)rv : (Int32)lv * (Int32)rv;
                else if (op == TokenType.Div)
                    val = dec ? (Decimal)lv / (Decimal)rv : (Int32)lv / (Int32)rv;
                else if (op == TokenType.Mod)
                    val = dec ? (Decimal)lv % (Decimal)rv : (Int32)lv % (Int32)rv;
                else if (op == TokenType.Plus)
                    val = dec ? (Decimal)lv + (Decimal)rv : (Int32)lv + (Int32)rv;
                else if (op == TokenType.Minus)
                    val = dec ? (Decimal)lv - (Decimal)rv : (Int32)lv - (Int32)rv;
                else if (op == TokenType.Less)
                    val = dec ? (Decimal)lv < (Decimal)rv : (Int32)lv < (Int32)rv;
                else if (op == TokenType.LessOrEqual)
                    val = dec ? (Decimal)lv <= (Decimal)rv : (Int32)lv <= (Int32)rv;
                else if (op == TokenType.Greater)
                    val = dec ? (Decimal)lv > (Decimal)rv : (Int32)lv > (Int32)rv;
                else if (op == TokenType.GreaterOrEqual)
                    val = dec ? (Decimal)lv >= (Decimal)rv : (Int32)lv >= (Int32)rv;

                if (val is Boolean)
                {
                    bool ret = Convert.ToBoolean(val);
                    return ret;
                }
                else if (dec)
                {
                    decimal ret = Convert.ToDecimal(val);
                    return ret;
                }
                else
                {
                    int ret = Convert.ToInt32(val);
                    return ret;
                }
            }
            else if (op == TokenType.Or || op == TokenType.And)
            {
                if (!(lv is Boolean && rv is Boolean))
                    throw new ParserException("Logical operation can be applied to operands of boolean type only", Line, Column);

                if (op == TokenType.Or)
                    return (Boolean)lv || (Boolean)rv;
                else if (op == TokenType.And)
                    return (Boolean)lv && (Boolean)rv;
            }

            return 0;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("(")
                .Append(LeftExpression.ToString()).Append(" ")
                .Append(Operator).Append(" ")
                .Append(RightExpression.ToString()).Append(")");
            return sb.ToString();
        }
    }
}
