using System;
using System.Collections.Generic;
using FuseCP.EnterpriseServer.Data.Configuration;
using FuseCP.EnterpriseServer.Data.Entities;
using System.ComponentModel.DataAnnotations.Schema;
#if NetCore
using Microsoft.EntityFrameworkCore;
#endif
#if NetFX
using System.Data.Entity;
#endif

namespace FuseCP.EnterpriseServer.Data.Configuration;

public partial class StorageSpaceLevelConfiguration: EntityTypeConfiguration<StorageSpaceLevel>
{
    public override void Configure() {
        HasKey(e => e.Id).HasName("PK_StorageSpaceLevel");
    }
}
