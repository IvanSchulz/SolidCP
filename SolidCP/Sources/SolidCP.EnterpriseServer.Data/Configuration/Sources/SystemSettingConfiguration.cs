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

public partial class SystemSettingConfiguration: EntityTypeConfiguration<SystemSetting>
{
    public override void Configure() {

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
