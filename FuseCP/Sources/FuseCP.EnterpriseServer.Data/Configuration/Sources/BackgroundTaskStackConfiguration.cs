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

public partial class BackgroundTaskStackConfiguration: EntityTypeConfiguration<BackgroundTaskStack>
{
    public override void Configure() {
        HasKey(e => e.TaskStackId).HasName("PK__Backgrou__5E44466FBE7A1B97");

        HasOne(d => d.Task).WithMany(p => p.BackgroundTaskStacks)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Backgroun__TaskI__3079F157");
    }
}
