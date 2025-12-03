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

public partial class PackagesTreeCacheConfiguration: EntityTypeConfiguration<PackagesTreeCache>
{
    public override void Configure() {
        //HasIndex(e => new { e.ParentPackageId, e.PackageId }, "PackagesTreeCacheIndex").IsClustered();

#if NetCore
        HasOne(d => d.Package).WithMany()
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PackagesTreeCache_Packages1");

        HasOne(d => d.ParentPackage).WithMany()
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PackagesTreeCache_Packages");
#else
        HasRequired(d => d.Package).WithMany();
        HasRequired(d => d.ParentPackage).WithMany();
#endif

		#region Seed Data
		HasData(() => new PackagesTreeCache[] {
			new PackagesTreeCache() { PackageId = 1, ParentPackageId = 1 }
		});
		#endregion
    }
}
