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

using BackgroundTaskParameter = FuseCP.EnterpriseServer.Data.Entities.BackgroundTaskParameter;

public partial class BackgroundTaskParameterConfiguration: EntityTypeConfiguration<BackgroundTaskParameter>
{
    public override void Configure() {
		HasKey(e => e.ParameterId).HasName("PK_BackgroundTaskParameter");
		if (IsSqlServer)
		{
			Property(e => e.SerializerValue).HasColumnType("ntext");
		}
		else if (IsCore && (IsMySql || IsMariaDb || IsSqlite || IsPostgreSql))
		{
			Property(e => e.SerializerValue).HasColumnType("TEXT");
			if (IsSqlite) Property(e => e.Name).HasColumnType("TEXT COLLATE NOCASE");
		}

#if NetCore
		HasOne(d => d.Task).WithMany(p => p.BackgroundTaskParameters)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_BackgrounTaskParameter_Task");
#else
		HasRequired(d => d.Task).WithMany(p => p.BackgroundTaskParameters);
#endif
    }
}
