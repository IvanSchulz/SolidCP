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
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NSwag;
using NSwag.CodeGeneration.OperationNameGenerators;
using System.Text.RegularExpressions;

namespace TypeScriptClientGenerator
{
    public class FuseCPOperationNameGenerator : IOperationNameGenerator
    {
        public bool SupportsMultipleClients => true;

        public HashSet<string> Clients = new HashSet<string>();
        public string GetClientName(OpenApiDocument document, string path, string httpMethod, OpenApiOperation operation)
        {
            var clientName = Regex.Match(path, @"(?<=^/?api/)[^/]*").Value;
            Clients.Add(clientName);
            return clientName;
        }

        public string GetOperationName(OpenApiDocument document, string path, string httpMethod, OpenApiOperation operation)
        {
            var operationName = Regex.Match(path, @"(?<=^/?api/[^/]*?/)[^/]*").Value;
            return operationName;
        }
    }
}
