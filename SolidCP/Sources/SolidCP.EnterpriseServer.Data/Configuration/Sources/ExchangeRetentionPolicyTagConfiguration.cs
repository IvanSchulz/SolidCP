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

public partial class ExchangeRetentionPolicyTagConfiguration: EntityTypeConfiguration<ExchangeRetentionPolicyTag>
{
    public override void Configure() {
        HasKey(e => e.TagId).HasName("PK__Exchange__657CFA4C686574AB");
    }
}
