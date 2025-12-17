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
using System.Collections;
using System.Collections.Generic;
using FuseCP.Providers.OS;
using FuseCP.Providers.Web;

namespace FuseCP.Providers.EnterpriseStorage
{
    /// <summary>
    /// Summary description for IEnterpriseStorage.
    /// </summary>
    public interface IEnterpriseStorage
    {
        SystemFile[] GetFolders(string organizationId, WebDavSetting[] settings);
        SystemFile[] GetFoldersWithoutFrsm(string organizationId, WebDavSetting[] settings);
        SystemFile GetFolder(string organizationId, string folderName, WebDavSetting setting);
        void CreateFolder(string organizationId, string folder, WebDavSetting setting);
        SystemFile RenameFolder(string organizationId, string originalFolder, string newFolder, WebDavSetting setting);
        void DeleteFolder(string organizationId, string folder, WebDavSetting setting);
        bool SetFolderWebDavRules(string organizationId, string folder, WebDavSetting setting, WebDavFolderRule[] rules);
        WebDavFolderRule[] GetFolderWebDavRules(string organizationId, string folder, WebDavSetting setting);
        bool CheckFileServicesInstallation();
        SystemFile[] Search(string organizationId, string[] searchPaths, string searchText, string userPrincipalName, bool recursive);
        SystemFile[] GetQuotasForOrganization(SystemFile[] folders);
        void MoveFolder(string oldPath, string newPath);
    }
}
