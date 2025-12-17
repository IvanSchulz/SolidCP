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

using System;
using System.Collections.Generic;
using FuseCP.EnterpriseServer.Data.Configuration;
using FuseCP.EnterpriseServer.Data.Entities;
using System.ComponentModel.DataAnnotations.Schema;
#if NetCore
using Microsoft.EntityFrameworkCore;
#endif
#if NetFX
using System.Data.Entity;
#endif

namespace FuseCP.EnterpriseServer.Data.Configuration;

public partial class AccessTokenConfiguration: EntityTypeConfiguration<AccessToken>
{
	public override void Configure() {
		HasKey(e => e.Id).HasName("PK_AccessToken");

		Property(e => e.SmsResponse).IsUnicode(false);
        if (IsSqlServer) Property(e => e.ExpirationDate).HasColumnType("datetime");
		else if (IsCore && IsSqliteFX) Property(e => e.AccessTokenGuid).HasColumnType("BLOB");

#if NetCore
		HasOne(d => d.Account).WithMany(p => p.AccessTokens).HasConstraintName("FK_AccessTokens_UserId");
#else
        HasRequired(d => d.Account).WithMany(p => p.AccessTokens);
#endif
    }
}
