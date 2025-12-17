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
#if ScaffoldedEntities
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
#if NetCore
using Microsoft.EntityFrameworkCore;
#endif

namespace FuseCP.EnterpriseServer.Data.Entities.Sources;

[Table("ScheduleTaskViewConfiguration")]
#if NetCore
[PrimaryKey("ConfigurationId", "TaskId")]
#endif
public partial class ScheduleTaskViewConfiguration
{
    [Key]
    [Column("TaskID")]
    [StringLength(100)]
    public string TaskId { get; set; }

    [Key]
    [Column("ConfigurationID")]
    [StringLength(100)]
    public string ConfigurationId { get; set; }

    [Required]
    [StringLength(100)]
    public string Environment { get; set; }

    [Required]
    [StringLength(100)]
    public string Description { get; set; }

    [ForeignKey("TaskId")]
    [InverseProperty("ScheduleTaskViewConfigurations")]
    public virtual ScheduleTask Task { get; set; }
}
#endif
