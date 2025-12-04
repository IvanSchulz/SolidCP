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

using BackgroundTask = FuseCP.EnterpriseServer.Data.Entities.BackgroundTask;

public partial class BackgroundTaskConfiguration: EntityTypeConfiguration<BackgroundTask>
{
    public override void Configure() {
		HasKey(e => e.Id).HasName("PK_BackgroundTask");
		if (IsSqlServer)
        {
            Property(e => e.StartDate).HasColumnType("datetime");
            Property(e => e.FinishDate).HasColumnType("datetime");
        }
        else if (IsCore && IsSqliteFX) Property(e => e.Guid).HasColumnType("BLOB");
	}
}
