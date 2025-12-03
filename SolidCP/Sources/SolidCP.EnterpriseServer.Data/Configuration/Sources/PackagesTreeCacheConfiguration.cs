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

public partial class PackagesTreeCacheConfiguration: EntityTypeConfiguration<PackagesTreeCache>
{
    public override void Configure() {
        HasIndex(e => new { e.ParentPackageId, e.PackageId }, "PackagesTreeCacheIndex").IsClustered();

        HasOne(d => d.Package).WithMany()
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PackagesTreeCache_Packages1");

        HasOne(d => d.ParentPackage).WithMany()
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PackagesTreeCache_Packages");

        #region Seed Data
        HasData(() => new PackagesTreeCache[] {
            new PackagesTreeCache() { ParentPackageId = 1, PackageId = 1 }
        });
        #endregion

    }
}
