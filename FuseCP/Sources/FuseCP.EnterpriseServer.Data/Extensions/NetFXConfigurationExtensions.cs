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

#if NetFX
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Data.Entity.ModelConfiguration.Configuration;
using System.Linq.Expressions;


namespace FuseCP.EnterpriseServer.Data
{
	public static class NetFXConfigurationExtensions
	{
		/*
		public static PrimaryKeyIndexConfiguration HasKey<TEntity>(this EntityTypeConfiguration<TEntity> entity, Expression<Func<TEntity, object?>> keyExpression) where TEntity: class
		{
			PrimaryKeyIndexConfiguration conf = null;
			entity.HasKey(keyExpression, key => conf = key);
			return conf;
		}

		public static IndexConfiguration HasIndex<TEntity>(this EntityTypeConfiguration<TEntity> entity, Expression<Func<TEntity, object?>> indexExpression, string name) where TEntity: class => entity.HasIndex(indexExpression).HasName(name);

		public static object HasData<TEntity>(this EntityTypeConfiguration<TEntity> entity, params TEntity[] data) where TEntity: class => new object();
		*/

		public static PrimitivePropertyConfiguration ValueGeneratedOnAdd(this PrimitivePropertyConfiguration config) => config.HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
	}
}
#endif
