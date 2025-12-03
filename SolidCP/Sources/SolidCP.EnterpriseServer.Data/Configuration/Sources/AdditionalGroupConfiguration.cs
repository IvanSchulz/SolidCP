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

public partial class AdditionalGroupConfiguration: EntityTypeConfiguration<AdditionalGroup>
{
    public override void Configure() {
        HasKey(e => e.Id).HasName("PK__Addition__3214EC27FB54BA0E");
    }
}
