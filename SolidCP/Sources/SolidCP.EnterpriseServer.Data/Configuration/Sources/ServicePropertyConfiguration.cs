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

public partial class ServicePropertyConfiguration: EntityTypeConfiguration<ServiceProperty>
{
    public override void Configure() {
        HasKey(e => new { e.ServiceId, e.PropertyName }).HasName("PK_ServiceProperties_1");

        HasOne(d => d.Service).WithMany(p => p.ServiceProperties).HasConstraintName("FK_ServiceProperties_Services");
    }
}
