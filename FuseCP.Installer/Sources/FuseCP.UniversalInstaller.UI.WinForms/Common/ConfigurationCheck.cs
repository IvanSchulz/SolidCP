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

namespace FuseCP.UniversalInstaller.WinForms;

public enum CheckTypes
{
	None,
	OperatingSystem,
	WindowsOperatingSystem,
	IISVersion,
	InitSystem,
	ApacheVersion,
	Net10Runtime,
	ASPNET,
	FCPServer,
	FCPEnterpriseServer,
	FCPPortal,
	FCPWebDavPortal,
	ASPNET32
}

public enum CheckStatuses
{
	Success,
	Warning,
	Error
}

public class ConfigurationCheck
{
	public ConfigurationCheck(CheckTypes checkType, string action)
	{
		this.CheckType = checkType;
		this.Action = action;
	}

	public CheckTypes CheckType { get; set; }
	public string Action { get; set; }
	public CheckStatuses Status { get; set; }
	public string Details { get; set; }
}
