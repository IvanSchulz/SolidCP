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
using System.IO;
using System.Windows.Forms;

namespace FuseCP.Setup
{
	/// <summary>
	/// Shows copy process.
	/// </summary>
	public sealed class CopyProcess
	{
		private ProgressBar progressBar;
		private DirectoryInfo sourceFolder;
		private DirectoryInfo destFolder;

		/// <summary>
		/// Initializes a new instance of the class.
		/// </summary>
		/// <param name="bar">Progress bar.</param>
		/// <param name="source">Source folder.</param>
		/// <param name="destination">Destination folder.</param>
		public CopyProcess(object bar, string source, string destination)
		{
            this.progressBar = bar as ProgressBar;
			this.sourceFolder = new DirectoryInfo(source);
			this.destFolder = new DirectoryInfo(destination);
		}

		/// <summary>
		/// Copy source to the destination folder.
		/// </summary>
		internal void Run()
		{
			// unzip
			long totalSize = FileUtils.CalculateFolderSize(sourceFolder.FullName);
			long copied = 0;

            if (progressBar != null)
            {
                progressBar.Minimum = 0;
                progressBar.Maximum = 100;
                progressBar.Value = 0;
            }

			int i = 0;
			List<DirectoryInfo> folders = new List<DirectoryInfo>();
			List<FileInfo> files = new List<FileInfo>();

			DirectoryInfo di = null;
			//FileInfo fi = null;
			string path = null;

			// Part 1: Indexing
			folders.Add(sourceFolder);
			while (i < folders.Count)
			{
				foreach (DirectoryInfo info in folders[i].GetDirectories())
				{
					if (!folders.Contains(info))
						folders.Add(info);
				}
				foreach (FileInfo info in folders[i].GetFiles())
				{
					files.Add(info);
				}
				i++;
			}

			// Part 2: Destination Folders Creation
			///////////////////////////////////////////////////////
			for (i = 0; i < folders.Count; i++)
			{
				if (folders[i].Exists)
				{
					path = destFolder.FullName +
						Path.DirectorySeparatorChar +
						folders[i].FullName.Remove(0, sourceFolder.FullName.Length);

					di = new DirectoryInfo(path);

					// Prevent IOException
					if (!di.Exists)
						di.Create();
				}
			}

			// Part 3: Source to Destination File Copy
			///////////////////////////////////////////////////////
			for (i = 0; i < files.Count; i++)
			{
				if (files[i].Exists)
				{
					path = destFolder.FullName +
						Path.DirectorySeparatorChar +
						files[i].FullName.Remove(0, sourceFolder.FullName.Length + 1);
					FileUtils.CopyFile(files[i], path);
					copied += files[i].Length;
					if (totalSize != 0)
					{
                        if (progressBar != null)
                        {
                            progressBar.Value = Convert.ToInt32(copied * 100 / totalSize);
                        }
					}
				}
			}
		}
	}
}

