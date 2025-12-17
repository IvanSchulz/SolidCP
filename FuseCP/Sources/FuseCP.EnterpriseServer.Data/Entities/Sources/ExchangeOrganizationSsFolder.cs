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
[Index("ItemId", Name = "ExchangeOrganizationSsFoldersIdx_ItemId")]
[Index("StorageSpaceFolderId", Name = "ExchangeOrganizationSsFoldersIdx_StorageSpaceFolderId")]
#endif
public partial class ExchangeOrganizationSsFolder
{
    [Key]
    public int Id { get; set; }

    public int ItemId { get; set; }

    [Required]
    [StringLength(100)]
#if NetCore
    [Unicode(false)]
#endif
    public string Type { get; set; }

    public int StorageSpaceFolderId { get; set; }

    [ForeignKey("ItemId")]
    [InverseProperty("ExchangeOrganizationSsFolders")]
    public virtual ExchangeOrganization Item { get; set; }

    [ForeignKey("StorageSpaceFolderId")]
    [InverseProperty("ExchangeOrganizationSsFolders")]
    public virtual StorageSpaceFolder StorageSpaceFolder { get; set; }
}
#endif
