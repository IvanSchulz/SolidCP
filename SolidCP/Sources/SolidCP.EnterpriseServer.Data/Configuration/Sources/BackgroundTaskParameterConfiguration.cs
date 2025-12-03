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

using BackgroundTaskParameter = FuseCP.EnterpriseServer.Data.Entities.BackgroundTaskParameter;

public partial class BackgroundTaskParameterConfiguration: EntityTypeConfiguration<BackgroundTaskParameter>
{
    public override void Configure() {
        HasKey(e => e.ParameterId).HasName("PK__Backgrou__F80C629704591A62");

        HasOne(d => d.Task).WithMany(p => p.BackgroundTaskParameters)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Backgroun__TaskI__2AC11801");
    }
}
