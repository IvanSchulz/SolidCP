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
using IO=System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace FuseCP.Tests
{
	public class Paths
	{
		public const string App = "FuseCP";

		static string project = null;

		public static string Test
		{
			get
			{
				if (project == null)
				{
					var path = IO.Path.GetDirectoryName(new Uri(System.Reflection.Assembly.GetExecutingAssembly().CodeBase).LocalPath);
					var dir = IO.Path.GetFileName(path);
					while (Regex.IsMatch(dir, @"^((net[0-9][.0-9]*)|Debug|Release|bin|bin_dotnet)$"))
					{
						path = IO.Path.GetDirectoryName(path);
						dir = IO.Path.GetFileName(path);
					}
					project = path;
				}
				return project;
			}
		}

		public static string EnterpriseServer => IO.Path.GetFullPath(IO.Path.Combine(Test, $@"..\{App}.EnterpriseServer"));
		public static string Server => IO.Path.GetFullPath(IO.Path.Combine(Test, $@"..\{App}.Server"));
		public static string Portal => IO.Path.GetFullPath(IO.Path.Combine(Test, $@"..\{App}.WebPortal"));
		public static string WebDavPortal => IO.Path.GetFullPath(IO.Path.Combine(Test, $@"..\{App}.WebDavPortal"));
		public static string Installer => IO.Path.GetFullPath(IO.Path.Combine(Test, $@"..\..\..\{App}.WebSite\Sources\{App}.WebSite\Sources\"));
		public static string Wsl(string path)
		{
			if (path.Length > 1 && IO.Path.IsPathRooted(path)) path = char.ToLower(path[0]) + path.Substring(2);
			return "/mnt/" + path.Replace('\\', '/'); 
		}
		public static string Path(Component server)
		{
			switch (server)
			{
				case Component.Server:
					return Server;
				case Component.EnterpriseServer:
					return EnterpriseServer;
				case Component.Portal:
					return Portal;
				case Component.WebDavPortal:
					return WebDavPortal;
				case Component.Installer:
					return Installer;
				default:
					throw new ArgumentOutOfRangeException(nameof(server), server, null);
			}
		}
	}
}
