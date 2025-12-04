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

public partial class StorageSpaceLevelResourceGroupConfiguration: EntityTypeConfiguration<StorageSpaceLevelResourceGroup>
{
    public override void Configure() {
        HasKey(e => e.Id).HasName("PK_StorageSpaceLevelResourceGroup");

#if NetCore
        HasOne(d => d.Group).WithMany(p => p.StorageSpaceLevelResourceGroups).HasConstraintName("FK_StorageSpaceLevelResourceGroups_GroupId");

        HasOne(d => d.Level).WithMany(p => p.StorageSpaceLevelResourceGroups).HasConstraintName("FK_StorageSpaceLevelResourceGroups_LevelId");
#else
        HasRequired(d => d.Group).WithMany(p => p.StorageSpaceLevelResourceGroups);
        HasRequired(d => d.Level).WithMany(p => p.StorageSpaceLevelResourceGroups);
#endif
    }
}
