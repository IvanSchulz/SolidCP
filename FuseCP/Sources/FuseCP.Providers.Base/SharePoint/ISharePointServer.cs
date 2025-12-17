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

using FuseCP.Providers.OS;

namespace FuseCP.Providers.SharePoint
{
    public interface ISharePointServer
    {
        // sites
        void ExtendVirtualServer(SharePointSite site);
        void UnextendVirtualServer(string url, bool deleteContent);

        // backup/restore
        string BackupVirtualServer(string url, string fileName, bool zipBackup);
        void RestoreVirtualServer(string url, string fileName);
        byte[] GetTempFileBinaryChunk(string path, int offset, int length);
        string AppendTempFileBinaryChunk(string fileName, string path, byte[] chunk);

        // web parts
        string[] GetInstalledWebParts(string url);
        void InstallWebPartsPackage(string url, string packageName);
        void DeleteWebPartsPackage(string url, string packageName);

        // system users
        bool UserExists(string username);
        string[] GetUsers();
        SystemUser GetUser(string username);
        void CreateUser(SystemUser user);
        void UpdateUser(SystemUser user);
        void ChangeUserPassword(string username, string password);
        void DeleteUser(string username);

        // system groups
        bool GroupExists(string groupName);
        string[] GetGroups();
        SystemGroup GetGroup(string name);
        void CreateGroup(SystemGroup group);
        void UpdateGroup(SystemGroup group);
        void DeleteGroup(string name);
    }
}
