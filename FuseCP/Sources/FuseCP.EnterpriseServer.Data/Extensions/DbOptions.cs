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

#if NetCore
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;

namespace FuseCP.EnterpriseServer.Data
{

	public class DbOptions<T>: DbContextOptions<T> where T: Microsoft.EntityFrameworkCore.DbContext
	{
		public DbType DbType { get; private set; }
		public string ConnectionString { get; private set; }
		public bool InitSeedData { get; private set; }
		public DbOptions(DbType dbType, string connectionString = null, bool initSeedData = false)
		{
			DbType = dbType;
			ConnectionString = connectionString;
			InitSeedData = initSeedData;
		}
		public DbOptions(DbContext context) {
			DbType = context.DbType;
			ConnectionString = context.NativeConnectionString;
			InitSeedData = context.InitSeedData;
		}
	}
}
#endif
