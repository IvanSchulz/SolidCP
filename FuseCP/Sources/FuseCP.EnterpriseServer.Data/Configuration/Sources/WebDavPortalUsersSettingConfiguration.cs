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

public partial class WebDavPortalUsersSettingConfiguration: EntityTypeConfiguration<WebDavPortalUsersSetting>
{
    public override void Configure() {
        HasKey(e => e.Id).HasName("PK__WebDavPo__3214EC276D704EE1");

        HasOne(d => d.Account).WithMany(p => p.WebDavPortalUsersSettings).HasConstraintName("FK_WebDavPortalUsersSettings_UserId");
    }
}
