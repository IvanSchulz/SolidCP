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

namespace FuseCP.Templates
{
    internal enum TokenType
    {
        EOF,
        Text,

        Attribute, // tag attribute
        Identifier, // object.prop1.prop2

        // literals
        Integer,    // 1234
        Decimal,    // 12.34, 12,33
        String,     // "test\nstring with escapes and 'quotes' inside"
                    // 'single "quoted" string'
        Null,       // null
        Empty,      // empty
        True,       // true
        False,      // false

        // symbols
        LParen,		// (
        RParen,		// )
        LBracket,	// [
        RBracket,	// ]
        LBrace,     // {
        RBrace,     // }
        Colon,      // :
        Dot,        // .
        Comma,        // ,

        // operators
        Plus,       // +
        Minus,      // -
        Div,        // /
        Mult,       // *
        Mod,        // %
        BinOr,      // |
        BinAnd,     // &
        Or,         // ||, OR
        And,        // &&, AND
        Not,        // !, NOT
        Assign,      // =

        // comparisons
        Greater,    // >
        GreaterOrEqual, // >=
        Less,       // <
        LessOrEqual,// <=
        Equal,      // ==
        NotEqual,   // !=

        // start statements
        Set,        // {set}
        Template,   // {template}
        Print,      // {$ exp }
        If,         // {if exp}
        ElseIf,     // {elseif exp}
        Else,       // {else}
        Foreach,    // {foreach identifier in exp [index identifier]}
        For,        // {for identifier = exp to exp}

        // end statements
        EndTemplate,// {/template}
        EndIf,      // {/if}
        EndForeach, // {/foreach}
        EndFor,     // {/for}

        // custom tags
        OpenTag,    // <ad:Custom>
        CloseTag,   // </ad:Custom>
        EmptyTag    // .../>
    }
}
