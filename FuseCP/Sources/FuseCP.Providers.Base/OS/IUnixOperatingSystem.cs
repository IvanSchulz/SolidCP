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
    [System.FlagsAttribute]
	public enum UnixFileMode
	{
		None = 0,
		OtherExecute = 1,
		OtherWrite = 2,
		OtherRead = 4,
		GroupExecute = 8,
		GroupWrite = 16,
		GroupRead = 32,
		UserExecute = 64,
		UserWrite = 128,
		UserRead = 256,
		StickyBit = 512,
		SetGroup = 1024,
		SetUser = 2048
	}

	/// <summary>
	/// Summary description for IOperationSystem.
	/// </summary>
	public interface IUnixOperatingSystem: IOperatingSystem
	{
		UnixFileMode GetUnixPermissions(string path);
		void GrantUnixPermissions(string path, UnixFileMode mode, bool resetChildPermissions = false);
		void ChangeUnixFileOwner(string path, string owner, string group, bool setChildren = false);
		UnixFileOwner GetUnixFileOwner(string path);
		Shell Sh { get; }
		Shell Bash { get; }
		Shell PowerShell { get; }
		Installer Apt { get; }
		Installer Yum { get; }
		Installer Brew { get; }
		bool IsSystemd { get; }
		bool IsOpenRC { get; }
	}
}
