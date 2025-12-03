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

public partial class StorageSpaceFolderConfiguration: EntityTypeConfiguration<StorageSpaceFolder>
{
    public override void Configure() {
        HasKey(e => e.Id).HasName("PK__StorageS__3214EC07E83EAA64");

        HasOne(d => d.StorageSpace).WithMany(p => p.StorageSpaceFolders).HasConstraintName("FK_StorageSpaceFolders_StorageSpaceId");
    }
}
