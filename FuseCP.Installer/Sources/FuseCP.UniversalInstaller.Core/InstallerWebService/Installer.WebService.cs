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
using System.Net;
using System.Data;
using System.Xml;
using System.Xml.Linq;
using FuseCP.EnterpriseServer.Data;
using FuseCP.Providers.OS;

namespace FuseCP.UniversalInstaller
{
	public interface IInstallerWebService
	{
		public ComponentUpdateInfo GetComponentUpdate(string componentCode, string release);
		public Task<ComponentUpdateInfo> GetComponentUpdateAsync(string componentCode, string release);
		public List<ComponentInfo> GetAvailableComponents();
		public Task<List<ComponentInfo>> GetAvailableComponentsAsync();
		public ComponentUpdateInfo GetLatestComponentUpdate(string componentCode);
		public Task<ComponentUpdateInfo> GetLatestComponentUpdateAsync(string componentCode);
		public ReleaseFileInfo GetReleaseFileInfo(string componentCode, string version);
		public Task<ReleaseFileInfo> GetReleaseFileInfoAsync(string componentCode, string version);
		public byte[] GetFileChunk(string file, int offset, int size);
		public Task<byte[]> GetFileChunkAsync(string file, int offset, int size);
		public long GetFileSize(string file);
		public Task<long> GetFileSizeAsync(string file);
		public string Url { get; }
	}

	public partial class Installer
	{
#if UseDebugWebService
		public const bool UseDebugWebService = true;
#else
		public const bool UseDebugWebService = false;
#endif
		public virtual IInstallerWebService InstallerWebService
		{
			get
			{
				string url = Settings.Installer.WebServiceUrl;
				if (string.IsNullOrEmpty(url)) url = UseDebugWebService ?
                        "http://fusecp.mooo.com/Services/InstallerService-Beta.asmx" :
                        "http://installer.fusecp.com/Services/InstallerService-1.0.asmx";

				var type = GetType($"FuseCP.UniversalInstaller.InstallerWebService, FuseCP.UniversalInstaller.Runtime.{
					(OSInfo.IsCore ? "NetCore" : "NetFX")}");

				var webService = Activator.CreateInstance(type, url) as IInstallerWebService;
				
				// check if we need to add a proxy to access Internet
				if (Settings.Installer.Proxy != null)
				{
					string proxyServer = Settings.Installer.Proxy.Address;
					if (!String.IsNullOrEmpty(proxyServer))
					{
						IWebProxy proxy = new WebProxy(proxyServer);
						var proxyUsername = Settings.Installer.Proxy.Username;
						var proxyPassword = Settings.Installer.Proxy.Password;
						if (!String.IsNullOrEmpty(proxyUsername))
							proxy.Credentials = new NetworkCredential(proxyUsername, proxyPassword);
						WebRequest.DefaultWebProxy = proxy;
					} else
					{
						WebRequest.DefaultWebProxy = null;
					}
				}

				return webService;
			}
		}

		public const string GitHubUrl = "https://github.com/FuseCP/FuseCP";
 		public GitHubReleases GitHub => new GitHubReleases(Settings.Installer.GitHubUrl ?? GitHubUrl);
		public Releases Releases => new Releases();
	}
}
