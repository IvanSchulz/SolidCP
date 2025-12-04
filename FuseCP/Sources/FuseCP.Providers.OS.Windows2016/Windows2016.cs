// Copyright (c) 2016, SolidCP
// SolidCP is distributed under the Creative Commons Share-alike license
// 
// Redistribution and use in source and binary forms, with or without modification,
// are permitted provided that the following conditions are met:
//
// - Redistributions of source code must  retain  the  above copyright notice, this
//   list of conditions and the following disclaimer.
//
// - Redistributions in binary form  must  reproduce the  above  copyright  notice,
//   this list of conditions  and  the  following  disclaimer in  the documentation
//   and/or other materials provided with the distribution.
//
// - Neither  the  name  of  FuseCP  nor   the   names  of  its
//   contributors may be used to endorse or  promote  products  derived  from  this
//   software without specific prior written permission.
//
// THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND
// ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING,  BUT  NOT  LIMITED TO, THE IMPLIED
// WARRANTIES  OF  MERCHANTABILITY   AND  FITNESS  FOR  A  PARTICULAR  PURPOSE  ARE
// DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT HOLDER OR CONTRIBUTORS BE LIABLE FOR
// ANY DIRECT, INDIRECT, INCIDENTAL,  SPECIAL,  EXEMPLARY, OR CONSEQUENTIAL DAMAGES
// (INCLUDING, BUT NOT LIMITED TO,  PROCUREMENT  OF  SUBSTITUTE  GOODS OR SERVICES;
// LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION)  HOWEVER  CAUSED AND ON
// ANY  THEORY  OF  LIABILITY,  WHETHER  IN  CONTRACT,  STRICT  LIABILITY,  OR TORT
// (INCLUDING NEGLIGENCE OR OTHERWISE)  ARISING  IN  ANY WAY OUT OF THE USE OF THIS
// SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.

using Microsoft.Win32;
using Mono.Unix.Native;
using FuseCP.Providers;
using FuseCP.Providers.Common;
using FuseCP.Providers.DNS;
using FuseCP.Providers.DomainLookup;
using FuseCP.Providers.HostedSolution;
using FuseCP.Providers.Utils;
using FuseCP.Server.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Management;
using System.Management.Automation;
using System.Management.Automation.Runspaces;
using System.Reflection;
using System.Runtime.InteropServices;
using System.ServiceProcess;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;


namespace FuseCP.Providers.OS
{

    public class Windows2016 : HostingServiceProviderBase, IWindowsOperatingSystem
    {
        #region Constants
        private const string ODBC_SOURCES_KEY = @"SOFTWARE\ODBC\ODBC.INI";
        private const string ODBC_NAMES_KEY = @"SOFTWARE\ODBC\ODBC.INI\ODBC Data Sources";
        private const string ODBC_SOURCES_KEY_NAME = @"ODBC Data Sources";
        private const string ODBC_INST_KEY = @"SOFTWARE\ODBC\ODBCINST.INI\";

        private const string DSN_DESCRIPTION = @"FuseCP Data Source";

        private const string MSSQL_DRIVER = "SQL Server";
        private const string MSSQL_NATIVE_DRIVER = "SQL Native Client";
        private const string MYSQL_DRIVER = "MySQL ODBC";// 3.51 Driver";
        private const string MARIADB_DRIVER = "MariaDB ODBC";
        private const string MSACCESS_DRIVER = "Microsoft Access Driver (*.mdb)";
        private const string MSACCESS2010_DRIVER = "Microsoft Access Driver (*.mdb, *.accdb)";
        private const string MSEXCEL_DRIVER = "Microsoft Excel Driver (*.xls)";
        private const string MSEXCEL2010_DRIVER = "Microsoft Excel Driver (*.xls, *.xlsx, *.xlsm, *.xlsb)";
        private const string TEXT_DRIVER = "Microsoft Text Driver (*.txt; *.csv)";

        #endregion

        #region Properties
        protected string UsersHome
        {
            get { return FileUtils.EvaluateSystemVariables(ProviderSettings["UsersHome"]); }
        }
        #endregion

        #region Files
        public string PathCombine(params string[] segments) => Path.Combine(segments);

        public virtual string CreatePackageFolder(string initialPath)
        {
            return FileUtils.CreatePackageFolder(initialPath);
        }

        public virtual bool FileExists(string path)
        {
            return FileUtils.FileExists(path);
        }

        public virtual bool DirectoryExists(string path)
        {
            return FileUtils.DirectoryExists(path);
        }

        public virtual SystemFile GetFile(string path)
        {
            return FileUtils.GetFile(path);
        }

        public virtual SystemFile[] GetFiles(string path)
        {
            return FileUtils.GetFiles(path);
        }

        public virtual SystemFile[] GetDirectoriesRecursive(string rootFolder, string path)
        {
            return FileUtils.GetDirectoriesRecursive(rootFolder, path);
        }

        public virtual SystemFile[] GetFilesRecursive(string rootFolder, string path)
        {
            return FileUtils.GetFilesRecursive(rootFolder, path);
        }

        public virtual SystemFile[] GetFilesRecursiveByPattern(string rootFolder, string path, string pattern)
        {
            return FileUtils.GetFilesRecursiveByPattern(rootFolder, path, pattern);
        }

        public virtual byte[] GetFileBinaryContent(string path)
        {
            return FileUtils.GetFileBinaryContent(path);
        }

        public virtual byte[] GetFileBinaryContentUsingEncoding(string path, string encoding)
        {
            return FileUtils.GetFileBinaryContent(path, encoding);
        }

        public virtual byte[] GetFileBinaryChunk(string path, int offset, int length)
        {
            return FileUtils.GetFileBinaryChunk(path, offset, length);
        }

        public virtual string GetFileTextContent(string path)
        {
            return FileUtils.GetFileTextContent(path);
        }

        public virtual void CreateFile(string path)
        {
            FileUtils.CreateFile(path);
        }

        public virtual void CreateDirectory(string path)
        {
            FileUtils.CreateDirectory(path);
        }

        public virtual void ChangeFileAttributes(string path, DateTime createdTime, DateTime changedTime)
        {
            FileUtils.ChangeFileAttributes(path, createdTime, changedTime);
        }

        public virtual void DeleteFile(string path)
        {
            FileUtils.DeleteFile(path);
        }

        public virtual void DeleteFiles(string[] files)
        {
            FileUtils.DeleteFiles(files);
        }

        public virtual void DeleteEmptyDirectories(string[] directories)
        {
            FileUtils.DeleteEmptyDirectories(directories);
        }

        public virtual void UpdateFileBinaryContent(string path, byte[] content)
        {
            FileUtils.UpdateFileBinaryContent(path, content);
        }

        public virtual void UpdateFileBinaryContentUsingEncoding(string path, byte[] content, string encoding)
        {
            FileUtils.UpdateFileBinaryContent(path, content, encoding);
        }

        public virtual void AppendFileBinaryContent(string path, byte[] chunk)
        {
            FileUtils.AppendFileBinaryContent(path, chunk);
        }

        public virtual void UpdateFileTextContent(string path, string content)
        {
            FileUtils.UpdateFileTextContent(path, content);
        }

        public virtual void MoveFile(string sourcePath, string destinationPath)
        {
            FileUtils.MoveFile(sourcePath, destinationPath);
        }

        public virtual void CopyFile(string sourcePath, string destinationPath)
        {
            FileUtils.CopyFile(sourcePath, destinationPath);
        }

        public virtual void ZipFiles(string zipFile, string rootPath, string[] files)
        {
            FileUtils.ZipFiles(zipFile, rootPath, files);
        }

        public virtual string[] UnzipFiles(string zipFile, string destFolder)
        {
            return FileUtils.UnzipFiles(zipFile, destFolder);
        }

        public virtual void CreateBackupZip(string zipFile, string rootPath)
        {
            FileUtils.CreateBackupZip(zipFile, rootPath);
        }

        public virtual void CreateAccessDatabase(string databasePath)
        {
            FileUtils.CreateAccessDatabase(databasePath);
        }

        public UserPermission[] GetGroupNtfsPermissions(string path, UserPermission[] users, string usersOU)
        {
            return SecurityUtils.GetGroupNtfsPermissions(path, users,
                 ServerSettings, usersOU, null);
        }

        public void GrantGroupNtfsPermissions(string path, UserPermission[] users, string usersOU, bool resetChildPermissions)
        {
            SecurityUtils.GrantGroupNtfsPermissions(path, users, resetChildPermissions,
                 ServerSettings, usersOU, null);
        }

        public virtual void DeleteDirectoryRecursive(string rootPath)
        {
            FileUtils.DeleteDirectoryRecursive(rootPath);
        }
        #endregion

        #region ODBC DSNs
        public virtual string[] GetInstalledOdbcDrivers()
        {
            List<string> drivers = new List<string>();
            if (IsDriverInstalled(MSSQL_DRIVER)) drivers.Add("MsSql");
            if (IsDriverInstalled(MSSQL_NATIVE_DRIVER)) drivers.Add("MsSqlNative");
            if (IsDriverInstalled(GetDriverName(MYSQL_DRIVER))) drivers.Add("MySql");
            if (IsDriverInstalled(GetDriverName(MARIADB_DRIVER))) drivers.Add("MariaDB");
            if (IsDriverInstalled(MSACCESS_DRIVER)) drivers.Add("MsAccess");
            if (IsDriverInstalled(MSACCESS2010_DRIVER)) drivers.Add("MsAccess2010");
            if (IsDriverInstalled(MSEXCEL_DRIVER)) drivers.Add("Excel");
            if (IsDriverInstalled(MSEXCEL2010_DRIVER)) drivers.Add("Excel2010");
            if (IsDriverInstalled(TEXT_DRIVER)) drivers.Add("Text");
            return drivers.ToArray();
        }

        public virtual string[] GetDSNNames()
        {
            // check DSN name
            RegistryKey keyNames = Registry.LocalMachine.OpenSubKey(ODBC_NAMES_KEY);
            if (keyNames == null)
                return new string[0];

            // open DSNs tree
            RegistryKey keyDsn = Registry.LocalMachine.OpenSubKey(ODBC_SOURCES_KEY);
            if (keyDsn == null)
                return new string[0];

            return keyDsn.GetSubKeyNames();
        }

        public virtual SystemDSN GetDSN(string dsnName)
        {
            // check DSN name
            RegistryKey keyNames = Registry.LocalMachine.OpenSubKey(ODBC_NAMES_KEY);
            if (keyNames == null)
                return null;

            string driverName = (string)keyNames.GetValue(dsnName);
            if (driverName == null)
                return null;

            // open DSN tree
            RegistryKey keyDsn = Registry.LocalMachine.OpenSubKey(ODBC_SOURCES_KEY + "\\" + dsnName);
            if (keyDsn == null)
                return null;

            SystemDSN dsn = new SystemDSN();
            dsn.Name = dsnName;
            if (driverName == MSSQL_DRIVER || driverName == MSSQL_NATIVE_DRIVER)
            {
                dsn.Driver = (driverName == MSSQL_DRIVER) ? "MsSql" : "MsSqlNative";
                dsn.DatabaseServer = (string)keyDsn.GetValue("Server");
                dsn.DatabaseName = (string)keyDsn.GetValue("Database");
                dsn.DatabaseUser = (string)keyDsn.GetValue("LastUser");
            }
            else if (driverName.ToLower().StartsWith(MYSQL_DRIVER.ToLower()))
            {
                dsn.Driver = "MySql";
                dsn.DatabaseServer = (string)keyDsn.GetValue("SERVER");
                dsn.DatabaseName = (string)keyDsn.GetValue("DATABASE");
                dsn.DatabaseUser = (string)keyDsn.GetValue("UID");
                dsn.DatabasePassword = (string)keyDsn.GetValue("PWD");
            }
            else if (driverName.ToLower().StartsWith(MARIADB_DRIVER.ToLower()))
            {
                dsn.Driver = "MariaDB";
                dsn.DatabaseServer = (string)keyDsn.GetValue("SERVER");
                dsn.DatabaseName = (string)keyDsn.GetValue("DATABASE");
                dsn.DatabaseUser = (string)keyDsn.GetValue("UID");
                dsn.DatabasePassword = (string)keyDsn.GetValue("PWD");
            }
            else if (driverName == MSACCESS_DRIVER)
            {
                dsn.Driver = "MsAccess";
                dsn.DatabaseName = (string)keyDsn.GetValue("DBQ");
                dsn.DatabaseUser = (string)keyDsn.GetValue("UID");
                dsn.DatabasePassword = (string)keyDsn.GetValue("PWD");
            }
            else if (driverName == MSACCESS2010_DRIVER)
            {
                dsn.Driver = "MsAccess2010";
                dsn.DatabaseName = (string)keyDsn.GetValue("DBQ");
                dsn.DatabaseUser = (string)keyDsn.GetValue("UID");
                dsn.DatabasePassword = (string)keyDsn.GetValue("PWD");
            }
            else if (driverName == MSEXCEL_DRIVER)
            {
                dsn.Driver = "Excel";
                dsn.DatabaseName = (string)keyDsn.GetValue("DBQ");
            }
            else if (driverName == MSEXCEL2010_DRIVER)
            {
                dsn.Driver = "Excel2010";
                dsn.DatabaseName = (string)keyDsn.GetValue("DBQ");
            }
            else if (driverName == TEXT_DRIVER)
            {
                dsn.Driver = "Text";
                dsn.DatabaseName = (string)keyDsn.GetValue("DefaultDir");
            }

            return dsn;
        }

        public virtual void CreateDSN(SystemDSN dsn)
        {
            switch (dsn.Driver.ToLower())
            {
                case "mssql":
                    CreateMsSqlDsn(dsn, MSSQL_DRIVER);
                    break;
                case "mssqlnative":
                    CreateMsSqlDsn(dsn, MSSQL_NATIVE_DRIVER);
                    break;
                case "mysql":
                    CreateMySqlDsn(dsn);
                    break;
                case "mariadb":
                    CreateMariaDBDsn(dsn);
                    break;
                case "msaccess":
                    CreateMsAccessDsn(dsn);
                    break;
                case "msaccess2010":
                    CreateMsAccess2010Dsn(dsn);
                    break;
                case "excel":
                    CreateExcelDsn(dsn);
                    break;
                case "excel2010":
                    CreateExcel2010Dsn(dsn);
                    break;
                case "text":
                    CreateTextDsn(dsn);
                    break;
            }
        }

        public virtual void UpdateDSN(SystemDSN dsn)
        {
            // delete DSN
            DeleteDSN(dsn.Name);

            // create again
            CreateDSN(dsn);
        }

        public virtual void DeleteDSN(string dsnName)
        {
            // delete ODBC name
            RegistryKey list = Registry.LocalMachine.OpenSubKey(ODBC_NAMES_KEY, true);
            list.DeleteValue(dsnName);
            list.Close();

            // delete from ODBC tree
            RegistryKey root = Registry.LocalMachine.OpenSubKey(ODBC_SOURCES_KEY, true);
            root.DeleteSubKeyTree(dsnName);
            root.Close();
        }
        #endregion

        #region Synchronizing
        public FolderGraph GetFolderGraph(string path)
        {
            if (!path.EndsWith("\\"))
                path += "\\";

            FolderGraph graph = new FolderGraph();
            graph.Hash = CalculateFileHash(path, path, graph.CheckSums);

            // copy hash to arrays
            graph.CheckSumKeys = new uint[graph.CheckSums.Count];
            graph.CheckSumValues = new FileHash[graph.CheckSums.Count];
            graph.CheckSums.Keys.CopyTo(graph.CheckSumKeys, 0);
            graph.CheckSums.Values.CopyTo(graph.CheckSumValues, 0);

            return graph;
        }

        public void ExecuteSyncActions(FileSyncAction[] actions)
        {
            // perform all operations but not delete ones
            foreach (FileSyncAction action in actions)
            {
                if (action.ActionType == SyncActionType.Create)
                {
                    FileUtils.CreateDirectory(action.DestPath);
                    continue;
                }
                else if (action.ActionType == SyncActionType.Copy)
                {
                    FileUtils.CopyFile(action.SrcPath, action.DestPath);
                }
                else if (action.ActionType == SyncActionType.Move)
                {
                    FileUtils.MoveFile(action.SrcPath, action.DestPath);
                }
            }

            // unzip file
            // ...after delete

            // delete files
            foreach (FileSyncAction action in actions)
            {
                if (action.ActionType == SyncActionType.Delete)
                {
                    FileUtils.DeleteFile(action.DestPath);
                }
            }
        }

        private FileHash CalculateFileHash(string rootFolder, string path, Dictionary<uint, FileHash> checkSums)
        {
            CRC32 crc32 = new CRC32();

            // check if this is a folder
            if (Directory.Exists(path))
            {
                FileHash folder = new FileHash();
                folder.IsFolder = true;
                folder.Name = Path.GetFileName(path);
                folder.FullName = path.Substring(rootFolder.Length - 1);

                // process child folders and files
                List<string> childFiles = new List<string>();
                childFiles.AddRange(Directory.GetDirectories(path));
                childFiles.AddRange(Directory.GetFiles(path));

                foreach (string childFile in childFiles)
                {
                    FileHash childHash = CalculateFileHash(rootFolder, childFile, checkSums);
                    folder.Files.Add(childHash);

                    // check sum
                    folder.CheckSum += childHash.CheckSum;
                    folder.CheckSum += ConvertCheckSumToInt(crc32.ComputeHash(Encoding.UTF8.GetBytes(childHash.Name)));

                    //Debug.WriteLine(folder.CheckSum + " : " + folder.FullName);
                }

                // move list to array
                folder.FilesArray = folder.Files.ToArray();

                if (!checkSums.ContainsKey(folder.CheckSum))
                    checkSums.Add(folder.CheckSum, folder);

                return folder;
            }

            FileHash file = new FileHash();
            file.Name = Path.GetFileName(path);
            file.FullName = path.Substring(rootFolder.Length - 1);

            // calculate CRC32
            using (FileStream fs = File.Open(path, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                file.CheckSum = ConvertCheckSumToInt(
                    crc32.ComputeHash(fs));
            }

            if (!checkSums.ContainsKey(file.CheckSum))
                checkSums.Add(file.CheckSum, file);

            //Debug.WriteLine(file.CheckSum + " : " + file.FullName);

            return file;
        }

        private uint ConvertCheckSumToInt(byte[] sumBytes)
        {
            uint checkSum = (uint)sumBytes[0] << 24;
            checkSum |= (uint)sumBytes[1] << 16;
            checkSum |= (uint)sumBytes[2] << 8;
            checkSum |= (uint)sumBytes[3] << 0;
            return checkSum;
        }
        #endregion

        #region HostingServiceProvider methods
        public override string[] Install()
        {
            List<string> messages = new List<string>();

            // create folder if it not exists
            try
            {
                if (!FileUtils.DirectoryExists(UsersHome))
                {
                    FileUtils.CreateDirectory(UsersHome);
                }
            }
            catch (Exception ex)
            {
                messages.Add(String.Format("Folder '{0}' could not be created: {1}",
                     UsersHome, ex.Message));
            }
            return messages.ToArray();
        }

        public override void DeleteServiceItems(ServiceProviderItem[] items)
        {
            foreach (ServiceProviderItem item in items)
            {
                try
                {
                    if (item is HomeFolder)
                        // delete home folder
                        DeleteFile(item.Name);
                    else if (item is SystemDSN)
                        // delete DSN
                        DeleteDSN(item.Name);
                }
                catch (Exception ex)
                {
                    Log.WriteError(String.Format("Error deleting '{0}' {1}", item.Name, item.GetType().Name), ex);
                }
            }
        }

        public override ServiceProviderItemDiskSpace[] GetServiceItemsDiskSpace(ServiceProviderItem[] items)
        {
            List<ServiceProviderItemDiskSpace> itemsDiskspace = new List<ServiceProviderItemDiskSpace>();
            foreach (ServiceProviderItem item in items)
            {
                if (item is HomeFolder)
                {
                    try
                    {
                        string path = item.Name;

                        Log.WriteStart(String.Format("Calculating '{0}' folder size", path));

                        // calculate disk space
                        ServiceProviderItemDiskSpace diskspace = new ServiceProviderItemDiskSpace();
                        diskspace.ItemId = item.Id;
                        diskspace.DiskSpace = FileUtils.CalculateFolderSize(path);
                        itemsDiskspace.Add(diskspace);

                        Log.WriteEnd(String.Format("Calculating '{0}' folder size", path));
                    }
                    catch (Exception ex)
                    {
                        Log.WriteError(ex);
                    }
                }
            }
            return itemsDiskspace.ToArray();
        }
        #endregion

        #region Private Helpers
        private void CreateMsSqlDsn(SystemDSN dsn, string driverName)
        {
            // get driver path
            string driver = GetDriverPath(driverName);

            // add ODBC name
            RegisterDSN(dsn.Name, driverName);

            // add ODBC tree
            RegistryKey keyDsn = CreateDSNNode(dsn.Name);
            keyDsn.SetValue("Server", dsn.DatabaseServer);
            keyDsn.SetValue("Driver", driver);
            keyDsn.SetValue("Description", DSN_DESCRIPTION);
            keyDsn.SetValue("Database", dsn.DatabaseName);
            keyDsn.SetValue("LastUser", dsn.DatabaseUser);
            keyDsn.Close();
        }

        private void CreateMySqlDsn(SystemDSN dsn)
        {
            // get driver path
            string driver = GetDriverPath(MYSQL_DRIVER);

            // add ODBC name
            RegisterDSN(dsn.Name, MYSQL_DRIVER);

            // add ODBC tree
            RegistryKey keyDsn = CreateDSNNode(dsn.Name);
            keyDsn.SetValue("SERVER", dsn.DatabaseServer);
            keyDsn.SetValue("Driver", driver);
            keyDsn.SetValue("DESCRIPTION", DSN_DESCRIPTION);
            keyDsn.SetValue("DATABASE", dsn.DatabaseName);
            keyDsn.SetValue("UID", dsn.DatabaseUser);
            keyDsn.SetValue("PWD", dsn.DatabasePassword);
            keyDsn.Close();
        }

        private void CreateMariaDBDsn(SystemDSN dsn)
        {
            // get driver path
            string driver = GetDriverPath(MARIADB_DRIVER);

            // add ODBC name
            RegisterDSN(dsn.Name, MARIADB_DRIVER);

            // add ODBC tree
            RegistryKey keyDsn = CreateDSNNode(dsn.Name);
            keyDsn.SetValue("SERVER", dsn.DatabaseServer);
            keyDsn.SetValue("Driver", driver);
            keyDsn.SetValue("DESCRIPTION", DSN_DESCRIPTION);
            keyDsn.SetValue("DATABASE", dsn.DatabaseName);
            keyDsn.SetValue("UID", dsn.DatabaseUser);
            keyDsn.SetValue("PWD", dsn.DatabasePassword);
            keyDsn.Close();
        }

        private void CreateMsAccessDsn(SystemDSN dsn)
        {
            // get driver path
            string driver = GetDriverPath(MSACCESS_DRIVER);

            // add ODBC name
            RegisterDSN(dsn.Name, MSACCESS_DRIVER);

            // add ODBC tree
            RegistryKey keyDsn = CreateDSNNode(dsn.Name);
            keyDsn.SetValue("Driver", driver);
            keyDsn.SetValue("Description", DSN_DESCRIPTION);
            keyDsn.SetValue("DBQ", dsn.DatabaseName);
            keyDsn.SetValue("DriverId", 25);
            keyDsn.SetValue("FIL", "MS Access;");
            keyDsn.SetValue("SafeTransactions", 0);
            keyDsn.SetValue("UID", dsn.DatabaseUser);
            keyDsn.SetValue("PWD", dsn.DatabasePassword);

            // add "Engines/Jet" subkey
            RegistryKey keyEngines = keyDsn.CreateSubKey("Engines");
            RegistryKey keyJet = keyEngines.CreateSubKey("Jet");
            keyJet.SetValue("ImplicitCommitSync", "");
            keyJet.SetValue("MaxBufferSize", 2048);
            keyJet.SetValue("PageTimeout", 5);
            keyJet.SetValue("Threads", 3);
            keyJet.SetValue("UserCommitSync", "Yes");

            keyJet.Close();
            keyEngines.Close();
            keyDsn.Close();
        }

        private void CreateMsAccess2010Dsn(SystemDSN dsn)
        {
            // get driver path
            string driver = GetDriverPath(MSACCESS2010_DRIVER);

            // add ODBC name
            RegisterDSN(dsn.Name, MSACCESS2010_DRIVER);

            // add ODBC tree
            RegistryKey keyDsn = CreateDSNNode(dsn.Name);
            keyDsn.SetValue("Driver", driver);
            keyDsn.SetValue("Description", DSN_DESCRIPTION);
            keyDsn.SetValue("DBQ", dsn.DatabaseName);
            keyDsn.SetValue("DriverId", 25);
            keyDsn.SetValue("FIL", "MS Access;");
            keyDsn.SetValue("SafeTransactions", 0);
            keyDsn.SetValue("UID", dsn.DatabaseUser);
            keyDsn.SetValue("PWD", dsn.DatabasePassword);

            // add "Engines/Jet" subkey
            RegistryKey keyEngines = keyDsn.CreateSubKey("Engines");
            RegistryKey keyJet = keyEngines.CreateSubKey("Jet");
            keyJet.SetValue("ImplicitCommitSync", "");
            keyJet.SetValue("MaxBufferSize", 2048);
            keyJet.SetValue("PageTimeout", 5);
            keyJet.SetValue("Threads", 3);
            keyJet.SetValue("UserCommitSync", "Yes");

            keyJet.Close();
            keyEngines.Close();
            keyDsn.Close();
        }

        private void CreateExcelDsn(SystemDSN dsn)
        {
            // get driver path
            string driver = GetDriverPath(MSEXCEL_DRIVER);

            // add ODBC name
            RegisterDSN(dsn.Name, MSEXCEL_DRIVER);

            // add ODBC tree
            RegistryKey keyDsn = CreateDSNNode(dsn.Name);
            keyDsn.SetValue("Driver", driver);
            keyDsn.SetValue("Description", DSN_DESCRIPTION);
            keyDsn.SetValue("DBQ", dsn.DatabaseName);
            keyDsn.SetValue("DefaultDir", Path.GetDirectoryName(dsn.DatabaseName));
            keyDsn.SetValue("DriverId", 790);
            keyDsn.SetValue("FIL", "excel 8.0;");
            keyDsn.SetValue("SafeTransactions", 0);
            keyDsn.SetValue("UID", "");
            keyDsn.SetValue("ReadOnly", new byte[] { 1 });

            // add "Engines/Excel" subkey
            RegistryKey keyEngines = keyDsn.CreateSubKey("Engines");
            RegistryKey keyExcel = keyEngines.CreateSubKey("Excel");
            keyExcel.SetValue("ImplicitCommitSync", "");
            keyExcel.SetValue("MaxScanRows", 8);
            keyExcel.SetValue("FirstRowHasNames", new byte[] { 1 });
            keyExcel.SetValue("Threads", 3);
            keyExcel.SetValue("UserCommitSync", "Yes");

            keyExcel.Close();
            keyEngines.Close();
            keyDsn.Close();
        }

        private void CreateExcel2010Dsn(SystemDSN dsn)
        {
            // get driver path
            string driver = GetDriverPath(MSEXCEL2010_DRIVER);

            // add ODBC name
            RegisterDSN(dsn.Name, MSEXCEL2010_DRIVER);

            // add ODBC tree
            RegistryKey keyDsn = CreateDSNNode(dsn.Name);
            keyDsn.SetValue("Driver", driver);
            keyDsn.SetValue("Description", DSN_DESCRIPTION);
            keyDsn.SetValue("DBQ", dsn.DatabaseName);
            keyDsn.SetValue("DefaultDir", Path.GetDirectoryName(dsn.DatabaseName));
            keyDsn.SetValue("DriverId", 790);
            keyDsn.SetValue("FIL", "excel 8.0;");
            keyDsn.SetValue("SafeTransactions", 0);
            keyDsn.SetValue("UID", "");
            keyDsn.SetValue("ReadOnly", new byte[] { 1 });

            // add "Engines/Excel" subkey
            RegistryKey keyEngines = keyDsn.CreateSubKey("Engines");
            RegistryKey keyExcel = keyEngines.CreateSubKey("Excel");
            keyExcel.SetValue("ImplicitCommitSync", "");
            keyExcel.SetValue("MaxScanRows", 8);
            keyExcel.SetValue("FirstRowHasNames", new byte[] { 1 });
            keyExcel.SetValue("Threads", 3);
            keyExcel.SetValue("UserCommitSync", "Yes");

            keyExcel.Close();
            keyEngines.Close();
            keyDsn.Close();
        }

        private void CreateTextDsn(SystemDSN dsn)
        {
            // get driver path
            string driver = GetDriverPath(TEXT_DRIVER);

            // add ODBC name
            RegisterDSN(dsn.Name, TEXT_DRIVER);

            // add ODBC tree
            RegistryKey keyDsn = CreateDSNNode(dsn.Name);
            keyDsn.SetValue("Driver", driver);
            keyDsn.SetValue("Description", DSN_DESCRIPTION);
            keyDsn.SetValue("DefaultDir", dsn.DatabaseName);
            keyDsn.SetValue("DriverId", 27);
            keyDsn.SetValue("FIL", "text;");
            keyDsn.SetValue("SafeTransactions", 0);
            keyDsn.SetValue("UID", "");

            // add "Engines/Text" subkey
            RegistryKey keyEngines = keyDsn.CreateSubKey("Engines");
            RegistryKey keyText = keyEngines.CreateSubKey("Text");
            keyText.SetValue("ImplicitCommitSync", "");
            keyText.SetValue("Threads", 3);
            keyText.SetValue("UserCommitSync", "Yes");

            keyText.Close();
            keyEngines.Close();
            keyDsn.Close();
        }

        private void RegisterDSN(string dsnName, string driverName)
        {
            RegistryKey list = Registry.LocalMachine.OpenSubKey(ODBC_NAMES_KEY, true);

            if (list == null)
            {
                // create "SOFTWARE\ODBC\ODBC.INI\ODBC Data Sources" node
                RegistryKey keyOdbc = Registry.LocalMachine.OpenSubKey(ODBC_SOURCES_KEY, true);
                list = keyOdbc.CreateSubKey(ODBC_SOURCES_KEY_NAME);
            }

            list.SetValue(dsnName, GetDriverName(driverName));
            list.Close();
        }

        private RegistryKey CreateDSNNode(string dsnName)
        {
            RegistryKey root = Registry.LocalMachine.OpenSubKey(ODBC_SOURCES_KEY, true);
            return root.CreateSubKey(dsnName);
        }

        private string GetDriverName(string driverName)
        {
            // get driver path
            string[] keyDrivers = Registry.LocalMachine.OpenSubKey(ODBC_INST_KEY).GetSubKeyNames();

            foreach (string keyDriver in keyDrivers)
            {
                if (keyDriver.ToLower().StartsWith(driverName.ToLower()))
                    return keyDriver;
            }
            return null;
        }

        private string GetDriverPath(string driverName)
        {
            // get driver path
            string[] keyDrivers = Registry.LocalMachine.OpenSubKey(ODBC_INST_KEY).GetSubKeyNames();

            foreach (string keyDriver in keyDrivers)
            {
                if (keyDriver.ToLower().StartsWith(driverName.ToLower()))
                    return (string)Registry.LocalMachine.OpenSubKey(ODBC_INST_KEY + keyDriver).GetValue("Driver");
            }

            throw new Exception(String.Format("'{0}' driver is not installed on the system", driverName));
        }

        private bool IsDriverInstalled(string driverName)
        {
            if (String.IsNullOrEmpty(driverName))
                return false;

            RegistryKey keyDriver = Registry.LocalMachine.OpenSubKey(ODBC_INST_KEY + driverName);
            return (keyDriver != null);
        }
        #endregion

        #region Properties
        protected string PrimaryDomainController
        {
            get { return ProviderSettings["PrimaryDomainController"]; }
        }

        #endregion Properties

        #region Event Viewer
        public virtual List<string> GetLogNames()
        {
            List<string> logs = new List<string>();
            EventLog[] eventLogs = EventLog.GetEventLogs();
            foreach (EventLog eventLog in eventLogs)
            {
                logs.Add(eventLog.Log);
            }
            return logs;
        }

        public virtual List<SystemLogEntry> GetLogEntries(string logName)
        {
            SystemLogEntriesPaged result = new SystemLogEntriesPaged();
            List<SystemLogEntry> entries = new List<SystemLogEntry>();

            if (String.IsNullOrEmpty(logName))
                return entries;

            EventLog log = new EventLog(logName);
            EventLogEntryCollection logEntries = log.Entries;
            int count = logEntries.Count;

            // iterate in reverse order
            for (int i = count - 1; i >= 0; i--)
                entries.Add(CreateLogEntry(logEntries[i], false));

            return entries;
        }

        public SystemLogEntriesPaged GetLogEntriesPaged(string logName, int startRow, int maximumRows)
        {
            SystemLogEntriesPaged result = new SystemLogEntriesPaged();
            List<SystemLogEntry> entries = new List<SystemLogEntry>();

            if (String.IsNullOrEmpty(logName))
            {
                result.Count = 0;
                result.Entries = new SystemLogEntry[] { };
                return result;
            }

            EventLog log = new EventLog(logName);
            EventLogEntryCollection logEntries = log.Entries;
            int count = logEntries.Count;
            result.Count = count;

            // iterate in reverse order
            startRow = count - 1 - startRow;
            int endRow = startRow - maximumRows + 1;
            if (endRow < 0)
                endRow = 0;

            for (int i = startRow; i >= endRow; i--)
                entries.Add(CreateLogEntry(logEntries[i], true));

            result.Entries = entries.ToArray();

            return result;
        }

        public void ClearLog(string logName)
        {
            EventLog log = new EventLog(logName);
            log.Clear();
        }

        private SystemLogEntry CreateLogEntry(EventLogEntry logEntry, bool includeMessage)
        {
            SystemLogEntry entry = new SystemLogEntry();
            switch (logEntry.EntryType)
            {
                case EventLogEntryType.Error: entry.EntryType = SystemLogEntryType.Error; break;
                case EventLogEntryType.Warning: entry.EntryType = SystemLogEntryType.Warning; break;
                case EventLogEntryType.Information: entry.EntryType = SystemLogEntryType.Information; break;
                case EventLogEntryType.SuccessAudit: entry.EntryType = SystemLogEntryType.SuccessAudit; break;
                case EventLogEntryType.FailureAudit: entry.EntryType = SystemLogEntryType.FailureAudit; break;
            }

            entry.Created = logEntry.TimeGenerated;
            entry.Source = logEntry.Source;
            entry.Category = logEntry.Category;
            entry.EventID = logEntry.InstanceId;
            entry.UserName = logEntry.UserName;
            entry.MachineName = logEntry.MachineName;

            if (includeMessage)
                entry.Message = logEntry.Message;

            return entry;
        }
        #endregion

        #region Terminal connections
        public TerminalSession[] GetTerminalServicesSessions()
        {
            try
            {
                Log.WriteStart("GetTerminalServicesSessions");
                List<TerminalSession> sessions = new List<TerminalSession>();
                string ret = OS.Shell.Default.Exec("qwinsta").Output().Result;

                // parse returned string
                StringReader reader = new StringReader(ret);
                string line = null;
                int lineIndex = 0;
                while ((line = reader.ReadLine()) != null)
                {
                    /*if (line.IndexOf("USERNAME") != -1 )
                        continue;*/
                    //
                    if (lineIndex == 0)
                    {
                        lineIndex++;
                        continue;
                    }

                    Regex re = new Regex(@"(\S+)\s+", RegexOptions.Multiline | RegexOptions.IgnoreCase);
                    MatchCollection matches = re.Matches(line);

                    // add row to the table
                    string username = matches[1].Value.Trim();
                    if (Regex.IsMatch(username, "^[0-9]*$"))
                    {
                        username = "";
                    }

                    if (username != "")
                    {
                        TerminalSession session = new TerminalSession();
                        //
                        session.SessionId = Int32.Parse(matches[2].Value.Trim());
                        session.Username = username;
                        session.Status = matches[3].Value.Trim();

                        sessions.Add(session);
                    }
                    //
                    lineIndex++;
                }
                reader.Close();

                Log.WriteEnd("GetTerminalServicesSessions");
                return sessions.ToArray();
            }
            catch (Exception ex)
            {
                Log.WriteError("GetTerminalServicesSessions", ex);
                throw;
            }
        }

        public void CloseTerminalServicesSession(int sessionId)
        {
            try
            {
                Log.WriteStart("CloseTerminalServicesSession");
                OS.Shell.Default.Exec($"rwinsta {sessionId}");
                Log.WriteEnd("CloseTerminalServicesSession");
            }
            catch (Exception ex)
            {
                Log.WriteError("CloseTerminalServicesSession", ex);
                throw;
            }
        }
        #endregion

        #region Windows Processes
        public OSProcess[] GetOSProcesses()
        {
            try
            {
                WmiHelper wmi = new WmiHelper("root\\cimv2");
                ManagementObjectCollection objProcesses = wmi.ExecuteQuery(
                    "SELECT * FROM Win32_Process");

                // get processes
                var processes = objProcesses
                    .OfType<ManagementObject>()
                    .Select(m =>
                    {
                        int pid = int.Parse(m["ProcessID"].ToString());
                        string name = m["Name"].ToString();

                        // get user info
                        string username = "";
                        try
                        {
                            string[] methodParams = new string[2];
                            m.InvokeMethod("GetOwner", (object[])methodParams);
                            username = methodParams[0];
                        }
                        catch { }

                        var args = m["CommandLine"] as string ?? "";
                        string cmd = "";
                        if (args.Length > 0 && args[0] == '"')
                        {
                            var pos = args.IndexOf('"', 1);
                            if (pos > 0)
                            {
                                cmd = args.Substring(1, pos);
                                args = args.Substring(pos + 1);
                            }
                            else
                            {
                                cmd = args;
                                args = "";
                            }
                        }
                        else
                        {
                            var pos = args.IndexOf(' ');
                            if (pos > 0)
                            {
                                cmd = args.Substring(0, pos);
                                args = args.Substring(pos + 1);
                            }
                            else
                            {
                                cmd = args;
                                args = "";
                            }
                        }
                        args = args.Trim();

                        return new OSProcess()
                        {
                            Pid = pid,
                            Name = name,
                            Username = username,
                            MemUsage = long.Parse(m["WorkingSetSize"].ToString()),
                            Arguments = args,
                            Command = cmd
                        };
                    });

                var cpuUsageCollection = wmi.ExecuteQuery("SELECT * FROM Win32_PerfFormattedData_PerfProc_Process");
                var cpuUsages = cpuUsageCollection
                    .OfType<ManagementObject>()
                    .Select(m =>
                    {
                        var pid = int.Parse(m["IDProcess"].ToString());
                        var cpuUsage = float.Parse(m["PercentProcessorTime"].ToString()) / 100 / Environment.ProcessorCount;
                        return new
                        {
                            Pid = pid,
                            CpuUsage = cpuUsage
                        };
                    });

                return processes
                    // outer join processes with cpuUsages
                    .GroupJoin(cpuUsages, p => p.Pid, u => u.Pid, (p, u) =>
                    new
                    {
                        CpuUsages = u,
                        OSProcess = p
                    })
                    .SelectMany(
                        c => c.CpuUsages.DefaultIfEmpty().Take(1),
                        (p, cpu) =>
                        {
                            p.OSProcess.CpuUsage = cpu?.CpuUsage ?? 0;
                            return p.OSProcess;
                        }
                    )
                    .OrderBy(p => p.Name)
                    .ToArray();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public void TerminateOSProcess(int pid)
        {
            try
            {
                Process[] processes = Process.GetProcesses();
                foreach (Process process in processes)
                {
                    if (process.Id == pid)
                        process.Kill();
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        #endregion

        #region Windows Services
        public OSService[] GetOSServices()
        {
            try
            {
                List<OSService> winServices = new List<OSService>();

                System.ServiceProcess.ServiceController[] services = System.ServiceProcess.ServiceController.GetServices();
                foreach (var service in services)
                {
                    OSService winService = new OSService();
                    winService.Id = service.ServiceName;
                    winService.Name = service.DisplayName;
                    winService.Description = service.DisplayName;
                    winService.CanStop = service.CanStop;
                    winService.CanPauseAndContinue = service.CanPauseAndContinue;

                    OSServiceStatus status = OSServiceStatus.ContinuePending;
                    switch (service.Status)
                    {
                        case ServiceControllerStatus.ContinuePending: status = OSServiceStatus.ContinuePending; break;
                        case ServiceControllerStatus.Paused: status = OSServiceStatus.Paused; break;
                        case ServiceControllerStatus.PausePending: status = OSServiceStatus.PausePending; break;
                        case ServiceControllerStatus.Running: status = OSServiceStatus.Running; break;
                        case ServiceControllerStatus.StartPending: status = OSServiceStatus.StartPending; break;
                        case ServiceControllerStatus.Stopped: status = OSServiceStatus.Stopped; break;
                        case ServiceControllerStatus.StopPending: status = OSServiceStatus.StopPending; break;
                    }
                    winService.Status = status;

                    winServices.Add(winService);
                }

                return winServices.ToArray();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public void ChangeOSServiceStatus(string id, OSServiceStatus status)
        {
            try
            {
                // get all services
                System.ServiceProcess.ServiceController[] services = System.ServiceProcess.ServiceController.GetServices();

                // find required service
                foreach (var service in services)
                {
                    if (String.Compare(service.ServiceName, id, true) == 0)
                    {
                        if (status == OSServiceStatus.Paused
                            && service.Status == ServiceControllerStatus.Running)
                            service.Pause();
                        else if (status == OSServiceStatus.Running
                            && service.Status == ServiceControllerStatus.Stopped)
                            service.Start();
                        else if (status == OSServiceStatus.Stopped
                            && ((service.Status == ServiceControllerStatus.Running) ||
                                (service.Status == ServiceControllerStatus.Paused)))
                            service.Stop();
                        else if (status == OSServiceStatus.ContinuePending
                            && service.Status == ServiceControllerStatus.Paused)
                            service.Continue();
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        #endregion

        #region Server informations
        public SystemResourceUsageInfo GetSystemResourceUsageInfo()
        {
            try
            {
                Log.WriteStart("GetSystemResourceUsageInfo");
                return new SystemResourceUsageInfo
                {
                    SystemMemoryInfo = GetSystemMemoryInfo(),
                    ProcessorTimeUsagePercent = GetProcessorTotalProcessorTime(),
                };

            }
            catch (Exception ex)
            {
                Log.WriteError("GetSystemResourceUsageInfo", ex);
                throw;
            }
            finally
            {
                Log.WriteEnd("GetSystemResourceUsageInfo");
            }
        }

        protected short GetProcessorTotalProcessorTime()
        {
            short totalRunTime = 0;
            try
            {
                using (var _powerShell = new PowerShellManager(null))
                {
                    Collection<PSObject> result = null;
                    //this command doesn't support WSMan protocol, if that important, then we can use WMIv2 to get this data. Or use Invoke-Command
                    var cmd = new Command("Get-Counter");
                    cmd.Parameters.Add("Counter", @"\Processor(_Total)\% Processor Time");
                    result = _powerShell.Execute(cmd, true, true);

                    if (result != null && result.Count > 0)
                    {
                        dynamic[] counterSamples = (dynamic[])result[0].Members["CounterSamples"].Value;
                        if (counterSamples != null && counterSamples.Length > 0)
                            return Convert.ToInt16(counterSamples[0].CookedValue);
                    }
                }
            }
            catch (Exception ex)
            {
                Log.WriteError("GetHypervisorLogicalProcessorTotalRunTime", ex);
                //ignore error, return default value
            }

            return totalRunTime;
        }

        public SystemMemoryInfo GetSystemMemoryInfo()
        {
            try
            {
                Log.WriteStart("GetSystemMemoryInfo");
                SystemMemoryInfo memory = new SystemMemoryInfo();

                WmiHelper wmi = new WmiHelper("root\\cimv2");
                ManagementObjectCollection objOses = wmi.ExecuteQuery("SELECT * FROM Win32_OperatingSystem");
                foreach (ManagementObject objOs in objOses)
                {
                    memory.FreePhysicalKB = UInt64.Parse(objOs["FreePhysicalMemory"].ToString());
                    memory.TotalVisibleSizeKB = UInt64.Parse(objOs["TotalVisibleMemorySize"].ToString());
                    memory.TotalVirtualSizeKB = UInt64.Parse(objOs["TotalVirtualMemorySize"].ToString());
                    memory.FreeVirtualKB = UInt64.Parse(objOs["FreeVirtualMemory"].ToString());
                }
                Log.WriteEnd("GetSystemMemoryInfo");
                return memory;
            }
            catch (Exception ex)
            {
                Log.WriteError("GetSystemMemoryInfo", ex);
                throw;
            }
        }

        public virtual bool IsUnix() => false;
        #endregion

        #region System Commands

        public string ExecuteSystemCommand(string user, string password, string path, string args)
        {
            try
            {
                string result = FileUtils.ExecuteSystemCommand(user, password, path, args);
                return result;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        #endregion

        Shell cmd, powershell;
        Installer winget, chocolatey;

        public virtual Installer WinGet => winget != null ? winget : winget = new WinGet();
        public virtual Installer Chocolatey => chocolatey != null ? chocolatey : chocolatey = new Chocolatey();

        public Shell Cmd => cmd != null ? cmd : cmd = new Cmd();

        public Shell PowerShell => powershell != null ? powershell : powershell = new PowerShell();

        public Shell DefaultShell => Cmd;

        public Installer DefaultInstaller => WinGet;

        public OSPlatformInfo GetOSPlatform() => new OSPlatformInfo()
        {
            OSPlatform = OSInfo.OSPlatform,
            IsCore = OSInfo.IsCore
        };

        protected Web.IWebServer webServer = null;
        public virtual Web.IWebServer WebServer
            => webServer ??= ApplySettings((IHostingServiceProvider)Activator.CreateInstance(WebServerType));

        protected virtual Web.IWebServer ApplySettings<T>(T provider) where T : IHostingServiceProvider
        {
            var settings = provider.GetProviderDefaultSettings();
            var hosting = provider as HostingServiceProviderBase;
            hosting.ProviderSettings = new ServiceProviderSettings();
            foreach (var setting in settings)
            {
                hosting.ProviderSettings.Settings.Add(setting.Name, setting.Value);
            }
            return provider as Web.IWebServer;
        }

        ServiceController serviceController = null;
        public virtual ServiceController ServiceController => serviceController ??= new WindowsServiceController();

        public virtual WSLShell WSL => WSLShell.Default;

        static TraceListener defaultTraceListener = null;
        public TraceListener DefaultTraceListener => defaultTraceListener ?? (defaultTraceListener = new FuseCP.Server.Utils.EventLogTraceListener());


        public override bool IsInstalled()
        {
           var version = OSInfo.WindowsVersion;
            return version == WindowsVersion.WindowsServer2016
                || version == WindowsVersion.Windows10;
        }

        public void RebootSystem()
        {
            try
            {
                WmiHelper wmi = new WmiHelper("root\\cimv2");
                ManagementObjectCollection objOses = wmi.ExecuteQuery("SELECT * FROM Win32_OperatingSystem");
                foreach (ManagementObject objOs in objOses)
                {
                    objOs.Scope.Options.EnablePrivileges = true;
                    objOs.InvokeMethod("Reboot", null);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public List<DnsRecordInfo> GetDomainDnsRecords(string domain, string dnsServer, DnsRecordType recordType, int pause)
        {
            const string DnsTimeOutMessage = @"dns request timed out";
            const int DnsTimeOutRetryCount = 3;

            Thread.Sleep(pause);

            //nslookup -type=mx google.com 195.46.39.39
            var command = $"nslookup -type={recordType} {domain} {dnsServer}";

            // execute system command
            string raw = string.Empty;
            int triesCount = 0;

            do
            {
                raw = Shell.Standard.Exec(command).Output().Result;
            }
            while (raw.ToLowerInvariant().Contains(DnsTimeOutMessage) && ++triesCount < DnsTimeOutRetryCount);

            //timeout check 
            if (raw.ToLowerInvariant().Contains(DnsTimeOutMessage))
            {
                return null;
            }

            var records = ParseNsLookupResult(raw, dnsServer, recordType);

            return records.ToList();
        }

        private IEnumerable<DnsRecordInfo> ParseNsLookupResult(string raw, string dnsServer, DnsRecordType recordType)
        {
            const string MxRecordPattern = @"mail exchanger = (.+)";
            const string NsRecordPattern = @"nameserver = (.+)";

            var records = new List<DnsRecordInfo>();

            var recordTypePattern = string.Empty;

            switch (recordType)
            {
                case DnsRecordType.NS:
                    {
                        recordTypePattern = NsRecordPattern;
                        break;
                    }
                case DnsRecordType.MX:
                    {
                        recordTypePattern = MxRecordPattern;
                        break;
                    }
            }

            var regex = new Regex(recordTypePattern, RegexOptions.IgnoreCase);

            foreach (Match match in regex.Matches(raw))
            {
                if (match.Groups.Count != 2)
                {
                    continue;
                }

                var dnsRecord = new DnsRecordInfo
                {
                    Value = match.Groups[1].Value != null ? match.Groups[1].Value.Replace("\r\n", "").Replace("\r", "").Replace("\n", "").ToLowerInvariant().Trim() : null,
                    RecordType = recordType,
                    DnsServer = dnsServer
                };

                records.Add(dnsRecord);
            }

            return records;
        }
        public virtual bool CheckFileServicesInstallation()
        {

            ManagementClass objMC = new ManagementClass("Win32_ServerFeature");
            ManagementObjectCollection objMOC = objMC.GetInstances();

            // 01.09.2015 roland.breitschaft@x-company.de
            // Problem: Method not work on German Systems, because the searched Feature-Name does not exist
            // Fix: Add German String for FSRM-Feature            

            //foreach (ManagementObject objMO in objMOC)
            //    if (objMO.Properties["Name"].Value.ToString().ToLower().Contains("file server resource manager"))
            //        return true;
            foreach (ManagementObject objMO in objMOC)
            {
                var id = objMO.Properties["ID"].Value.ToString().ToLower();
                var name = objMO.Properties["Name"].Value.ToString().ToLower();
                if (id.Contains("72") || id.Contains("104"))
                    return true;
                else if (name.Contains("file server resource manager")
                     || name.Contains("ressourcen-manager f?r dateiserver"))
                    return true;
            }

            return false;
        }

        public virtual void SetQuotaLimitOnFolder(string folderPath, string shareNameDrive, QuotaType quotaType, string quotaLimit, int mode, string wmiUserName, string wmiPassword)
        {
            Log.WriteStart("SetQuotaLimitOnFolder");
            Log.WriteInfo("FolderPath : {0}", folderPath);
            Log.WriteInfo("QuotaLimit : {0}", quotaLimit);

            string path = folderPath;

            if (shareNameDrive != null)
                path = Path.Combine(shareNameDrive + @":\", folderPath);

            Runspace runSpace = null;
            try
            {
                runSpace = OpenRunspace();

                if (path.IndexOfAny(Path.GetInvalidPathChars()) == -1)
                {
                    if (!FileUtils.DirectoryExists(path))
                        FileUtils.CreateDirectory(path);

                    if (quotaLimit.Contains("-"))
                    {
                        RemoveOldQuotaOnFolder(runSpace, path);
                    }
                    else
                    {
                        var quota = CalculateQuota(quotaLimit);

                        switch (mode)
                        {
                            //deleting old quota and creating new one
                            case 0:
                                {
                                    RemoveOldQuotaOnFolder(runSpace, path);
                                    ChangeQuotaOnFolder(runSpace, "New-FsrmQuota", path, quotaType, quota);
                                    break;
                                }
                            //modifying folder quota
                            case 1:
                                {
                                    ChangeQuotaOnFolder(runSpace, "Set-FsrmQuota", path, quotaType, quota);
                                    break;
                                }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log.WriteError("SetQuotaLimitOnFolder", ex);
                throw;
            }
            finally
            {
                CloseRunspace(runSpace);
            }

            Log.WriteEnd("SetQuotaLimitOnFolder");
        }

        public virtual Quota GetQuotaOnFolder(string folderPath, string wmiUserName, string wmiPassword)
        {
            Log.WriteStart("GetQuotaLimitOnFolder");
            Log.WriteInfo("FolderPath : {0}", folderPath);
            
            
            Runspace runSpace = null;
            Quota quota = new Quota();

            try
            {
                runSpace = OpenRunspace();

                if (folderPath.IndexOfAny(Path.GetInvalidPathChars()) == -1)
                {
                    Command cmd = new Command("Get-FsrmQuota");
                    cmd.Parameters.Add("Path", folderPath);
                    var result = ExecuteShellCommand(runSpace, cmd, false);

                    if (result.Count > 0)
                    {
                        quota.Size = ConvertBytesToMB(Convert.ToInt64(GetPSObjectProperty(result[0], "Size")));
                        quota.QuotaType = Convert.ToBoolean(GetPSObjectProperty(result[0], "SoftLimit")) ? QuotaType.Soft : QuotaType.Hard;
                        quota.Usage = ConvertBytesToMB(Convert.ToInt64(GetPSObjectProperty(result[0], "usage")));
                    }
                }
            }
            catch (Exception ex)
            {
                Log.WriteError("GetQuotaLimitOnFolder", ex);
                throw;
            }
            finally
            {
                CloseRunspace(runSpace);
            }

            Log.WriteEnd("GetQuotaLimitOnFolder");

            return quota;
        }

        public virtual Dictionary<string, Quota> GetQuotasForOrganization(string folderPath, string wmiUserName, string wmiPassword)
        {
            Log.WriteStart("GetQuotasLimitsForOrganization");

            // 05.09.2015 roland.breitschaft@x-company.de
            // New: Add LogInfo
            Log.WriteInfo("FolderPath : {0}", folderPath);

            Runspace runSpace = null;
            Quota quota = null;
            var quotas = new Dictionary<string, Quota>();

            try
            {
                runSpace = OpenRunspace();

                Command cmd = new Command("Get-FsrmQuota");

                cmd.Parameters.Add("Path", folderPath + "\\*");
                var result = ExecuteShellCommand(runSpace, cmd, false);                

                if (result.Count > 0)
                {
                    foreach (var element in result)
                    {
                        quota = new Quota();

                        quota.Size = ConvertBytesToMB(Convert.ToInt64(GetPSObjectProperty(element, "Size")));
                        quota.QuotaType = Convert.ToBoolean(GetPSObjectProperty(element, "SoftLimit")) ? QuotaType.Soft : QuotaType.Hard;
                        quota.Usage = ConvertBytesToMB(Convert.ToInt64(GetPSObjectProperty(element, "usage")));

                        quotas.Add(Convert.ToString(GetPSObjectProperty(element, "Path")), quota);
                    }
                }
            }
            catch (Exception ex)
            {
                Log.WriteError("GetQuotasLimitsForOrganization", ex);
                throw;
            }
            finally
            {
                CloseRunspace(runSpace);
            }

            Log.WriteEnd("GetQuotasLimitsForOrganization");

            return quotas;
        }

        public UInt64 CalculateQuota(string quota)
        {
            UInt64 OneKb = 1024;
            UInt64 OneMb = OneKb * 1024;
            UInt64 OneGb = OneMb * 1024;

            UInt64 result = 0;

            // Quota Unit
            if (quota.ToLower().Contains("gb"))
            {
                result = UInt64.Parse(quota.ToLower().Replace("gb", "")) * OneGb;
            }
            else if (quota.ToLower().Contains("mb"))
            {
                result = UInt64.Parse(quota.ToLower().Replace("mb", "")) * OneMb;
            }
            else
            {
                result = UInt64.Parse(quota.ToLower().Replace("kb", "")) * OneKb;
            }

            return result;
        }

        public int ConvertMegaBytesToGB(int megabytes)
        {
            int OneGb = 1024;

            if (megabytes == -1)
                return megabytes;

            return (int)(megabytes/ OneGb);
        }

        public int ConvertBytesToMB(long bytes)
        {
            int OneKb = 1024;
            int OneMb = OneKb * 1024;

            if (bytes == 0)
                return 0;

            return (int)(bytes / OneMb);
        }

        public void RemoveOldQuotaOnFolder(Runspace runSpace, string path)
        {
            try
            {
                runSpace = OpenRunspace();
                if (!string.IsNullOrEmpty(path))
                {
                    Command cmd = new Command("Remove-FsrmQuota");
                    cmd.Parameters.Add("Path", path);
                    ExecuteShellCommand(runSpace, cmd, false);
                }
            }
            catch { /* do nothing */ }
        }

        public void ChangeQuotaOnFolder(Runspace runSpace, string command, string path, QuotaType quotaType, UInt64 quota)
        {
            Command cmd = new Command(command);
            cmd.Parameters.Add("Path", path);
            cmd.Parameters.Add("Size", quota);

            if (quotaType == QuotaType.Soft)
            {
                cmd.Parameters.Add("SoftLimit", true);
            }

            ExecuteShellCommand(runSpace, cmd, false);
        }

        public virtual bool InstallFsrmService()
        {
            Log.WriteStart("InstallFsrmService");

            Runspace runSpace = null;
            try
            {
                runSpace = OpenRunspace();

                Command cmd = new Command("Install-WindowsFeature");
                cmd.Parameters.Add("Name", "FS-Resource-Manager");
                cmd.Parameters.Add("IncludeManagementTools", true);

                ExecuteShellCommand(runSpace, cmd, false);
            }
            catch (Exception ex)
            {
                Log.WriteError("InstallFsrmService", ex);

                return false;
            }
            finally
            {
                Log.WriteEnd("InstallFsrmService");

                CloseRunspace(runSpace);
            }

            return true;
        }

        #region PowerShell integration
        private static InitialSessionState session = null;

        protected virtual Runspace OpenRunspace()
        {
             Log.WriteStart("OpenRunspace");

            if (session == null)
            {
                session = InitialSessionState.CreateDefault();
                session.ImportPSModule(new string[] { "FileServerResourceManager" });
            }
            Runspace runSpace = RunspaceFactory.CreateRunspace(session);
            //
            runSpace.Open();
            //
            runSpace.SessionStateProxy.SetVariable("ConfirmPreference", "none");
            Log.WriteEnd("OpenRunspace");
            return runSpace;
        }

        protected void CloseRunspace(Runspace runspace)
        {
            try
            {
                if (runspace != null && runspace.RunspaceStateInfo.State == RunspaceState.Opened)
                {
                    runspace.Close();
                }
            }
            catch (Exception ex)
            {
                Log.WriteError("Runspace error", ex);
            }
        }

        protected Collection<PSObject> ExecuteShellCommand(Runspace runSpace, Command cmd)
        {
            return ExecuteShellCommand(runSpace, cmd, true);
        }

        protected Collection<PSObject> ExecuteLocalScript(Runspace runSpace,  List<string> scripts, out object[] errors, params string[] moduleImports)
        {
            return ExecuteRemoteScript(runSpace, null ,scripts, out errors, moduleImports);
        }

        protected Collection<PSObject> ExecuteRemoteScript(Runspace runSpace, string hostName, List<string> scripts, out object[] errors, params string[] moduleImports)
        {
            Command invokeCommand = new Command("Invoke-Command");

            if (!string.IsNullOrEmpty(hostName))
            {
                invokeCommand.Parameters.Add("ComputerName", hostName);
            }

            RunspaceInvoke invoke = new RunspaceInvoke();
            string commandString = moduleImports.Any() ? string.Format("import-module {0};", string.Join(",", moduleImports)) : string.Empty;

            commandString = string.Format("{0};{1}", commandString, string.Join(";", scripts.ToArray()));

            ScriptBlock sb = invoke.Invoke(string.Format("{{{0}}}", commandString))[0].BaseObject as ScriptBlock;

            invokeCommand.Parameters.Add("ScriptBlock", sb);

            return ExecuteShellCommand(runSpace, invokeCommand, false, out errors);
        }

        protected Collection<PSObject> ExecuteShellCommand(Runspace runSpace, Command cmd, bool useDomainController)
        {
            object[] errors;
            return ExecuteShellCommand(runSpace, cmd, useDomainController, out errors);
        }

        internal Collection<PSObject> ExecuteShellCommand(Runspace runSpace, Command cmd, out object[] errors)
        {
            return ExecuteShellCommand(runSpace, cmd, true, out errors);
        }

        internal Collection<PSObject> ExecuteShellCommand(Runspace runSpace, Command cmd, bool useDomainController, out object[] errors)
        {
            Log.WriteStart("ExecuteShellCommand");

            // 05.09.2015 roland.breitschaft@x-company.de
            // New: Add LogInfo
            Log.WriteInfo("Command              : {0}", cmd.CommandText);
            foreach (var par in cmd.Parameters)            
                Log.WriteInfo("Parameter            : Name {0}, Value {1}", par.Name, par.Value);
            Log.WriteInfo("UseDomainController  : {0}", useDomainController);

            List<object> errorList = new List<object>();

            if (useDomainController)
            {
                CommandParameter dc = new CommandParameter("DomainController", PrimaryDomainController);
                if (!cmd.Parameters.Contains(dc))
                {
                    cmd.Parameters.Add(dc);
                }
            }

            Collection<PSObject> results = null;
            // Create a pipeline
            Pipeline pipeLine = runSpace.CreatePipeline();
            using (pipeLine)
            {
                // Add the command
                pipeLine.Commands.Add(cmd);
                // Execute the pipeline and save the objects returned.
                results = pipeLine.Invoke();

                // Log out any errors in the pipeline execution
                // NOTE: These errors are NOT thrown as exceptions! 
                // Be sure to check this to ensure that no errors 
                // happened while executing the command.
                if (pipeLine.Error != null && pipeLine.Error.Count > 0)
                {
                    foreach (object item in pipeLine.Error.ReadToEnd())
                    {
                        errorList.Add(item);
                        string errorMessage = string.Format("Invoke error: {0}", item);
                        Log.WriteWarning(errorMessage);
                    }
                }
            }
            pipeLine = null;
            errors = errorList.ToArray();
            Log.WriteEnd("ExecuteShellCommand");
            return results;
        }

        protected object GetPSObjectProperty(PSObject obj, string name)
        {
            return obj.Members[name].Value;
        }

        /// <summary>
        /// Returns the identity of the object from the shell execution result
        /// </summary>
        /// <param name="result"></param>
        /// <returns></returns>
        internal string GetResultObjectIdentity(Collection<PSObject> result)
        {
            Log.WriteStart("GetResultObjectIdentity");
            if (result == null)
                throw new ArgumentNullException("result", "Execution result is not specified");

            if (result.Count < 1)
                throw new ArgumentException("Execution result is empty", "result");

            if (result.Count > 1)
                throw new ArgumentException("Execution result contains more than one object", "result");

            PSMemberInfo info = result[0].Members["Identity"];
            if (info == null)
                throw new ArgumentException("Execution result does not contain Identity property", "result");

            string ret = info.Value.ToString();
            Log.WriteEnd("GetResultObjectIdentity");
            return ret;
        }

        internal string GetResultObjectDN(Collection<PSObject> result)
        {
            Log.WriteStart("GetResultObjectDN");
            if (result == null)
                throw new ArgumentNullException("result", "Execution result is not specified");

            if (result.Count < 1)
                throw new ArgumentException("Execution result does not contain any object");

            if (result.Count > 1)
                throw new ArgumentException("Execution result contains more than one object");

            PSMemberInfo info = result[0].Members["DistinguishedName"];
            if (info == null)
                throw new ArgumentException("Execution result does not contain DistinguishedName property", "result");

            string ret = info.Value.ToString();
            Log.WriteEnd("GetResultObjectDN");
            return ret;
        }


        #endregion

        protected virtual Type WebServerType => Type.GetType("FuseCP.Providers.Web.IIs100, FuseCP.Providers.Web.IIs100");
    }
}
