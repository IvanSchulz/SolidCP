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

public partial class RdsMessageConfiguration: EntityTypeConfiguration<RdsMessage>
{
    public override void Configure() {
        Property(e => e.UserName).IsFixedLength();

		if (IsSqlServer)
		{
			Property(e => e.MessageText).HasColumnType("ntext");
			Property(e => e.Date).HasColumnType("datetime");
		}
		else if (IsCore && (IsMySql || IsMariaDb || IsSqlite || IsPostgreSql))
		{
			Property(e => e.MessageText).HasColumnType("TEXT");
		}

#if NetCore
        HasOne(d => d.RdsCollection).WithMany(p => p.RdsMessages).HasConstraintName("FK_RDSMessages_RDSCollections");
#else
		// TODO required or optional?
		HasRequired(d => d.RdsCollection).WithMany(p => p.RdsMessages);
#endif
    }
}
