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

public partial class ScheduleConfiguration: EntityTypeConfiguration<Schedule>
{
    public override void Configure() {
        HasOne(d => d.Package).WithMany(p => p.Schedules)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_Schedule_Packages");

        HasOne(d => d.Task).WithMany(p => p.Schedules)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Schedule_ScheduleTasks");

        #region Seed Data
        HasData(() => new Schedule[] {
            new Schedule() { ScheduleId = 1, Enabled = true, FromTime = DateTime.Parse("2000-01-01T11:00:00.0000000Z").ToUniversalTime(), HistoriesNumber = 7, Interval = 0, MaxExecutionTime = 3600,
                NextRun = DateTime.Parse("2010-07-16T12:53:02.4700000Z").ToUniversalTime(), PackageId = 1, PriorityId = "Normal", ScheduleName = "Calculate Disk Space", ScheduleTypeId = "Daily", StartTime = DateTime.Parse("2000-01-01T11:30:00.0000000Z").ToUniversalTime(),
                TaskId = "SCHEDULE_TASK_CALCULATE_PACKAGES_DISKSPACE", ToTime = DateTime.Parse("2000-01-01T11:00:00.0000000Z").ToUniversalTime(), WeekMonthDay = 1 },
            new Schedule() { ScheduleId = 2, Enabled = true, FromTime = DateTime.Parse("2000-01-01T11:00:00.0000000Z").ToUniversalTime(), HistoriesNumber = 7, Interval = 0, MaxExecutionTime = 3600,
                NextRun = DateTime.Parse("2010-07-16T12:53:02.4770000Z").ToUniversalTime(), PackageId = 1, PriorityId = "Normal", ScheduleName = "Calculate Bandwidth", ScheduleTypeId = "Daily", StartTime = DateTime.Parse("2000-01-01T11:00:00.0000000Z").ToUniversalTime(),
                TaskId = "SCHEDULE_TASK_CALCULATE_PACKAGES_BANDWIDTH", ToTime = DateTime.Parse("2000-01-01T11:00:00.0000000Z").ToUniversalTime(), WeekMonthDay = 1 }
        });
        #endregion

    }
}
