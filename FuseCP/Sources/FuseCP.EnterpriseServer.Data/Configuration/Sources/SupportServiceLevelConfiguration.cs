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

public partial class SupportServiceLevelConfiguration: EntityTypeConfiguration<SupportServiceLevel>
{
    public override void Configure() {
        HasKey(e => e.LevelId).HasName("PK__SupportS__09F03C06E93AE0C4");
    }
}
