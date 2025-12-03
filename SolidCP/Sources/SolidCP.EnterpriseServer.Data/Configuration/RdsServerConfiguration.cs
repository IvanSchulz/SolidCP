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

public partial class RdsServerConfiguration: EntityTypeConfiguration<RdsServer>
{
    public override void Configure() {
        HasKey(e => e.Id).HasName("PK_RdsServer");

#if NetCore
        Property(e => e.ConnectionEnabled).HasDefaultValue(true);

        HasOne(d => d.RdsCollection).WithMany(p => p.RdsServers).HasConstraintName("FK_RDSServers_RDSCollectionId");
#else
        HasOptional(d => d.RdsCollection).WithMany(p => p.RdsServers);
#endif
    }
}
