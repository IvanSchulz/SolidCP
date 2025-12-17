// Copyright (C) 2025 FuseCP
//
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.

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

public partial class SystemSettingConfiguration: EntityTypeConfiguration<SystemSetting>
{
    public override void Configure() {

		if (IsSqlServer) Property(e => e.PropertyValue).HasColumnType("ntext");
		else if (IsCore && (IsMySql || IsMariaDb || IsSqlite || IsPostgreSql))
		{
			Property(e => e.PropertyValue).HasColumnType("TEXT");
			if (IsCore && IsSqlite)
			{
				Property(e => e.SettingsName).HasColumnType("TEXT COLLATE NOCASE");
				Property(e => e.PropertyName).HasColumnType("TEXT COLLATE NOCASE");
			}
		}

		#region Seed Data
		HasData(() => new SystemSetting[] {
			new SystemSetting() { SettingsName = "AccessIpsSettings", PropertyName = "AccessIps", PropertyValue = "" },
			new SystemSetting() { SettingsName = "AuthenticationSettings", PropertyName = "CanPeerChangeMfa", PropertyValue = "True" },
			new SystemSetting() { SettingsName = "AuthenticationSettings", PropertyName = "MfaTokenAppDisplayName", PropertyValue = "FuseCP" },
			new SystemSetting() { SettingsName = "BackupSettings", PropertyName = "BackupsPath", PropertyValue = "c:\\HostingBackups" },
			new SystemSetting() { SettingsName = "SmtpSettings", PropertyName = "SmtpEnableSsl", PropertyValue = "False" },
			new SystemSetting() { SettingsName = "SmtpSettings", PropertyName = "SmtpPort", PropertyValue = "25" },
			new SystemSetting() { SettingsName = "SmtpSettings", PropertyName = "SmtpServer", PropertyValue = "127.0.0.1" },
			new SystemSetting() { SettingsName = "SmtpSettings", PropertyName = "SmtpUsername", PropertyValue = "postmaster" }
		});
		#endregion
    }
}
