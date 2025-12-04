// This file is auto generated, do not edit.
using System;
using System.Collections.Generic;
using FuseCP.EnterpriseServer.Data.Configuration;
using FuseCP.EnterpriseServer.Data.Entities;
using FuseCP.EnterpriseServer.Data.Extensions;
#if NetCore
using Microsoft.EntityFrameworkCore;
#endif
#if NetFX
using System.Data.Entity;
#endif

namespace FuseCP.EnterpriseServer.Data.Configuration;

public partial class CrmuserConfiguration: EntityTypeConfiguration<Crmuser>
{
    public override void Configure() {
        Property(e => e.ChangedDate).HasDefaultValueSql("(getdate())");
        Property(e => e.CreatedDate).HasDefaultValueSql("(getdate())");

        HasOne(d => d.Account).WithMany(p => p.Crmusers)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CRMUsers_ExchangeAccounts");
    }
}
