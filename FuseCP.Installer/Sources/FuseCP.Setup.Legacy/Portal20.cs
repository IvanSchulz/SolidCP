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
using System.Text;
using System.Windows.Forms;
using FuseCP.Setup.Actions;

namespace FuseCP.Setup
{
    /// <summary>
    /// Release 2.1.0
    /// </summary>
    public class Portal210 : Portal
    {
        public static new object Install(object obj)
        {
            //
            return Portal.InstallBase(obj, minimalInstallerVersion: "2.0.0");
        }

        public static new DialogResult Uninstall(object obj)
        {
            return Portal.Uninstall(obj);
        }

        public static new DialogResult Setup(object obj)
        {
            return Portal.Setup(obj);
        }

        public static new DialogResult Update(object obj)
        {
            return UpdateBase(obj,
                minimalInstallerVersion: "2.0.0",
                versionToUpgrade: "2.0.0,2.1.0",
                updateSql: false);
        }
    }

    /// <summary>
    /// Release 2.0.0
    /// </summary>
    public class Portal200 : Portal
    {
        public static new object Install(object obj)
        {
            //
            return Portal.InstallBase(obj, minimalInstallerVersion: "2.0.0");
        }

        public static new DialogResult Uninstall(object obj)
        {
            return Portal.Uninstall(obj);
        }

        public static new DialogResult Setup(object obj)
        {
            return Portal.Setup(obj);
        }

        public static new DialogResult Update(object obj)
        {
            return UpdateBase(obj,
                minimalInstallerVersion: "2.0.0",
                versionsToUpgrade: "1.2.1,2.0.0",
                updateSql: false,
                versionSpecificAction: new InstallAction(ActionTypes.ConfigureSecureSessionModuleInWebConfig));
        }
    }
}
