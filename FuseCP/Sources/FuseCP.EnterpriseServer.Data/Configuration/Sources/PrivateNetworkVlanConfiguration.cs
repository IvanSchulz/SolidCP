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

public partial class PrivateNetworkVlanConfiguration: EntityTypeConfiguration<PrivateNetworkVlan>
{
    public override void Configure() {
        HasKey(e => e.VlanId).HasName("PK__PrivateN__834813555DED7474");

        HasOne(d => d.Server).WithMany(p => p.PrivateNetworkVlans)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_ServerID");
    }
}
