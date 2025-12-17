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
using System.ServiceModel;
using System.Data;
using System.Xml;
using System.Xml.Linq;
using FuseCP.EnterpriseServer.Data;

namespace FuseCP.UniversalInstaller
{
	public class InstallerWebService : InstallerService_SoapClient, IInstallerWebService
	{
		public InstallerWebService(string url) : base(
			new BasicHttpBinding() { MaxReceivedMessageSize = Core.SetupLoader.ChunkSize * 2 },
			new EndpointAddress(url)) {
			Url = url;
		}

		public new string Url { get; private set; }
		private Stream XStream(XElement xml)
		{
			var stream = new MemoryStream();
			var writer = XmlWriter.Create(stream);
			xml.WriteTo(writer);
			writer.Flush();
			stream.Seek(0, SeekOrigin.Begin);
			return stream;
		}
		protected DataSet DataSet(ArrayOfXElement xml)
		{
			if (xml == null) return null;

			var set = new DataSet();
			using (var stream = XStream(xml.Nodes[0]))
			{
				set.ReadXmlSchema(stream);
			}
			using (var stream = XStream(xml.Nodes[1]))
			{
				set.ReadXml(stream);
			}
			return set;
		}

		protected List<T> FromResultCollection<T>(ArrayOfXElement xml) where T : class
			=> ObjectUtils.CreateListFromDataSet<T>(DataSet(xml));
		protected T FromResult<T>(ArrayOfXElement xml) where T : class => FromResultCollection<T>(xml).FirstOrDefault();

		public new ComponentUpdateInfo GetComponentUpdate(string componentCode, string release)
			=> FromResult<ComponentUpdateInfo>(base.GetComponentUpdate(componentCode, release));
		public new async Task<ComponentUpdateInfo> GetComponentUpdateAsync(string componentCode, string release)
			=> FromResult<ComponentUpdateInfo>(await base.GetComponentUpdateAsync(componentCode, release));

		private void Filter(List<ComponentInfo> result)
		{
			// exclude Standalone vs. other components
			if (Installer.Current.Settings.Installer.InstalledComponents.Any(c => c.ComponentCode == "standalone"))
			{
				result.RemoveAll(c => c.ComponentCode == "enterprise server" || c.ComponentCode == "serverunix" || c.ComponentCode == "server" ||
						c.ComponentCode == "portal" || c.ComponentCode == "WebDavPortal" || c.ComponentCode == "serveraspv2");
			}
			else if (Installer.Current.Settings.Installer.InstalledComponents.Any(c => c.ComponentCode == "enterprise server" || c.ComponentCode == "serverunix" || c.ComponentCode == "server" ||
						c.ComponentCode == "portal" || c.ComponentCode == "WebDavPortal" || c.ComponentCode == "serveraspv2"))
			{
				result.RemoveAll(c => c.ComponentCode == "standalone");
			}
		}
		public new List<ComponentInfo> GetAvailableComponents()
		{
			var result = FromResultCollection<ComponentInfo>(base.GetAvailableComponents());
			foreach (var component in result) component.VersionName = component.Version.ToString(3);
			Filter(result);
			return result;
		}
		public new async Task<List<ComponentInfo>> GetAvailableComponentsAsync()
		{
			var result = FromResultCollection<ComponentInfo>(await base.GetAvailableComponentsAsync());
			foreach (var component in result) component.VersionName = component.Version.ToString(3);
			Filter(result);
			return result;
		}
		public new ComponentUpdateInfo GetLatestComponentUpdate(string componentCode) => FromResult<ComponentUpdateInfo>(base.GetLatestComponentUpdate(componentCode));
		public new async Task<ComponentUpdateInfo> GetLatestComponentUpdateAsync(string componentCode) => FromResult<ComponentUpdateInfo>(await base.GetLatestComponentUpdateAsync(componentCode));
		public new ReleaseFileInfo GetReleaseFileInfo(string componentCode, string version) => FromResultCollection<ReleaseFileInfo>(base.GetReleaseFileInfo(componentCode, version)).Single();
		public new async Task<ReleaseFileInfo> GetReleaseFileInfoAsync(string componentCode, string version) => FromResultCollection<ReleaseFileInfo>(await base.GetReleaseFileInfoAsync(componentCode, version)).Single();
		public new async Task<byte[]> GetFileChunkAsync(string file, int offset, int size)
			=> (await base.GetFileChunkAsync(file, offset, size)).GetFileChunkResult;

	}
}
