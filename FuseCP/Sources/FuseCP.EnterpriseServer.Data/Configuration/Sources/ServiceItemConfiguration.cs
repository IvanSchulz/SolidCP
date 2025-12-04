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

public partial class ServiceItemConfiguration: EntityTypeConfiguration<ServiceItem>
{
    public override void Configure() {
        HasOne(d => d.ItemType).WithMany(p => p.ServiceItems).HasConstraintName("FK_ServiceItems_ServiceItemTypes");

        HasOne(d => d.Package).WithMany(p => p.ServiceItems).HasConstraintName("FK_ServiceItems_Packages");

        HasOne(d => d.Service).WithMany(p => p.ServiceItems).HasConstraintName("FK_ServiceItems_Services");
    }
}
