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
    public class Server150 : Server
    {
        public static new object Install(object obj)
        {
            //
            return Server.InstallBase(obj, minimalInstallerVersion: "1.0.1");
        }

        public static new object Uninstall(object obj)
        {
            return Server.Uninstall(obj);
        }

        public static new object Setup(object obj)
        {
            return Server.Setup(obj);
        }

        public static new object Update(object obj)
        {
            return Server.UpdateBase(obj,
                 minimalInstallerVersion: "1.0.1",
                 versionToUpgrade: "1.5.0,1.4.9,1.4.8,1.4.7,1.4.6,1.4.5",
                 updateSql: false);
        }
    }
    /// <summary>
    /// Release 1.4.5
    /// </summary>
    public class Server145 : Server
    {
        public static new object Install(object obj)
        {
            //
            return Server.InstallBase(obj, minimalInstallerVersion: "1.0.1");
        }

        public static new object Uninstall(object obj)
        {
            return Server.Uninstall(obj);
        }

        public static new object Setup(object obj)
        {
            return Server.Setup(obj);
        }

        public static new object Update(object obj)
        {
            return Server.UpdateBase(obj,
                 minimalInstallerVersion: "1.0.1",
                 versionToUpgrade: "1.4.8,1.4.7,1.4.6,1.4.5,1.4.4",
                 updateSql: false);
        }
    }
    /// <summary>
    /// Release 1.4.4
    /// </summary>
    public class Server144 : Server
    {
        public static new object Install(object obj)
        {
            //
            return Server.InstallBase(obj, "1.0.1");
        }

        public static new object Uninstall(object obj)
        {
            return Server.Uninstall(obj);
        }

        public static new object Setup(object obj)
        {
            return Server.Setup(obj);
        }

        public static new object Update(object obj)
        {
            return Server.UpdateBase(obj,
                 minimalInstallerVersion: "1.0.1",
                 versionToUpgrade: "1.4.3,1.4.2",
                 updateSql: false);
        }
    }
    /// <summary>
    /// Release 1.4.3
    /// </summary>
    public class Server143 : Server
    {
        public static new object Install(object obj)
        {
            //
            return Server.InstallBase(obj, "1.0.1");
        }

        public static new object Uninstall(object obj)
        {
            return Server.Uninstall(obj);
        }

        public static new object Setup(object obj)
        {
            return Server.Setup(obj);
        }

        public static new object Update(object obj)
        {
            return Server.UpdateBase(obj,
                 minimalInstallerVersion: "1.0.1",
                 versionToUpgrade: "1.4.2,1.4.1",
                 updateSql: false);
        }
    }
    /// <summary>
    /// Release 1.4.2
    /// </summary>
    public class Server142 : Server
    {
        public static new object Install(object obj)
        {
            //
            return Server.InstallBase(obj, "1.0.1");
        }

        public static new object Uninstall(object obj)
        {
            return Server.Uninstall(obj);
        }

        public static new object Setup(object obj)
        {
            return Server.Setup(obj);
        }

        public static new object Update(object obj)
        {
            return Server.UpdateBase(obj,
                 minimalInstallerVersion: "1.0.1",
                 versionToUpgrade: "1.4.1,1.4.0",
                 updateSql: false);
        }
    }
    /// <summary>
    /// Release 1.4.1
    /// </summary>
    public class Server141 : Server
    {
        public static new object Install(object obj)
        {
            //
            return Server.InstallBase(obj, "1.0.1");
        }

        public static new object Uninstall(object obj)
        {
            return Server.Uninstall(obj);
        }

        public static new object Setup(object obj)
        {
            return Server.Setup(obj);
        }

        public static new object Update(object obj)
        {
            return Server.UpdateBase(obj,
                 minimalInstallerVersion: "1.0.1",
                 versionToUpgrade: "1.4.0,1.3.0",
                 updateSql: false);
        }
    }
    /// <summary>
    /// Release 1.4.0
    /// </summary>
    public class Server140 : Server
    {
        public static new object Install(object obj)
        {
            //
            return Server.InstallBase(obj, "1.0.1");
        }

        public static new object Uninstall(object obj)
        {
            return Server.Uninstall(obj);
        }

        public static new object Setup(object obj)
        {
            return Server.Setup(obj);
        }

        public static new object Update(object obj)
        {
            return Server.UpdateBase(obj,
                 minimalInstallerVersion: "1.0.1",
                 versionToUpgrade: "1.3.0,1.2.1",
                 updateSql: false);
        }
    }
    /// <summary>
    /// Release 1.3.0
    /// </summary>
    public class Server130 : Server
    {
        public static new object Install(object obj)
        {
            //
            return Server.InstallBase(obj, "1.0.1");
        }

        public static new object Uninstall(object obj)
        {
            return Server.Uninstall(obj);
        }

        public static new object Setup(object obj)
        {
            return Server.Setup(obj);
        }

        public static new object Update(object obj)
        {
            return Server.UpdateBase(obj,
                 minimalInstallerVersion: "1.0.1",
                 versionToUpgrade: "1.2.1,1.2.0",
                 updateSql: false);
        }
    }
    /// <summary>
    /// Release 1.2.1
    /// </summary>
    public class Server121 : Server
    {
        public static new object Install(object obj)
        {
            //
            return Server.InstallBase(obj, "1.0.1");
        }

        public static new object Uninstall(object obj)
        {
            return Server.Uninstall(obj);
        }

        public static new object Setup(object obj)
        {
            return Server.Setup(obj);
        }

        public static new object Update(object obj)
        {
            return Server.UpdateBase(obj,
                 minimalInstallerVersion: "1.0.1",
                 versionToUpgrade: "1.2.0,1.1.2",
                 updateSql: false);
        }
    }

    /// <summary>
    /// Release 1.2.0
    /// </summary>
    public class Server120 : Server
    {
        public static new object Install(object obj)
        {
            //
            return Server.InstallBase(obj, "1.0.1");
        }

        public static new object Uninstall(object obj)
        {
            return Server.Uninstall(obj);
        }

        public static new object Setup(object obj)
        {
            return Server.Setup(obj);
        }

        public static new object Update(object obj)
        {
            return Server.UpdateBase(obj,
                 minimalInstallerVersion: "1.0.1",
                 versionToUpgrade: "1.1.4,1.1.2",
                 updateSql: false);
        }
    }

    /// <summary>
    /// Release 1.1.4
    /// </summary>
    public class Server114 : Server
    {
        public static new object Install(object obj)
        {
            return Server.InstallBase(obj, "1.0.1");
        }

        public static new object Uninstall(object obj)
        {
            return Server.Uninstall(obj);
        }

        public static new object Setup(object obj)
        {
            return Server.Setup(obj);
        }

        public static new object Update(object obj)
        {
            return Server.UpdateBase(obj,
                minimalInstallerVersion: "1.0.1",
                versionToUpgrade: "1.1.3,1.1.2",
                updateSql: false);
        }
    }

    /// <summary>
    /// Release 1.1.3
    /// </summary>
    public class Server113 : Server
    {
        public static new object Install(object obj)
        {
            return Server.InstallBase(obj, "1.0.1");
        }

        public static new object Uninstall(object obj)
        {
            return Server.Uninstall(obj);
        }

        public static new object Setup(object obj)
        {
            return Server.Setup(obj);
        }

        public static new object Update(object obj)
        {
            return Server.UpdateBase(obj,
                minimalInstallerVersion: "1.0.1",
                versionToUpgrade: "1.1.2,1.1.1",
                updateSql: false);
        }
    }

    /// <summary>
    /// Release 1.1.2
    /// </summary>
    public class Server112 : Server
    {
        public static new object Install(object obj)
        {
            return Server.InstallBase(obj, "1.0.1");
        }

        public static new object Uninstall(object obj)
        {
            return Server.Uninstall(obj);
        }

        public static new object Setup(object obj)
        {
            return Server.Setup(obj);
        }

        public static new object Update(object obj)
        {
            return Server.UpdateBase(obj,
                minimalInstallerVersion: "1.0.1",
                versionToUpgrade: "1.1.1,1.1.0",
                updateSql: false);
        }
    }

    /// <summary>
    /// Release 1.1.1
    /// </summary>
    public class Server111 : Server
    {
        public static new object Install(object obj)
        {
            return Server.InstallBase(obj, "1.0.1");
        }

        public static new object Uninstall(object obj)
        {
            return Server.Uninstall(obj);
        }

        public static new object Setup(object obj)
        {
            return Server.Setup(obj);
        }

        public static new object Update(object obj)
        {
            return Server.UpdateBase(obj,
                minimalInstallerVersion: "1.0.1",
                versionToUpgrade: "1.1.0,1.0.4",
                updateSql: false);
        }
    }

    /// <summary>
    /// Release 1.1.0
    /// </summary>
    public class Server110 : Server
    {
        public static new object Install(object obj)
        {
            return Server.InstallBase(obj, "1.0.1");
        }

        public static new object Uninstall(object obj)
        {
            return Server.Uninstall(obj);
        }

        public static new object Setup(object obj)
        {
            return Server.Setup(obj);
        }

        public static new object Update(object obj)
        {
            return Server.UpdateBase(obj,
                minimalInstallerVersion: "1.0.1",
                versionToUpgrade: "1.0.4,1.0.3",
                updateSql: false);
        }
    }

    /// Release 1.0.4
    /// </summary>
    public class Server104 : Server
    {
        public static new object Install(object obj)
        {
            return Server.InstallBase(obj, minimalInstallerVersion: "1.0.1");
        }

        public static new object Uninstall(object obj)
        {
            return Server.Uninstall(obj);
        }

        public static new object Setup(object obj)
        {
            return Server.Setup(obj);
        }

        public static new object Update(object obj)
        {
            return Server.UpdateBase(obj,
                minimalInstallerVersion: "1.0.1",
                versionToUpgrade: "1.0.3,1.0.1",
                updateSql: false);
        }
    }

    /// Release 1.0.3
    /// </summary>
    public class Server103 : Server
    {
        public static new object Install(object obj)
        {
            return Server.InstallBase(obj, minimalInstallerVersion: "1.0.1");
        }

        public static new object Uninstall(object obj)
        {
            return Server.Uninstall(obj);
        }

        public static new object Setup(object obj)
        {
            return Server.Setup(obj);
        }

        public static new object Update(object obj)
        {
            return Server.UpdateBase(obj,
                minimalInstallerVersion: "1.0.1",
                versionToUpgrade: "1.0.2,1.0.1",
                updateSql: false);
        }
    }

    /// Release 1.0.2
    /// </summary>
    public class Server102 : Server
    {
        public static new object Install(object obj)
        {
            return Server.InstallBase(obj, minimalInstallerVersion: "1.0.1");
        }

        public static new object Uninstall(object obj)
        {
            return Server.Uninstall(obj);
        }

        public static new object Setup(object obj)
        {
            return Server.Setup(obj);
        }

        public static new object Update(object obj)
        {
            return UpdateBase(obj, "1.0.1", "1.0.0", false);
        }
    }

    /// <summary>
    /// Release 1.0.1
    /// </summary>
    public class Server101 : Server
    {
        public static new object Install(object obj)
        {
            return Server.InstallBase(obj, minimalInstallerVersion: "1.0.1");
        }

        public static new object Uninstall(object obj)
        {
            return Server.Uninstall(obj);
        }

        public static new object Setup(object obj)
        {
            return Server.Setup(obj);
        }

        public static new object Update(object obj)
        {
            return UpdateBase(obj, "1.0.0", "1.0", false);
        }
    }

    /// <summary>
    /// Release 1.0
    /// </summary>
    public class Server10 : Server
    {
        public static new object Install(object obj)
        {
            return Server.InstallBase(obj, "1.0.0");
        }

        public static new object Uninstall(object obj)
        {
            return Server.Uninstall(obj);
        }

        public static new object Setup(object obj)
        {
            return Server.Setup(obj);
        }
    }
}
