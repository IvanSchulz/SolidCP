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
    /// <summary>
    /// </summary>
    public class ParserException : Exception
    {
        int line;
        int column;
        int position;
        int length;

        /// <summary>
        /// </summary>
        /// <param name="message"></param>
        /// <param name="line"></param>
        /// <param name="column"></param>
        public ParserException(string message, int line, int column)
            : base(message)
        {
            this.line = line;
            this.column = column;
        }

        /// <summary>
        /// </summary>
        /// <param name="message"></param>
        /// <param name="line"></param>
        /// <param name="column"></param>
        /// <param name="position"></param>
        /// <param name="length"></param>
        public ParserException(string message, int line, int column, int position, int length)
            : base(message)
        {
            this.line = line;
            this.column = column;
            this.position = position;
            this.length = length;
        }

        /// <summary>
        /// </summary>
        public int Line
        {
            get { return line; }
            set { line = value; }
        }

        /// <summary>
        /// </summary>
        public int Column
        {
            get { return column; }
            set { column = value; }
        }

        /// <summary>
        /// </summary>
        public int Position
        {
            get { return position; }
            set { position = value; }
        }

        /// <summary>
        /// </summary>
        public int Length
        {
            get { return length; }
            set { length = value; }
        }

    }
}
