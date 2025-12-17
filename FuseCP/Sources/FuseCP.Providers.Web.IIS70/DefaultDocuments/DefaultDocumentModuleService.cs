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

using Microsoft.Web.Administration;

namespace FuseCP.Providers.Web.Iis.DefaultDocuments
{
	using Common;
	using Microsoft.Web.Administration;
	using Microsoft.Web.Management.Server;
	using System;
	using System.Text;
	using System.Collections.Generic;
	using System.Collections;
	using FuseCP.Providers.Web.Iis.Utility;

	internal sealed class DefaultDocsModuleService : ConfigurationModuleService
	{
		public const string ValueAttribute = "value";

		public string GetDefaultDocumentSettings(ServerManager srvman, string siteId)
		{
			// Load web site configuration
			var config = srvman.GetWebConfiguration(siteId);
			// Load corresponding section
			var section = config.GetSection(Constants.DefaultDocumentsSection);
			//
			var filesCollection = section.GetCollection("files");
			// Build default documents
			var defaultDocs = new List<String>();
			//
			foreach (var item in filesCollection)
			{
				var item2Get = GetDefaultDocument(item);
				//
				if (String.IsNullOrEmpty(item2Get))
					continue;
				//
				defaultDocs.Add(item2Get);
			}
			//
			return String.Join(",", defaultDocs.ToArray());
		}

		public void SetDefaultDocumentsEnabled(string siteId, bool enabled)
		{
			using (var srvman = GetServerManager())
			{
				// Load web site configuration
				var config = srvman.GetWebConfiguration(siteId);
				// Load corresponding section
				var section = config.GetSection(Constants.DefaultDocumentsSection);
				//
				section.SetAttributeValue("enabled", enabled);
				//
				srvman.CommitChanges();
			}
		}

		public void SetDefaultDocumentSettings(string siteId, string defaultDocs)
		{
			#region Revert to parent settings (inherited)
			using (var srvman = GetServerManager())
			{
				// Load web site configuration
				var config = srvman.GetWebConfiguration(siteId);
				// Load corresponding section
				var section = config.GetSection(Constants.DefaultDocumentsSection);
				//
				section.RevertToParent();
				//
				srvman.CommitChanges();
			} 
			#endregion

			// Exit if no changes have been made
			if (String.IsNullOrEmpty(defaultDocs))
				return;

			// Update default documents list
			var docs2Add = defaultDocs.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

			#region Put changes in effect
			using (var srvman = GetServerManager())
			{
				// Load web site configuration
				var config = srvman.GetWebConfiguration(siteId);
				// Load corresponding section
				var section = config.GetSection(Constants.DefaultDocumentsSection);
				//
				var filesCollection = section.GetCollection("files");
				// The only solution to override inherited default documents is to use <clear/> element
				filesCollection.Clear();
				//
				foreach (var item in docs2Add)
				{
					// The default document specified exists
					if (FindDefaultDocument(filesCollection, item) > -1)
						continue;
					//
					var item2Add = CreateDefaultDocument(filesCollection, item);
					//
					if (item2Add == null)
						continue;
					//
					filesCollection.Add(item2Add);
				}
				//
				srvman.CommitChanges();
			} 
			#endregion
		}

		private string GetDefaultDocument(ConfigurationElement element)
		{
			if (element == null)
				return null;
			//
			return Convert.ToString(element.GetAttributeValue(ValueAttribute));
		}

		private ConfigurationElement CreateDefaultDocument(ConfigurationElementCollection collection, string valueStr)
		{
			if (valueStr == null)
				return null;
			//
			valueStr = valueStr.Trim();
			//
			if (String.IsNullOrEmpty(valueStr))
				return null;

			//
			ConfigurationElement file2Add = collection.CreateElement("add");
			file2Add.SetAttributeValue(ValueAttribute, valueStr);
			//
			return file2Add;
		}

		private int FindDefaultDocument(ConfigurationElementCollection collection, string valueStr)
		{
			for (int i = 0; i < collection.Count; i++)
			{
				var item = collection[i];
				//
				var valueObj = item.GetAttributeValue(ValueAttribute);
				//
				if (String.Equals((String)valueObj, valueStr, StringComparison.OrdinalIgnoreCase))
				{
					return i;
				}
			}
			//
			return -1;
		}
	}
}
