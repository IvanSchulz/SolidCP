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

public partial class SslCertificateConfiguration: EntityTypeConfiguration<SslCertificate>
{
    public override void Configure() {

        Property(e => e.Id).ValueGeneratedOnAdd();

		if (IsSqlServer)
		{
			Property(e => e.Csr).HasColumnType("ntext");
			Property(e => e.Certificate).HasColumnType("ntext");
			Property(e => e.Hash).HasColumnType("ntext");
			Property(e => e.Pfx).HasColumnType("ntext");
			Property(e => e.ValidFrom).HasColumnType("datetime");
			Property(e => e.ExpiryDate).HasColumnType("datetime");
		}
		else if (IsCore && (IsMySql || IsMariaDb || IsSqlite || IsPostgreSql))
		{
			Property(e => e.Csr).HasColumnType("TEXT");
			Property(e => e.Certificate).HasColumnType("TEXT");
			Property(e => e.Hash).HasColumnType("TEXT");
			Property(e => e.Pfx).HasColumnType("TEXT");
		}

    }
}
