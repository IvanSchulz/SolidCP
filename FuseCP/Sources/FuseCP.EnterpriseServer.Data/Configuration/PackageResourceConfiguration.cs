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

public partial class PackageResourceConfiguration: EntityTypeConfiguration<PackageResource>
{
    public override void Configure() {
        HasKey(e => new { e.PackageId, e.GroupId }).HasName("PK_PackageResources");

#if NetCore
        HasOne(d => d.Group).WithMany(p => p.PackageResources)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PackageResources_ResourceGroups");

        HasOne(d => d.Package).WithMany(p => p.PackageResources)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PackageResources_Packages");
#else
        HasRequired(d => d.Group).WithMany(p => p.PackageResources);
        HasRequired(d => d.Package).WithMany(p => p.PackageResources);
#endif
    }
}
