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

public partial class EnterpriseFoldersOwaPermissionConfiguration: EntityTypeConfiguration<EnterpriseFoldersOwaPermission>
{
    public override void Configure() {
        HasKey(e => e.Id).HasName("PK_EnterpriseFoldersOwaPermission");

#if NetCore
        HasOne(d => d.Account).WithMany(p => p.EnterpriseFoldersOwaPermissions).HasConstraintName("FK_EnterpriseFoldersOwaPermissions_AccountId");

        HasOne(d => d.Folder).WithMany(p => p.EnterpriseFoldersOwaPermissions).HasConstraintName("FK_EnterpriseFoldersOwaPermissions_FolderId");
#else
        HasRequired(d => d.Account).WithMany(p => p.EnterpriseFoldersOwaPermissions);
        HasRequired(d => d.Folder).WithMany(p => p.EnterpriseFoldersOwaPermissions);
#endif
    }
}
