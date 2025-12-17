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

﻿using System;
using System.Collections.Generic;
using System.Data;

namespace FuseCP.Providers.Database
{
    public class MsSqlServer2019 : MsSqlServer2017
    {
        public override bool IsInstalled()
        {
			return CheckVersion("15.");
		}

		public override string[] GetUsers()
        {
            DataTable dt = ExecuteQuery("select name from sys.sql_logins where name not like '##MS%' and IS_SRVROLEMEMBER ('sysadmin',name) = 0").Tables[0];
            List<string> users = new List<string>();
            foreach (DataRow dr in dt.Rows)
                users.Add(dr["name"].ToString());
            return users.ToArray();
        }
    }
}
