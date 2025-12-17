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
using System.Configuration;

namespace FuseCP.FixDefaultPublicFolderMailbox
{
    class Program
    {
        static void Main(string[] args)
        {
            Log.Initialize(ConfigurationManager.AppSettings["LogFile"]);

            bool showHelp = false;
            string param = null;

            if (args.Length==0)
            {
                showHelp = true;
            }
            else
            {
                param = args[0];

                if ((param == "/?") || (param.ToLower() == "/h"))
                    showHelp = true;
            }

            if (showHelp)
            {
                string name = typeof(Log).Assembly.GetName().Name;
                string version = typeof(Log).Assembly.GetName().Version.ToString(3);

                Console.WriteLine("FuseCP Fix default public folder mailbox. " + version);
                Console.WriteLine("Usage :");
                Console.WriteLine(name + " [/All]");
                Console.WriteLine("or");
                Console.WriteLine(name + " [OrganizationId]");
                return;
            }

            Log.WriteApplicationStart();

            if (param.ToLower() == "/all")
                param = null;

            Fix.Start(param);

            Log.WriteApplicationEnd();
        }
    }
}
