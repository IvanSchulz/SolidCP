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
using MySql.Data.MySqlClient;
using System.IO;

using FuseCP.Server.Utils;
using FuseCP.Providers.Utils;
using FuseCP.Providers;
using System.Reflection;
using System.Data.Common;

using FuseCP.Providers.Database;

namespace FuseCP.Providers.Database
{
    public class MariaDB103 : MariaDB102
    {

        public MariaDB103(): base() { }

        public override bool IsInstalled() => IsInstalled("10.3");

        public override long CalculateDatabaseSize(string database)
        {
            DataTable dt = ExecuteQuery(string.Format("SELECT SUM(data_length + index_length) AS 'Size' FROM information_schema.TABLES WHERE TABLE_SCHEMA = '{0}'", database));
            string dbsize = dt.Rows[0]["Size"].ToString();
            if (String.IsNullOrEmpty(dbsize)) // empty database
            {
                dbsize = "0";
            }
            return Convert.ToInt64(dbsize);
        }

        #region private helper methods

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

        private DataTable ExecuteQuery(string commandText, string connectionString)
        {
            return ExecuteQueryDataSet(commandText, connectionString).Tables[0];
        }

        private DataTable ExecuteQuery(string commandText)
        {
            return ExecuteQueryDataSet(commandText).Tables[0];
        }

        private DataSet ExecuteQueryDataSet(string commandText)
        {
            return ExecuteQueryDataSet(commandText, ConnectionString);
        }

        private DataSet ExecuteQueryDataSet(string commandText, string connectionString)
        {
            MySqlConnection conn = new MySqlConnection(connectionString);
            MySqlDataAdapter adapter = new MySqlDataAdapter(commandText, conn);
            DataSet ds = new DataSet();
            adapter.Fill(ds);
            return ds;
        }

        private string[] GetDatabaseUsers(string databaseName)
        {
            DataTable dtResult = ExecuteQuery(String.Format("SELECT User FROM db WHERE Db='{0}' AND Host='%' AND " +
                "Select_priv = 'Y' AND " +
                "Insert_priv = 'Y' AND " +
                "Update_priv = 'Y' AND  " +
                "Delete_priv = 'Y' AND  " +
                "Index_priv = 'Y' AND  " +
                "Alter_priv = 'Y' AND  " +
                "Create_priv = 'Y' AND  " +
                "Drop_priv = 'Y' AND  " +
                "Create_tmp_table_priv = 'Y' AND  " +
                "Lock_tables_priv = 'Y'", databaseName.ToLower()));
            //
            List<string> users = new List<string>();
            //
            if (dtResult != null)
            {
                if (dtResult.DefaultView != null)
                {
                    DataView dvUsers = dtResult.DefaultView;
                    //
                    foreach (DataRowView drUser in dvUsers)
                    {
                        if (!Convert.IsDBNull(drUser["user"]))
                        {
                            users.Add(Convert.ToString(drUser["user"]));
                        }
                    }
                }
            }
            //
            return users.ToArray();
        }

        private string[] GetUserDatabases(string username)
        {
            DataTable dtResult = ExecuteQuery(String.Format("SELECT Db FROM db WHERE LOWER(User)='{0}' AND Host='%' AND " +
                "Select_priv = 'Y' AND " +
                "Insert_priv = 'Y' AND " +
                "Update_priv = 'Y' AND  " +
                "Delete_priv = 'Y' AND  " +
                "Index_priv = 'Y' AND  " +
                "Alter_priv = 'Y' AND  " +
                "Create_priv = 'Y' AND  " +
                "Drop_priv = 'Y' AND  " +
                "Create_tmp_table_priv = 'Y' AND  " +
                "Lock_tables_priv = 'Y'", username.ToLower()));
            //
            List<string> databases = new List<string>();
            //
            //
            if (dtResult != null)
            {
                if (dtResult.DefaultView != null)
                {
                    DataView dvDatabases = dtResult.DefaultView;
                    //
                    foreach (DataRowView drDatabase in dvDatabases)
                    {
                        if (!Convert.IsDBNull(drDatabase["db"]))
                        {
                            databases.Add(Convert.ToString(drDatabase["db"]));
                        }
                    }
                }
            }
            //
            return databases.ToArray();
        }

        private void AddUserToDatabase(string databaseName, string user)
        {
            // grant database access
            ExecuteNonQuery(String.Format("GRANT ALL PRIVILEGES ON `{0}`.* TO '{1}'@'%'",
                    databaseName, user));
        }

        private void RemoveUserFromDatabase(string databaseName, string user)
        {
            // revoke db access
            ExecuteNonQuery(String.Format("REVOKE ALL PRIVILEGES ON `{0}`.* FROM '{1}'@'%'",
                    databaseName, user));
        }

        private void CloseDatabaseConnections(string database)
        {
            DataTable dtProcesses = ExecuteQuery("SHOW PROCESSLIST");
            //
            string filter = String.Format("db = '{0}'", database);
            //
            if (dtProcesses.Columns["db"].DataType == typeof(System.Byte[]))
                filter = String.Format("Convert(db, 'System.String') = '{0}'", database);

            DataView dvProcesses = new DataView(dtProcesses);
            foreach (DataRowView rowSid in dvProcesses)
            {
                string cmdText = String.Format("KILL {0}", rowSid["Id"]);
                try
                {
                    ExecuteNonQuery(cmdText);
                }
                catch (Exception ex)
                {
                    Log.WriteError("Cannot drop MariaDB connection: " + cmdText, ex);
                }
            }
        }

         #endregion


    }
}
