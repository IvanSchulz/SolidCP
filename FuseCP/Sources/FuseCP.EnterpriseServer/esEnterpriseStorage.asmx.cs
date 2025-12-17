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
using System.Collections.Specialized;
using System.Data;
using System.Web;
using System.Collections;
using System.Collections.Generic;
using FuseCP.Web.Services;
using System.ComponentModel;

using FuseCP.Providers.Common;
using FuseCP.Providers.HostedSolution;
using FuseCP.Providers.EnterpriseStorage;
using FuseCP.Providers.ResultObjects;
using FuseCP.Providers.OS;
using FuseCP.Providers.Web;
using FuseCP.EnterpriseServer.Base.HostedSolution;

namespace FuseCP.EnterpriseServer
{
    /// <summary>
    /// Summary description for esEnterpriseStorage
    /// </summary>
    [WebService(Namespace = "http://smbsaas/fusecp/enterpriseserver")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [Policy("EnterpriseServerPolicy")]
    [ToolboxItem(false)]
    public class esEnterpriseStorage : WebService
    {
        [WebMethod]
        public int AddWebDavAccessToken(WebDavAccessToken accessToken)
        {
           return EnterpriseStorageController.AddWebDavAccessToken(accessToken);
        }

        [WebMethod]
        public void DeleteExpiredWebDavAccessTokens()
        {
            EnterpriseStorageController.DeleteExpiredWebDavAccessTokens();
        }

        [WebMethod]
        public WebDavAccessToken GetWebDavAccessTokenById(int id)
        {
            return EnterpriseStorageController.GetWebDavAccessTokenById(id);
        }

        [WebMethod]
        public WebDavAccessToken GetWebDavAccessTokenByAccessToken(Guid accessToken)
        {
            return EnterpriseStorageController.GetWebDavAccessTokenByAccessToken(accessToken);
        }

        [WebMethod]
        public bool CheckFileServicesInstallation(int serviceId)
        {
            return EnterpriseStorageController.CheckFileServicesInstallation(serviceId);
        }

        [WebMethod]
        public SystemFile[] GetEnterpriseFolders(int itemId)
        {
            return EnterpriseStorageController.GetFolders(itemId);
        }

        [WebMethod]
        public SystemFile[] GetUserRootFolders(int itemId, int accountId, string userName, string displayName)
        {
            return EnterpriseStorageController.GetUserRootFolders(itemId, accountId, userName, displayName);
        }

        [WebMethod]
        public SystemFile GetEnterpriseFolder(int itemId, string folderName)
        {
            return EnterpriseStorageController.GetFolder(itemId, folderName);
        }

        [WebMethod]
        public SystemFile GetEnterpriseFolderWithExtraData(int itemId, string folderName, bool loadDriveMapInfo)
        {
            return EnterpriseStorageController.GetFolder(itemId, folderName, loadDriveMapInfo);
        }

        [WebMethod]
        public ResultObject CreateEnterpriseFolder(int itemId, string folderName, int quota, QuotaType quotaType, bool addDefaultGroup)
        {
            return EnterpriseStorageController.CreateFolder(itemId, folderName, quota, quotaType, addDefaultGroup);
        }

        [WebMethod]
        public ResultObject CreateEnterpriseSubFolder(int itemId, string folderPath)
        {
            return EnterpriseStorageController.CreateSubFolder(itemId, folderPath);
        }

        [WebMethod]
        public ResultObject DeleteEnterpriseFolder(int itemId, string folderName)
        {
            return EnterpriseStorageController.DeleteFolder(itemId, folderName);
        }

        [WebMethod]
        public ESPermission[] GetEnterpriseFolderPermissions(int itemId, string folderName)
        {
            return EnterpriseStorageController.GetFolderPermission(itemId, folderName);
        }

        [WebMethod]
        public ResultObject SetEnterpriseFolderPermissions(int itemId, string folderName, ESPermission[] permission)
        {
            return EnterpriseStorageController.SetFolderPermission(itemId, folderName, permission);
        }

        [WebMethod]
        public List<ExchangeAccount> SearchESAccounts(int itemId, string filterColumn, string filterValue, string sortColumn)
        {
            return EnterpriseStorageController.SearchESAccounts(itemId, filterColumn, filterValue, sortColumn);
        }

        [WebMethod]
        public SystemFilesPaged GetEnterpriseFoldersPaged(int itemId, bool loadUsagesData, bool loadWebdavRules, bool loadMappedDrives, string filterValue, string sortColumn, int startRow, int maximumRows)
        {
            return EnterpriseStorageController.GetEnterpriseFoldersPaged(itemId, loadUsagesData, loadWebdavRules, loadMappedDrives, filterValue, sortColumn, startRow, maximumRows);
        }

        [WebMethod]
        public SystemFile RenameEnterpriseFolder(int itemId, string oldName, string newName)
        {
            return EnterpriseStorageController.RenameFolder(itemId, oldName, newName);
        }

        [WebMethod]
        public ResultObject CreateEnterpriseStorage(int packageId, int itemId)
        {
            return EnterpriseStorageController.CreateEnterpriseStorage(packageId, itemId);
        }

        [WebMethod]
        public bool CheckEnterpriseStorageInitialization(int packageId, int itemId)
        {
            return EnterpriseStorageController.CheckEnterpriseStorageInitialization(packageId, itemId);
        }

        [WebMethod]
        public bool CheckUsersDomainExists(int itemId)
        {
            return EnterpriseStorageController.CheckUsersDomainExists(itemId);
        }

        [WebMethod]
        public string GetWebDavPortalUserSettingsByAccountId(int accountId)
        {
            return EnterpriseStorageController.GetWebDavPortalUserSettingsByAccountId(accountId);
        }

        [WebMethod]
        public void UpdateWebDavPortalUserSettings(int accountId, string settings)
        {
            EnterpriseStorageController.UpdateUserSettings(accountId,settings);
        }

        [WebMethod]
        public SystemFile[] SearchFiles(int itemId, string[] searchPaths, string searchText, string userPrincipalName, bool recursive)
        {
           return EnterpriseStorageController.SearchFiles(itemId, searchPaths, searchText, userPrincipalName, recursive);
        }

        [WebMethod]
        public int GetEnterpriseStorageServiceId(int itemId)
        {
            return EnterpriseStorageController.GetEnterpriseStorageServiceId(itemId);
        }

        [WebMethod]
        public void SetEsFolderShareSettings(int itemId, string folderName, bool abeIsEnabled, bool edaIsEnabled)
        {
            EnterpriseStorageController.SetEsFolderShareSettings(itemId, folderName, abeIsEnabled, edaIsEnabled);
        }

        #region Directory Browsing

        [WebMethod]
        public bool GetDirectoryBrowseEnabled(int itemId, string site)
        {
            return EnterpriseStorageController.GetDirectoryBrowseEnabled(itemId, site);
        }

        [WebMethod]
        public void SetDirectoryBrowseEnabled(int itemId, string site, bool enabled)
        {
            EnterpriseStorageController.SetDirectoryBrowseEnabled(itemId, site, enabled);
        }

        [WebMethod]
        public void SetEnterpriseFolderSettings(int itemId, SystemFile folder, ESPermission[] permissions, bool directoyBrowsingEnabled, int quota, QuotaType quotaType)
        {
            EnterpriseStorageController.StartSetEnterpriseFolderSettingsBackgroundTask(itemId, folder, permissions, directoyBrowsingEnabled, quota, quotaType);
        }

        [WebMethod]
        public void SetEnterpriseFolderGeneralSettings(int itemId, SystemFile folder, bool directoyBrowsingEnabled, int quota, QuotaType quotaType)
        {
            EnterpriseStorageController.SetESGeneralSettings(itemId, folder, directoyBrowsingEnabled, quota, quotaType);
        }

        [WebMethod]
        public void SetEnterpriseFolderPermissionSettings(int itemId, SystemFile folder, ESPermission[] permissions)
        {
            EnterpriseStorageController.SetESFolderPermissionSettings(itemId, folder, permissions);
        }

        [WebMethod]
        public OrganizationUser[] GetFolderOwaAccounts(int itemId, SystemFile folder)
        {
           return  EnterpriseStorageController.GetFolderOwaAccounts(itemId, folder.Name);
        }

        [WebMethod]
        public void SetFolderOwaAccounts(int itemId, SystemFile folder, OrganizationUser[] users)
        {
            EnterpriseStorageController.SetFolderOwaAccounts(itemId, folder.Name, users);
        }

        [WebMethod]
        public List<string> GetUserEnterpriseFolderWithOwaEditPermission(int itemId, List<int> accountIds)
        {
            return EnterpriseStorageController.GetUserEnterpriseFolderWithOwaEditPermission(itemId, accountIds);
        }

        [WebMethod]
        public ResultObject MoveToStorageSpace(int itemId, string folderName)
        {
           return EnterpriseStorageController.MoveToStorageSpace(itemId, folderName);
        }

        #endregion

        #region Statistics

        [WebMethod]
        public OrganizationStatistics GetStatistics(int itemId)
        {
            return EnterpriseStorageController.GetStatistics(itemId);
        }

        [WebMethod]
        public OrganizationStatistics GetStatisticsByOrganization(int itemId)
        {
            return EnterpriseStorageController.GetStatisticsByOrganization(itemId);
        }

        #endregion

        #region Drive Mapping

        [WebMethod]
        public ResultObject CreateMappedDrive(int packageId, int itemId, string driveLetter, string labelAs, string folderName)
        {
            return EnterpriseStorageController.CreateMappedDrive(packageId, itemId, driveLetter, labelAs, folderName);
        }

        [WebMethod]
        public ResultObject DeleteMappedDrive(int itemId, string driveLetter)
        {
            return EnterpriseStorageController.DeleteMappedDrive(itemId, driveLetter);
        }

        [WebMethod]
        public MappedDrivesPaged GetDriveMapsPaged(int itemId, string filterValue, string sortColumn, int startRow, int maximumRows)
        {
            return EnterpriseStorageController.GetDriveMapsPaged(itemId, filterValue, sortColumn, startRow, maximumRows);
        }

        [WebMethod]
        public string[] GetUsedDriveLetters(int itemId)
        {
            return EnterpriseStorageController.GetUsedDriveLetters(itemId);
        }

        [WebMethod]
        public SystemFile[] GetNotMappedEnterpriseFolders(int itemId)
        {
            return EnterpriseStorageController.GetNotMappedEnterpriseFolders(itemId);
        }

        #endregion
    }
}
