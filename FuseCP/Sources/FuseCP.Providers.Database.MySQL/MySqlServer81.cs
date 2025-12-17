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
using System.Text.RegularExpressions;
using System.Data;
using Microsoft.Win32;
//using MySql.Data.MySqlClient;
using System.IO;

using FuseCP.Providers.OS;
using System.Reflection;
using MySql.Data.MySqlClient;

namespace FuseCP.Providers.Database
{
    public class MySqlServer81: MySqlServer80
	{
		public MySqlServer81(): base() {	}

		public override bool IsInstalled() => IsInstalled("8.1");
	}
}
