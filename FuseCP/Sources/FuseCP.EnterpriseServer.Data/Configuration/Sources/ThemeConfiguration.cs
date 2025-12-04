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

public partial class ThemeConfiguration: EntityTypeConfiguration<Theme>
{
    public override void Configure() {

        #region Seed Data
        HasData(() => new Theme[] {
            new Theme() { ThemeId = 1, DisplayName = "FuseCP v1", DisplayOrder = 1, Enabled = 1, LTRName = "Default", RTLName = "Default" }
        });
        #endregion

    }
}
