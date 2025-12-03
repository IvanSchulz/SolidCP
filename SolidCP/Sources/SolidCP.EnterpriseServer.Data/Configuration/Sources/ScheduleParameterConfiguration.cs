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

public partial class ScheduleParameterConfiguration: EntityTypeConfiguration<ScheduleParameter>
{
    public override void Configure() {
        HasOne(d => d.Schedule).WithMany(p => p.ScheduleParameters).HasConstraintName("FK_ScheduleParameters_Schedule");

        #region Seed Data
        HasData(() => new ScheduleParameter[] {
            new ScheduleParameter() { ScheduleId = 1, ParameterId = "SUSPEND_OVERUSED", ParameterValue = "false" },
            new ScheduleParameter() { ScheduleId = 2, ParameterId = "SUSPEND_OVERUSED", ParameterValue = "false" }
        });
        #endregion

    }
}
