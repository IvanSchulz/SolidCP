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
using System.DirectoryServices;
using System.DirectoryServices.ActiveDirectory;
using System.Collections.Generic;
using System.Text;
using System.Management;

namespace FuseCP.VmConfig
{
	class ChangeAdministratorPasswordModule : IProvisioningModule
	{
		#region IProvisioningModule Members

		public ExecutionResult Run(ref ExecutionContext context)
		{
			ExecutionResult ret = new ExecutionResult();
			ret.ResultCode = 0;
			ret.ErrorMessage = null;
			ret.RebootRequired = false;
			
			context.ActivityDescription = "Changing password for built-in local Administrator account...";
			context.Progress = 0;
			if (!context.Parameters.ContainsKey("Password"))
			{
				ret.ResultCode = 2;
				ret.ErrorMessage = "Parameter 'Password' not found";
				Log.WriteError(ret.ErrorMessage);
				context.Progress = 100;
				return ret;
			}
			string password = context.Parameters["Password"];
			if (string.IsNullOrEmpty(password))
			{
				ret.ResultCode = 2;
				ret.ErrorMessage = "Password is null or empty";
				Log.WriteError(ret.ErrorMessage);
				context.Progress = 100;
				return ret;
			}
			try
			{
                string userPath = string.Format("WinNT://{0}/{1}", System.Environment.MachineName, GetAdministratorName());
				DirectoryEntry userEntry = new DirectoryEntry(userPath);
				userEntry.Invoke("SetPassword", new object[] { password });
				userEntry.CommitChanges();
				userEntry.Close();
			}
			catch (Exception ex)
			{
				if (IsPasswordPolicyException(ex))
				{
					ret.ResultCode = 1;
					ret.ErrorMessage = "The password does not meet the password policy requirements. Check the minimum password length, password complexity and password history requirements.";
				}
				else
				{
					ret.ResultCode = 2;
					ret.ErrorMessage = ex.ToString();
				}
				Log.WriteError(ret.ErrorMessage);
			}
			if (ret.ResultCode == 0)
			{
				Log.WriteInfo("Password has been changed successfully");
			}
			context.Progress = 100;
			return ret;
		}

		#endregion


		private bool IsPasswordPolicyException(Exception ex)
		{
			//0x800708C5
			if (ex is System.Runtime.InteropServices.COMException &&
				((System.Runtime.InteropServices.COMException)ex).ErrorCode == -2147022651)
			{
				return true;
			}
			if (ex.InnerException != null)
				return IsPasswordPolicyException(ex.InnerException);
			else
				return false;
		}

        private string GetAdministratorName()
        {
            WmiUtils wmi = new WmiUtils("root\\cimv2");
            ManagementObjectCollection objUsers = wmi.ExecuteQuery("Select * From Win32_UserAccount Where LocalAccount = TRUE");

            foreach (ManagementObject objUser in objUsers)
            {
                string sid = (string)objUser["SID"];
                if (sid != null && sid.StartsWith("S-1-5-") && sid.EndsWith("-500"))
                    return (string)objUser["Name"];
            }

            return null;
        }
	}
}
