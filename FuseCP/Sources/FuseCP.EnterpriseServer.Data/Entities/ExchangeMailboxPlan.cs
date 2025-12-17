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
using FuseCP.Providers.HostedSolution;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
#if NetCore
using Microsoft.EntityFrameworkCore;
#endif

namespace FuseCP.EnterpriseServer.Data.Entities;

#if NetCore
[Index("ItemId", Name = "ExchangeMailboxPlansIdx_ItemID")]
[Index("MailboxPlanId", Name = "IX_ExchangeMailboxPlans", IsUnique = true)]
#endif
public partial class ExchangeMailboxPlan
{
    [Key]
    public int MailboxPlanId { get; set; }

    [Column("ItemID")]
    public int ItemId { get; set; }

    [Required]
    [StringLength(300)]
    public string MailboxPlan { get; set; }

    public int? MailboxPlanType { get; set; }

    public bool EnableActiveSync { get; set; }

    [Column("EnableIMAP")]
    public bool EnableImap { get; set; }

    [Column("EnableMAPI")]
    public bool EnableMapi { get; set; }

    [Column("EnableOWA")]
    public bool EnableOwa { get; set; }

    [Column("EnablePOP")]
    public bool EnablePop { get; set; }

    public bool IsDefault { get; set; }

    public int IssueWarningPct { get; set; }

    public int KeepDeletedItemsDays { get; set; }

    [Column("MailboxSizeMB")]
    public int MailboxSizeMb { get; set; }

    [Column("MaxReceiveMessageSizeKB")]
    public int MaxReceiveMessageSizeKb { get; set; }

    public int MaxRecipients { get; set; }

    [Column("MaxSendMessageSizeKB")]
    public int MaxSendMessageSizeKb { get; set; }

    public int ProhibitSendPct { get; set; }

    public int ProhibitSendReceivePct { get; set; }

    public bool HideFromAddressBook { get; set; }

    public bool? AllowLitigationHold { get; set; }

    public int? RecoverableItemsWarningPct { get; set; }

    public int? RecoverableItemsSpace { get; set; }

    [StringLength(256)]
    public string LitigationHoldUrl { get; set; }

    [StringLength(512)]
    public string LitigationHoldMsg { get; set; }

    public bool? Archiving { get; set; }

    public bool? EnableArchiving { get; set; }

    [Column("ArchiveSizeMB")]
    public int? ArchiveSizeMb { get; set; }

    public int? ArchiveWarningPct { get; set; }

    public bool? EnableAutoReply { get; set; }

    public bool? IsForJournaling { get; set; }

    public bool? EnableForceArchiveDeletion { get; set; }

    [InverseProperty("MailboxPlan")]
    public virtual ICollection<ExchangeAccount> ExchangeAccounts { get; set; } = new List<ExchangeAccount>();

    [ForeignKey("ItemId")]
    [InverseProperty("ExchangeMailboxPlans")]
    public virtual ExchangeOrganization Item { get; set; }
}
#endif
