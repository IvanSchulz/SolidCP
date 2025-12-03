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

public partial class CommentConfiguration: EntityTypeConfiguration<Comment>
{
    public override void Configure() {
        Property(e => e.CreatedDate).HasDefaultValueSql("(getdate())");

        HasOne(d => d.User).WithMany(p => p.CommentsNavigation).HasConstraintName("FK_Comments_Users");
    }
}
