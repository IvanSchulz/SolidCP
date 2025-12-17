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

// This file is auto generated, do not edit.
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

public partial class LyncUserConfiguration : EntityTypeConfiguration<LyncUser>
{
	public override void Configure()
	{

		if (IsSqlServer)
		{
			Property(e => e.CreatedDate).HasColumnType("datetime");
			Property(e => e.ModifiedDate).HasColumnType("datetime");
		} else if (IsCore && IsSqlite)
		{
			Property(e => e.SipAddress).HasColumnType("TEXT COLLATE NOCASE");
		}

#if NetCore
		if (IsSqlServer)
		{
			Property(e => e.CreatedDate).HasDefaultValueSql("(getdate())");
			Property(e => e.ModifiedDate).HasDefaultValueSql("(getdate())");
		}

		HasOne(d => d.LyncUserPlan).WithMany(p => p.LyncUsers)
				.OnDelete(DeleteBehavior.ClientSetNull)
				.HasConstraintName("FK_LyncUsers_LyncUserPlans");
#else
			HasRequired(d => d.LyncUserPlan).WithMany(p => p.LyncUsers);
#endif
    }
}
