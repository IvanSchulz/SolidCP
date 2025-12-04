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

public partial class ThemeConfiguration: EntityTypeConfiguration<Theme>
{
    public override void Configure() {

		#region Seed Data
		HasData(() => new Theme[] {
			new Theme() { DisplayName = "FuseCP v1", DisplayOrder = 1, Enabled = 1, LTRName = "Default", RTLName = "Default", ThemeId = 1 }
		});
		#endregion
	}
}
