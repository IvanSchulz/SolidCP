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
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Runtime.Remoting;
using System.Text;
using System.Reflection;
using Microsoft.Win32;
using FuseCP.Providers.HostedSolution;
using FuseCP.Server.Utils;
using FuseCP.Providers.Utils;
using FuseCP.Providers.OS;
using FuseCP.Providers.Common;

using System.Management;
using System.Management.Automation;
using System.Management.Automation.Runspaces;
using System.Collections.ObjectModel;
using System.DirectoryServices;
using System.Security.Cryptography.X509Certificates;
using System.Collections;
using System.Xml;
using FuseCP.EnterpriseServer.Base.RDS;
using System.Security.Principal;
using System.Security.AccessControl;


namespace FuseCP.Providers.RemoteDesktopServices
{
	public class Windows2025 : Windows2022
	{
		public override bool IsInstalled()
		{
			WindowsVersion version = OSInfo.WindowsVersion;
			return version == WindowsVersion.WindowsServer2025;
		}
	}
}

