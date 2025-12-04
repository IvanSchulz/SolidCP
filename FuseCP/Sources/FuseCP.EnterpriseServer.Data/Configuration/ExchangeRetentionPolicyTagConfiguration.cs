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

public partial class ExchangeRetentionPolicyTagConfiguration : EntityTypeConfiguration<ExchangeRetentionPolicyTag>
{
	public override void Configure()
	{
		if (IsCore && IsSqlite) Property(e => e.TagName).HasColumnType("TEXT COLLATE NOCASE");
	
		HasKey(e => e.TagId).HasName("PK_ExchangeRetentionPolicyTag");
    }
}
