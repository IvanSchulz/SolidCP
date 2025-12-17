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

namespace FuseCP.Providers.RemoteDesktopServices
{    
    [Serializable]
    public class RdsUserSession
    {
        public string CollectionName { get; set; }
        public string UserName { get; set; }
        public string UnifiedSessionId { get; set; }
        public string SessionState { get; set; }
        public string HostServer { get; set; }
        public string DomainName { get; set; }
        public bool IsVip { get; set; }
        public string SamAccountName { get; set; }
    }
}
