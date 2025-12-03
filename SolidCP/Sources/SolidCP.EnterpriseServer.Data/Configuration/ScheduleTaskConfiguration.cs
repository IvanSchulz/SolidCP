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

public partial class ScheduleTaskConfiguration: EntityTypeConfiguration<ScheduleTask>
{
    public override void Configure() {

        #region Seed Data
        HasData(() => new ScheduleTask[] {
            new ScheduleTask() { TaskId = "SCHEDULE_TASK_ACTIVATE_PAID_INVOICES", TaskType = "FuseCP.Ecommerce.EnterpriseServer.ActivatePaidInvoicesTask, FuseCP.EnterpriseS" +
                "erver.Code" },
            new ScheduleTask() { TaskId = "SCHEDULE_TASK_AUDIT_LOG_REPORT", RoleId = 3, TaskType = "FuseCP.EnterpriseServer.AuditLogReportTask, FuseCP.EnterpriseServer.Code" },
            new ScheduleTask() { TaskId = "SCHEDULE_TASK_BACKUP", RoleId = 1, TaskType = "FuseCP.EnterpriseServer.BackupTask, FuseCP.EnterpriseServer.Code" },
            new ScheduleTask() { TaskId = "SCHEDULE_TASK_BACKUP_DATABASE", RoleId = 3, TaskType = "FuseCP.EnterpriseServer.BackupDatabaseTask, FuseCP.EnterpriseServer.Code" },
            new ScheduleTask() { TaskId = "SCHEDULE_TASK_CALCULATE_EXCHANGE_DISKSPACE", RoleId = 2, TaskType = "FuseCP.EnterpriseServer.CalculateExchangeDiskspaceTask, FuseCP.EnterpriseServe" +
                "r.Code" },
            new ScheduleTask() { TaskId = "SCHEDULE_TASK_CALCULATE_PACKAGES_BANDWIDTH", RoleId = 1, TaskType = "FuseCP.EnterpriseServer.CalculatePackagesBandwidthTask, FuseCP.EnterpriseServe" +
                "r.Code" },
            new ScheduleTask() { TaskId = "SCHEDULE_TASK_CALCULATE_PACKAGES_DISKSPACE", RoleId = 1, TaskType = "FuseCP.EnterpriseServer.CalculatePackagesDiskspaceTask, FuseCP.EnterpriseServe" +
                "r.Code" },
            new ScheduleTask() { TaskId = "SCHEDULE_TASK_CANCEL_OVERDUE_INVOICES", TaskType = "FuseCP.Ecommerce.EnterpriseServer.CancelOverdueInvoicesTask, FuseCP.Enterprise" +
                "Server.Code" },
            new ScheduleTask() { TaskId = "SCHEDULE_TASK_CHECK_WEBSITE", RoleId = 3, TaskType = "FuseCP.EnterpriseServer.CheckWebSiteTask, FuseCP.EnterpriseServer.Code" },
            new ScheduleTask() { TaskId = "SCHEDULE_TASK_DELETE_EXCHANGE_ACCOUNTS", RoleId = 3, TaskType = "FuseCP.EnterpriseServer.DeleteExchangeAccountsTask, FuseCP.EnterpriseServer.Co" +
                "de" },
            new ScheduleTask() { TaskId = "SCHEDULE_TASK_DOMAIN_EXPIRATION", RoleId = 3, TaskType = "FuseCP.EnterpriseServer.DomainExpirationTask, FuseCP.EnterpriseServer.Code" },
            new ScheduleTask() { TaskId = "SCHEDULE_TASK_DOMAIN_LOOKUP", RoleId = 1, TaskType = "FuseCP.EnterpriseServer.DomainLookupViewTask, FuseCP.EnterpriseServer.Code" },
            new ScheduleTask() { TaskId = "SCHEDULE_TASK_FTP_FILES", RoleId = 3, TaskType = "FuseCP.EnterpriseServer.FTPFilesTask, FuseCP.EnterpriseServer.Code" },
            new ScheduleTask() { TaskId = "SCHEDULE_TASK_GENERATE_INVOICES", TaskType = "FuseCP.Ecommerce.EnterpriseServer.GenerateInvoicesTask, FuseCP.EnterpriseServe" +
                "r.Code" },
            new ScheduleTask() { TaskId = "SCHEDULE_TASK_HOSTED_SOLUTION_REPORT", RoleId = 2, TaskType = "FuseCP.EnterpriseServer.HostedSolutionReportTask, FuseCP.EnterpriseServer.Code" },
            new ScheduleTask() { TaskId = "SCHEDULE_TASK_NOTIFY_OVERUSED_DATABASES", RoleId = 2, TaskType = "FuseCP.EnterpriseServer.NotifyOverusedDatabasesTask, FuseCP.EnterpriseServer.C" +
                "ode" },
            new ScheduleTask() { TaskId = "SCHEDULE_TASK_RUN_PAYMENT_QUEUE", TaskType = "FuseCP.Ecommerce.EnterpriseServer.RunPaymentQueueTask, FuseCP.EnterpriseServer" +
                ".Code" },
            new ScheduleTask() { TaskId = "SCHEDULE_TASK_RUN_SYSTEM_COMMAND", RoleId = 1, TaskType = "FuseCP.EnterpriseServer.RunSystemCommandTask, FuseCP.EnterpriseServer.Code" },
            new ScheduleTask() { TaskId = "SCHEDULE_TASK_SEND_MAIL", RoleId = 3, TaskType = "FuseCP.EnterpriseServer.SendMailNotificationTask, FuseCP.EnterpriseServer.Code" },
            new ScheduleTask() { TaskId = "SCHEDULE_TASK_SUSPEND_OVERDUE_INVOICES", TaskType = "FuseCP.Ecommerce.EnterpriseServer.SuspendOverdueInvoicesTask, FuseCP.Enterpris" +
                "eServer.Code" },
            new ScheduleTask() { TaskId = "SCHEDULE_TASK_SUSPEND_PACKAGES", RoleId = 2, TaskType = "FuseCP.EnterpriseServer.SuspendOverusedPackagesTask, FuseCP.EnterpriseServer.C" +
                "ode" },
            new ScheduleTask() { TaskId = "SCHEDULE_TASK_USER_PASSWORD_EXPIRATION_NOTIFICATION", RoleId = 1, TaskType = "FuseCP.EnterpriseServer.UserPasswordExpirationNotificationTask, FuseCP.Enterpr" +
                "iseServer.Code" },
            new ScheduleTask() { TaskId = "SCHEDULE_TASK_ZIP_FILES", RoleId = 3, TaskType = "FuseCP.EnterpriseServer.ZipFilesTask, FuseCP.EnterpriseServer.Code" },
            new ScheduleTask() { TaskId = "SCHEDULE_TASK_CHECK_WEBSITES_SSL", RoleId = 3, TaskType = "FuseCP.EnterpriseServer.CheckWebsitesSslTask, FuseCP.EnterpriseServer.Code" }
        });
        #endregion
    }
}
