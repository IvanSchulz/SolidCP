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
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.Net.Mail;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using FuseCP.EnterpriseServer.Data;

using FuseCP.EnterpriseServer;


namespace FuseCP.EnterpriseServer
{
    public class UserHelper: ControllerBase
    {
        public UserHelper(ControllerBase provider): base(provider) { }

        public UserInfoInternal GetUser(string username)
        {
            return GetUser(Database.GetUserByUsernameInternally(username));
        }

        public UserInfoInternal GetUser(int userId)
        {
            return GetUser(Database.GetUserById(SecurityContext.User.UserId, userId));
        }

        private UserInfoInternal GetUser(IDataReader reader)
        {
            // try to get user from database
            UserInfoInternal user = ObjectUtils.FillObjectFromDataReader<UserInfoInternal>(reader);

            if (user != null)
            {
                user.Password = CryptoUtils.Decrypt(user.Password);
            }
            return user;
        }

    }
}
