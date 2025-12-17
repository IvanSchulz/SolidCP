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

[Table("RDSCollections")]
public partial class Rdscollection
{
    [Key]
    [Column("ID")]
    public int Id { get; set; }

    [Column("ItemID")]
    public int ItemId { get; set; }

    [StringLength(255)]
    public string Name { get; set; }

    [StringLength(255)]
    public string Description { get; set; }

    [StringLength(255)]
    public string DisplayName { get; set; }

    [InverseProperty("Rdscollection")]
    public virtual ICollection<RdscollectionSetting> RdscollectionSettings { get; set; } = new List<RdscollectionSetting>();

    [InverseProperty("Rdscollection")]
    public virtual ICollection<RdscollectionUser> RdscollectionUsers { get; set; } = new List<RdscollectionUser>();

    [InverseProperty("Rdscollection")]
    public virtual ICollection<Rdsmessage> Rdsmessages { get; set; } = new List<Rdsmessage>();

    [InverseProperty("Rdscollection")]
    public virtual ICollection<Rdsserver> Rdsservers { get; set; } = new List<Rdsserver>();
}
#endif
