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

using System.IO;

namespace FuseCP.LinuxVmConfig
{
    public enum OsVersionEnum
    {
        NA,
        Linux_NA,
        Ubuntu,
        CentOS,
        FreeBSD
    }

    public static class OsVersion
    {
        private static OsVersionEnum osVersion = OsVersionEnum.NA;
        public static OsVersionEnum GetOsVersion()
        {
            if (osVersion != OsVersionEnum.NA) return osVersion;

            if (File.Exists("/bin/freebsd-version"))
            {
                osVersion = OsVersionEnum.FreeBSD;
            }
            else
            {
                osVersion = OsVersionEnum.Linux_NA;
            }

            if (osVersion == OsVersionEnum.Linux_NA)
            {
                ExecutionResult res = ShellHelper.RunCmd("cat /etc/os-release | grep \"ID=\"");
                if (res.ResultCode != 1) osVersion = FindOsVersion(res.Value);
            }
            return osVersion;
        }

        private static OsVersionEnum FindOsVersion(string result)
        {
            if (result.ToLower().Contains("freebsd"))
            {
                return OsVersionEnum.FreeBSD;
            }
            else if (result.ToLower().Contains("ubuntu"))
            {
                return OsVersionEnum.Ubuntu;
            }
            else if (result.ToLower().Contains("centos"))
            {
                return OsVersionEnum.CentOS;
            }
            else if (result.ToLower().Contains("linux"))
            {
                return OsVersionEnum.Linux_NA;
            }
            return osVersion;
        }
    }
}
