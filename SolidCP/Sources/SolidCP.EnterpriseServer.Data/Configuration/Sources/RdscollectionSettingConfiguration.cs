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

public partial class RdscollectionSettingConfiguration: EntityTypeConfiguration<RdscollectionSetting>
{
    public override void Configure() {
        HasOne(d => d.Rdscollection).WithMany(p => p.RdscollectionSettings).HasConstraintName("FK_RDSCollectionSettings_RDSCollections");
    }
}
