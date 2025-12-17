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

namespace FuseCP.Installer.Common
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
            FileStream stream = new FileStream(fileName, FileMode.Append, FileAccess.Write);
            stream.Write(content, 0, content.Length);
            stream.Close();
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
        /// Returns application offline directory.
        /// </summary>
        /// <returns>Application offline directory.</returns>
        public static string GetOfflineDataDirectory()
        {
            return Path.Combine(GetCurrentDirectory(), "Offline");
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
    }
}
