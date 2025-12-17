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

[Table("PackagesBandwidth")]
#if NetCore
[PrimaryKey("PackageId", "GroupId", "LogDate")]
#endif
public partial class PackagesBandwidth
{
    [Key]
    [Column("PackageID")]
    public int PackageId { get; set; }

    [Key]
    [Column("GroupID")]
    public int GroupId { get; set; }

    [Key]
    [Column(TypeName = "datetime")]
    public DateTime LogDate { get; set; }

    public long BytesSent { get; set; }

    public long BytesReceived { get; set; }

    [ForeignKey("GroupId")]
    [InverseProperty("PackagesBandwidths")]
    public virtual ResourceGroup Group { get; set; }

    [ForeignKey("PackageId")]
    [InverseProperty("PackagesBandwidths")]
    public virtual Package Package { get; set; }
}
#endif
