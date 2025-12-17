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

namespace FuseCP.Providers.Database
{
	/// <summary>
	/// Summary description for ISqlServerProvider.
	/// </summary>
	public interface IDatabaseServer
	{
		// databases
		bool CheckConnectivity(string databaseName, string username, string password);
        DataSet ExecuteSqlQuery(string databaseName, string commandText);
		void ExecuteSqlNonQuery(string databaseName, string commandText);
        DataSet ExecuteSqlQuerySafe(string databaseName, string username, string password, string commandText);
        void ExecuteSqlNonQuerySafe(string databaseName, string username, string password, string commandText);
		bool DatabaseExists(string databaseName);
        string[] GetDatabases();
		SqlDatabase GetDatabase(string databaseName);
        void CreateDatabase(SqlDatabase database);
		void UpdateDatabase(SqlDatabase database);
		void DeleteDatabase(string databaseName);
		long CalculateDatabaseSize(string database);

		// database maintenaince
		void TruncateDatabase(string databaseName);
        byte[] GetTempFileBinaryChunk(string path, int offset, int length);
        string AppendTempFileBinaryChunk(string fileName, string path, byte[] chunk);
        string BackupDatabase(string databaseName, string backupFileName, bool zipBackupFile);
		void RestoreDatabase(string databaseName, string[] fileNames);

		// users
		bool UserExists(string userName);
        string[] GetUsers();
		SqlUser GetUser(string username, string[] databases);
		void CreateUser(SqlUser user, string password);
		void UpdateUser(SqlUser user, string[] databases);
        void DeleteUser(string username, string[] databases);
		void ChangeUserPassword(string username, string password);
    }
}
