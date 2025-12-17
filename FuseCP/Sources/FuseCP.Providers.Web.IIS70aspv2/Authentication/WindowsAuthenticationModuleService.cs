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

    internal sealed class WindowsAuthModuleService : ConfigurationModuleService
    {
		public const string EnabledAttribute = "enabled";

        public PropertyBag GetAuthenticationSettings(ServerManager srvman, string siteId)
        {
            PropertyBag bag = new PropertyBag();
			var config = srvman.GetApplicationHostConfiguration();
			//
			var section = config.GetSection(Constants.WindowsAuthenticationSection, siteId);
			//
			bag[AuthenticationGlobals.IsLocked] = section.IsLocked;
			bag[AuthenticationGlobals.Enabled] = Convert.ToBoolean(section.GetAttributeValue(EnabledAttribute));
            return bag;
        }

        public void SetEnabled(string siteId, bool enabled)
        {
			using (var srvman = GetServerManager())
			{
				var config = srvman.GetApplicationHostConfiguration();
				//
				var section = config.GetSection(Constants.WindowsAuthenticationSection, siteId);
				//
				section.SetAttributeValue(EnabledAttribute, enabled);
				//
				srvman.CommitChanges();
			}
        }
    }
}
