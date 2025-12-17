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

#if NetCore
[Index("PackageId", Name = "HostingPlansIdx_PackageID")]
[Index("ServerId", Name = "HostingPlansIdx_ServerID")]
[Index("UserId", Name = "HostingPlansIdx_UserID")]
#endif
public partial class HostingPlan
{
    [Key]
    [Column("PlanID")]
    public int PlanId { get; set; }

    [Column("UserID")]
    public int? UserId { get; set; }

    [Column("PackageID")]
    public int? PackageId { get; set; }

    [Column("ServerID")]
    public int? ServerId { get; set; }

    [Required]
    [StringLength(200)]
    public string PlanName { get; set; }

    [Column(TypeName = "ntext")]
    public string PlanDescription { get; set; }

    public bool Available { get; set; }

    [Column(TypeName = "money")]
    public decimal? SetupPrice { get; set; }

    [Column(TypeName = "money")]
    public decimal? RecurringPrice { get; set; }

    public int? RecurrenceUnit { get; set; }

    public int? RecurrenceLength { get; set; }

    public bool? IsAddon { get; set; }

    [InverseProperty("Plan")]
    public virtual ICollection<HostingPlanQuota> HostingPlanQuota { get; set; } = new List<HostingPlanQuota>();

    [InverseProperty("Plan")]
    public virtual ICollection<HostingPlanResource> HostingPlanResources { get; set; } = new List<HostingPlanResource>();

    [ForeignKey("PackageId")]
    [InverseProperty("HostingPlans")]
    public virtual Package Package { get; set; }

    [InverseProperty("Plan")]
    public virtual ICollection<PackageAddon> PackageAddons { get; set; } = new List<PackageAddon>();

    [InverseProperty("Plan")]
    public virtual ICollection<Package> Packages { get; set; } = new List<Package>();

    [ForeignKey("ServerId")]
    [InverseProperty("HostingPlans")]
    public virtual Server Server { get; set; }

    [ForeignKey("UserId")]
    [InverseProperty("HostingPlans")]
    public virtual User User { get; set; }
}
#endif
