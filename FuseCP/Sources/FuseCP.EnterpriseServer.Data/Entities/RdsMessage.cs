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

[Table("RDSMessages")]
#if NetCore
[Index("RdsCollectionId", Name = "RDSMessagesIdx_RDSCollectionId")]
#endif
public partial class RdsMessage
{
    [Key]
    public int Id { get; set; }

    [Column("RDSCollectionId")]
    public int RdsCollectionId { get; set; }

    [Required(AllowEmptyStrings = true)]
    //[Column(TypeName = "ntext")]
    public string MessageText { get; set; }

    [Required]
    [StringLength(250)]
    public string UserName { get; set; }

    //[Column(TypeName = "datetime")]
    public DateTime Date { get; set; }

    [ForeignKey("RdsCollectionId")]
    [InverseProperty("RdsMessages")]
    public virtual RdsCollection RdsCollection { get; set; }
}
#endif
