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

public partial class DomainDnsRecordConfiguration: EntityTypeConfiguration<DomainDnsRecord>
{
    public override void Configure() {
		HasKey(e => e.Id).HasName("PK_DomainDnsRecord");

		if (IsSqlServer) Property(e => e.Date).HasColumnType("datetime");

#if NetCore
        HasOne(d => d.Domain).WithMany(p => p.DomainDnsRecords).HasConstraintName("FK_DomainDnsRecords_DomainId");
#else
		HasRequired(d => d.Domain).WithMany(p => p.DomainDnsRecords);
#endif
    }
}
