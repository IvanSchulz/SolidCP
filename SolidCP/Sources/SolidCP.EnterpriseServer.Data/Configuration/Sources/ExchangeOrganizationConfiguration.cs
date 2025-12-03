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

public partial class ExchangeOrganizationConfiguration: EntityTypeConfiguration<ExchangeOrganization>
{
    public override void Configure() {
        Property(e => e.ItemId).ValueGeneratedNever();

        HasOne(d => d.Item).WithOne(p => p.ExchangeOrganization).HasConstraintName("FK_ExchangeOrganizations_ServiceItems");
    }
}
