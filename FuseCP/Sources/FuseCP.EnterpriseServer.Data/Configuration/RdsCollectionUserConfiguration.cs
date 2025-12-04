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

public partial class RdsCollectionUserConfiguration: EntityTypeConfiguration<RdsCollectionUser>
{
    public override void Configure() {
        HasKey(e => e.Id).HasName("PK_RdsCollectionUser");

#if NetCore
        HasOne(d => d.Account).WithMany(p => p.RdsCollectionUsers).HasConstraintName("FK_RDSCollectionUsers_UserId");
        HasOne(d => d.RdsCollection).WithMany(p => p.RdsCollectionUsers).HasConstraintName("FK_RDSCollectionUsers_RDSCollectionId");
#else
        HasRequired(d => d.Account).WithMany(p => p.RdsCollectionUsers);
        HasRequired(d => d.RdsCollection).WithMany(p => p.RdsCollectionUsers);
#endif
    }
}
