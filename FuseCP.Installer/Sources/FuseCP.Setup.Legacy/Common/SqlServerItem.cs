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

using FuseCP.EnterpriseServer.Data;

﻿namespace FuseCP.Setup
{
	public sealed class SqlServerItem
	{
		public string Server { get; set; }
		public bool WindowsAuthentication { get; set; }
		public string User { get; set; }
		public string Password { get; set; }
		public string Database { get; set; }
		public DbType DatabaseType { get; set; }
		public int Port { get; set; }
	}
}
