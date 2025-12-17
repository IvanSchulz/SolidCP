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
using System.Diagnostics;

namespace FuseCP.LinuxVmConfig
{
    public class ShellHelper
    {
        internal const string Shell_Linux = "/bin/bash";
        private static readonly string Shell_FreeBSD = AppDomain.CurrentDomain.BaseDirectory + "sh";//use external shell to out from Linux-emulator

        public static ExecutionResult RunCmd(string cmd)
        {
            ExecutionResult ret = new ExecutionResult();
            ret.ResultCode = 0;
            ret.ErrorMessage = null;
            string shellPath = null;
            try
            {
                if (OsVersion.GetOsVersion() == OsVersionEnum.FreeBSD)
                {
                    shellPath = Shell_FreeBSD;
                }
                else
                {
                    shellPath = Shell_Linux;
                }
                string escapedArgs = cmd.Replace("\"", "\\\"");
                using (Process process = new Process())
                {
                    process.StartInfo = new ProcessStartInfo
                    {
                        FileName = shellPath,
                        Arguments = $"-c \"{escapedArgs}\"",
                        RedirectStandardOutput = true,
                        RedirectStandardError = true,
                        UseShellExecute = false,
                        CreateNoWindow = true,
                    };
                    process.Start();
                    ret.Value = process.StandardOutput.ReadToEnd();
                    ret.ErrorMessage = process.StandardError.ReadToEnd();
                    process.WaitForExit();
                }
                return ret;
            }
            catch (Exception ex)
            {
                ret.ResultCode = 1;
                ret.ErrorMessage = "ShellHelper error: " + ex.ToString();
                return ret;
            }
        }

        public static ExecutionResult ChangeUserPassword(string userName, string password)
        {
            ExecutionResult ret = new ExecutionResult();
            ret.ResultCode = 0;
            ret.ErrorMessage = null;

            try
            {
                string shellPath = null;
                string arguments = null;
                if (OsVersion.GetOsVersion() == OsVersionEnum.FreeBSD)
                {
                    shellPath = Shell_FreeBSD;
                    arguments = $"-c \"echo \"{password}\" | pw usermod {userName} -h 0\"";
                }
                else
                {
                    shellPath = Shell_Linux;
                    arguments = $"-c \"passwd {userName}\"";
                }
                using (Process process = new Process())
                {
                    process.StartInfo = new ProcessStartInfo
                    {
                        FileName = shellPath,
                        Arguments = arguments,
                        RedirectStandardOutput = true,
                        RedirectStandardError = true,
                        RedirectStandardInput = true,
                        UseShellExecute = false,
                        CreateNoWindow = true,
                    };
                    process.Start();
                    if (OsVersion.GetOsVersion() != OsVersionEnum.FreeBSD)//Input redirect dont work on FreeBSD
                    {
                        System.Threading.Thread.Sleep(1000);
                        process.StandardInput.WriteLine(password);
                        System.Threading.Thread.Sleep(1000);
                        process.StandardInput.WriteLine(password);
                    }
                    process.WaitForExit();
                }
                return ret;
            }
            catch (Exception ex)
            {
                ret.ResultCode = 1;
                ret.ErrorMessage = "ShellHelper error: " + ex.ToString();
                return ret;
            }
        }
    }
}
