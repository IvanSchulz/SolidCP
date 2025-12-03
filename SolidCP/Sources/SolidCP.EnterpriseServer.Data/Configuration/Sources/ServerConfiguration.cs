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

public partial class ServerConfiguration: EntityTypeConfiguration<Server>
{
    public override void Configure() {
        Property(e => e.Adenabled).HasDefaultValue(false);
        Property(e => e.ServerUrl).HasDefaultValue("");

        HasOne(d => d.PrimaryGroup).WithMany(p => p.Servers).HasConstraintName("FK_Servers_ResourceGroups");
    }
}
