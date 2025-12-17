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

using FuseCP.Providers.Database;

namespace FuseCP.EnterpriseServer
{
    /// <summary>
    /// Summary description for esApplicationsInstaller
    /// </summary>
    [WebService(Namespace = "http://smbsaas/fusecp/enterpriseserver")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [Policy("EnterpriseServerPolicy")]
    [ToolboxItem(false)]
    public class esDatabaseServers: WebService
    {
        #region Databases
        [WebMethod]
        public DataSet GetRawSqlDatabasesPaged(int packageId, string groupName,
            string filterColumn, string filterValue, string sortColumn, int startRow, int maximumRows)
        {
            return DatabaseServerController.GetRawSqlDatabasesPaged(packageId, groupName, filterColumn, filterValue,
                sortColumn, startRow, maximumRows);
        }

        [WebMethod]
        public List<SqlDatabase> GetSqlDatabases(int packageId, string groupName, bool recursive)
        {
            return DatabaseServerController.GetSqlDatabases(packageId, groupName, recursive);
        }

        [WebMethod]
        public SqlDatabase GetSqlDatabase(int itemId)
        {
            return DatabaseServerController.GetSqlDatabase(itemId);
        }

        [WebMethod]
        public int AddSqlDatabase(SqlDatabase item, string groupName)
        {
            return DatabaseServerController.AddSqlDatabase(item, groupName);
        }

        [WebMethod]
        public int UpdateSqlDatabase(SqlDatabase item)
        {
            return DatabaseServerController.UpdateSqlDatabase(item);
        }

        [WebMethod]
        public int DeleteSqlDatabase(int itemId)
        {
            return DatabaseServerController.DeleteSqlDatabase(itemId);
        }

        [WebMethod]
        public string BackupSqlDatabase(int itemId, string backupName,
                bool zipBackup, bool download, string folderName)
        {
            return DatabaseServerController.BackupSqlDatabase(itemId, backupName, zipBackup,
                download, folderName);
        }

        [WebMethod]
        public byte[] GetSqlBackupBinaryChunk(int itemId, string path, int offset, int length)
        {
            return DatabaseServerController.GetSqlBackupBinaryChunk(itemId, path, offset, length);
        }

        [WebMethod]
        public string AppendSqlBackupBinaryChunk(int itemId, string fileName, string path, byte[] chunk)
        {
            return DatabaseServerController.AppendSqlBackupBinaryChunk(itemId, fileName, path, chunk);
        }

        [WebMethod]
        public int RestoreSqlDatabase(int itemId, string[] uploadedFiles, string[] packageFiles)
        {
            return DatabaseServerController.RestoreSqlDatabase(itemId, uploadedFiles, packageFiles);
        }

        [WebMethod]
        public int TruncateSqlDatabase(int itemId)
        {
            return DatabaseServerController.TruncateSqlDatabase(itemId);
        }

		[WebMethod]
		public DatabaseBrowserConfiguration GetDatabaseBrowserConfiguration(int packageId, string groupName)
		{
			return DatabaseServerController.GetDatabaseBrowserConfiguration(packageId, groupName);
		}

		[WebMethod]
		public DatabaseBrowserConfiguration GetDatabaseBrowserLogonScript(int packageId,
			string groupName, string username)
		{
			return DatabaseServerController.GetDatabaseBrowserLogonScript(packageId, groupName, username);
		}

        #endregion

        #region Users
        [WebMethod]
        public DataSet GetRawSqlUsersPaged(int packageId, string groupName,
            string filterColumn, string filterValue, string sortColumn, int startRow, int maximumRows)
        {
            return DatabaseServerController.GetRawSqlUsersPaged(packageId, groupName,
                filterColumn, filterValue, sortColumn, startRow, maximumRows);
        }

        [WebMethod]
        public List<SqlUser> GetSqlUsers(int packageId, string groupName, bool recursive)
        {
            return DatabaseServerController.GetSqlUsers(packageId, groupName, recursive);
        }

        [WebMethod]
        public SqlUser GetSqlUser(int itemId)
        {
            return DatabaseServerController.GetSqlUser(itemId);
        }

        [WebMethod]
        public int AddSqlUser(SqlUser item, string groupName)
        {
            return DatabaseServerController.AddSqlUser(item, groupName);
        }

        [WebMethod]
        public int UpdateSqlUser(SqlUser item)
        {
            return DatabaseServerController.UpdateSqlUser(item);
        }

        [WebMethod]
        public int DeleteSqlUser(int itemId)
        {
            return DatabaseServerController.DeleteSqlUser(itemId);
        }
        #endregion
    }
}
