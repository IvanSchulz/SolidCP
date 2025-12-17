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
[PrimaryKey("PackageId", "GroupId")]
#endif
public partial class PackageResource
{
    [Key]
    [Column("PackageID", Order = 1)]
    public int PackageId { get; set; }

    [Key]
    [Column("GroupID", Order = 2)]
    public int GroupId { get; set; }

    public bool CalculateDiskspace { get; set; }

    public bool CalculateBandwidth { get; set; }

    [ForeignKey("GroupId")]
    [InverseProperty("PackageResources")]
    public virtual ResourceGroup Group { get; set; }

    [ForeignKey("PackageId")]
    [InverseProperty("PackageResources")]
    public virtual Package Package { get; set; }
}
#endif
