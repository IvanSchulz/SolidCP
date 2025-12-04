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

using GlobalDnsRecord = FuseCP.EnterpriseServer.Data.Entities.GlobalDnsRecord;

public partial class GlobalDnsRecordConfiguration: EntityTypeConfiguration<GlobalDnsRecord>
{
    public override void Configure() {
        HasOne(d => d.Ipaddress).WithMany(p => p.GlobalDnsRecords).HasConstraintName("FK_GlobalDnsRecords_IPAddresses");

        HasOne(d => d.Package).WithMany(p => p.GlobalDnsRecords)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_GlobalDnsRecords_Packages");

        HasOne(d => d.Server).WithMany(p => p.GlobalDnsRecords).HasConstraintName("FK_GlobalDnsRecords_Servers");

        HasOne(d => d.Service).WithMany(p => p.GlobalDnsRecords)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_GlobalDnsRecords_Services");
    }
}
