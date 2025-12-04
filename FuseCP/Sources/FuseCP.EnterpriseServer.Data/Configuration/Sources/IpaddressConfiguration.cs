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

public partial class IpaddressConfiguration: EntityTypeConfiguration<Ipaddress>
{
    public override void Configure() {
        HasOne(d => d.Server).WithMany(p => p.Ipaddresses)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_IPAddresses_Servers");
    }
}
