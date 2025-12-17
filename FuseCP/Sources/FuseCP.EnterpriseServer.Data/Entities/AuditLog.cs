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

[Table("AuditLog")]
public partial class AuditLog
{
    [Key]
    [Column("RecordID")]
    [StringLength(32)]
#if NetCore
    [Unicode(false)]
#endif
    public string RecordId { get; set; }

    [Column("UserID")]
    public int? UserId { get; set; }

    [StringLength(50)]
    public string Username { get; set; }

    [Column("ItemID")]
    public int? ItemId { get; set; }

    [Column("SeverityID")]
    public int SeverityId { get; set; }

    //[Column(TypeName = "datetime")]
    public DateTime StartDate { get; set; }
    
    //[Column(TypeName = "datetime")]
    public DateTime FinishDate { get; set; }

    [Required(AllowEmptyStrings = true)]
    [StringLength(50)]
#if NetCore
    [Unicode(false)]
#endif
    public string SourceName { get; set; }

    [Required(AllowEmptyStrings = true)]
    [StringLength(50)]
#if NetCore
    [Unicode(false)]
#endif
    public string TaskName { get; set; }

    [StringLength(100)]
    public string ItemName { get; set; }

    //[Column(TypeName = "ntext")]
    public string ExecutionLog { get; set; }

    [Column("PackageID")]
    public int? PackageId { get; set; }
}
#endif
