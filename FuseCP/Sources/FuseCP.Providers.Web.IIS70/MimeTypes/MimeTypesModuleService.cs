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
using FuseCP.Providers.Web.Iis.Common;
using Microsoft.Web.Administration;
using FuseCP.Providers.Web.Iis.Utility;

namespace FuseCP.Providers.Web.MimeTypes
{
	internal sealed class MimeTypesModuleService : ConfigurationModuleService
	{
		public const string FileExtensionAttribute = "fileExtension";
		public const string MimeTypeAttribute = "mimeType";

		/// <summary>
		/// Loads available mime maps into supplied virtual iisDirObject description.
		/// </summary>
		/// <param name="vdir">Virtual iisDirObject description.</param>
		public void GetMimeMaps(ServerManager srvman, WebAppVirtualDirectory virtualDir)
		{
			var config = srvman.GetWebConfiguration(virtualDir.FullQualifiedPath);
			//
			var section = config.GetSection(Constants.StaticContentSection);
			//
			var mappings = new List<MimeMap>();
			//
			foreach (var item in section.GetCollection())
			{
				var item2Get = GetMimeMap(item);
				//
				if (item2Get == null)
					continue;
				//
				mappings.Add(item2Get);
			}
			//
			virtualDir.MimeMaps = mappings.ToArray();
		}

		/// <summary>
		/// Saves mime types from virtual iisDirObject description into configuration file.
		/// </summary>
		/// <param name="vdir">Virtual iisDirObject description.</param>
		public void SetMimeMaps(WebAppVirtualDirectory virtualDir)
		{
			#region Revert to parent settings (inherited)
			using (var srvman = GetServerManager())
			{
				var config = srvman.GetWebConfiguration(virtualDir.FullQualifiedPath);
				//
				var section = config.GetSection(Constants.StaticContentSection);
				//
				section.RevertToParent();
				//
				srvman.CommitChanges();
			} 
			#endregion

			// Ensure mime maps are set
			if (virtualDir.MimeMaps == null || virtualDir.MimeMaps.Length == 0)
				return;

			#region Put the change in effect
			using (var srvman = GetServerManager())
			{
				var config = srvman.GetWebConfiguration(virtualDir.FullQualifiedPath);
				//
				var section = config.GetSection(Constants.StaticContentSection);
				//
				var typesCollection = section.GetCollection();
				//
				foreach (var item in virtualDir.MimeMaps)
				{
					// Make sure mime-type mapping file extension is formatted exactly as it should be
					if (!item.Extension.StartsWith("."))
						item.Extension = "." + item.Extension;
					//
					int indexOf = FindMimeMap(typesCollection, item);
					//
					if (indexOf > -1)
					{
						var item2Renew = typesCollection[indexOf];
						//
						FillConfigurationElementWithData(item2Renew, item);
						//
						continue;
					}
					//
					typesCollection.Add(CreateMimeMap(typesCollection, item));
				}
				//
				srvman.CommitChanges();
			}
			#endregion
		}

		private MimeMap GetMimeMap(ConfigurationElement element)
		{
			// skip inherited mime mappings
			if (element == null || !element.IsLocallyStored)
				return null;
			//
			return new MimeMap
			{
				Extension	= Convert.ToString(element.GetAttributeValue(FileExtensionAttribute)),
				MimeType	= Convert.ToString(element.GetAttributeValue(MimeTypeAttribute))
			};
		}

		private ConfigurationElement CreateMimeMap(ConfigurationElementCollection collection, MimeMap mapping)
		{
			if (mapping == null
				|| String.IsNullOrEmpty(mapping.MimeType)
					|| String.IsNullOrEmpty(mapping.Extension))
			{
				return null;
			}
			//
			var item2Add = collection.CreateElement("mimeMap");
			//
			FillConfigurationElementWithData(item2Add, mapping);
			//
			return item2Add;
		}

		private void FillConfigurationElementWithData(ConfigurationElement item2Fill, MimeMap mapping)
		{
			if (mapping == null
				|| item2Fill == null
					|| String.IsNullOrEmpty(mapping.MimeType)
						|| String.IsNullOrEmpty(mapping.Extension))
			{
				return;
			}
			//
			item2Fill.SetAttributeValue(MimeTypeAttribute, mapping.MimeType);
			item2Fill.SetAttributeValue(FileExtensionAttribute, mapping.Extension);
		}

		private int FindMimeMap(ConfigurationElementCollection collection, MimeMap mapping)
		{
			for (int i = 0; i < collection.Count; i++)
			{
				var item = collection[i];
				//
				if (String.Equals(item.GetAttributeValue(FileExtensionAttribute), mapping.Extension))
				{
					return i;
				}
			}
			//
			return -1;
		}
	}
}
