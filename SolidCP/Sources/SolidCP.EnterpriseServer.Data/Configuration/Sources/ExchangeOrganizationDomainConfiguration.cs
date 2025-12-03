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

public partial class ExchangeOrganizationDomainConfiguration: EntityTypeConfiguration<ExchangeOrganizationDomain>
{
    public override void Configure() {
        Property(e => e.IsHost).HasDefaultValue(false);

        HasOne(d => d.Item).WithMany(p => p.ExchangeOrganizationDomains).HasConstraintName("FK_ExchangeOrganizationDomains_ServiceItems");
    }
}
