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

namespace FuseCP.Setup
{
	public enum ActionTypes
	{
		None,
		CopyFiles,
		CreateWebSite,
		CryptoKey,
		ServerPassword,
		UpdateServerPassword,
		UpdateConfig,
		UpdateEnterpriseServerUrl,
		CreateDatabase,
		CreateDatabaseUser,
		ExecuteSql,
		DeleteRegistryKey,
		DeleteDirectory,
		DeleteDatabase,
		DeleteDatabaseUser,
		DeleteDatabaseLogin,
		DeleteWebSite,
		DeleteVirtualDirectory,
		DeleteUserAccount,
		DeleteUserMembership,
		DeleteApplicationPool,
		DeleteFiles,
		UpdateWebSite,
		ConfigureLetsEncrypt,
		//UpdateFiles,
		Backup,
		BackupDatabase,
		BackupDirectory,
		BackupConfig,
		UpdatePortal2811,
		UpdateEnterpriseServer2810,
		UpdateServer2810,
		CreateShortcuts,
		DeleteShortcuts,
		UpdateServers,
		CopyWebConfig,
		UpdateWebConfigNamespaces,
		StopApplicationPool,
		StartApplicationPool,
		RegisterWindowsService,
		UnregisterWindowsService,
		RegisterUnixService,
		UnregisterUnixService,
		StartWindowsService,
		StopWindowsService,
		StartUnixService,
		StopUnixService,
		ServiceSettings,
		CreateUserAccount,
		InitSetupVariables,
		UpdateServerAdminPassword,
		UpdateLicenseInformation,
		ConfigureStandaloneServerData,
		CreateFCPServerLogin,
		FolderPermissions,
		AddCustomErrorsPage,
		SwitchServer2AspNet40,
		SwitchEntServer2AspNet40,
		SwitchWebPortal2AspNet40,
        SwitchWebDavPortal2AspNet40,
        ConfigureSecureSessionModuleInWebConfig,
        RestoreConfig,
        UpdateXml,
        SaveConfig,
        DeleteDirectoryFiles
	}
	
	public class InstallAction
	{
		public InstallAction(ActionTypes type)
		{
			this.ActionType = type;
		}

		public ActionTypes ActionType{ get; set; }
		

		public string Name{ get; set; }
		

		public string Log{ get; set; }
		

		public string Description{ get; set; }
		

		public string ConnectionString{ get; set; }
		

		public string Key{ get; set; }
		

		public string Path{ get; set; }
		

		public string UserName{ get; set; }
		

		public string SiteId{ get; set; }
		

		public string Source{ get; set; }
		

		public string Destination{ get; set; }
		

		public bool Empty{ get; set; }
		

		public string IP{ get; set; }
		

		public string Port{ get; set; }
		

		public string Domain{ get; set; }
		

		public string[] Membership { get; set; }
		
		
		public SetupVariables SetupVariables { get; set; }

		
		public string Url { get; set; }

        public Func<string, bool> FileFilter { get; set; }
	
	}
}
