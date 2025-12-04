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

public partial class PackageVlanConfiguration: EntityTypeConfiguration<PackageVlan>
{
    public override void Configure() {
        HasKey(e => e.PackageVlanId).HasName("PK_PackageVlan");

#if NetCore
        Property(e => e.IsDmz).HasDefaultValue(false);

        HasOne(d => d.Package).WithMany(p => p.PackageVlans).HasConstraintName("FK_PackageID");

        HasOne(d => d.Vlan).WithMany(p => p.PackageVlans).HasConstraintName("FK_VlanID");
#else
        HasRequired(d => d.Package).WithMany(p => p.PackageVlans);
        HasRequired(d => d.Vlan).WithMany(p => p.PackageVlans);
#endif
    }
}
