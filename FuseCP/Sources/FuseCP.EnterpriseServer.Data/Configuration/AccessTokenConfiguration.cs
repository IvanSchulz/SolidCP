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

public partial class AccessTokenConfiguration: EntityTypeConfiguration<AccessToken>
{
	public override void Configure() {
		HasKey(e => e.Id).HasName("PK_AccessToken");

		Property(e => e.SmsResponse).IsUnicode(false);
        if (IsSqlServer) Property(e => e.ExpirationDate).HasColumnType("datetime");
		else if (IsCore && IsSqliteFX) Property(e => e.AccessTokenGuid).HasColumnType("BLOB");

#if NetCore
		HasOne(d => d.Account).WithMany(p => p.AccessTokens).HasConstraintName("FK_AccessTokens_UserId");
#else
        HasRequired(d => d.Account).WithMany(p => p.AccessTokens);
#endif
    }
}
