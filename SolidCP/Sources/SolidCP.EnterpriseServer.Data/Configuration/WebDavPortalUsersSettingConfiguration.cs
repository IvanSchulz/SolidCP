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

public partial class WebDavPortalUsersSettingConfiguration: EntityTypeConfiguration<WebDavPortalUsersSetting>
{
    public override void Configure() {
        HasKey(e => e.Id).HasName("PK_WebDavPortalUsersSetting");

#if NetCore
        HasOne(d => d.Account).WithMany(p => p.WebDavPortalUsersSettings).HasConstraintName("FK_WebDavPortalUsersSettings_UserId");
#else
        HasRequired(d => d.Account).WithMany(p => p.WebDavPortalUsersSettings);
#endif

    }
}
