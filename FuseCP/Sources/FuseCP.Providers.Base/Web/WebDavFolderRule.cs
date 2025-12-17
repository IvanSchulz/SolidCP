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
using FuseCP.Providers.HostedSolution;

namespace FuseCP.Providers.Web
{
    public enum WebDavAccess
    {
        Read = 1,
        Source = 16,
        Write = 2
    }

    [Serializable]
    public class WebDavFolderRule
    {
        public List<string> Pathes { get; set; }
        public List<string> Users { get; set; }
        public List<string> Roles { get; set; }

        public int AccessRights
        {
            get
            {
                int result = 0;

                if (Read)
                {
                    result |= (int)WebDavAccess.Read;
                }

                if (Write)
                {
                    result |= (int)WebDavAccess.Write;
                }

                if (Source)
                {
                    result |= (int)WebDavAccess.Source;
                }

                return result;
            }
        }

        public bool Read { get; set; }
        public bool Write { get; set; }
        public bool Source { get; set; }

        public WebDavFolderRule()
        {
            Pathes = new List<string>();
            Users = new List<string>();
            Roles = new List<string>();
        }
    }
}
