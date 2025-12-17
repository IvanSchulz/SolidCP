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

namespace FuseCP.EnterpriseServer
{
    public class OneTimePasswordHelper: ControllerBase
    {
        public OneTimePasswordHelper(ControllerBase provider) : base(provider) { }

        public string SetOneTimePassword(int userId)
        {
            int passwordLength = 12; // default length

            // load password policy
            UserSettings userSettings = UserController.GetUserSettings(userId, UserSettings.FuseCP_POLICY);
            string passwordPolicy = userSettings["PasswordPolicy"];

            if (!String.IsNullOrEmpty(passwordPolicy))
            {
                // get third parameter - max length
                try
                {
                    passwordLength = Utils.ParseInt(passwordPolicy.Split(';')[2].Trim(), passwordLength);
                }
                catch { /* skip */ }
            }

            // generate password
            var password = Utils.GetRandomString(passwordLength);

            Database.SetUserOneTimePassword(userId, CryptoUtils.Encrypt(password), (int) OneTimePasswordStates.Active);

            return password;
        }

        public void FireSuccessAuth(UserInfoInternal user)
        {
            Database.SetUserOneTimePassword(user.UserId, CryptoUtils.Encrypt(user.Password), (int) OneTimePasswordStates.Expired);
        }
    }
}
