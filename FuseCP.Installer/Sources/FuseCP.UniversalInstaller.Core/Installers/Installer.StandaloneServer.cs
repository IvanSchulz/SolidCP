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
using System.Reflection;
using FuseCP.Providers;
using FuseCP.Providers.Web;
using FuseCP.Providers.OS;
using FuseCP.Providers.Utils;
using FuseCP.EnterpriseServer.Data;
using System.Globalization;
using System.Security.Policy;
using System.Diagnostics.Contracts;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Data;
using System.Xml.Linq;

namespace FuseCP.UniversalInstaller
{
	public abstract partial class Installer
	{
		public virtual void InstallStandaloneServerPrerequisites() { }
		public virtual void RemoveStandaloneServerPrerequisites() { }
		public virtual void CreateStandaloneUsers()
		{
			CreateServerUser();
			if (!Settings.WebPortal.EmbedEnterpriseServer) CreateEnterpriseServerUser();
			CreateWebPortalUser();
			if (OSInfo.IsWindows) CreateWebDavPortalUser();
		}

		public virtual void RemoveStandaloneUsers()
		{
			RemoveServerUser();
			if (!Settings.WebPortal.EmbedEnterpriseServer) RemoveEnterpriseServerUser();
			RemoveWebPortalUser();
			if (OSInfo.IsWindows) RemoveWebDavPortalUser();
		}
		public virtual void SetStandaloneServerFilePermissions() {
			SetEnterpriseServerFilePermissions();
			SetServerFilePermissions();
			SetWebPortalFilePermissions();
			if (OSInfo.IsWindows) SetWebDavPortalFilePermissions();
		}
		public virtual void SetStandaloneServerFileOwner()
		{
			SetEnterpriseServerFileOwner();
			SetServerFileOwner();
			SetWebPortalFileOwner();
			if (OSInfo.IsWindows) SetWebDavPortalFileOwner();
		}
		public virtual void SetStandaloneServerSettings() { }
		public virtual void InstallStandaloneServer()
		{
			ResetEstimatedOutputLines();
			CountInstallDatabaseStatements();
			InstallStandaloneServerPrerequisites();
			CopyStandaloneServer(true, StandaloneInstallFilter);
			CreateStandaloneUsers();
			InstallDatabase();
			InstallSchedulerService();
			ConfigureStandaloneServer();
			SetStandaloneServerFilePermissions();
			SetStandaloneServerFileOwner();
			InstallServerWebsite();
			InstallWebPortalWebsite();
			if (OSInfo.IsWindows) InstallWebDavPortalWebsite();
			else RemoveWebDavPortalFolder();
		}
		public virtual void UpdateStandaloneServerConfig() {
			UpdateServerConfig();
			UpdateEnterpriseServerConfig();
			UpdateWebPortalConfig();
			if (OSInfo.IsWindows) UpdateWebDavPortalConfig();
		}
		public virtual void UpdateStandaloneServer()
		{
			ResetEstimatedOutputLines();
			CountUpdateDatabaseStatements();
			InstallStandaloneServerPrerequisites();
			CopyStandaloneServer(true, StandaloneUpdateFiler);
			UpdateDatabase();
			UpdateStandaloneServerConfig();
            InstallSchedulerService();
            ConfigureStandaloneServer();
			SetStandaloneServerFilePermissions();
			SetStandaloneServerFileOwner();
			InstallServerWebsite();
			InstallWebPortalWebsite();
			if (OSInfo.IsWindows) InstallWebDavPortalWebsite();
			else RemoveWebDavPortalFolder();
		}

		public virtual void SetupStandaloneServer()
		{
			RemoveServerWebsite();
			RemoveWebPortalWebsite();
			if (OSInfo.IsWindows) RemoveWebDavPortalWebsite();
            InstallSchedulerService();
            ConfigureStandaloneServer();
			InstallServerWebsite();
			InstallWebPortalWebsite();
			if (OSInfo.IsWindows) InstallWebDavPortalWebsite();
		}
		public virtual void InstallStandaloneServerWebsite() => InstallWebPortalWebsite();
		public virtual void RemoveSetupFolder()
		{
			var dir = Path.Combine(Settings.Standalone.InstallPath, "Setup");
			if (Directory.Exists(dir)) Directory.Delete(dir, true);
		}
		public virtual void RemoveStandaloneServerFolder()
		{
			RemoveEnterpriseServerFolder();
			RemoveServerFolder();
			RemoveWebPortalFolder();
			RemoveWebDavPortalFolder();
			RemoveSetupFolder();
		}

		public virtual void RemoveStandaloneServer()
		{
			RemoveWebDavPortalWebsite();
			RemoveWebPortalWebsite();
			RemoveServerWebsite();
            RemoveSchedulerService();
			DeleteDatabase();
			RemoveStandaloneServerFolder();
		}
		public virtual void ReadStandaloneServerConfiguration() { }

		public virtual string PathWithSpaces(string path)
		{
			return Regex.Replace(path, "(?<=[a-z])([A-Z])", " $1");
		}
		public virtual void ConfigureStandaloneServer() {
			Settings.WebPortal.EmbedEnterpriseServer = true;
			Settings.WebPortal.EnterpriseServerPath = Path.Combine("..", EnterpriseServerFolder);
			ConfigureServer();
			ConfigureEnterpriseServer();
			ConfigureWebPortal();
			if (OSInfo.IsWindows) ConfigureWebDavPortal();
		}

		public virtual void CopyStandaloneServer(bool clearDestination = false, Func<string, string> filter = null)
		{
			filter ??= SetupFilter;
			var websitePath = Settings.Standalone.InstallPath;
			InstallWebRootPath = Path.GetDirectoryName(websitePath);
			CopyFiles(ComponentTempPath, websitePath, clearDestination, filter);
		}
	}
}
