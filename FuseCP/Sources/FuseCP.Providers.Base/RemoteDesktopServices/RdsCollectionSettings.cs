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
    public class RdsCollectionSettings
    {
        public int Id { get; set; }
        public int RdsCollectionId { get; set; }
        public int DisconnectedSessionLimitMin { get; set; }
        public int ActiveSessionLimitMin { get; set; }
        public int IdleSessionLimitMin { get; set; }
        public string BrokenConnectionAction { get; set; }
        public bool AutomaticReconnectionEnabled { get; set; }
        public bool TemporaryFoldersDeletedOnExit { get; set; }
        public bool TemporaryFoldersPerSession { get; set; }
        public string ClientDeviceRedirectionOptions { get; set; }
        public bool ClientPrinterRedirected { get; set; }
        public bool ClientPrinterAsDefault { get; set; }
        public bool RDEasyPrintDriverEnabled { get; set; }
        public int MaxRedirectedMonitors { get; set; }
        public string SecurityLayer { get; set; }
        public string EncryptionLevel { get; set; }
        public bool AuthenticateUsingNLA { get; set; }
    }
}
