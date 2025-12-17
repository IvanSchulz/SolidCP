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
using System.Runtime.InteropServices;

using System.Collections.Generic;
using System.Text;


namespace FuseCP.VmConfig
{
	class ChangeComputerNameModule : IProvisioningModule
	{
		//P/Invoke signature
		[DllImport("Kernel32.dll", SetLastError = true, CharSet = CharSet.Auto)]
		[return: MarshalAs(UnmanagedType.Bool)]
		private static extern bool SetComputerNameEx(COMPUTER_NAME_FORMAT nameType, [MarshalAs(UnmanagedType.LPTStr)] string lpBuffer);

		private enum COMPUTER_NAME_FORMAT : int
		{
			ComputerNameNetBIOS,
			ComputerNameDnsHostname,
			ComputerNameDnsDomain,
			ComputerNameDnsFullyQualified,
			ComputerNamePhysicalNetBIOS,
			ComputerNamePhysicalDnsHostname,
			ComputerNamePhysicalDnsDomain,
			ComputerNamePhysicalDnsFullyQualified,
			ComputerNameMax
		}

		#region IProvisioningModule Members

		public ExecutionResult Run(ref ExecutionContext context)
		{
			ExecutionResult ret = new ExecutionResult();
			ret.ResultCode = 0;
			ret.ErrorMessage = null;
			ret.RebootRequired = true;

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
			// Call SetComputerEx
            string computerName = context.Parameters["FullComputerName"];
            string netBiosName = computerName;
            string primaryDnsSuffix = "";
            int idx = netBiosName.IndexOf(".");
            if (idx != -1)
            {
                netBiosName = computerName.Substring(0, idx);
                primaryDnsSuffix = computerName.Substring(idx + 1);
            }

			try
			{
                // set NetBIOS name
				bool res = SetComputerNameEx(COMPUTER_NAME_FORMAT.ComputerNamePhysicalDnsHostname, netBiosName);
				if (!res)
				{
					ret.ResultCode = 1;
					ret.ErrorMessage = "Unexpected error";
					Log.WriteError(ret.ErrorMessage);
				}

                // set primary DNS suffix
                res = SetComputerNameEx(COMPUTER_NAME_FORMAT.ComputerNamePhysicalDnsDomain, primaryDnsSuffix);
                if (!res)
                {
                    ret.ResultCode = 1;
                    ret.ErrorMessage = "Unexpected error";
                    Log.WriteError(ret.ErrorMessage);
                }
			}
			catch (Exception ex)
			{
				ret.ResultCode = 1;
				ret.ErrorMessage = ex.ToString();
				Log.WriteError(ret.ErrorMessage);
			}
			if (ret.ResultCode == 0)
			{
				Log.WriteInfo("Computer name has been changed successfully");
			}
			context.Progress = 100;
			return ret;
		}
		#endregion
		
	}
}
