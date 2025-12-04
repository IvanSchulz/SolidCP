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

public partial class RdsServerSettingConfiguration: EntityTypeConfiguration<RdsServerSetting>
{
    public override void Configure() {

		if (IsSqlServer) Property(e => e.PropertyValue).HasColumnType("ntext");
		else if (IsCore && (IsMySql || IsMariaDb || IsSqlite || IsPostgreSql))
		{
			Property(e => e.PropertyValue).HasColumnType("TEXT");
			if (IsSqlite)
			{
				Property(e => e.SettingsName).HasColumnType("TEXT COLLATE NOCASE");
				Property(e => e.PropertyName).HasColumnType("TEXT COLLATE NOCASE");
			}
		}
	}
}
