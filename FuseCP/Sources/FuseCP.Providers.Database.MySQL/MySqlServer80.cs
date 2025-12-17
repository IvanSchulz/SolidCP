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
using System.Text.RegularExpressions;
using System.Data;
using Microsoft.Win32;
//using MySql.Data.MySqlClient;
using System.IO;

using FuseCP.Providers.OS;
using System.Reflection;
using MySql.Data.MySqlClient;

namespace FuseCP.Providers.Database
{
    public class MySqlServer80 : MySqlServer57
	{

		public MySqlServer80(): base() {	}

		public override bool IsInstalled() => IsInstalled("8.0");

		public override void CreateUser(SqlUser user, string password)
		{
			if (user.Databases == null)
				user.Databases = new string[0];

			/*if (!((Regex.IsMatch(user.Name, @"[^\w\.-]")) && (user.Name.Length > 16)))
            {
                Exception ex = new Exception("INVALID_USERNAME");
                throw ex;
            }
            */
			ExecuteNonQuery(String.Format(
								"CREATE USER '{0}'@'%' IDENTIFIED BY '{1}'",
								user.Name, password));

			if (OldPassword)
				ChangeUserPassword(user.Name, password);

			// add access to databases
			foreach (string database in user.Databases)
				AddUserToDatabase(database, user.Name);
		}

		private void AddUserToDatabase(string databaseName, string user)
		{
			// grant database access
			ExecuteNonQuery(String.Format("GRANT ALL PRIVILEGES ON {0}.* TO '{1}'@'%'",
					databaseName, user));
		}

		public override void ExecuteSqlNonQuery(string databaseName, string commandText)
		{
			commandText = "USE " + databaseName + ";\n" + commandText;
			ExecuteNonQuery(commandText);
		}

		public override void ChangeUserPassword(string username, string password)
		{
			ExecuteNonQuery(String.Format("ALTER USER '{0}'@'%' IDENTIFIED BY '{1}';",
				username, password));
		}


		private int ExecuteNonQuery(string commandText)
		{
			return ExecuteNonQuery(commandText, ConnectionString);
		}

		private int ExecuteNonQuery(string commandText, string connectionString)
		{
			MySqlConnection conn = new MySqlConnection(connectionString);
			MySqlCommand cmd = new MySqlCommand(commandText, conn);
			conn.Open();
			int ret = cmd.ExecuteNonQuery();
			conn.Close();
			return ret;
		}

	}
}
