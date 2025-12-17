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

namespace FuseCP.UniversalInstaller
{
	public partial class Installer
	{
		public virtual string WebDavPortalSiteId => $"{FuseCP}WebDavPortal";
		public virtual string UnixWebDavPortalServiceId => "fusecp-webdavportal";
		public virtual void InstallWebDavPortalPrerequisites() { }
		public virtual void RemoveWebDavPortalPrerequisites() { }
		public virtual void CreateWebDavPortalUser() => CreateUser(Settings.WebDavPortal);
		public virtual void RemoveWebDavPortalUser() => RemoveUser(Settings.WebDavPortal.Username);
		public virtual void SetWebDavPortalFilePermissions() => SetFilePermissions(Settings.WebDavPortal.InstallPath, Settings.WebDavPortal.Username);
		public virtual void SetWebDavPortalFileOwner() => SetFileOwner(Settings.WebDavPortal.InstallPath, Settings.WebDavPortal.Username, FuseCPGroup);
		public virtual void InstallWebDavPortal()
		{
			InstallWebDavPortalPrerequisites();
			ReadWebDavPortalConfiguration();
			CopyWebDavPortal(true);//, //this.StandardInstallFilter);
			CreateWebDavPortalUser();
			ConfigureWebDavPortal();
			SetWebDavPortalFilePermissions();
			SetWebDavPortalFileOwner();
			InstallWebDavPortalWebsite();
		}
		public virtual void UpdateWebDavPortal()
		{
			InstallWebDavPortalPrerequisites();
			ReadWebDavPortalConfiguration();
			CopyWebDavPortal(true, StandardUpdateFilter);
			UpdateWebDavPortalConfig();
			ConfigureWebDavPortal();
			SetWebDavPortalFilePermissions();
			SetWebDavPortalFileOwner();
			InstallWebDavPortalWebsite();
		}
		public virtual void InstallWebDavPortalWebsite()
		{
			var web = Settings.WebDavPortal.InstallPath;
			var dll = Path.Combine(web, "bin_dotnet", "FuseCP.WebDavPortal.dll");
			InstallWebsite(WebDavPortalSiteId,
				web,
				Settings.WebDavPortal,
				FuseCPUnixGroup,
				dll,
				"FuseCP.WebDavPortal service, the WebDavPortal for the FuseCP control panel.",
				UnixWebDavPortalServiceId);
		}
		public virtual void SetupWebDavPortal()
		{
			RemoveWebDavPortalWebsite();
			ConfigureWebDavPortal();
			InstallWebDavPortalWebsite();
		}
		public virtual void RemoveWebDavPortalWebsite() {
			RemoveWebsite(WebDavPortalSiteId, Settings.WebDavPortal);
		}
		public virtual void ReadWebDavPortalConfiguration() { }
		public virtual void RemoveWebDavPortal()
		{
			RemoveWebDavPortalWebsite();
			RemoveWebDavPortalFolder();
			RemoveWebDavPortalUser();
		}
		public virtual void RemoveWebDavPortalFolder()
		{
			var dir = Settings.WebDavPortal.InstallPath;
			if (Directory.Exists(dir)) Directory.Delete(dir, true);
			InstallLog("Removed WebDavPortal files");
		}

		public virtual void UpdateWebDavPortalConfig() { }
		public virtual void ConfigureWebDavPortal() { }

		public virtual void CopyWebDavPortal(bool clearDestination = false, Func<string, string> filter = null)
		{
			filter ??= SetupFilter;
			var websitePath = Settings.WebDavPortal.InstallPath;
			InstallWebRootPath = Path.GetDirectoryName(websitePath);
			CopyFiles(ComponentTempPath, websitePath, clearDestination, filter);
		}
	}
}
