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
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Security;
using System.Text.RegularExpressions;
using System.Web;
using FuseCP.Providers.DNS;
using FuseCP.Providers.DomainLookup;


namespace FuseCP.Providers.OS
{
	/// <summary>
	/// Summary description for IOperationSystem.
	/// </summary>
	public interface IOperatingSystem
	{
		// files
		public string PathCombine(params string[] segments);
		string CreatePackageFolder(string initialPath);
		bool FileExists(string path);
		bool DirectoryExists(string path);
		SystemFile GetFile(string path);
		SystemFile[] GetFiles(string path);
		SystemFile[] GetDirectoriesRecursive(string rootFolder, string path);
		SystemFile[] GetFilesRecursive(string rootFolder, string path);
		SystemFile[] GetFilesRecursiveByPattern(string rootFolder, string path, string pattern);
		byte[] GetFileBinaryContent(string path);
		byte[] GetFileBinaryContentUsingEncoding(string path, string encoding);
		byte[] GetFileBinaryChunk(string path, int offset, int length);
		string GetFileTextContent(string path);
		void CreateFile(string path);
		void CreateDirectory(string path);
		void ChangeFileAttributes(string path, DateTime createdTime, DateTime changedTime);
		void DeleteFile(string path);
		void DeleteFiles(string[] files);
		void DeleteEmptyDirectories(string[] directories);
		void UpdateFileBinaryContent(string path, byte[] content);
		void UpdateFileBinaryContentUsingEncoding(string path, byte[] content, string encoding);
		void AppendFileBinaryContent(string path, byte[] chunk);
		void UpdateFileTextContent(string path, string content);
		void MoveFile(string sourcePath, string destinationPath);
		void CopyFile(string sourcePath, string destinationPath);
		void ZipFiles(string zipFile, string rootPath, string[] files);
		string[] UnzipFiles(string zipFile, string destFolder);
		void CreateBackupZip(string zipFile, string rootPath);

		// Synchronizing
		FolderGraph GetFolderGraph(string path);
		void ExecuteSyncActions(FileSyncAction[] actions);

		void SetQuotaLimitOnFolder(string folderPath, string shareNameDrive, QuotaType quotaType, string quotaLimit, int mode, string wmiUserName, string wmiPassword);
		Quota GetQuotaOnFolder(string folderPath, string wmiUserName, string wmiPassword);
		void DeleteDirectoryRecursive(string rootPath);

		// logging
		List<string> GetLogNames();
		List<SystemLogEntry> GetLogEntries(string logName);
		SystemLogEntriesPaged GetLogEntriesPaged(string logName, int startRow, int maximumRows);
		void ClearLog(string logName);

		// processes
		OSProcess[] GetOSProcesses();
		void TerminateOSProcess(int pid);
		OSService[] GetOSServices();
		void ChangeOSServiceStatus(string id, OSServiceStatus status);

		// reboot
		void RebootSystem();

		#region Server informations
		public SystemResourceUsageInfo GetSystemResourceUsageInfo();
		public SystemMemoryInfo GetSystemMemoryInfo();
		public bool IsUnix();
		#endregion

		// execute command
		string ExecuteSystemCommand(string user, string password, string path, string args);

		// DNS lookup
		List<DnsRecordInfo> GetDomainDnsRecords(string domain, string dnsServer, DnsRecordType recordType, int pause);

		OSPlatformInfo GetOSPlatform();

		Shell DefaultShell { get; }
		Installer DefaultInstaller { get; }
		Web.IWebServer WebServer { get; }
		ServiceController ServiceController { get; }

		TraceListener DefaultTraceListener { get; }
	}

}
