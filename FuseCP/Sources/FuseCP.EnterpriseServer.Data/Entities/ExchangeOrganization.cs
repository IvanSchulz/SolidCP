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

#if NetCore
[Index("OrganizationId", Name = "IX_ExchangeOrganizations_UniqueOrg", IsUnique = true)]
#endif
public partial class ExchangeOrganization
{
    [Key]
    [Column("ItemID")]
    public int ItemId { get; set; }

    [Required]
    [Column("OrganizationID")]
    [StringLength(128)]
    public string OrganizationId { get; set; }

    [Column("ExchangeMailboxPlanID")]
    public int? ExchangeMailboxPlanId { get; set; }

    [Column("LyncUserPlanID")]
    public int? LyncUserPlanId { get; set; }

    [Column("SfBUserPlanID")]
    public int? SfBuserPlanId { get; set; }

    [InverseProperty("Item")]
    public virtual ICollection<ExchangeMailboxPlan> ExchangeMailboxPlans { get; set; } = new List<ExchangeMailboxPlan>();

    [InverseProperty("Item")]
    public virtual ICollection<ExchangeOrganizationSsFolder> ExchangeOrganizationSsFolders { get; set; } = new List<ExchangeOrganizationSsFolder>();

    [ForeignKey("ItemId")]
#if NetCore
    [InverseProperty("ExchangeOrganization")]
#endif
    public virtual ServiceItem Item { get; set; }

    [InverseProperty("Item")]
    public virtual ICollection<LyncUserPlan> LyncUserPlans { get; set; } = new List<LyncUserPlan>();
}
#endif
