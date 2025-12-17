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
using FuseCP.EnterpriseServer.Data.Configuration;
using FuseCP.EnterpriseServer.Data.Entities;
using System.ComponentModel.DataAnnotations.Schema;
#if NetCore
using Microsoft.EntityFrameworkCore;
#endif
#if NetFX
using System.Data.Entity;
#endif

namespace FuseCP.EnterpriseServer.Data.Configuration;

public partial class ServiceConfiguration: EntityTypeConfiguration<Service>
{
    public override void Configure() {

		if (IsSqlServer) Property(e => e.Comments).HasColumnType("ntext");
		else if (IsCore && (IsMySql || IsMariaDb || IsSqlite || IsPostgreSql))
		{
            Property(e => e.Comments).HasColumnType("TEXT");
		}

#if NetCore
        HasOne(d => d.Cluster).WithMany(p => p.Services).HasConstraintName("FK_Services_Clusters");

        HasOne(d => d.Provider).WithMany(p => p.Services)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Services_Providers");

        HasOne(d => d.Server).WithMany(p => p.Services)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Services_Servers");
#else
		//TODO optional or required?
		HasOptional(d => d.Cluster).WithMany(p => p.Services);
        HasRequired(d => d.Provider).WithMany(p => p.Services);
        HasRequired(d => d.Server).WithMany(p => p.Services);
#endif
    }
}
