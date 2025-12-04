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

public partial class StorageSpaceFolderConfiguration: EntityTypeConfiguration<StorageSpaceFolder>
{
    public override void Configure() {
        HasKey(e => e.Id).HasName("PK_StorageSpaceFolder");

        Property(e => e.Name).IsUnicode(false);
        Property(e => e.Path).IsUnicode(false);
        Property(e => e.UncPath).IsUnicode(false);

#if NetCore
        HasOne(d => d.StorageSpace).WithMany(p => p.StorageSpaceFolders).HasConstraintName("FK_StorageSpaceFolders_StorageSpaceId");
#else
        HasRequired(d => d.StorageSpace).WithMany(p => p.StorageSpaceFolders);
#endif
    }
}
