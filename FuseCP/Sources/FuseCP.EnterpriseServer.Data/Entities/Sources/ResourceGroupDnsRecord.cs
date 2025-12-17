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
[Index("GroupId", Name = "ResourceGroupDnsRecordsIdx_GroupID")]
#endif
public partial class ResourceGroupDnsRecord
{
    [Key]
    [Column("RecordID")]
    public int RecordId { get; set; }

    public int RecordOrder { get; set; }

    [Column("GroupID")]
    public int GroupId { get; set; }

    [Required]
    [StringLength(50)]
#if NetCore
    [Unicode(false)]
#endif
    public string RecordType { get; set; }

    [Required]
    [StringLength(50)]
    public string RecordName { get; set; }

    [Required]
    [StringLength(200)]
    public string RecordData { get; set; }

    [Column("MXPriority")]
    public int? Mxpriority { get; set; }

    [ForeignKey("GroupId")]
    [InverseProperty("ResourceGroupDnsRecords")]
    public virtual ResourceGroup Group { get; set; }
}
#endif
