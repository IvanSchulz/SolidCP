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

namespace FuseCP.Updater.Common
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
		internal static void CreateDirectory(string path)
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
		internal static void SaveFileContent(string fileName, byte[] content)
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
        internal static void AppendFileContent(string fileName, byte[] content)
        {
            FileStream stream = new FileStream(fileName, FileMode.Append, FileAccess.Write);
            stream.Write(content, 0, content.Length);
            stream.Close();
        }
	    
		
		/// <summary>
		/// Deletes the specified file.
		/// </summary>
		/// <param name="fileName">The name of the file to be deleted.</param>
		internal static void DeleteFile(string fileName)
		{
			File.Delete(fileName);
		}

		/// <summary>
		/// Determines whether the specified file exists.
		/// </summary>
		/// <param name="fileName">The path to check.</param>
		/// <returns></returns>
		internal static bool FileExists(string fileName)
		{
			return File.Exists(fileName);
		}

		/// <summary>
		/// Determines whether the given path refers to an existing directory on disk.
		/// </summary>
		/// <param name="path">The path to test.</param>
		/// <returns></returns>
		internal static bool DirectoryExists(string path)
		{
			return Directory.Exists(path);
		}
		
		/// <summary>
		/// Deletes a directory and its contents.
		/// </summary>
		/// <param name="path">The name of the directory to remove. </param>
		/// <param name="recursive">true to remove directories, subdirectories, and files in path; otherwise, false.</param>
		internal static void DeleteDirectory(string path, bool recursive)
		{
			Directory.Delete(path, recursive);
		}
	}
}
