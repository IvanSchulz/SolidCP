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
[PrimaryKey("ProviderId", "PropertyName")]
#endif
public partial class ServiceDefaultProperty
{
    [Key]
    [Column("ProviderID")]
    public int ProviderId { get; set; }

    [Key]
    [StringLength(50)]
    public string PropertyName { get; set; }

    [StringLength(1000)]
    public string PropertyValue { get; set; }

    [ForeignKey("ProviderId")]
    [InverseProperty("ServiceDefaultProperties")]
    public virtual Provider Provider { get; set; }
}
#endif
