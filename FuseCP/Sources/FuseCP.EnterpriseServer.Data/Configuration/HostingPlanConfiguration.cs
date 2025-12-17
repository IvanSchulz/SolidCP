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

public partial class HostingPlanConfiguration : EntityTypeConfiguration<HostingPlan>
{
	public override void Configure()
	{

		if (IsSqlServer)
		{
			Property(e => e.PlanDescription).HasColumnType("ntext");
			Property(e => e.SetupPrice).HasColumnType("money");
			Property(e => e.RecurringPrice).HasColumnType("money");
		}
		else if (IsCore && (IsMySql || IsMariaDb || IsSqlite || IsPostgreSql))
		{
			Property(e => e.PlanDescription).HasColumnType("TEXT");
			if (IsSqlite) Property(e => e.PlanName).HasColumnType("TEXT COLLATE NOCASE");
		}


#if NetCore
        HasMany(d => d.Packages).WithOne(p => p.HostingPlan)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_HostingPlans_Packages");

        HasOne(d => d.Server).WithMany(p => p.HostingPlans).HasConstraintName("FK_HostingPlans_Servers");

        HasOne(d => d.User).WithMany(p => p.HostingPlans).HasConstraintName("FK_HostingPlans_Users");
#else
		HasMany(d => d.Packages).WithOptional(p => p.HostingPlan).WillCascadeOnDelete();
		HasOptional(d => d.Server).WithMany(p => p.HostingPlans);
		HasOptional(d => d.User).WithMany(p => p.HostingPlans);
#endif
    }
}
