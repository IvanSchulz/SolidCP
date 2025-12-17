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

namespace FuseCP.Providers.Utils
{
    /// <summary>
    /// Summary description for SystemSID.
    /// </summary>
    public class SystemSID
    {
        /// <summary>"Administrators" SID</summary>
        public const string ADMINISTRATORS = "S-1-5-32-544";

        /// <summary>"Local System (SYSTEM)" SID</summary>
        public const string SYSTEM = "S-1-5-18";

        /// <summary>"NETWORK SERVICE" SID</summary>
        public const string NETWORK_SERVICE = "S-1-5-20";

        /// <summary>"LOCAL SERVICE" SID</summary>
        public const string LOCAL_SERVICE = "S-1-5-19";
    }
}
