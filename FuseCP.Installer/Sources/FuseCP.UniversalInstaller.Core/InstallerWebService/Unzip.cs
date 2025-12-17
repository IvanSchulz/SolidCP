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
using System.IO.Compression;
using SharpCompress.Archives;
using SharpCompress.Archives.SevenZip;
using SharpCompress.Common;
using SharpCompress.Readers;

namespace FuseCP.UniversalInstaller;

public class Unzip
{
	public static CancellationTokenSource Cancel => Installer.Current.Cancel;
	public static void UnzipFile(string zipFile, string destFolder, Func<string, bool> filter = null, Stream stream = null,
		Action<long, long> progress = null)
	{
		if (zipFile.EndsWith(".7z")) Unzip7zFile(zipFile, destFolder, filter, stream, progress);
		else UnzipZipFile(zipFile, destFolder, filter, stream, progress);
	}
	public static void Unzip7zFile(string zipFile, string destFolder, Func<string, bool> filter = null, Stream stream = null,
		Action<long, long> progress = null)
	{
		try
		{
			if (filter == null) filter = name => true;

			Log.WriteStart("Unzipping file");
			Log.WriteInfo(string.Format("Unzipping file \"{0}\" to the folder \"{1}\"", zipFile, destFolder));

			using (var file = stream ?? new FileStream(zipFile, System.IO.FileMode.Open, FileAccess.Read))
			using (var zip = SevenZipArchive.Open(file))
			{
				long zipSize = zip.TotalUncompressSize;
				long unzipped = 0;

				int files = 0;

				var reader = zip.ExtractAllEntries();
				while (reader.MoveToNextEntry())
				{
					if (Cancel.IsCancellationRequested) break;

					if (filter(reader.Entry.Key))
					{
						try
						{
							if (reader.Entry.IsDirectory) Directory.CreateDirectory(Path.Combine(destFolder, reader.Entry.Key.Replace('/', Path.DirectorySeparatorChar)));
							else reader.WriteEntryToDirectory(destFolder, new ExtractionOptions() { ExtractFullPath = true, Overwrite = true });
						}
						catch (Exception ex)
						{
							throw;
						}
						
						files++;
						unzipped += reader.Entry.Size;

						if (zipSize != 0)
						{
							progress?.Invoke(unzipped, zipSize);
						}
					}
				}

				Installer.Current.Files = files;

				progress?.Invoke(zipSize, zipSize);
				Log.WriteEnd("Unzipped file");
			}
		}
		catch (Exception ex)
		{
			if (ex is ThreadAbortException)
				return;

			throw;
		}
	}
	public static void UnzipZipFile(string zipFile, string destFolder, Func<string, bool> filter = null, Stream stream = null,
		Action<long, long> progress = null)
	{
		try
		{
			if (filter == null) filter = name => true;

			Log.WriteStart("Unzipping file");
			Log.WriteInfo(string.Format("Unzipping file \"{0}\" to the folder \"{1}\"", zipFile, destFolder));

			using (var file = stream ?? new FileStream(zipFile, System.IO.FileMode.Open, FileAccess.Read))
			using (var zip = new ZipArchive(file))
			{
				long zipSize = file.Length;
				long unzipped = 0;

				int files = 0;

				foreach (var entry in zip.Entries)
				{
					if (Cancel.IsCancellationRequested) break;

					if (filter(entry.FullName))
					{
						if (string.IsNullOrEmpty(entry.Name))
						{
							Directory.CreateDirectory(Path.Combine(destFolder, entry.FullName.Replace('/', Path.DirectorySeparatorChar)));
						}
						else
						{
							entry.ExtractToFile(Path.Combine(destFolder, entry.FullName.Replace('/', Path.DirectorySeparatorChar)), true);
							files++;
						}
					}
					else if (!string.IsNullOrEmpty(entry.Name)) files++;

					unzipped += entry.CompressedLength;

					if (zipSize != 0) progress?.Invoke(unzipped, zipSize);
				}

				Installer.Current.Files = files;

				progress?.Invoke(zipSize, zipSize);

				Log.WriteEnd("Unzipped file");
			}
		}
		catch (Exception ex)
		{
			if (ex is ThreadAbortException)
				return;
			throw;
		}
	}
}
