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

public partial class PrivateIpaddressConfiguration: EntityTypeConfiguration<PrivateIpaddress>
{
    public override void Configure() {
        HasOne(d => d.Item).WithMany(p => p.PrivateIpaddresses).HasConstraintName("FK_PrivateIPAddresses_ServiceItems");
    }
}
