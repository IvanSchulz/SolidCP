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

public partial class TempIdConfiguration: EntityTypeConfiguration<TempId>
{
    public override void Configure() {
        if (IsCore && IsSqliteFX) Property(e => e.Scope).HasColumnType("BLOB");
    }
}
