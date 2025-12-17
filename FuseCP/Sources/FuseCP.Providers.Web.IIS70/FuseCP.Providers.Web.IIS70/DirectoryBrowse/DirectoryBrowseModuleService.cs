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

namespace FuseCP.Providers.Web.Iis.DirectoryBrowse
{
	using Microsoft.Web.Administration;
    using Microsoft.Web.Management.Server;
    using Common;
    using System;

    internal sealed class DirectoryBrowseModuleService : ConfigurationModuleService
    {
        public PropertyBag GetDirectoryBrowseSettings(ServerManager srvman, string siteId)
        {
			var config = srvman.GetWebConfiguration(siteId);
			//
			DirectoryBrowseSection directoryBrowseSection = (DirectoryBrowseSection)config.GetSection(Constants.DirectoryBrowseSection, typeof(DirectoryBrowseSection));
			//
			PropertyBag bag = new PropertyBag();
			bag[DirectoryBrowseGlobals.Enabled] = directoryBrowseSection.Enabled;
			bag[DirectoryBrowseGlobals.ShowFlags] = (int)directoryBrowseSection.ShowFlags;
			bag[DirectoryBrowseGlobals.ReadOnly] = directoryBrowseSection.IsLocked;
			return bag;
        }

        public void SetDirectoryBrowseEnabled(string siteId, bool enabled)
        {
			using (var srvman = GetServerManager())
			{
				var config = srvman.GetWebConfiguration(siteId);
				//
				var section = config.GetSection("system.webServer/directoryBrowse");
				//
				section.SetAttributeValue("enabled", enabled);
				//
				srvman.CommitChanges();
			}
        }

        public void SetDirectoryBrowseSettings(string siteId, PropertyBag updatedBag)
        {
			if (updatedBag == null)
				return;

			using (var srvman = GetServerManager())
			{
				var config = srvman.GetWebConfiguration(siteId);
				//
				DirectoryBrowseSection section = (DirectoryBrowseSection)config.GetSection(Constants.DirectoryBrowseSection, typeof(DirectoryBrowseSection));
				//
				section.Enabled = (bool)updatedBag[DirectoryBrowseGlobals.Enabled];
				section.ShowFlags = (DirectoryBrowseShowFlags)updatedBag[DirectoryBrowseGlobals.ShowFlags];
				//
				srvman.CommitChanges();
			}
        }
    }
}
