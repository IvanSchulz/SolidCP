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

public partial class PackageIpaddressConfiguration: EntityTypeConfiguration<PackageIpaddress>
{
    public override void Configure() {
        HasOne(d => d.Address).WithMany(p => p.PackageIpaddresses)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PackageIPAddresses_IPAddresses");

        HasOne(d => d.Item).WithMany(p => p.PackageIpaddresses).HasConstraintName("FK_PackageIPAddresses_ServiceItems");

        HasOne(d => d.Package).WithMany(p => p.PackageIpaddresses).HasConstraintName("FK_PackageIPAddresses_Packages");
    }
}
