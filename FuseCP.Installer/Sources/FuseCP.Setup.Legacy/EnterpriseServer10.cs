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
    /// Release 1.5.0
    /// </summary>
    public class EnterpriseServer150 : EnterpriseServer
    {
        public static new object Install(object obj)
        {
            //
            return EnterpriseServer.InstallBase(obj, minimalInstallerVersion: "1.0.1");
        }

        public static new DialogResult Uninstall(object obj)
        {
            return EnterpriseServer.Uninstall(obj);
        }

        public static new DialogResult Setup(object obj)
        {
            return EnterpriseServer.Setup(obj);
        }

        public static new DialogResult Update(object obj)
        {
            return UpdateBase(obj,
                 minimalInstallerVersion: "1.0.1",
                 versionToUpgrade: "1.5.0,1.4.9,1.4.8,1.4.7,1.4.6,1.4.5",
                 updateSql: true);
        }
    }
    /// <summary>
    /// Release 1.4.5
    /// </summary>
    public class EnterpriseServer145 : EnterpriseServer
    {
        public static new object Install(object obj)
        {
            //
            return EnterpriseServer.InstallBase(obj, minimalInstallerVersion: "1.0.1");
        }

        public static new DialogResult Uninstall(object obj)
        {
            return EnterpriseServer.Uninstall(obj);
        }

        public static new DialogResult Setup(object obj)
        {
            return EnterpriseServer.Setup(obj);
        }

        public static new DialogResult Update(object obj)
        {
            return UpdateBase(obj,
                 minimalInstallerVersion: "1.0.1",
                 versionToUpgrade: "1.4.8,1.4.7,1.4.6,1.4.5,1.4.4",
                 updateSql: true);
        }
    }
    /// <summary>
    /// Release 1.4.4
    /// </summary>
    public class EnterpriseServer144 : EnterpriseServer
    {
        public static new object Install(object obj)
        {
            //
            return EnterpriseServer.InstallBase(obj, "1.0.1");
        }

        public static new DialogResult Uninstall(object obj)
        {
            return EnterpriseServer.Uninstall(obj);
        }

        public static new DialogResult Setup(object obj)
        {
            return EnterpriseServer.Setup(obj);
        }

        public static new DialogResult Update(object obj)
        {
            return UpdateBase(obj,
                 minimalInstallerVersion: "1.0.1",
                 versionToUpgrade: "1.4.3,1.4.2",
                 updateSql: true);
        }
    }
    /// <summary>
    /// Release 1.4.3
    /// </summary>
    public class EnterpriseServer143 : EnterpriseServer
    {
        public static new object Install(object obj)
        {
            //
            return EnterpriseServer.InstallBase(obj, "1.0.1");
        }

        public static new DialogResult Uninstall(object obj)
        {
            return EnterpriseServer.Uninstall(obj);
        }

        public static new DialogResult Setup(object obj)
        {
            return EnterpriseServer.Setup(obj);
        }

        public static new DialogResult Update(object obj)
        {
            return UpdateBase(obj,
                 minimalInstallerVersion: "1.0.1",
                 versionToUpgrade: "1.4.2,1.4.1",
                 updateSql: true);
        }
    }
    /// <summary>
    /// Release 1.4.2
    /// </summary>
    public class EnterpriseServer142 : EnterpriseServer
    {
        public static new object Install(object obj)
        {
            //
            return EnterpriseServer.InstallBase(obj, "1.0.1");
        }

        public static new DialogResult Uninstall(object obj)
        {
            return EnterpriseServer.Uninstall(obj);
        }

        public static new DialogResult Setup(object obj)
        {
            return EnterpriseServer.Setup(obj);
        }

        public static new DialogResult Update(object obj)
        {
            return UpdateBase(obj,
                 minimalInstallerVersion: "1.0.1",
                 versionToUpgrade: "1.4.1,1.4.0",
                 updateSql: true);
        }
    }
    /// <summary>
    /// Release 1.4.1
    /// </summary>
    public class EnterpriseServer141 : EnterpriseServer
    {
        public static new object Install(object obj)
        {
            //
            return EnterpriseServer.InstallBase(obj, "1.0.1");
        }

        public static new DialogResult Uninstall(object obj)
        {
            return EnterpriseServer.Uninstall(obj);
        }

        public static new DialogResult Setup(object obj)
        {
            return EnterpriseServer.Setup(obj);
        }

        public static new DialogResult Update(object obj)
        {
            return UpdateBase(obj,
                 minimalInstallerVersion: "1.0.1",
                 versionToUpgrade: "1.4.0,1.3.0",
                 updateSql: true);
        }
    }
    /// <summary>
    /// Release 1.4.0
    /// </summary>
    public class EnterpriseServer140 : EnterpriseServer
    {
        public static new object Install(object obj)
        {
            //
            return EnterpriseServer.InstallBase(obj, "1.0.1");
        }

        public static new DialogResult Uninstall(object obj)
        {
            return EnterpriseServer.Uninstall(obj);
        }

        public static new DialogResult Setup(object obj)
        {
            return EnterpriseServer.Setup(obj);
        }

        public static new DialogResult Update(object obj)
        {
            return UpdateBase(obj,
                 minimalInstallerVersion: "1.0.1",
                 versionToUpgrade: "1.3.0,1.2.1",
                 updateSql: true);
        }
    }
    /// <summary>
    /// Release 1.3.0
    /// </summary>
    public class EnterpriseServer130 : EnterpriseServer
    {
        public static new object Install(object obj)
        {
            //
            return EnterpriseServer.InstallBase(obj, "1.0.1");
        }

        public static new DialogResult Uninstall(object obj)
        {
            return EnterpriseServer.Uninstall(obj);
        }

        public static new DialogResult Setup(object obj)
        {
            return EnterpriseServer.Setup(obj);
        }

        public static new DialogResult Update(object obj)
        {
            return UpdateBase(obj,
                 minimalInstallerVersion: "1.0.1",
                 versionToUpgrade: "1.2.1,1.2.0",
                 updateSql: true);
        }
    }

    /// <summary>
    /// Release 1.2.1
    /// </summary>
    public class EnterpriseServer121 : EnterpriseServer
    {
        public static new object Install(object obj)
        {
            //
            return EnterpriseServer.InstallBase(obj, "1.2.1");
        }

        public static new DialogResult Uninstall(object obj)
        {
            return EnterpriseServer.Uninstall(obj);
        }

        public static new DialogResult Setup(object obj)
        {
            return EnterpriseServer.Setup(obj);
        }

        public static new DialogResult Update(object obj)
        {
            return UpdateBase(obj,
                 minimalInstallerVersion: "1.0.1",
                 versionToUpgrade: "1.2.0,1.1.3",
                 updateSql: true);
        }
    }

    /// <summary>
    /// Release 1.2.0
    /// </summary>
    public class EnterpriseServer120 : EnterpriseServer
    {
        public static new object Install(object obj)
        {
            //
            return EnterpriseServer.InstallBase(obj, "1.1.0");
        }

        public static new DialogResult Uninstall(object obj)
        {
            return EnterpriseServer.Uninstall(obj);
        }

        public static new DialogResult Setup(object obj)
        {
            return EnterpriseServer.Setup(obj);
        }

        public static new DialogResult Update(object obj)
        {
            return UpdateBase(obj,
                  minimalInstallerVersion: "1.0.1",
                  versionToUpgrade: "1.1.4,1.1.2",
                  updateSql: true);
        }
    }

    /// <summary>
    /// Release 1.1.4
    /// </summary>
    public class EnterpriseServer114 : EnterpriseServer
    {
        public static new object Install(object obj)
        {
            return EnterpriseServer.InstallBase(obj, "1.0.1");
        }

        public static new DialogResult Uninstall(object obj)
        {
            return EnterpriseServer.Uninstall(obj);
        }

        public static new DialogResult Setup(object obj)
        {
            return EnterpriseServer.Setup(obj);
        }

        public static new DialogResult Update(object obj)
        {
            return UpdateBase(obj,
                  minimalInstallerVersion: "1.0.1",
                  versionToUpgrade: "1.1.3,1.1.2",
                  updateSql: true);
        }
    }

    /// <summary>
    /// Release 1.1.3
    /// </summary>
    public class EnterpriseServer113 : EnterpriseServer
    {
        public static new object Install(object obj)
        {
            return EnterpriseServer.InstallBase(obj, "1.0.1");
        }

        public static new DialogResult Uninstall(object obj)
        {
            return EnterpriseServer.Uninstall(obj);
        }

        public static new DialogResult Setup(object obj)
        {
            return EnterpriseServer.Setup(obj);
        }

        public static new DialogResult Update(object obj)
        {
            return UpdateBase(obj,
                  minimalInstallerVersion: "1.0.1",
                  versionToUpgrade: "1.1.2,1.1.1",
                  updateSql: true);
        }
    }

    /// <summary>
    /// Release 1.1.2
    /// </summary>
    public class EnterpriseServer112 : EnterpriseServer
    {
        public static new object Install(object obj)
        {
            return EnterpriseServer.InstallBase(obj, "1.0.1");
        }

        public static new DialogResult Uninstall(object obj)
        {
            return EnterpriseServer.Uninstall(obj);
        }

        public static new DialogResult Setup(object obj)
        {
            return EnterpriseServer.Setup(obj);
        }

        public static new DialogResult Update(object obj)
        {
            return UpdateBase(obj,
                 minimalInstallerVersion: "1.0.1",
                 versionToUpgrade: "1.1.1,1.1.0",
                 updateSql: true);
        }
    }

    /// <summary>
    /// Release 1.1.1
    /// </summary>
    public class EnterpriseServer111 : EnterpriseServer
    {
        public static new object Install(object obj)
        {
            return EnterpriseServer.InstallBase(obj, "1.0.1");
        }

        public static new DialogResult Uninstall(object obj)
        {
            return EnterpriseServer.Uninstall(obj);
        }

        public static new DialogResult Setup(object obj)
        {
            return EnterpriseServer.Setup(obj);
        }

        public static new DialogResult Update(object obj)
        {
            return UpdateBase(obj,
                 minimalInstallerVersion: "1.0.1",
                 versionToUpgrade: "1.1.0,1.0.4",
                 updateSql: true);
        }
    }

    /// <summary>
    /// Release 1.1.0
    /// </summary>
    public class EnterpriseServer110 : EnterpriseServer
    {
        public static new object Install(object obj)
        {
            return EnterpriseServer.InstallBase(obj, "1.0.1");
        }

        public static new DialogResult Uninstall(object obj)
        {
            return EnterpriseServer.Uninstall(obj);
        }

        public static new DialogResult Setup(object obj)
        {
            return EnterpriseServer.Setup(obj);
        }

        public static new DialogResult Update(object obj)
        {
            return UpdateBase(obj,
                minimalInstallerVersion: "1.0.1",
                versionToUpgrade: "1.0.4,1.0.3",
                updateSql: true);
        }
    }

    /// Release 1.0.4
    /// </summary>
    public class EnterpriseServer104 : EnterpriseServer
    {
        public static new object Install(object obj)
        {
            return EnterpriseServer.InstallBase(obj, minimalInstallerVersion: "1.0.1");
        }

        public static new DialogResult Uninstall(object obj)
        {
            return EnterpriseServer.Uninstall(obj);
        }

        public static new DialogResult Setup(object obj)
        {
            return EnterpriseServer.Setup(obj);
        }

        public static new DialogResult Update(object obj)
        {
            return UpdateBase(obj,
                minimalInstallerVersion: "1.0.1",
                versionToUpgrade: "1.0.3,1.0.1",
                updateSql: true);
        }
    }

    /// Release 1.0.3
    /// </summary>
    public class EnterpriseServer103 : EnterpriseServer
    {
        public static new object Install(object obj)
        {
            return EnterpriseServer.InstallBase(obj, minimalInstallerVersion: "1.0.1");
        }

        public static new DialogResult Uninstall(object obj)
        {
            return EnterpriseServer.Uninstall(obj);
        }

        public static new DialogResult Setup(object obj)
        {
            return EnterpriseServer.Setup(obj);
        }

        public static new DialogResult Update(object obj)
        {
            return UpdateBase(obj,
                minimalInstallerVersion: "1.0.1",
                versionToUpgrade: "1.0.2,1.0.1",
                updateSql: true);
        }
    }

    /// Release 1.0.2
    /// </summary>
    public class EnterpriseServer102 : EnterpriseServer
    {
        public static new object Install(object obj)
        {
            return EnterpriseServer.InstallBase(obj, minimalInstallerVersion: "1.0.1");
        }

        public static new DialogResult Uninstall(object obj)
        {
            return EnterpriseServer.Uninstall(obj);
        }

        public static new DialogResult Setup(object obj)
        {
            return EnterpriseServer.Setup(obj);
        }

        public static new DialogResult Update(object obj)
        {
            return UpdateBase(obj,
                minimalInstallerVersion: "1.0.1",
                versionToUpgrade: "1.0.1,1.0.0",
                updateSql: true);
        }
    }

    /// <summary>
    /// Release 1.0.1
    /// </summary>
    public class EnterpriseServer101 : EnterpriseServer
    {
        public static new object Install(object obj)
        {
            return EnterpriseServer10.InstallBase(obj, "1.0.0");
        }

        public static new DialogResult Uninstall(object obj)
        {
            return EnterpriseServer.Uninstall(obj);
        }

        public static new DialogResult Setup(object obj)
        {
            return EnterpriseServer.Setup(obj);
        }

        public static new DialogResult Update(object obj)
        {
            return UpdateBase(obj, "1.0.0", "1.0", true);
        }
    }

    /// <summary>
    /// Release 1.0
    /// </summary>
    public class EnterpriseServer10 : EnterpriseServer
    {
        public static new object Install(object obj)
        {
            return EnterpriseServer.InstallBase(obj, "1.0.0");
        }

        public static new DialogResult Uninstall(object obj)
        {
            return EnterpriseServer.Uninstall(obj);
        }

        public static new DialogResult Setup(object obj)
        {
            return EnterpriseServer.Setup(obj);
        }
    }
}
