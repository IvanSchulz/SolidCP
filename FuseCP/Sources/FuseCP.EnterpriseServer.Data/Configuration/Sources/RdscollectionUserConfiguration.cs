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

public partial class RdscollectionUserConfiguration: EntityTypeConfiguration<RdscollectionUser>
{
    public override void Configure() {
        HasKey(e => e.Id).HasName("PK__RDSColle__3214EC27E7092CA1");

        HasOne(d => d.Account).WithMany(p => p.RdscollectionUsers).HasConstraintName("FK_RDSCollectionUsers_UserId");

        HasOne(d => d.Rdscollection).WithMany(p => p.RdscollectionUsers).HasConstraintName("FK_RDSCollectionUsers_RDSCollectionId");
    }
}
