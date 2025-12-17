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

namespace FuseCP.Providers.FTP.IIs100.Config
{
    using Microsoft.Web.Administration;
    using System;

    internal class UserIsolationElement : ConfigurationElement
    {
        private ActiveDirectoryElement _activeDirectory;

        public ActiveDirectoryElement ActiveDirectory
        {
            get
            {
                if (this._activeDirectory == null)
                {
                    this._activeDirectory = (ActiveDirectoryElement) base.GetChildElement("activeDirectory", typeof(ActiveDirectoryElement));
                }
                return this._activeDirectory;
            }
        }

        public Mode Mode
        {
            get
            {
                return (Mode) base["mode"];
            }
            set
            {
                base["mode"] = (int) value;
            }
        }
    }
}

