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

public partial class CrmUserConfiguration : EntityTypeConfiguration<CrmUser>
{
	public override void Configure()
	{

		if (IsSqlServer)
		{
			Property(e => e.ChangedDate).HasColumnType("datetime");
			Property(e => e.CreatedDate).HasColumnType("datetime");
		}
		else if (IsCore && IsSqliteFX)
		{
			Property(e => e.CrmUserGuid).HasColumnType("BLOB");
			Property(e => e.BusinessUnitId).HasColumnType("BLOB");
		}

#if NetCore
			if (IsSqlServer)
		{
			Property(e => e.ChangedDate).HasDefaultValueSql("(getdate())");
			Property(e => e.CreatedDate).HasDefaultValueSql("(getdate())");
		}

		HasOne(d => d.Account).WithMany(p => p.CrmUsers)
				.OnDelete(DeleteBehavior.ClientSetNull)
				.HasConstraintName("FK_CRMUsers_ExchangeAccounts");
#else
		HasRequired(d => d.Account).WithMany(p => p.CrmUsers);
#endif
    }
}
