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
using System.IO;
using System.IO.Compression;
using System.Collections.Generic;
using System.Text;

namespace FuseCP.EnterpriseServer;

    public class FileUtils
    {
	#region Zip/Unzip Methods

	public static void ZipFiles(string zipFile, string rootPath, string[] files)
	{
		using (var stream = new FileStream(zipFile, FileMode.Truncate, FileAccess.Write))
		using (var zip = new ZipArchive(stream, ZipArchiveMode.Create, false, Encoding.UTF8))
		{
			foreach (string file in files)
			{
				string fullPath = Path.Combine(rootPath, file);
				if (Directory.Exists(fullPath))
				{
					//add directory with the same directory name
					zip.CreateEntry(file.Replace('\\', '/') + "/", CompressionLevel.Optimal);
				}
				else if (File.Exists(fullPath))
				{
					//add file to the root folder
					zip.CreateEntryFromFile(fullPath, file.Replace('\\', '/'), CompressionLevel.Optimal);
				}
			}
		}
	}

	public static List<string> UnzipFiles(string zipFile, string destFolder)
	{
		ZipFile.ExtractToDirectory(zipFile, destFolder);

		// return extracted files names
		return GetFileNames(destFolder);
	}

	#endregion

	#region Copy
	public static void CopyDirectoryContentUNC(string sourceDirectory, string destinationDirectory) {
            foreach(string dir in Directory.GetDirectories(sourceDirectory, "*", SearchOption.AllDirectories)) {
                string destinationPath = dir.Replace(sourceDirectory, destinationDirectory);
                if(!Directory.Exists(destinationPath)) { 
                    Directory.CreateDirectory(destinationPath);
                }
            }
            
            foreach(string file in Directory.GetFiles(sourceDirectory, "*.*", SearchOption.AllDirectories)) {
                string destinationPath = file.Replace(sourceDirectory, destinationDirectory);
                File.Copy(file, destinationPath, true);
            }
        }

        #endregion

        #region Helper Functions

        /// <summary>
        /// This function enumerates all directories and files of the <paramref name="direcrotyPath"/> specified.
        /// </summary>
        /// <param name="direcrotyPath">Path to the directory.</param>
        /// <returns>
        /// List of files and directories reside for the <paramref name="direcrotyPath"/> specified.
        /// Empty, when no files and directories are or path does not exists.
        /// </returns>
        public static List<string> GetFileNames(string direcrotyPath)
	{
		List<string> items = new List<string>();

		DirectoryInfo root = new DirectoryInfo(direcrotyPath);
		if (root.Exists)
		{
			// list directories
			foreach (DirectoryInfo dir in root.GetDirectories())
			{
				items.Add(
					System.IO.Path.Combine(direcrotyPath, dir.Name)
					);
			}

			// list files
			foreach (FileInfo file in root.GetFiles())
			{
				items.Add(
					System.IO.Path.Combine(direcrotyPath, file.Name)
					);
			}
		}

		return items;
	}

	#endregion
}
