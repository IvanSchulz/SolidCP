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
using System.Diagnostics;
using System.IO;
using System.Buffers;
using System.Net;
using System.Text;
using NeoSmart.AsyncLock;

namespace FuseCP.UniversalInstaller;

public class Releases
{
	public GitHubReleases GitHub => Installer.Current.GitHub;
	public IInstallerWebService WebService = Installer.Current.InstallerWebService;

	public const int MB = 1024 * 1024;
	public const long ChunkSize = 2 * MB;

	public CancellationTokenSource Cancel = Installer.Current.Cancel;

	public static AsyncLock Lock = new AsyncLock();
	public ComponentUpdateInfo GetComponentUpdate(string componentCode, string release)
		=> GetComponentUpdateAsync(componentCode, release).Result;
	public async Task<ComponentUpdateInfo> GetComponentUpdateAsync(string componentCode, string release)
	{
		using (var alock = await Lock.LockAsync())
		{
			var infos = new[] {
				GitHub.GetComponentUpdateAsync(componentCode, release),
				WebService.GetComponentUpdateAsync(componentCode, release)
			};
			var info = await infos[0];
			if (info == null) return await infos[1];
			return info;
		}
	}
	public List<ComponentInfo> GetAvailableComponents() => GetAvailableComponentsAsync().Result;
	public async Task<List<ComponentInfo>> GetAvailableComponentsAsync()
	{
		using (var alock = await Lock.LockAsync()) {
			var components = new[]
			{
				GitHub.GetAvailableComponentsAsync(),
				WebService.GetAvailableComponentsAsync()
			};
			await Task.WhenAll(components);
			var ghcomponents = await components[0];
			var wscomponents = await components[1];
			if (ghcomponents != null && ghcomponents.Count > 0 &&
				(wscomponents == null || wscomponents.Count == 0 ||
				ghcomponents[0].Version > wscomponents[0].Version)) return ghcomponents;
			else return wscomponents;
		}
	}
	public ComponentUpdateInfo GetLatestComponentUpdate(string componentCode) =>
		GetLatestComponentUpdateAsync(componentCode).Result;
	public async Task<ComponentUpdateInfo> GetLatestComponentUpdateAsync(string componentCode)
	{
		using (var alock = await Lock.LockAsync())
		{
			var infos = new[]
			{
				GitHub.GetLatestComponentUpdateAsync(componentCode),
				WebService.GetLatestComponentUpdateAsync(componentCode)
			};
			var ghinfo = await infos[0];
			var wsinfo = await infos[1];
			if (ghinfo != null &&
				(wsinfo == null || ghinfo.Version > wsinfo.Version)) return ghinfo;
			else return wsinfo;
		}
	}
	public ComponentUpdateInfo GetReleaseFileInfo(string componentCode, string version) =>
		GetReleaseFileInfoAsync(componentCode, version).Result;

	public async Task<ComponentUpdateInfo> GetReleaseFileInfoAsync(string componentCode, string version)
	{
		using (var alock = await Lock.LockAsync())
		{
			var infos = new[]
			{
				GitHub.GetReleaseFileInfoAsync(componentCode, version),
				WebService.GetReleaseFileInfoAsync(componentCode, version)
			};
			var info = await infos[0] ?? await infos[1];
			return info != null ? new ComponentUpdateInfo(info, version) : null;
		}
	}

	public async Task<string> GetDownloadUrlAsync(RemoteFile file)
	{
		if (!string.IsNullOrEmpty(file.DownloadUrl)) return file.DownloadUrl;
		if (file.Release?.GitHub == true) return await GitHub.GetDownloadUrl(file);
		else
		{
			var uri = new Uri(WebService.Url);
			return file.File.Replace("~", $"{uri.Scheme}://{uri.Authority}");
		}
    }
	HttpClientHandler ProxyHandler()
	{
		if (Installer.Current.Settings.Installer.Proxy != null && !string.IsNullOrEmpty(Installer.Current.Settings.Installer.Proxy.Address))
		{
			var settings = Installer.Current.Settings.Installer.Proxy;
			var proxy = new WebProxy(settings.Address, false);  // proxy address + port
			if (!string.IsNullOrEmpty(settings.Username) && !string.IsNullOrEmpty(settings.Password))
			{
				proxy.Credentials = new NetworkCredential(settings.Username, settings.Password); // if proxy needs auth
			}

			// Attach proxy to handler
			return new HttpClientHandler
			{
				Proxy = proxy,
				UseProxy = true
			};
		}
		return null;
	}
	public async Task<long> DownloadFileChunkAsync(string url, long offset, long length, byte[] buffer)
	{
		if (length <= 0) return 0;
		var handler = ProxyHandler();
		using var client = handler != null ? new HttpClient(handler) : new HttpClient();
		client.DefaultRequestHeaders.Range = new System.Net.Http.Headers.RangeHeaderValue(offset, offset + length - 1);
		using var response = await client.GetAsync(url, HttpCompletionOption.ResponseHeadersRead);
		response.EnsureSuccessStatusCode();
		if (response.StatusCode != HttpStatusCode.PartialContent) throw new InvalidDataException("Request did not return a PartialContent status code.");
		using var stream = await response.Content.ReadAsStreamAsync();
		var size = response.Content.Headers.ContentLength ?? 0;
		int totalRead = 0;
		int read;
		while ((read = await stream.ReadAsync(buffer, totalRead, (int)size - totalRead)) > 0)
		{
			totalRead += read;
		}
		return size;
	}
	public async Task<long> DownloadFileChunkAsync(string url, long offset, long length, Stream stream)
	{
		if (length <= 0) return 0;
		var handler = ProxyHandler();
		using var client = handler != null ? new HttpClient(handler) : new HttpClient();
		client.DefaultRequestHeaders.Range = new System.Net.Http.Headers.RangeHeaderValue(offset, offset + length - 1);
		using var response = await client.GetAsync(url, HttpCompletionOption.ResponseHeadersRead);
		response.EnsureSuccessStatusCode();
		if (response.StatusCode != HttpStatusCode.PartialContent) throw new InvalidDataException("Request did not return a PartialContent status code.");
		await response.Content.CopyToAsync(stream);
		return response.Content.Headers.ContentLength ?? 0;
	}
	public async Task<long> GetFileSizeAsync(string url)
	{
		var handler = ProxyHandler();
		using var client = handler != null ? new HttpClient(handler) : new HttpClient();
		var request = new HttpRequestMessage(HttpMethod.Head, url);
		using var response = await client.SendAsync(request);
		response.EnsureSuccessStatusCode();
		return response.Content.Headers.ContentLength ?? -1;
	}

	public void GetFile(RemoteFile file, string destinationFile, Action<long, long> progress = null) =>
		Task.Run(() => GetFileAsync(file, destinationFile, progress)).Wait();
	public async Task GetFileAsync(RemoteFile file, string destinationFile, Action<long, long> progress = null)
	{
		using (var alock = await Lock.LockAsync())
		{
			if (file.Release?.GitHub == true) await GitHub.GetFileAsync(file, destinationFile, progress);
			else
			{
				var url = await GetDownloadUrlAsync(file);

				var destinationPath = Path.GetDirectoryName(destinationFile);
				if (!Directory.Exists(destinationPath)) Directory.CreateDirectory(destinationPath);

				long downloaded = 0;
				long fileSize = await GetFileSizeAsync(url);

				if (fileSize == 0)
				{
					throw new FileNotFoundException("Service returned empty file.", file.File);
				}

				using (var fileStream = new FileStream(destinationFile, FileMode.Create, FileAccess.Write))
				{
					while (downloaded < fileSize)
					{
						// Throw OperationCancelledException if there is an incoming cancel request
						Installer.Current.Cancel.Token.ThrowIfCancellationRequested();

						var size = await DownloadFileChunkAsync(url, downloaded, ChunkSize, fileStream);

						downloaded += size;

						progress?.Invoke(downloaded, fileSize);

						if (size < ChunkSize) break;
					}
				}
			}
		}
	}
	public async Task GetFileAndUnzipAsync(RemoteFile file, string destinationFile, string destinationPath, Func<string, bool> filter = null, 
		Action<long, long> progress = null)
	{
		using (var alock = await Lock.LockAsync())
		{
			var url = await GetDownloadUrlAsync(file);

			if (string.IsNullOrEmpty(destinationPath)) destinationPath = Path.GetDirectoryName(destinationFile);
			if (!Directory.Exists(destinationPath)) Directory.CreateDirectory(destinationPath);

			if (url.EndsWith(".7z", StringComparison.OrdinalIgnoreCase) ||
				url.EndsWith(".zip", StringComparison.OrdinalIgnoreCase))
			{
				var singleFile = url.EndsWith(".dll.zip", StringComparison.OrdinalIgnoreCase) ||
					url.EndsWith(".dll.7z", StringComparison.OrdinalIgnoreCase);				
				using (var stream = new SeekableDownloadStream(this, url, destinationFile + ".tmp", true, singleFile ? progress : null))
				{
					Unzip.UnzipFile(destinationFile, destinationPath, filter, stream, singleFile ?  null : progress);
				}
			}
			else
			{
				await GetFileAsync(file, destinationFile, progress);
			}
		}
	}
}
