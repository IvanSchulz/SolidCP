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
using System.Management.Automation.Runspaces;
using System.Text;

namespace FuseCP.Providers.RemoteDesktopServices
{
    public class RdsMessenger
    {
        public void SendMessage(List<RdsMessageRecipient> recipients, string text, string primaryDomainController)
        {            
            Runspace runspace = null;

            try
            {
                runspace = RdsRunspaceExtensions.OpenRunspace();
                
                var messages = recipients.GroupBy(m => m.ComputerName, m => m.SessionId, (key, g) => new {
                                                 ComputerName = key, 
                                                 SessionIds = g.ToList()});

                foreach(var message in messages)
                {
                    List<string> scripts = new List<string>
                    {
                        string.Format("msg {0} \"{1}\"", string.Join(",", message.SessionIds.ToArray()), text)
                    };

                    object[] errors;
                    runspace.ExecuteRemoteShellCommand(message.ComputerName, scripts, primaryDomainController, out errors);
                }                               
            }
            finally
            {
                runspace.CloseRunspace();
            }
        }
    }
}
