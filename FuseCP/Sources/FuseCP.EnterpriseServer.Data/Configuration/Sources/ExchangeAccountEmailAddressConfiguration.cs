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

public partial class ExchangeAccountEmailAddressConfiguration: EntityTypeConfiguration<ExchangeAccountEmailAddress>
{
    public override void Configure() {
        HasOne(d => d.Account).WithMany(p => p.ExchangeAccountEmailAddresses).HasConstraintName("FK_ExchangeAccountEmailAddresses_ExchangeAccounts");
    }
}
