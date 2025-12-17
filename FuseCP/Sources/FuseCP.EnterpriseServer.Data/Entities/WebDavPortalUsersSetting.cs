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
[Index("AccountId", Name = "WebDavPortalUsersSettingsIdx_AccountId")]
#endif
public partial class WebDavPortalUsersSetting
{
    [Key]
    [Column("ID")]
    public int Id { get; set; }

    public int AccountId { get; set; }

    public string Settings { get; set; }

    [ForeignKey("AccountId")]
    [InverseProperty("WebDavPortalUsersSettings")]
    public virtual ExchangeAccount Account { get; set; }
}
#endif
