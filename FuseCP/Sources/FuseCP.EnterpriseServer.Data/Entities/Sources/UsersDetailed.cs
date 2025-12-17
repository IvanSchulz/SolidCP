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
[Keyless]
#endif
public partial class UsersDetailed
{
    [Column("UserID")]
    public int UserId { get; set; }

    [Column("RoleID")]
    public int RoleId { get; set; }

    [Column("StatusID")]
    public int StatusId { get; set; }

    public int? LoginStatusId { get; set; }

    [StringLength(32)]
    public string SubscriberNumber { get; set; }

    public int? FailedLogins { get; set; }

    [Column("OwnerID")]
    public int? OwnerId { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? Created { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? Changed { get; set; }

    public bool IsDemo { get; set; }

    [Column(TypeName = "ntext")]
    public string Comments { get; set; }

    public bool IsPeer { get; set; }

    [StringLength(50)]
    public string Username { get; set; }

    [StringLength(50)]
    public string FirstName { get; set; }

    [StringLength(50)]
    public string LastName { get; set; }

    [StringLength(255)]
    public string Email { get; set; }

    [StringLength(100)]
    public string CompanyName { get; set; }

    [StringLength(101)]
    public string FullName { get; set; }

    [StringLength(50)]
    public string OwnerUsername { get; set; }

    [StringLength(50)]
    public string OwnerFirstName { get; set; }

    [StringLength(50)]
    public string OwnerLastName { get; set; }

    [Column("OwnerRoleID")]
    public int? OwnerRoleId { get; set; }

    [StringLength(101)]
    public string OwnerFullName { get; set; }

    [StringLength(255)]
    public string OwnerEmail { get; set; }

    public int? Expr1 { get; set; }

    public int? PackagesNumber { get; set; }

    public bool? EcommerceEnabled { get; set; }
}
#endif
