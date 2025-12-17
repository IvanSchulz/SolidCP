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
[Index("GroupId", Name = "QuotasIdx_GroupID")]
[Index("ItemTypeId", Name = "QuotasIdx_ItemTypeID")]
#endif
public partial class Quota
{
    [Key]
    [Column("QuotaID")]
    public int QuotaId { get; set; }

    [Column("GroupID")]
    public int GroupId { get; set; }

    public int QuotaOrder { get; set; }

    [Required]
    [StringLength(50)]
    public string QuotaName { get; set; }

    [StringLength(200)]
    public string QuotaDescription { get; set; }

    [Column("QuotaTypeID")]
    public int QuotaTypeId { get; set; }

    public bool? ServiceQuota { get; set; }

    [Column("ItemTypeID")]
    public int? ItemTypeId { get; set; }

    public bool? HideQuota { get; set; }

    public int? PerOrganization { get; set; }

    [ForeignKey("GroupId")]
    [InverseProperty("Quota")]
    public virtual ResourceGroup Group { get; set; }

    [InverseProperty("Quota")]
    public virtual ICollection<HostingPlanQuota> HostingPlanQuota { get; set; } = new List<HostingPlanQuota>();

    [ForeignKey("ItemTypeId")]
    [InverseProperty("Quota")]
    public virtual ServiceItemType ItemType { get; set; }

    [InverseProperty("Quota")]
    public virtual ICollection<PackageQuota> PackageQuota { get; set; } = new List<PackageQuota>();
}
#endif
