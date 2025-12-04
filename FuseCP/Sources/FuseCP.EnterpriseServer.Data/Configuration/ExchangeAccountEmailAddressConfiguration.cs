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

public partial class ExchangeAccountEmailAddressConfiguration : EntityTypeConfiguration<ExchangeAccountEmailAddress>
{
	public override void Configure()
	{

#if NetCore
        HasOne(d => d.Account).WithMany(p => p.ExchangeAccountEmailAddresses).HasConstraintName("FK_ExchangeAccountEmailAddresses_ExchangeAccounts");
#else
		HasRequired(d => d.Account).WithMany(p => p.ExchangeAccountEmailAddresses);
#endif
    }
}
