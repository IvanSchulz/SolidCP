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

#if ScaffoldedEntities
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
#if NetCore
using Microsoft.EntityFrameworkCore;
#endif

namespace FuseCP.EnterpriseServer.Data.Entities;

public partial class BackgroundTask
{
    [Key]
    [Column("ID")]
    public int Id { get; set; }

    public Guid Guid { get; set; }

    [Column("TaskID")]
    [StringLength(255)]
    public string TaskId { get; set; }

    [Column("ScheduleID")]
    public int ScheduleId { get; set; }

    [Column("PackageID")]
    public int PackageId { get; set; }

    [Column("UserID")]
    public int UserId { get; set; }

    [Column("EffectiveUserID")]
    public int EffectiveUserId { get; set; }

    [StringLength(255)]
    public string TaskName { get; set; }

    [Column("ItemID")]
    public int? ItemId { get; set; }

    [StringLength(255)]
    public string ItemName { get; set; }

    //[Column(TypeName = "datetime")]
    public DateTime StartDate { get; set; }

    //[Column(TypeName = "datetime")]
    public DateTime? FinishDate { get; set; }

    public int IndicatorCurrent { get; set; }

    public int IndicatorMaximum { get; set; }

    public int MaximumExecutionTime { get; set; }

    public string Source { get; set; }

    public int Severity { get; set; }

    public bool? Completed { get; set; }

    public bool? NotifyOnComplete { get; set; }

    public BackgroundTaskStatus Status { get; set; }

    [InverseProperty("Task")]
    public virtual ICollection<BackgroundTaskLog> BackgroundTaskLogs { get; set; } = new List<BackgroundTaskLog>();

    [InverseProperty("Task")]
    public virtual ICollection<BackgroundTaskParameter> BackgroundTaskParameters { get; set; } = new List<BackgroundTaskParameter>();

    [InverseProperty("Task")]
    public virtual ICollection<BackgroundTaskStack> BackgroundTaskStacks { get; set; } = new List<BackgroundTaskStack>();
}
#endif
