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
using System.IO;

namespace FuseCP.Templates.AST
{
    internal class CallTemplateStatement : Statement
    {
        public string templateName;
        public Dictionary<string, Expression> parameters = new Dictionary<string, Expression>();
        List<Statement> statements = new List<Statement>();

        public CallTemplateStatement(int line, int column)
            : base(line, column)
        {
        }

        public string TemplateName
        {
            get { return templateName; }
            set { templateName = value; }
        }

        public Dictionary<string, Expression> Parameters
        {
            get { return parameters; }
        }

        public List<Statement> Statements
        {
            get { return statements; }
        }

        public override void Eval(TemplateContext context, System.IO.StringWriter writer)
        {
            // locate template
            if (!context.Templates.ContainsKey(templateName))
                throw new ParserException(String.Format("Custom template \"{0}\" is not defined", templateName), Line, Column);

            TemplateStatement tmp = context.Templates[templateName];

            // create template-specific context
            TemplateContext tmpContext = new TemplateContext();
            tmpContext.ParentContext = context;
            tmpContext.Templates = context.Templates;

            // evaluate inner statements
            StringWriter innerWriter = new StringWriter();
            foreach (Statement stm in Statements)
                stm.Eval(context, innerWriter);
            tmpContext.Variables["innerText"] = innerWriter.ToString();

            // set context variables
            foreach (string name in parameters.Keys)
                tmpContext.Variables[name] = parameters[name].Eval(context);

            // evaluate template statements
            foreach (Statement stm in tmp.Statements)
                stm.Eval(tmpContext, writer);
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("{").Append(templateName);

            foreach (string name in parameters.Keys)
                sb.Append(" ").Append(name).Append("=\"").Append(parameters[name].ToString()).Append("\"");

            sb.Append(" /}");
            return sb.ToString();
        }
    }
}
