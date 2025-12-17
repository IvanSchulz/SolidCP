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
[Index("StorageSpaceFolderId", Name = "EnterpriseFoldersIdx_StorageSpaceFolderId")]
#endif
public partial class EnterpriseFolder
{
    [Key]
    [Column("EnterpriseFolderID")]
    public int EnterpriseFolderId { get; set; }

    [Column("ItemID")]
    public int ItemId { get; set; }

    [Required]
    [StringLength(255)]
    public string FolderName { get; set; }

    public int FolderQuota { get; set; }

    [StringLength(255)]
    public string LocationDrive { get; set; }

    [StringLength(255)]
    public string HomeFolder { get; set; }

    [StringLength(255)]
    public string Domain { get; set; }

    public int? StorageSpaceFolderId { get; set; }

    [InverseProperty("Folder")]
    public virtual ICollection<EnterpriseFoldersOwaPermission> EnterpriseFoldersOwaPermissions { get; set; } = new List<EnterpriseFoldersOwaPermission>();

    [ForeignKey("StorageSpaceFolderId")]
    [InverseProperty("EnterpriseFolders")]
    public virtual StorageSpaceFolder StorageSpaceFolder { get; set; }
}
#endif
