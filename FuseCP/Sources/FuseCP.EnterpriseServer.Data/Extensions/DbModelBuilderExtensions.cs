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
#if NetFX
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration;
#endif

namespace FuseCP.EnterpriseServer.Data.Configuration
{
	public static class DbModelBuilderExtensions
	{
#if NetFX
		public static void ApplyConfiguration<T>(this DbModelBuilder builder, EntityTypeConfiguration<T> configuration) where T : class
		{
			builder.Configurations.Add(configuration);
		}
#endif
	}
}
