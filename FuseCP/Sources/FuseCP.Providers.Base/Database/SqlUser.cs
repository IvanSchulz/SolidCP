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

namespace FuseCP.Providers.Database
{
	public class SqlUser : ServiceProviderItem
	{
		private string defaultDatabase;
		private string[] databases;
		private string password;

		public SqlUser()
		{
		}

		public string DefaultDatabase 
		{
			get { return defaultDatabase; }
			set { defaultDatabase = value; } 
		}

		public string[] Databases
		{
			get { return databases; }
			set { databases = value; } 
		}

		[Persistent]
		public string Password
		{
			get { return this.password; }
			set { this.password = value; }
		}
	}
}
