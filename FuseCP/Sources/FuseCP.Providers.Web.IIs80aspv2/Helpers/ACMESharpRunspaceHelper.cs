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
using System.Linq;
using System.Management.Automation.Runspaces;
namespace FuseCP.Providers.Web
{
    public class ACMESharpRunspaceHelper : RunspaceHelper
    {
        public ACMESharpRunspaceHelper()
        {
        }

        public bool AreModulesInstalled(string[] modules)
        {
            object[] objArray;
            bool result;
            Runspace runspace = null;
            try
            {
                try
                {
                    runspace = this.OpenRunspace();
                    Command command = new Command("Import-Module");
                    command.Parameters.Add("Name", modules);
                    base.ExecuteShellCommand(runspace, command, out objArray);
                    result = !objArray.Any<object>();
                }
                catch (Exception exception)
                {
                    return false;
                }
            }
            finally
            {
                base.CloseRunspace(runspace);
            }
            return result;
        }

        public Runspace OpenRunspace()
        {
            return base.OpenRunspace(new string[] { "ACMESharp" });
        }
    }
}
