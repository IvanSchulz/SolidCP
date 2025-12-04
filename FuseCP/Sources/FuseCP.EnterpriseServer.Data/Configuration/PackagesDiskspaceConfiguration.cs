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

public partial class PackagesDiskspaceConfiguration: EntityTypeConfiguration<PackagesDiskspace>
{
    public override void Configure() {

#if NetCore
        HasOne(d => d.Group).WithMany(p => p.PackagesDiskspaces)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PackagesDiskspace_ResourceGroups");

        HasOne(d => d.Package).WithMany(p => p.PackagesDiskspaces)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PackagesDiskspace_Packages");
#else
        HasRequired(d => d.Group).WithMany(p => p.PackagesDiskspaces);
        HasRequired(d => d.Package).WithMany(p => p.PackagesDiskspaces);
#endif
    }
}
