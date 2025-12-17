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
using System.IO;
using FuseCP.Providers.OS;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;

namespace FuseCP.UniversalInstaller
{

    /// <summary>
    /// File utils.
    /// </summary>
    public sealed class FileUtils
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        private FileUtils()
        {
        }

        /// <summary>
        /// Creates drectory with the specified directory.
        /// </summary>
        /// <param name="path">The directory path to create.</param>
        public static void CreateDirectory(string path)
        {
            string dir = Path.GetDirectoryName(path);
            if(!Directory.Exists(dir))
            {
                // create directory structure
                Directory.CreateDirectory(dir);
            }
        }

        /// <summary>
        /// Saves file content.
        /// </summary>
        /// <param name="fileName">File name.</param>
        /// <param name="content">The array of bytes to write.</param>
        public static void SaveFileContent(string fileName, byte[] content)
        {
            FileStream stream = new FileStream(fileName, FileMode.Create);
            stream.Write(content, 0, content.Length);
            stream.Close();
        }

        /// <summary>
        /// Saves file content.
        /// </summary>
        /// <param name="fileName">File name.</param>
        /// <param name="content">The array of bytes to write.</param>
        public static void AppendFileContent(string fileName, byte[] content)
        {
            if (Directory.Exists(Path.GetDirectoryName(fileName)))
            {
                FileStream stream = new FileStream(fileName, FileMode.Append, FileAccess.Write);
                stream.Write(content, 0, content.Length);
                stream.Close();
            }
        }

        /// <summary>
        /// Deletes the specified file.
        /// </summary>
        /// <param name="fileName">The name of the file to be deleted.</param>
        public static void DeleteFile(string fileName)
        {
            int attempts = 0;
            while (true)
            {
                try
                {
                    DeleteFileInternal(fileName);
                    break;
                }
                catch (Exception)
                {
                    if (attempts > 2)
                        throw;

                    attempts++;
                    System.Threading.Thread.Sleep(1000);
                }
            }
        }

        /// <summary>
        /// Deletes the specified file.
        /// </summary>
        /// <param name="fileName">The name of the file to be deleted.</param>
        private static void DeleteReadOnlyFile(string fileName)
        {
            FileInfo info = new FileInfo(fileName);
            info.Attributes = FileAttributes.Normal;
            info.Delete();
        }

        /// <summary>
        /// Deletes the specified file.
        /// </summary>
        /// <param name="fileName">The name of the file to be deleted.</param>
        private static void DeleteFileInternal(string fileName)
        {
            try
            {
                File.Delete(fileName);
            }
            catch (UnauthorizedAccessException)
            {
                DeleteReadOnlyFile(fileName);
            }
        }

        /// <summary>
        /// Deletes the specified directory.
        /// </summary>
        /// <param name="directory">The name of the directory to be deleted.</param>
        public static void DeleteDirectory(string directory)
        {
            if (!Directory.Exists(directory))
                return;

            // iterate through child folders
            string[] dirs = Directory.GetDirectories(directory);
            foreach (string dir in dirs)
            {
                DeleteDirectory(dir);
            }

            // iterate through child files
            string[] files = Directory.GetFiles(directory);
            foreach (string file in files)
            {
                DeleteFile(file);
            }

            //try to delete dir for 3 times
            int attempts = 0;
            while (true)
            {
                try
                {
                    DeleteDirectoryInternal(directory);
                    break;
                }
                catch (Exception)
                {
                    if (attempts > 2)
                        throw;

                    attempts++;
                    System.Threading.Thread.Sleep(1000);
                }
            }
        }

        /// <summary>
        /// Deletes the specified directory.
        /// </summary>
        /// <param name="directory">The name of the directory to be deleted.</param>
        private static void DeleteDirectoryInternal(string directory)
        {
            try
            {
                Directory.Delete(directory);
            }
            catch (IOException)
            {
                DeleteReadOnlyDirectory(directory);
            }
        }

        /// <summary>
        /// Deletes the specified directory.
        /// </summary>
        /// <param name="directory">The name of the directory to be deleted.</param>
        private static void DeleteReadOnlyDirectory(string directory)
        {
            DirectoryInfo info = new DirectoryInfo(directory);
            info.Attributes = FileAttributes.Normal;
            info.Delete();
        }

        /// <summary>
        /// Determines whether the specified file exists.
        /// </summary>
        /// <param name="fileName">The path to check.</param>
        /// <returns></returns>
        public static bool FileExists(string fileName)
        {
            return File.Exists(fileName);
        }

        /// <summary>
        /// Determines whether the given path refers to an existing directory on disk.
        /// </summary>
        /// <param name="path">The path to test.</param>
        /// <returns></returns>
        public static bool DirectoryExists(string path)
        {
            return Directory.Exists(path);
        }
        
        /// <summary>
        /// Returns current application path.
        /// </summary>
        /// <returns>Curent application path.</returns>
        public static string GetCurrentDirectory()
        {
            return AppDomain.CurrentDomain.BaseDirectory;
        }

        /// <summary>
        /// Returns application temp directory.
        /// </summary>
        /// <returns>Application temp directory.</returns>
        public static string GetTempDirectory()
        {
            return Path.Combine(GetCurrentDirectory(), "Tmp");
        }

        /// <summary>
        /// Returns application data directory.
        /// </summary>
        /// <returns>Application data directory.</returns>
        public static string GetDataDirectory()
        {
            return Path.Combine(GetCurrentDirectory(), "Data");
        }

        /// <summary>
        /// Deletes application's Data folder.
        /// </summary>
        public static void DeleteDataDirectory()
        {
            try
            {
                DeleteDirectory(GetDataDirectory());
            }
            catch (Exception ex)
            {
                Log.WriteError("IO Error", ex);
            }
        }

        /// <summary>
        /// Deletes application Tmp directory.
        /// </summary>
        public static void DeleteTempDirectory()
        {
            try
            {
                DeleteDirectory(GetTempDirectory());
            }
            catch (Exception ex)
            {
                Log.WriteError("IO Error", ex);
            }
        }

		public static long CalculateFolderSize(string path)
		{
			int files = 0;
			int folders = 0;
			return CalculateFolderSize(path, out files, out folders);
		}

		public static int CalculateFiles(string path)
		{
			int files = 0;
			int folders = 0;
			CalculateFolderSize(path, out files, out folders);
			return files;
		}

		public static long CalculateFolderSize(string path, out int files, out int folders)
		{
			if (OSInfo.IsWindows) return CalculateFolderSizeWindows(path, out files, out folders);
			else
			{
				long size = 0;
				files = 0; folders = 0;
				var infos = new DirectoryInfo(path).EnumerateFileSystemInfos("*.*", SearchOption.AllDirectories);
				foreach (var info in infos)
				{
					if (info is FileInfo)
					{
						size += ((FileInfo)info).Length;
						files++;
					}
					else if (info is DirectoryInfo)
					{
						folders++;
					}
				}
				return size;
			}
		}
		[DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Auto)]
		public static extern bool GetDiskFreeSpaceEx(string lpDirectoryName,
		   out ulong lpFreeBytesAvailable,
		   out ulong lpTotalNumberOfBytes,
		   out ulong lpTotalNumberOfFreeBytes);

		public static bool GetDiskFreeSpace(string path, out ulong freeBytesAvailable, out ulong totalNumberOfBytes, out ulong totalNumberOfFreeBytes)
		{
			if (OSInfo.IsWindows)
			{
				return GetDiskFreeSpaceEx(path, out freeBytesAvailable, out totalNumberOfBytes, out totalNumberOfFreeBytes);
			}
			else
			{
				path = new DirectoryInfo(path).FullName;

				// on unix use the df command
				var dfoutput = Shell.Default.Exec("df -P").Output().Result;
				if (dfoutput != null)
				{
					var matches = Regex.Matches(dfoutput, @"^[^\s]+\s+(?<total>[0-9]+)\s+(?<used>[0-9]+)\s+(?<available>[0-9]+)\s+(?<capacity>[0-9\.]+)%\s+(?<path>.*)$", RegexOptions.Multiline);
					var drives = matches.OfType<Match>()
						.Select(m => new
						{
							Total = ulong.Parse(m.Groups["total"].Value) * 1024,
							Used = ulong.Parse(m.Groups["used"].Value) * 1024,
							Available = ulong.Parse(m.Groups["available"].Value) * 1024,
							Capacity = float.Parse(m.Groups["capacity"].Value) / 100f,
							Path = m.Groups["path"].Value
						});
					var match = drives
						.Where(m => path.StartsWith(m.Path))
						.OrderByDescending(m => m.Path.Length)
						.FirstOrDefault();

					if (match == null)
					{
						match = drives.OrderByDescending(m => m.Available).FirstOrDefault();
					}

					if (match != null)
					{
						totalNumberOfBytes = match.Total;
						totalNumberOfFreeBytes = match.Available;
						freeBytesAvailable = match.Available;
						return true;
					}
				}
			}
			totalNumberOfFreeBytes = 0;
			totalNumberOfBytes = 0;
			freeBytesAvailable = 0;
			return false;
		}


		public static long CalculateFolderSizeWindows(string path, out int files, out int folders)
		{
			files = 0;
			folders = 0;

			if (!Directory.Exists(path))
				return 0;

			IntPtr INVALID_HANDLE_VALUE = new IntPtr(-1);
			long size = 0;
			FindData findData = new FindData();

			IntPtr findHandle;

			findHandle = Kernel32.FindFirstFile(@"\\?\" + path + @"\*", findData);
			if (findHandle != INVALID_HANDLE_VALUE)
			{

				do
				{
					if ((findData.fileAttributes & (int)FileAttributes.Directory) != 0)
					{

						if (findData.fileName != "." && findData.fileName != "..")
						{
							folders++;

							int subfiles, subfolders;
							string subdirectory = path + (path.EndsWith(@"\") ? "" : @"\") +
								findData.fileName;
							size += CalculateFolderSize(subdirectory, out subfiles, out subfolders);
							folders += subfolders;
							files += subfiles;
						}
					}
					else
					{
						// File
						files++;

						size += (long)findData.nFileSizeLow + (long)findData.nFileSizeHigh * 4294967296;
					}
				}
				while (Kernel32.FindNextFile(findHandle, findData));
				Kernel32.FindClose(findHandle);

			}

			return size;
		}

		public static string SizeToString(long size)
		{
			if (size >= 0x400 && size < 0x100000)
				// kilobytes
				return SizeToKB(size);
			else if (size >= 0x100000 && size < 0x40000000)
				// megabytes
				return SizeToMB(size);
			else if (size >= 0x40000000 && size < 0x10000000000)
				// gigabytes
				return SizeToGB(size);
			else
				return string.Format("{0:0.0}", size);
		}

		public static string SizeToKB(long size)
		{
			return string.Format("{0:0.0} KB", (float)size / 1024);
		}

		public static string SizeToMB(long size)
		{
			return string.Format("{0:0.0} MB", (float)size / 1024 / 1024);
		}

		public static string SizeToGB(long size)
		{
			return string.Format("{0:0.0} GB", (float)size / 1024 / 1024 / 1024);
		}

		#region File Size Calculation helper classes
		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
		class FindData
		{
			public int fileAttributes;
			public int creationTime_lowDateTime;
			public int creationTime_highDateTime;
			public int lastAccessTime_lowDateTime;
			public int lastAccessTime_highDateTime;
			public int lastWriteTime_lowDateTime;
			public int lastWriteTime_highDateTime;
			public int nFileSizeHigh;
			public int nFileSizeLow;
			public int dwReserved0;
			public int dwReserved1;
			[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
			public String fileName;
			[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 14)]
			public String alternateFileName;
		}

		class Kernel32
		{
			[DllImport("Kernel32.dll", CharSet = CharSet.Auto)]
			public static extern IntPtr FindFirstFile(String fileName, [In, Out] FindData findFileData);

			[DllImport("kernel32", CharSet = CharSet.Auto)]
			[return: MarshalAs(UnmanagedType.Bool)]
			public static extern bool FindNextFile(IntPtr hFindFile, [In, Out] FindData lpFindFileData);

			[DllImport("kernel32", CharSet = CharSet.Auto)]
			[return: MarshalAs(UnmanagedType.Bool)]
			public static extern bool FindClose(IntPtr hFindFile);
		}
		#endregion

	}
}
