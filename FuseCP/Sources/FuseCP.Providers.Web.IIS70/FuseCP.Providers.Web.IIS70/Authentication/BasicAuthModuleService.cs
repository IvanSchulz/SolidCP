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

namespace FuseCP.Providers.Web.Iis.Authentication
{
    using Common;
    using Microsoft.Web.Administration;
    using Microsoft.Web.Management.Server;
    using System;

    internal sealed class BasicAuthModuleService : ConfigurationModuleService
    {
		public const string EnabledAttribute = "enabled";

        public void GetAuthenticationSettingsNonApp(ServerManager srvman, WebVirtualDirectory virtualDir)
        {
            var config = srvman.GetApplicationHostConfiguration();
            //
            var section = config.GetSection(Constants.BasicAuthenticationSection, virtualDir.FullQualifiedPath);
            //
            virtualDir.EnableBasicAuthentication = Convert.ToBoolean(section.GetAttributeValue(EnabledAttribute));
        }

        public void GetAuthenticationSettings(ServerManager srvman, WebAppVirtualDirectory virtualDir)
        {
			var config = srvman.GetApplicationHostConfiguration();
			//
			var section = config.GetSection(Constants.BasicAuthenticationSection, virtualDir.FullQualifiedPath);
			//
			virtualDir.EnableBasicAuthentication = Convert.ToBoolean(section.GetAttributeValue(EnabledAttribute));
        }

        public void SetAuthenticationSettings(WebVirtualDirectory virtualDir)
        {
            using (var srvman = GetServerManager())
            {
                var config = srvman.GetApplicationHostConfiguration();
                //
                var section = config.GetSection(Constants.BasicAuthenticationSection, virtualDir.FullQualifiedPath);
                //
                section.SetAttributeValue(EnabledAttribute, virtualDir.EnableBasicAuthentication);
                //
                srvman.CommitChanges();
            }
        }

        public void SetAuthenticationSettings(WebAppVirtualDirectory virtualDir)
        {
			using (var srvman = GetServerManager())
			{
				var config = srvman.GetApplicationHostConfiguration();
				//
				var section = config.GetSection(Constants.BasicAuthenticationSection, virtualDir.FullQualifiedPath);
				//
				section.SetAttributeValue(EnabledAttribute, virtualDir.EnableBasicAuthentication);
				//
				srvman.CommitChanges();
			}
        }
    }
}

