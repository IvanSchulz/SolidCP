// This file is auto generated, do not edit.
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

public partial class RdsCollectionConfiguration : EntityTypeConfiguration<RdsCollection>
{
    public override void Configure() {

        if (IsCore && IsSqlite) Property(e => e.Name).HasColumnType("TEXT COLLATE NOCASE");

        HasKey(e => e.Id).HasName("PK_RdsCollection");
    }
}
