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

namespace FuseCP.Providers.Web.Iis.ClassicAsp
{
    using Common;
    using Microsoft.Web.Administration;
    using Microsoft.Web.Management.Server;
    using System;

    internal sealed class ClassicAspModuleService : ConfigurationModuleService
    {
		public const string SectionName = "system.webServer/asp";
		public const string EnableParentPathsAttribute = "enableParentPaths";

		public PropertyBag GetClassicAspSettings(ServerManager srvman, string siteId)
        {
			var config = srvman.GetApplicationHostConfiguration();
			//
			var aspSection = config.GetSection(SectionName, siteId);
			//
			PropertyBag bag = new PropertyBag();
			//
			bag[ClassicAspGlobals.EnableParentPaths] = Convert.ToBoolean(aspSection.GetAttributeValue(EnableParentPathsAttribute));
			//
			return bag;
        }

        public void SetClassicAspSettings(WebAppVirtualDirectory virtualDir)
        {
			using (var srvman = GetServerManager())
			{
				var config = srvman.GetApplicationHostConfiguration();
				//
				var aspSection = config.GetSection(SectionName, virtualDir.FullQualifiedPath);
				//
				aspSection.SetAttributeValue(EnableParentPathsAttribute, virtualDir.EnableParentPaths);
				//
				srvman.CommitChanges();
			}
        }
    }
}

