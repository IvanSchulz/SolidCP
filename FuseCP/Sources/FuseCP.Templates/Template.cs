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
using System.IO;
using System.Collections.Generic;
using System.Text;
using FuseCP.Templates.AST;
using System.Collections;

namespace FuseCP.Templates
{
    /// <summary>
    /// Template allows to process block of texts containing special directives such as variables,
    /// conditional statements and loops.
    /// </summary>
    public class Template
    {
        string data;
        Lexer lexer = null;
        Parser parser = null;
        List<AST.Statement> statements = null;
        TemplateContext context = new TemplateContext();

        /// <summary>
        /// Initializes a new instance of Template with specified template text.
        /// </summary>
        /// <param name="data">Template text.</param>
        public Template(string data)
        {
            if (data == null)
                throw new ArgumentNullException("data");

            this.data = data;
        }

        /// <summary>
        /// Initializes a new instance of Template with specified StringReader containing template text.
        /// </summary>
        /// <param name="reader">StringReader containing template text.</param>
        public Template(StringReader reader)
        {
            if(reader == null)
                throw new ArgumentNullException("reader");

            this.data = reader.ReadToEnd();
        }

        /// <summary>
        /// Verifies template syntax and throws <see cref="ParserException">ParserException</see> when error is found.
        /// </summary>
        /// <exception cref="ParserException">Thrown when error is found.</exception>
        public void CheckSyntax()
        {
            // create lexer
            lexer = new Lexer(data);

            // create parser
            parser = new Parser(lexer);

            // parse template
            parser.Parse();
        }

        /// <summary>
        /// Evaluates template and returns the result as a string.
        /// </summary>
        /// <returns>String containing evaluated template.</returns>
        public string Evaluate()
        {
            // create writer to hold result
            StringWriter writer = new StringWriter();

            // evaluate
            Evaluate(writer);

            // return result
            return writer.ToString();
        }

        /// <summary>
		/// Evaluates template and returns the result as a string.
		/// </summary>
		/// <returns>String containing evaluated template.</returns>
		public string Evaluate(Hashtable items)
		{
			// copy items from hashtable
			foreach (string keyName in items.Keys)
			{
				this[keyName] = items[keyName];
			}

			// evaluate
			return Evaluate();
		}

		/// <summary>
		/// Evaluates template to the StringWriter.
        /// </summary>
        /// <param name="writer">StringWriter to write evaluation results.</param>
        public void Evaluate(StringWriter writer)
        {
            if(writer == null)
                throw new ArgumentNullException("writer");

            if (lexer == null)
            {
                // create lexer
                lexer = new Lexer(data);

                // create parser
                parser = new Parser(lexer);

                // parse template
                statements = parser.Parse();
            }

            // index custom templates
            int i = 0;
            while (i < statements.Count)
            {
                TemplateStatement tmpStatement = statements[i] as TemplateStatement;
                if (tmpStatement != null)
                {
                    if (context.Templates.ContainsKey(tmpStatement.Name))
                    {
                        throw new Exception($"Cannot add template Statement with name {tmpStatement.Name} as that template already exists");
                    }

                    context.Templates.Add(tmpStatement.Name, tmpStatement);
                    statements.RemoveAt(i);
                    continue;
                }
                i++;
            }

            // evaluate template statements
            foreach (AST.Statement stm in statements)
            {
                // eval next statement
                stm.Eval(context, writer);
            }
        }

        /// <summary>
        /// Gets or sets the value of template context variable.
        /// </summary>
        /// <param name="name">The name of the context variable. Variable names are case-insensitive.</param>
        /// <returns>Returns the value of the context variable.</returns>
        public object this[string name]
        {
            get { return context.Variables.ContainsKey(name) ? context.Variables[name] : null; }
            set { context.Variables[name] = value; }
        }
    }
}
