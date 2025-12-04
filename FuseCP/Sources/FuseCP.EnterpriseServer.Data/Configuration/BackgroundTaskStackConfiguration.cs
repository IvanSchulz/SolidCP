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

public partial class BackgroundTaskStackConfiguration: EntityTypeConfiguration<BackgroundTaskStack>
{
    public override void Configure() {
		HasKey(e => e.TaskStackId).HasName("PK_BackgroundTaskStack");

#if NetCore
        HasOne(d => d.Task).WithMany(p => p.BackgroundTaskStacks)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_BackgroundTaskStack_Task");
#else
		HasRequired(d => d.Task).WithMany(p => p.BackgroundTaskStacks);
#endif

    }
}
