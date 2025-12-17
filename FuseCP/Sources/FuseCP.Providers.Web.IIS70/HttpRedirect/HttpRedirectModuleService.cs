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
using System.Collections;
using FuseCP.Providers.Web.Iis.Utility;

namespace FuseCP.Providers.Web.HttpRedirect
{
	internal class HttpRedirectModuleService : ConfigurationModuleService
	{
		public const string EnabledAttribute = "enabled";
		public const string ExactDestinationAttribute = "exactDestination";
		public const string ChildOnlyAttribute = "childOnly";
		public const string DestinationAttribute = "destination";
		public const string HttpResponseStatusAttribute = "httpResponseStatus";

		public void GetHttpRedirectSettings(ServerManager srvman, WebAppVirtualDirectory virtualDir)
		{
			// Load web site configuration
			var config = srvman.GetWebConfiguration(virtualDir.FullQualifiedPath);
			// Load corresponding section
			var section = config.GetSection(Constants.HttpRedirectSection);
			//
			if (!Convert.ToBoolean(section.GetAttributeValue(EnabledAttribute)))
				return;
			//
			virtualDir.RedirectExactUrl = Convert.ToBoolean(section.GetAttributeValue(ExactDestinationAttribute));
			virtualDir.RedirectDirectoryBelow = Convert.ToBoolean(section.GetAttributeValue(ChildOnlyAttribute));
			virtualDir.HttpRedirect = Convert.ToString(section.GetAttributeValue(DestinationAttribute));
			virtualDir.RedirectPermanent = String.Equals("301", Convert.ToString(section.GetAttributeValue(HttpResponseStatusAttribute)));		
		}

		public void SetHttpRedirectSettings(WebAppVirtualDirectory virtualDir)
		{
			#region Revert to parent settings (inherited)
			using (var srvman = GetServerManager())
			{
				// Load web site configuration
				var config = srvman.GetWebConfiguration(virtualDir.FullQualifiedPath);
				// Load corresponding section
				var section = config.GetSection(Constants.HttpRedirectSection);
				//
				section.RevertToParent();
				//
				srvman.CommitChanges();
			} 
			#endregion

			// HttpRedirect property is not specified so defaults to the parent
			if (String.IsNullOrEmpty(virtualDir.HttpRedirect))
				return;

			#region Put changes in effect
			using (var srvman = GetServerManager())
			{
				// Load web site configuration
				var config = srvman.GetWebConfiguration(virtualDir.FullQualifiedPath);
				// Load corresponding section
				var section = config.GetSection(Constants.HttpRedirectSection);
				// Enable http redirect feature
				section.SetAttributeValue(EnabledAttribute, true);
				section.SetAttributeValue(ExactDestinationAttribute, virtualDir.RedirectExactUrl);
				section.SetAttributeValue(DestinationAttribute, virtualDir.HttpRedirect);
				section.SetAttributeValue(ChildOnlyAttribute, virtualDir.RedirectDirectoryBelow);
				// Configure HTTP Response Status
				if (virtualDir.RedirectPermanent)
					section.SetAttributeValue(HttpResponseStatusAttribute, "Permanent");
				else
					section.SetAttributeValue(HttpResponseStatusAttribute, "Found");
				//
				srvman.CommitChanges();
			} 
			#endregion
		}
	}
}
