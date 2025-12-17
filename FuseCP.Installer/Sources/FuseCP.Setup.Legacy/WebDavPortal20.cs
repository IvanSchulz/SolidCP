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

namespace FuseCP.Setup
{
    /// <summary>
    /// Release 2.1.0
    /// </summary>
    public class WebDavPortal210 : WebDavPortal
    {
        public static new object Install(object obj)
        {
            //
            return WebDavPortal.InstallBase(obj, minimalInstallerVersion: "2.0.0");
        }

        public static new object Uninstall(object obj)
        {
            return WebDavPortal.Uninstall(obj);
        }

        public static new object Setup(object obj)
        {
            return WebDavPortal.Setup(obj);
        }

        public static new object Update(object obj)
        {
            return WebDavPortal.UpdateBase(obj,
                minimalInstallerVersion: "2.0.0",
                versionToUpgrade: "2.0.0,2.1.0",
                updateSql: false);
        }
    }

    /// <summary>
    /// Release 2.0.0
    /// </summary>
    public class WebDavPortal200 : WebDavPortal
    {
        public static new object Install(object obj)
        {
            //
            return WebDavPortal.InstallBase(obj, minimalInstallerVersion: "2.0.0");
        }

        public static new object Uninstall(object obj)
        {
            return WebDavPortal.Uninstall(obj);
        }

        public static new object Setup(object obj)
        {
            return WebDavPortal.Setup(obj);
        }

        public static new object Update(object obj)
        {
            return WebDavPortal.UpdateBase(obj,
                minimalInstallerVersion: "2.0.0",
                versionToUpgrade: "1.2.1,2.0.0",
                updateSql: false);
        }
    }
}
