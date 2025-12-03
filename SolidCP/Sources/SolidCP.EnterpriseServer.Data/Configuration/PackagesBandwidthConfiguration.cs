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

public partial class PackagesBandwidthConfiguration: EntityTypeConfiguration<PackagesBandwidth>
{
    public override void Configure() {

        if (IsSqlServer) Property(e => e.LogDate).HasColumnType("datetime");

#if NetCore
        HasOne(d => d.Group).WithMany(p => p.PackagesBandwidths)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PackagesBandwidth_ResourceGroups");

        HasOne(d => d.Package).WithMany(p => p.PackagesBandwidths)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PackagesBandwidth_Packages");
#else
        HasRequired(d => d.Group).WithMany(p => p.PackagesBandwidths);
        HasRequired(d => d.Package).WithMany(p => p.PackagesBandwidths);
#endif
    }
}
