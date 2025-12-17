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
[Index("LyncUserPlanId", Name = "IX_LyncUserPlans", IsUnique = true)]
[Index("ItemId", Name = "LyncUserPlansIdx_ItemID")]
#endif
public partial class LyncUserPlan
{
    [Key]
    public int LyncUserPlanId { get; set; }

    [Column("ItemID")]
    public int ItemId { get; set; }

    [Required]
    [StringLength(300)]
    public string LyncUserPlanName { get; set; }

    public int? LyncUserPlanType { get; set; }

    [Column("IM")]
    public bool Im { get; set; }

    public bool Mobility { get; set; }

    public bool MobilityEnableOutsideVoice { get; set; }

    public bool Federation { get; set; }

    public bool Conferencing { get; set; }

    public bool EnterpriseVoice { get; set; }

    public int VoicePolicy { get; set; }

    public bool IsDefault { get; set; }

    public bool RemoteUserAccess { get; set; }

    [Column("PublicIMConnectivity")]
    public bool PublicImconnectivity { get; set; }

    public bool AllowOrganizeMeetingsWithExternalAnonymous { get; set; }

    public int? Telephony { get; set; }

    [Column("ServerURI")]
    [StringLength(300)]
    public string ServerUri { get; set; }

    [StringLength(300)]
    public string ArchivePolicy { get; set; }

    [StringLength(300)]
    public string TelephonyDialPlanPolicy { get; set; }

    [StringLength(300)]
    public string TelephonyVoicePolicy { get; set; }

    [ForeignKey("ItemId")]
    [InverseProperty("LyncUserPlans")]
    public virtual ExchangeOrganization Item { get; set; }

    [InverseProperty("LyncUserPlan")]
    public virtual ICollection<LyncUser> LyncUsers { get; set; } = new List<LyncUser>();
}
#endif
