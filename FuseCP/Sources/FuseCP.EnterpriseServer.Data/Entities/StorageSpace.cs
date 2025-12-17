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
using FuseCP.Providers.OS;
#if NetCore
using Microsoft.EntityFrameworkCore;
#endif

namespace FuseCP.EnterpriseServer.Data.Entities;

#if NetCore
[Index("ServerId", Name = "StorageSpacesIdx_ServerId")]
[Index("ServiceId", Name = "StorageSpacesIdx_ServiceId")]
#endif
public partial class StorageSpace
{
    [Key]
    public int Id { get; set; }

    [Required]
    [StringLength(300)]
#if NetCore
    [Unicode(false)]
#endif
    public string Name { get; set; }

    public int ServiceId { get; set; }

    public int ServerId { get; set; }

    public int LevelId { get; set; }

    [Required]
#if NetCore
    [Unicode(false)]
#endif
    public string Path { get; set; }

    public bool IsShared { get; set; }

#if NetCore
    [Unicode(false)]
#endif
    public string UncPath { get; set; }

    public QuotaType FsrmQuotaType { get; set; }

    public long FsrmQuotaSizeBytes { get; set; }

    public bool IsDisabled { get; set; }

    [ForeignKey("ServerId")]
    [InverseProperty("StorageSpaces")]
    public virtual Server Server { get; set; }

    [ForeignKey("ServiceId")]
    [InverseProperty("StorageSpaces")]
    public virtual Service Service { get; set; }

    [InverseProperty("StorageSpace")]
    public virtual ICollection<StorageSpaceFolder> StorageSpaceFolders { get; set; } = new List<StorageSpaceFolder>();
}
#endif
