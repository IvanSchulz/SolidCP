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

public partial class DomainDnsRecordConfiguration: EntityTypeConfiguration<DomainDnsRecord>
{
    public override void Configure() {
        HasKey(e => e.Id).HasName("PK__DomainDn__3214EC27DE726E9A");

        HasOne(d => d.Domain).WithMany(p => p.DomainDnsRecords).HasConstraintName("FK_DomainDnsRecords_DomainId");
    }
}
