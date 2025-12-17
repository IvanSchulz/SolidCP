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
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FuseCP.EnterpriseServer.Data
{
	public interface IDbProvider
	{
		DbType DbType { get; }
	}

	public class SqlServerProvider : IDbProvider
	{
		public DbType DbType => DbType.SqlServer;
	}

	public class MySqlProvider : IDbProvider
	{
		public DbType DbType => DbType.MySql;
	}
	public class MariaDbProvider : IDbProvider
	{
		public DbType DbType => DbType.MariaDb;
	}
	public class PostgreSqlProvider : IDbProvider
	{
		public DbType DbType => DbType.PostgreSql;
	}
	public class SqliteProvider : IDbProvider
	{
		public DbType DbType => DbType.Sqlite;
	}
	public class OracleProvider : IDbProvider
	{
		public DbType DbType => DbType.Oracle;
	}
}
