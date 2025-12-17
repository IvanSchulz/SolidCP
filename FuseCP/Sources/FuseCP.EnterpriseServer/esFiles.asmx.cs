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

namespace FuseCP.EnterpriseServer
{
    /// <summary>
    /// Summary description for esApplicationsInstaller
    /// </summary>
    [WebService(Namespace = "http://smbsaas/fusecp/enterpriseserver")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [Policy("EnterpriseServerPolicy")]
    [ToolboxItem(false)]
    public class esFiles: WebService
    {
        [WebMethod]
        public SystemSettings GetFileManagerSettings()
        {
            return FilesController.GetFileManagerSettings();
        }

        [WebMethod]
        public string GetHomeFolder(int packageId)
        {
            return FilesController.GetHomeFolder(packageId);
        }

        [WebMethod]
        public List<SystemFile> GetFiles(int packageId, string path, bool includeFiles)
        {
            return FilesController.GetFiles(packageId, path, includeFiles);
        }

        [WebMethod]
        public List<SystemFile> GetFilesByMask(int packageId, string path, string filesMask)
        {
            return FilesController.GetFilesByMask(packageId, path, filesMask);
        }

        [WebMethod]
        public byte[] GetFileBinaryContent(int packageId, string path)
        {
            return FilesController.GetFileBinaryContent(packageId, path);
        }

		[WebMethod]
		public byte[] GetFileBinaryContentUsingEncoding(int packageId, string path, string encoding)
		{
			return FilesController.GetFileBinaryContentUsingEncoding(packageId, path, encoding);
		}

        [WebMethod]
        public int UpdateFileBinaryContent(int packageId, string path, byte[] content)
        {
            return FilesController.UpdateFileBinaryContent(packageId, path, content);
        }

		[WebMethod]
		public int UpdateFileBinaryContentUsingEncoding(int packageId, string path, byte[] content, string encoding)
		{
			return FilesController.UpdateFileBinaryContentUsingEncoding(packageId, path, content, encoding);
		}

        [WebMethod]
        public byte[] GetFileBinaryChunk(int packageId, string path, int offset, int length)
        {
            return FilesController.GetFileBinaryChunk(packageId, path, offset, length);
        }

        [WebMethod]
        public int AppendFileBinaryChunk(int packageId, string path, byte[] chunk)
        {
            return FilesController.AppendFileBinaryChunk(packageId, path, chunk);
        }

        [WebMethod]
        public int DeleteFiles(int packageId, string[] files)
        {
            return FilesController.DeleteFiles(packageId, files);
        }

        [WebMethod]
        public int CreateFile(int packageId, string path)
        {
            return FilesController.CreateFile(packageId, path);
        }

        [WebMethod]
        public int CreateFolder(int packageId, string path)
        {
            return FilesController.CreateFolder(packageId, path);
        }

        [WebMethod]
        public int CopyFiles(int packageId, string[] files, string destFolder)
        {
            return FilesController.CopyFiles(packageId, files, destFolder);
        }

        [WebMethod]
        public int MoveFiles(int packageId, string[] files, string destFolder)
        {
            return FilesController.MoveFiles(packageId, files, destFolder);
        }

        [WebMethod]
        public int RenameFile(int packageId, string oldPath, string newPath)
        {
            return FilesController.RenameFile(packageId, oldPath, newPath);
        }

        [WebMethod]
        public void UnzipFiles(int packageId, string[] files)
        {
            FilesController.UnzipFiles(packageId, files);
        }

        [WebMethod]
        public int ZipFiles(int packageId, string[] files, string archivePath)
        {
            return FilesController.ZipFiles(packageId, files, archivePath);
        }

        [WebMethod]
        public int ZipRemoteFiles(int packageId, string rootFolder, string[] files, string archivePath)
        {
            return FilesController.ZipRemoteFiles(packageId, rootFolder, files, archivePath);
        }

        [WebMethod]
        public int CreateAccessDatabase(int packageId, string dbPath)
        {
            return FilesController.CreateAccessDatabase(packageId, dbPath);
        }

        [WebMethod]
        public int CalculatePackageDiskspace(int packageId)
        {
            return FilesController.CalculatePackageDiskspace(packageId);
        }

        [WebMethod]
        public UserPermission[] GetFilePermissions(int packageId, string path)
        {
            return FilesController.GetFilePermissions(packageId, path);
        }

		[WebMethod]
		public UnixFilePermissions GetUnixFilePermissions(int packageId, string path)
		{
			return FilesController.GetUnixFilePermissions(packageId, path);
		}

		[WebMethod]
        public int SetFilePermissions(int packageId, string path, UserPermission[] users, bool resetChildPermissions)
        {
            return FilesController.SetFilePermissions(packageId, path, users, resetChildPermissions);
        }

		[WebMethod]
		public int SetUnixFilePermissions(int packageId, string path,
        	string owner,
	        string group,
        	Providers.OS.UnixFileMode permissions,
	        bool resetChildPermissions)
		{
			return FilesController.SetUnixFilePermissions(packageId, path, owner, group, permissions, resetChildPermissions);
		}

		[WebMethod]
        public FolderGraph GetFolderGraph(int packageId, string path)
        {
            return FilesController.GetFolderGraph(packageId, path);
        }

        [WebMethod]
        public void ExecuteSyncActions(int packageId, FileSyncAction[] actions)
        {
            FilesController.ExecuteSyncActions(packageId, actions);
        }

        //CO Changes
        [WebMethod]
        public int ApplyEnableHardQuotaFeature(int packageId)
        {
            return FilesController.ApplyEnableHardQuotaFeature(packageId);
        }
        //END
    }
}
