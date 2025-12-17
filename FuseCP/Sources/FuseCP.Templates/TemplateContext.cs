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

using FuseCP.Templates.AST;

namespace FuseCP.Templates
{
    internal class TemplateContext
    {
        Dictionary<string, object> variables = new Dictionary<string, object>(StringComparer.InvariantCultureIgnoreCase);
        Dictionary<string, TemplateStatement> templates = new Dictionary<string, TemplateStatement>(StringComparer.InvariantCultureIgnoreCase);

        public TemplateContext ParentContext { get; set; }

        public Dictionary<string, object> Variables
        {
            get { return variables; }
        }

        public Dictionary<string, TemplateStatement> Templates
        {
            get { return templates; }
            set { templates = value; }
        }
    }
}
