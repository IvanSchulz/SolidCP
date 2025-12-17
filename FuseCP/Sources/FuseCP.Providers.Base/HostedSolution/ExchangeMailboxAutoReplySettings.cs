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

namespace FuseCP.Providers.HostedSolution
{
    public enum OofState
    {
        Disabled,
        Enabled,
        Scheduled
    }

    public enum ExternalAudience
    {
        None,
        Known,
        All
    }

    public class ExchangeMailboxAutoReplySettings
    {
        public OofState AutoReplyState { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public ExternalAudience ExternalAudience { get; set; }
        public string InternalMessage { get; set; }
        public string ExternalMessage { get; set; }
    }
}
