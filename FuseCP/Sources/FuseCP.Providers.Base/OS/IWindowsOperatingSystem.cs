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
using System.Collections;
using System.Collections.Generic;
using Microsoft.Win32;
using System.Diagnostics;
using System.IO;
using System.Security;
using System.Text.RegularExpressions;
using System.Web;
using FuseCP.Providers.DNS;
using FuseCP.Providers.DomainLookup;

namespace FuseCP.Providers.OS
{
    /// <summary>
    /// Summary description for IOperationSystem.
    /// </summary>
    public interface IWindowsOperatingSystem: IOperatingSystem
	{
		UserPermission[] GetGroupNtfsPermissions(string path, UserPermission[] users, string usersOU);
		void GrantGroupNtfsPermissions(string path, UserPermission[] users, string usersOU, bool resetChildPermissions);

		// ODBC DSNs
		string[] GetInstalledOdbcDrivers();
		string[] GetDSNNames();
		SystemDSN GetDSN(string dsnName);
		void CreateDSN(SystemDSN dsn);
		void UpdateDSN(SystemDSN dsn);
		void DeleteDSN(string dsnName);

		// Access databases
		void CreateAccessDatabase(string databasePath);

		// File Services
		bool CheckFileServicesInstallation();
		bool InstallFsrmService();

		// Terminal Services
		TerminalSession[] GetTerminalServicesSessions();
		void CloseTerminalServicesSession(int sessionId);



		Installer WinGet { get; }
		Installer Chocolatey { get; }
		Shell Cmd { get; }
		Shell PowerShell { get; }
		WSLShell WSL { get; }
	}
}
