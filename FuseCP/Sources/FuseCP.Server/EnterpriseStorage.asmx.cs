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
using System.Data;
using System.Web;
using System.Collections;
using FuseCP.Web.Services;
using System.ComponentModel;
using FuseCP.Providers;
using FuseCP.Providers.EnterpriseStorage;
using FuseCP.Providers.OS;
using FuseCP.Server.Utils;
using FuseCP.Providers.Web;

namespace FuseCP.Server
{
    /// <summary>
    /// Summary description for EnterpriseStorage
    /// </summary>
    [WebService(Namespace = "http://smbsaas/fusecp/server/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [Policy("ServerPolicy")]
    [ToolboxItem(false)]
    public class EnterpriseStorage : HostingServiceProviderWebService, IEnterpriseStorage
    {
        private IEnterpriseStorage EnterpriseStorageProvider
        {
            get { return (IEnterpriseStorage)Provider; }
        }


        [WebMethod, SoapHeader("settings")]
        public SystemFile[] GetFolders(string organizationId, WebDavSetting[] settings)
        {
            try
            {
                Log.WriteStart("'{0}' GetFolders", ProviderSettings.ProviderName);
                SystemFile[] result = EnterpriseStorageProvider.GetFolders(organizationId, settings);
                Log.WriteEnd("'{0}' GetFolders", ProviderSettings.ProviderName);
                return result;
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("'{0}' GetFolders", ProviderSettings.ProviderName), ex);
                throw;
            }
        }

        [WebMethod, SoapHeader("settings")]
        public SystemFile[] GetFoldersWithoutFrsm(string organizationId, WebDavSetting[] settings)
        {
            try
            {
                Log.WriteStart("'{0}' GetFolders", ProviderSettings.ProviderName);
                SystemFile[] result = EnterpriseStorageProvider.GetFoldersWithoutFrsm(organizationId, settings);
                Log.WriteEnd("'{0}' GetFolders", ProviderSettings.ProviderName);
                return result;
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("'{0}' GetFolders", ProviderSettings.ProviderName), ex);
                throw;
            }
        }

        [WebMethod, SoapHeader("settings")]
        public SystemFile GetFolder(string organizationId, string folder, WebDavSetting setting)
        {
            try
            {
                Log.WriteStart("'{0}' GetFolder", ProviderSettings.ProviderName);
                SystemFile result = EnterpriseStorageProvider.GetFolder(organizationId, folder, setting);
                Log.WriteEnd("'{0}' GetFolder", ProviderSettings.ProviderName);
                return result;
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("'{0}' GetFolder", ProviderSettings.ProviderName), ex);
                throw;
            }
        }

        [WebMethod, SoapHeader("settings")]
        public void CreateFolder(string organizationId, string folder, WebDavSetting setting)
        {
            try
            {
                Log.WriteStart("'{0}' CreateFolder", ProviderSettings.ProviderName);
                EnterpriseStorageProvider.CreateFolder(organizationId, folder, setting);
                Log.WriteEnd("'{0}' CreateFolder", ProviderSettings.ProviderName);
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("'{0}' CreateFolder", ProviderSettings.ProviderName), ex);
                throw;
            }
        }

        [WebMethod, SoapHeader("settings")]
        public void DeleteFolder(string organizationId, string folder, WebDavSetting setting)
        {
            try
            {
                Log.WriteStart("'{0}' DeleteFolder", ProviderSettings.ProviderName);
                EnterpriseStorageProvider.DeleteFolder(organizationId, folder, setting);
                Log.WriteEnd("'{0}' DeleteFolder", ProviderSettings.ProviderName);
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("'{0}' DeleteFolder", ProviderSettings.ProviderName), ex);
                throw;
            }
        }

        [WebMethod, SoapHeader("settings")]
        public bool SetFolderWebDavRules(string organizationId, string folder, WebDavSetting setting, WebDavFolderRule[] rules)
        {
            try
            {
                Log.WriteStart("'{0}' SetFolderWebDavRules", ProviderSettings.ProviderName);
                bool bResult =  EnterpriseStorageProvider.SetFolderWebDavRules(organizationId, folder, setting, rules);
                Log.WriteEnd("'{0}' SetFolderWebDavRules", ProviderSettings.ProviderName);
                return bResult;
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("'{0}' SetFolderWebDavRules", ProviderSettings.ProviderName), ex);
                throw;
            }
        }

        [WebMethod, SoapHeader("settings")]
        public WebDavFolderRule[] GetFolderWebDavRules(string organizationId, string folder, WebDavSetting setting)
        {
            try
            {
                Log.WriteStart("'{0}' GetFolderWebDavRules", ProviderSettings.ProviderName);
                Providers.Web.WebDavFolderRule[]  webDavFolderRule =  EnterpriseStorageProvider.GetFolderWebDavRules(organizationId, folder, setting);
                Log.WriteEnd("'{0}' GetFolderWebDavRules", ProviderSettings.ProviderName);
                return webDavFolderRule;
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("'{0}' GetFolderWebDavRules", ProviderSettings.ProviderName), ex);
                throw;
            }
        }

        [WebMethod, SoapHeader("settings")]
        public bool CheckFileServicesInstallation()
        {
            try
            {
                Log.WriteStart("'{0}' CheckFileServicesInstallation", ProviderSettings.ProviderName);
                bool bResult = EnterpriseStorageProvider.CheckFileServicesInstallation();
                Log.WriteEnd("'{0}' CheckFileServicesInstallation", ProviderSettings.ProviderName);
                return bResult;
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("'{0}' CheckFileServicesInstallation", ProviderSettings.ProviderName), ex);
                throw;
            }
        }

        [WebMethod, SoapHeader("settings")]
        public SystemFile[] Search(string organizationId, string[] searchPaths, string searchText, string userPrincipalName, bool recursive)
        {
            try
            {
                Log.WriteStart("'{0}' Search", ProviderSettings.ProviderName);
                var searchResults = EnterpriseStorageProvider.Search(organizationId, searchPaths, searchText, userPrincipalName, recursive);
                Log.WriteEnd("'{0}' Search", ProviderSettings.ProviderName);
                return searchResults;
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("'{0}' Search", ProviderSettings.ProviderName), ex);
                throw;
            }
        }

        [WebMethod, SoapHeader("settings")]
        public SystemFile RenameFolder(string organizationId, string originalFolder, string newFolder, WebDavSetting setting)
        {
            try
            {
                Log.WriteStart("'{0}' RenameFolder", ProviderSettings.ProviderName);
                SystemFile systemFile = EnterpriseStorageProvider.RenameFolder(organizationId, originalFolder, newFolder, setting);
                Log.WriteEnd("'{0}' RenameFolder", ProviderSettings.ProviderName);
                return systemFile;
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("'{0}' RenameFolder", ProviderSettings.ProviderName), ex);
                throw;
            }
        }

        [WebMethod, SoapHeader("settings")]
        public SystemFile[] GetQuotasForOrganization(SystemFile[] folders)
        {
            try
            {
                Log.WriteStart("'{0}' GetQuotasForOrganization", ProviderSettings.ProviderName);
                var newFolders = EnterpriseStorageProvider.GetQuotasForOrganization(folders);
                Log.WriteEnd("'{0}' GetQuotasForOrganization", ProviderSettings.ProviderName);
                return newFolders;
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("'{0}' GetQuotasForOrganization", ProviderSettings.ProviderName), ex);
                throw;
            }
        }

        [WebMethod, SoapHeader("settings")]
        public void MoveFolder(string oldPath, string newPath)
        {
            try
            {
                Log.WriteStart("'{0}' MoveFolder", ProviderSettings.ProviderName);
                EnterpriseStorageProvider.MoveFolder(oldPath, newPath);
                Log.WriteEnd("'{0}' MoveFolder", ProviderSettings.ProviderName);
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("'{0}' MoveFolder", ProviderSettings.ProviderName), ex);
                throw;
            }
        }

    }
}
