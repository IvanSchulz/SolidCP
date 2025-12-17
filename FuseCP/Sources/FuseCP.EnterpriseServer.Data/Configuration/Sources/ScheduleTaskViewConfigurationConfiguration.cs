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

public partial class ScheduleTaskViewConfigurationConfiguration: EntityTypeConfiguration<ScheduleTaskViewConfiguration>
{
    public override void Configure() {
        HasOne(d => d.Task).WithMany(p => p.ScheduleTaskViewConfigurations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ScheduleTaskViewConfiguration_ScheduleTaskViewConfiguration");

        #region Seed Data
        HasData(() => new ScheduleTaskViewConfiguration[] {
            new ScheduleTaskViewConfiguration() { ConfigurationId = "ASP_NET", TaskId = "SCHEDULE_TASK_ACTIVATE_PAID_INVOICES", Description = "~/DesktopModules/FuseCP/ScheduleTaskControls/EmptyView.ascx", Environment = "ASP.NET" },
            new ScheduleTaskViewConfiguration() { ConfigurationId = "ASP_NET", TaskId = "SCHEDULE_TASK_AUDIT_LOG_REPORT", Description = "~/DesktopModules/FuseCP/ScheduleTaskControls/AuditLogReportView.ascx", Environment = "ASP.NET" },
            new ScheduleTaskViewConfiguration() { ConfigurationId = "ASP_NET", TaskId = "SCHEDULE_TASK_BACKUP", Description = "~/DesktopModules/FuseCP/ScheduleTaskControls/Backup.ascx", Environment = "ASP.NET" },
            new ScheduleTaskViewConfiguration() { ConfigurationId = "ASP_NET", TaskId = "SCHEDULE_TASK_BACKUP_DATABASE", Description = "~/DesktopModules/FuseCP/ScheduleTaskControls/BackupDatabase.ascx", Environment = "ASP.NET" },
            new ScheduleTaskViewConfiguration() { ConfigurationId = "ASP_NET", TaskId = "SCHEDULE_TASK_CALCULATE_EXCHANGE_DISKSPACE", Description = "~/DesktopModules/FuseCP/ScheduleTaskControls/EmptyView.ascx", Environment = "ASP.NET" },
            new ScheduleTaskViewConfiguration() { ConfigurationId = "ASP_NET", TaskId = "SCHEDULE_TASK_CALCULATE_PACKAGES_BANDWIDTH", Description = "~/DesktopModules/FuseCP/ScheduleTaskControls/EmptyView.ascx", Environment = "ASP.NET" },
            new ScheduleTaskViewConfiguration() { ConfigurationId = "ASP_NET", TaskId = "SCHEDULE_TASK_CALCULATE_PACKAGES_DISKSPACE", Description = "~/DesktopModules/FuseCP/ScheduleTaskControls/EmptyView.ascx", Environment = "ASP.NET" },
            new ScheduleTaskViewConfiguration() { ConfigurationId = "ASP_NET", TaskId = "SCHEDULE_TASK_CANCEL_OVERDUE_INVOICES", Description = "~/DesktopModules/FuseCP/ScheduleTaskControls/EmptyView.ascx", Environment = "ASP.NET" },
            new ScheduleTaskViewConfiguration() { ConfigurationId = "ASP_NET", TaskId = "SCHEDULE_TASK_CHECK_WEBSITE", Description = "~/DesktopModules/FuseCP/ScheduleTaskControls/CheckWebsite.ascx", Environment = "ASP.NET" },
            new ScheduleTaskViewConfiguration() { ConfigurationId = "ASP_NET", TaskId = "SCHEDULE_TASK_DOMAIN_EXPIRATION", Description = "~/DesktopModules/FuseCP/ScheduleTaskControls/DomainExpirationView.ascx", Environment = "ASP.NET" },
            new ScheduleTaskViewConfiguration() { ConfigurationId = "ASP_NET", TaskId = "SCHEDULE_TASK_DOMAIN_LOOKUP", Description = "~/DesktopModules/FuseCP/ScheduleTaskControls/DomainLookupView.ascx", Environment = "ASP.NET" },
            new ScheduleTaskViewConfiguration() { ConfigurationId = "ASP_NET", TaskId = "SCHEDULE_TASK_FTP_FILES", Description = "~/DesktopModules/FuseCP/ScheduleTaskControls/SendFilesViaFtp.ascx", Environment = "ASP.NET" },
            new ScheduleTaskViewConfiguration() { ConfigurationId = "ASP_NET", TaskId = "SCHEDULE_TASK_GENERATE_INVOICES", Description = "~/DesktopModules/FuseCP/ScheduleTaskControls/EmptyView.ascx", Environment = "ASP.NET" },
            new ScheduleTaskViewConfiguration() { ConfigurationId = "ASP_NET", TaskId = "SCHEDULE_TASK_HOSTED_SOLUTION_REPORT", Description = "~/DesktopModules/FuseCP/ScheduleTaskControls/HostedSolutionReport.ascx", Environment = "ASP.NET" },
            new ScheduleTaskViewConfiguration() { ConfigurationId = "ASP_NET", TaskId = "SCHEDULE_TASK_NOTIFY_OVERUSED_DATABASES", Description = "~/DesktopModules/FuseCP/ScheduleTaskControls/NotifyOverusedDatabases.ascx", Environment = "ASP.NET" },
            new ScheduleTaskViewConfiguration() { ConfigurationId = "ASP_NET", TaskId = "SCHEDULE_TASK_RUN_PAYMENT_QUEUE", Description = "~/DesktopModules/FuseCP/ScheduleTaskControls/EmptyView.ascx", Environment = "ASP.NET" },
            new ScheduleTaskViewConfiguration() { ConfigurationId = "ASP_NET", TaskId = "SCHEDULE_TASK_RUN_SYSTEM_COMMAND", Description = "~/DesktopModules/FuseCP/ScheduleTaskControls/ExecuteSystemCommand.ascx", Environment = "ASP.NET" },
            new ScheduleTaskViewConfiguration() { ConfigurationId = "ASP_NET", TaskId = "SCHEDULE_TASK_SEND_MAIL", Description = "~/DesktopModules/FuseCP/ScheduleTaskControls/SendEmailNotification.ascx", Environment = "ASP.NET" },
            new ScheduleTaskViewConfiguration() { ConfigurationId = "ASP_NET", TaskId = "SCHEDULE_TASK_SUSPEND_OVERDUE_INVOICES", Description = "~/DesktopModules/FuseCP/ScheduleTaskControls/EmptyView.ascx", Environment = "ASP.NET" },
            new ScheduleTaskViewConfiguration() { ConfigurationId = "ASP_NET", TaskId = "SCHEDULE_TASK_SUSPEND_PACKAGES", Description = "~/DesktopModules/FuseCP/ScheduleTaskControls/SuspendOverusedSpaces.ascx", Environment = "ASP.NET" },
            new ScheduleTaskViewConfiguration() { ConfigurationId = "ASP_NET", TaskId = "SCHEDULE_TASK_USER_PASSWORD_EXPIRATION_NOTIFICATION", Description = "~/DesktopModules/FuseCP/ScheduleTaskControls/UserPasswordExpirationNotification" +
                "View.ascx", Environment = "ASP.NET" },
            new ScheduleTaskViewConfiguration() { ConfigurationId = "ASP_NET", TaskId = "SCHEDULE_TASK_ZIP_FILES", Description = "~/DesktopModules/FuseCP/ScheduleTaskControls/ZipFiles.ascx", Environment = "ASP.NET" }
        });
        #endregion

    }
}
