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

public partial class ExchangeOrganizationSsFolderConfiguration: EntityTypeConfiguration<ExchangeOrganizationSsFolder>
{
    public override void Configure() {
        HasKey(e => e.Id).HasName("PK__Exchange__3214EC07A047A7E6");

        HasOne(d => d.Item).WithMany(p => p.ExchangeOrganizationSsFolders).HasConstraintName("FK_ExchangeOrganizationSsFolders_ItemId");

        HasOne(d => d.StorageSpaceFolder).WithMany(p => p.ExchangeOrganizationSsFolders).HasConstraintName("FK_ExchangeOrganizationSsFolders_StorageSpaceFolderId");
    }
}
