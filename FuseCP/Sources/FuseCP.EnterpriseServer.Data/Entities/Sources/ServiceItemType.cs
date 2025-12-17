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
[Index("GroupId", Name = "ServiceItemTypesIdx_GroupID")]
#endif
public partial class ServiceItemType
{
    [Key]
    [Column("ItemTypeID")]
    public int ItemTypeId { get; set; }

    [Column("GroupID")]
    public int? GroupId { get; set; }

    [StringLength(50)]
    public string DisplayName { get; set; }

    [StringLength(200)]
    public string TypeName { get; set; }

    public int TypeOrder { get; set; }

    public bool? CalculateDiskspace { get; set; }

    public bool? CalculateBandwidth { get; set; }

    public bool? Suspendable { get; set; }

    public bool? Disposable { get; set; }

    public bool? Searchable { get; set; }

    public bool Importable { get; set; }

    public bool Backupable { get; set; }

    [ForeignKey("GroupId")]
    [InverseProperty("ServiceItemTypes")]
    public virtual ResourceGroup Group { get; set; }

    [InverseProperty("ItemType")]
    public virtual ICollection<Quota> Quota { get; set; } = new List<Quota>();

    [InverseProperty("ItemType")]
    public virtual ICollection<ServiceItem> ServiceItems { get; set; } = new List<ServiceItem>();
}
#endif
