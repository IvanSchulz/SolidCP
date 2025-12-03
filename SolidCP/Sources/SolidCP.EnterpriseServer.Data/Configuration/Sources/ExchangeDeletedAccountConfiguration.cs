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

public partial class ExchangeDeletedAccountConfiguration: EntityTypeConfiguration<ExchangeDeletedAccount>
{
    public override void Configure() {
        HasKey(e => e.Id).HasName("PK__Exchange__3214EC278D6C8EE0");
    }
}
