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

public partial class ServiceItemPropertyConfiguration: EntityTypeConfiguration<ServiceItemProperty>
{
    public override void Configure() {
        HasOne(d => d.Item).WithMany(p => p.ServiceItemProperties).HasConstraintName("FK_ServiceItemProperties_ServiceItems");
    }
}
