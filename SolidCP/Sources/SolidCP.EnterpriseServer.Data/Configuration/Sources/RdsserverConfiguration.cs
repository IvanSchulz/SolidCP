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

public partial class RdsserverConfiguration: EntityTypeConfiguration<Rdsserver>
{
    public override void Configure() {
        HasKey(e => e.Id).HasName("PK__RDSServe__3214EC27815F05B3");

        Property(e => e.ConnectionEnabled).HasDefaultValue(true);

        HasOne(d => d.Rdscollection).WithMany(p => p.Rdsservers).HasConstraintName("FK_RDSServers_RDSCollectionId");
    }
}
