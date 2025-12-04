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

public partial class BackgroundTaskLogConfiguration: EntityTypeConfiguration<BackgroundTaskLog>
{
    public override void Configure() {
        HasKey(e => e.LogId).HasName("PK__Backgrou__5E5499A8D6123A0F");

        HasOne(d => d.Task).WithMany(p => p.BackgroundTaskLogs)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Backgroun__TaskI__2D9D84AC");
    }
}
