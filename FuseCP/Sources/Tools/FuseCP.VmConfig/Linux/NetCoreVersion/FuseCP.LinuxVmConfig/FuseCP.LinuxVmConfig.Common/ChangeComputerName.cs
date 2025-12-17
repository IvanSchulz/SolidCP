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

namespace FuseCP.LinuxVmConfig
{
    public class ChangeComputerName
    {
        internal const string cloudCfg = "/etc/cloud/cloud.cfg";
        internal const string hosts = "/etc/hosts";
        internal const string hostname = "/etc/hostname";
        internal const string rcConf = "/etc/rc.conf";
        internal const string compatLinux = "/compat/linux";

        public static ExecutionResult Run(ref ExecutionContext context)
        {
            ExecutionResult ret = new ExecutionResult();
            ret.ResultCode = 0;
            ret.ErrorMessage = null;
            ret.RebootRequired = true;
            try
            {
                context.ActivityDescription = "Changing computer name...";
                context.Progress = 0;
                if (!context.Parameters.ContainsKey("FullComputerName"))
                {
                    ret.ResultCode = 2;
                    ret.ErrorMessage = "Parameter 'FullComputerName' not found";
                    Log.WriteError(ret.ErrorMessage);
                    context.Progress = 100;
                    return ret;
                }
                string computerName = context.Parameters["FullComputerName"].ToLower();
                string netBiosName = computerName;
                int idx = netBiosName.IndexOf(".");
                if (idx != -1)
                {
                    netBiosName = computerName.Substring(0, idx);
                }
                ExecutionResult res;
                res = ShellHelper.RunCmd("hostname -s");
                string hostNameOld = null;
                if (res.Value != null) hostNameOld = res.Value.Trim();

                res = ShellHelper.RunCmd("hostname");
                string fullNameOld = null;
                if (res.Value != null) fullNameOld = res.Value.Trim();

                string domain = computerName.Replace(netBiosName + ".", "");
                string domainOld = null;
                if (hostNameOld != null && hostNameOld.Length > 0 && fullNameOld != null && fullNameOld.Length > 0)
                {
                    domainOld = fullNameOld.Replace(hostNameOld + ".", "");
                }

                switch (OsVersion.GetOsVersion())
                {
                    case OsVersionEnum.Ubuntu:
                        TxtHelper.ReplaceStr(hostname, computerName, 0);
                        if (domainOld != null && domainOld.Length > 0) TxtHelper.ReplaceStr(hosts, domainOld, domain);
                        if (hostNameOld != null && hostNameOld.Length > 0) TxtHelper.ReplaceStr(hosts, hostNameOld, netBiosName);
                        TxtHelper.ReplaceStr(cloudCfg, "preserve_hostname: false", "preserve_hostname: true");
                        TxtHelper.ReplaceStr(cloudCfg, "# preserve_hostname: true", "preserve_hostname: true");
                        TxtHelper.ReplaceStr(cloudCfg, "#preserve_hostname: true", "preserve_hostname: true");
                        break;
                    case OsVersionEnum.CentOS:
                        TxtHelper.ReplaceStr(hostname, computerName, 0);
                        if (domainOld != null && domainOld.Length > 0) TxtHelper.ReplaceStr(hosts, domainOld, domain);
                        if (hostNameOld != null && hostNameOld.Length > 0) TxtHelper.ReplaceStr(hosts, hostNameOld, netBiosName);
                        break;
                    case OsVersionEnum.FreeBSD:
                        ShellHelper.RunCmd("cp -p " + rcConf + " " + compatLinux + rcConf);
                        ShellHelper.RunCmd("cp -p " + hosts + " " + compatLinux + hosts);
                        TxtHelper.ReplaceStr(compatLinux + rcConf, "hostname=\"" + computerName + "\"", TxtHelper.GetStrPos(compatLinux + rcConf, "hostname", 0, -1));
                        if (domainOld != null && domainOld.Length > 0) TxtHelper.ReplaceStr(compatLinux + hosts, domainOld, domain);
                        if (hostNameOld != null && hostNameOld.Length > 0) TxtHelper.ReplaceStr(compatLinux + hosts, hostNameOld, netBiosName);
                        ShellHelper.RunCmd("cp -p " + compatLinux + rcConf + " " + rcConf);
                        ShellHelper.RunCmd("cp -p " + compatLinux + hosts + " " + hosts);
                        break;
                    default:
                        TxtHelper.ReplaceStr(hostname, computerName, 0);
                        if (domainOld != null && domainOld.Length > 0) TxtHelper.ReplaceStr(hosts, domainOld, domain);
                        if (hostNameOld != null && hostNameOld.Length > 0) TxtHelper.ReplaceStr(hosts, hostNameOld, netBiosName);
                        break;
                }
            }catch(Exception ex)
            {
                ret.ResultCode = 1;
                ret.ErrorMessage = "ChangeComputerName error: " + ex.ToString();
                Log.WriteError(ret.ErrorMessage);
            }
            if (ret.ResultCode == 0)
            {
                Log.WriteInfo("Computer name has been changed successfully");
            }
            context.Progress = 100;
            return ret;
        }
    }
}
