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

public partial class BlackBerryUserConfiguration: EntityTypeConfiguration<BlackBerryUser>
{
    public override void Configure() {

		if (IsSqlServer)
		{
			Property(e => e.CreatedDate).HasColumnType("datetime");
			Property(e => e.ModifiedDate).HasColumnType("datetime");
		}

#if NetCore
        if (IsSqlServer) Property(e => e.CreatedDate).HasDefaultValueSql("(getdate())");

        HasOne(d => d.Account).WithMany(p => p.BlackBerryUsers)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_BlackBerryUsers_ExchangeAccounts");
#else
		HasRequired(d => d.Account).WithMany(p => p.BlackBerryUsers);
#endif
    }
}
