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

public partial class CommentConfiguration: EntityTypeConfiguration<Comment>
{
    public override void Configure() {

        Property(e => e.ItemTypeId).IsUnicode(false);
        if (IsSqlServer) Property(e => e.CreatedDate).HasColumnType("datetime");
        else if (IsCore && IsSqlite) Property(e => e.ItemTypeId).HasColumnType("TEXT COLLATE NOCASE");

#if NetCore
		if (IsSqlServer) Property(e => e.CreatedDate).HasDefaultValueSql("(getdate())");

        HasOne(d => d.User).WithMany(p => p.CommentsNavigation).HasConstraintName("FK_Comments_Users");
#else
		HasRequired(d => d.User).WithMany(p => p.CommentsNavigation);
#endif
    }
}
