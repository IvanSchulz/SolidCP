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

using System.Collections.Generic;
using System.IO;
using FuseCP.Providers.OS;

namespace FuseCP.Providers.StorageSpaces
{
    public interface IStorageSpace
    {
        List<SystemFile> GetAllDriveLetters();
        List<SystemFile> GetSystemSubFolders(string path);
        void UpdateStorageSettings(string fullPath, long qouteSizeBytes, QuotaType type);
        void ClearStorageSettings(string fullPath, string uncPath);
        void UpdateFolderQuota(string fullPath, long qouteSizeBytes, QuotaType type);
        Quota GetFolderQuota(string fullPath);
        void CreateFolder(string fullPath);
        void DeleteFolder(string fullPath);
        bool RenameFolder(string originalPath, string newName);
        bool FileOrDirectoryExist(string fullPath);
        void SetFolderNtfsPermissions(string fullPath, UserPermission[] permissions, bool isProtected, bool preserveInheritance);
        StorageSpaceFolderShare ShareFolder(string fullPath, string shareName);
        SystemFile[] Search(string[] searchPaths, string searchText, bool recursive);
        byte[] GetFileBinaryChunk(string path, int offset, int length);
        void RemoveShare(string fullPath);
        void ShareSetAbeState(string path, bool enabled);
        bool ShareGetAbeState(string path);
        bool ShareGetEncyptDataAccessStatus(string path);
        void ShareSetEncyptDataAccess(string path, bool enabled);
    }
}
