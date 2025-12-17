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
using System.Data;
using System.Web;
using System.Collections;
using System.Collections.Generic;
using FuseCP.Web.Services;
using System.ComponentModel;

namespace FuseCP.EnterpriseServer
{
    /// <summary>
    /// Summary description for esApplicationsInstaller
    /// </summary>
    [WebService(Namespace = "http://smbsaas/fusecp/enterpriseserver")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [Policy("CommonPolicy")]
    [ToolboxItem(false)]
    public class esAuthentication: WebService
    {
        [WebMethod]
        public int AuthenticateUser(string username, string password, string ip)
        {
            return UserController.AuthenticateUser(username, password, ip);
        }

        [WebMethod]
        public UserInfo GetUserByUsernamePassword(string username, string password, string ip)
        {
            return UserController.GetUserByUsernamePassword(username, password, ip);
        }

        [WebMethod]
        public int ChangeUserPasswordByUsername(string username, string oldPassword, string newPassword, string ip)
        {
            return UserController.ChangeUserPassword(username, oldPassword, newPassword, ip);
        }

        [WebMethod]
        public int SendPasswordReminder(string username, string ip)
        {
            return UserController.SendPasswordReminder(username, ip);
        }

		[WebMethod]
		public bool GetSystemSetupMode()
		{
			return SystemController.GetSystemSetupMode();
		}

		[WebMethod]
		public int SetupControlPanelAccounts(string passwordA, string passwordB, string ip)
		{
			return SystemController.SetupControlPanelAccounts(passwordA, passwordB, ip);
		}

        [WebMethod]
        public DataSet GetLoginThemes()
        {
            return SystemController.GetThemes();
        }

        [WebMethod]
        public bool ValidatePin(string username, string pin)
        {
            return UserController.ValidatePin(username, pin, TimeSpan.FromSeconds(120));
        }

        [WebMethod]
        public int SendPin(string username)
        {
            return UserController.SendVerificationCode(username);
        }
    }
}
