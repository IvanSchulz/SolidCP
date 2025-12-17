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

using FuseCP.Providers.OS;
using FuseCP.Providers.SharePoint;

namespace FuseCP.EnterpriseServer
{
    /// <summary>
    /// Summary description for esApplicationsInstaller
    /// </summary>
    [WebService(Namespace = "http://smbsaas/fusecp/enterpriseserver")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [Policy("EnterpriseServerPolicy")]
    [ToolboxItem(false)]
    public class esSharePointServers: WebService
    {

        #region Sites
        [WebMethod]
        public DataSet GetRawSharePointSitesPaged(int packageId,
            string filterColumn, string filterValue, string sortColumn, int startRow, int maximumRows)
        {
            return SharePointServerController.GetRawSharePointSitesPaged(packageId, filterColumn, filterValue,
                sortColumn, startRow, maximumRows);
        }

        [WebMethod]
        public List<SharePointSite> GetSharePointSites(int packageId, bool recursive)
        {
            return SharePointServerController.GetSharePointSites(packageId, recursive);
        }

        [WebMethod]
        public SharePointSite GetSharePointSite(int itemId)
        {
            return SharePointServerController.GetSite(itemId);
        }

        [WebMethod]
        public int AddSharePointSite(SharePointSite item)
        {
            return SharePointServerController.AddSite(item);
        }

        [WebMethod]
        public int DeleteSharePointSite(int itemId)
        {
            return SharePointServerController.DeleteSite(itemId);
        }
        #endregion

        #region Backup/Restore
        [WebMethod]
        public string BackupVirtualServer(int itemId, string fileName,
            bool zipBackup, bool download, string folderName)
        {
            return SharePointServerController.BackupVirtualServer(itemId, fileName, zipBackup, download, folderName);
        }

        [WebMethod]
        public byte[] GetSharePointBackupBinaryChunk(int itemId, string path, int offset, int length)
        {
            return SharePointServerController.GetSharePointBackupBinaryChunk(itemId, path, offset, length);
        }

        [WebMethod]
        public string AppendSharePointBackupBinaryChunk(int itemId, string fileName, string path, byte[] chunk)
        {
            return SharePointServerController.AppendSharePointBackupBinaryChunk(itemId, fileName, path, chunk);
        }

        [WebMethod]
        public int RestoreVirtualServer(int itemId, string uploadedFile, string packageFile)
        {
            return SharePointServerController.RestoreVirtualServer(itemId, uploadedFile, packageFile);
        }
        #endregion

        #region Web Parts
        [WebMethod]
        public string[] GetInstalledWebParts(int itemId)
        {
            return SharePointServerController.GetInstalledWebParts(itemId);
        }

        [WebMethod]
        public int InstallWebPartsPackage(int itemId, string uploadedFile, string packageFile)
        {
            return SharePointServerController.InstallWebPartsPackage(itemId, uploadedFile, packageFile);
        }

        [WebMethod]
        public int DeleteWebPartsPackage(int itemId, string packageName)
        {
            return SharePointServerController.DeleteWebPartsPackage(itemId, packageName);
        }
        #endregion

        #region Users
        [WebMethod]
        public DataSet GetRawSharePointUsersPaged(int packageId,
            string filterColumn, string filterValue, string sortColumn, int startRow, int maximumRows)
        {
            return SharePointServerController.GetRawSharePointUsersPaged(packageId,
                filterColumn, filterValue, sortColumn, startRow, maximumRows);
        }

        [WebMethod]
        public List<SystemUser> GetSharePointUsers(int packageId, bool recursive)
        {
            return SharePointServerController.GetSharePointUsers(packageId, recursive);
        }

        [WebMethod]
        public SystemUser GetSharePointUser(int itemId)
        {
            return SharePointServerController.GetSharePointUser(itemId);
        }

        [WebMethod]
        public int AddSharePointUser(SystemUser item)
        {
            return SharePointServerController.AddSharePointUser(item); ;
        }

        [WebMethod]
        public int UpdateSharePointUser(SystemUser item)
        {
            return SharePointServerController.UpdateSharePointUser(item);
        }

        [WebMethod]
        public int DeleteSharePointUser(int itemId)
        {
            return SharePointServerController.DeleteSharePointUser(itemId);
        }

        #endregion

        #region Groups
        [WebMethod]
        public DataSet GetRawSharePointGroupsPaged(int packageId,
            string filterColumn, string filterValue, string sortColumn, int startRow, int maximumRows)
        {
            return SharePointServerController.GetRawSharePointGroupsPaged(packageId,
                filterColumn, filterValue, sortColumn, startRow, maximumRows);
        }

        [WebMethod]
        public List<SystemGroup> GetSharePointGroups(int packageId, bool recursive)
        {
            return SharePointServerController.GetSharePointGroups(packageId, recursive);
        }

        [WebMethod]
        public SystemGroup GetSharePointGroup(int itemId)
        {
            return SharePointServerController.GetSharePointGroup(itemId);
        }

        [WebMethod]
        public int AddSharePointGroup(SystemGroup item)
        {
            return SharePointServerController.AddSharePointGroup(item);
        }

        [WebMethod]
        public int UpdateSharePointGroup(SystemGroup item)
        {
            return SharePointServerController.UpdateSharePointGroup(item);
        }

        [WebMethod]
        public int DeleteSharePointGroup(int itemId)
        {
            return SharePointServerController.DeleteSharePointGroup(itemId);
        }
        #endregion
    }
}
