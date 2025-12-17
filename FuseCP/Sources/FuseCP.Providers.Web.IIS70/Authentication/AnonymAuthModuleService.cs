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
    using Microsoft.Web.Administration;
    using Microsoft.Web.Management.Server;
    using Common;
	using Utility;
    using System;
    using System.Globalization;

    internal sealed class AnonymAuthModuleService : ConfigurationModuleService
    {
		public const string EnabledAttribute = "enabled";
		public const string UserNameAttribute = "userName";
		public const string PasswordAttribute = "password";

		public PropertyBag GetAuthenticationSettings(ServerManager srvman, string siteId)
        {
			var config = srvman.GetApplicationHostConfiguration();
			//
			var section = config.GetSection(Constants.AnonymousAuthenticationSection, siteId);
			//
			PropertyBag bag = new PropertyBag();
			//
			bag[AuthenticationGlobals.AnonymousAuthenticationUserName] = Convert.ToString(section.GetAttributeValue(UserNameAttribute));
			bag[AuthenticationGlobals.AnonymousAuthenticationPassword] = Convert.ToString(section.GetAttributeValue(PasswordAttribute));
			bag[AuthenticationGlobals.Enabled] = Convert.ToBoolean(section.GetAttributeValue(EnabledAttribute));
			bag[AuthenticationGlobals.IsLocked] = section.IsLocked;
			//
			return bag;
        }

        public void SetAuthenticationSettings(string virtualPath, string userName, 
			string password, bool enabled)
        {
			using (var srvman = GetServerManager())
			{
				var config = srvman.GetApplicationHostConfiguration();
				//
				var section = config.GetSection(Constants.AnonymousAuthenticationSection, virtualPath);
				//
				section.SetAttributeValue(EnabledAttribute, enabled);
				section.SetAttributeValue(UserNameAttribute, userName);
				section.SetAttributeValue(PasswordAttribute, password);
				//
				srvman.CommitChanges();
			}
        }

        public void RemoveAuthenticationSettings(string virtualPath)
        {
			using (var srvman = GetServerManager())
			{
				var config = srvman.GetApplicationHostConfiguration();
				//
				config.RemoveLocationPath(virtualPath);
				//
				srvman.CommitChanges();
			}
        }
    }
}
