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
using System.Windows.Forms;
using System.Collections.Generic;
using System.Text;

using Ionic.Zip;

namespace FuseCP.Setup.Common
{
	/// <summary>
	/// Shows progress of file zipping.
	/// </summary>
	public sealed class ZipIndicator
	{
		private ProgressBar progressBar;
		string sourcePath;
		string zipFile;
		int totalFiles = 0;
		int files = 0;

		public ZipIndicator(object progressBar, string sourcePath, string zipFile)
		{
			this.progressBar = progressBar as ProgressBar;
			this.sourcePath = sourcePath;
			this.zipFile = zipFile;
		}

		public void Start()
		{
			totalFiles = FileUtils.CalculateFiles(sourcePath);
			using (ZipFile zip = new ZipFile())
			{
				zip.AddProgress += ShowProgress;
				zip.AlternateEncoding = Encoding.UTF8;
				zip.AlternateEncodingUsage = ZipOption.AsNecessary;
				zip.AddDirectory(sourcePath);
				zip.Save(zipFile);
			}
		}

		private void ShowProgress(object sender, AddProgressEventArgs e)
		{
			if (e.EventType == ZipProgressEventType.Adding_AfterAddEntry)
			{
				string fileName = e.CurrentEntry.FileName;
				files++;
                if (this.progressBar != null)
                {
                    this.progressBar.Value = Convert.ToInt32(files * 100 / totalFiles);
                    this.progressBar.Update();
                }
			}
		}
	}
}
