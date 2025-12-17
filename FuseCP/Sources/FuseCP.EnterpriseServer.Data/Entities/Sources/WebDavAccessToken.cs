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
[Index("AccountId", Name = "WebDavAccessTokensIdx_AccountID")]
#endif
public partial class WebDavAccessToken
{
    [Key]
    [Column("ID")]
    public int Id { get; set; }

    [Required]
    public string FilePath { get; set; }

    [Required]
    public string AuthData { get; set; }

    public Guid AccessToken { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime ExpirationDate { get; set; }

    [Column("AccountID")]
    public int AccountId { get; set; }

    public int ItemId { get; set; }

    [ForeignKey("AccountId")]
    [InverseProperty("WebDavAccessTokens")]
    public virtual ExchangeAccount Account { get; set; }
}
#endif
