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

using GlobalDnsRecord = FuseCP.EnterpriseServer.Data.Entities.GlobalDnsRecord;

public partial class GlobalDnsRecordConfiguration : EntityTypeConfiguration<GlobalDnsRecord>
{
	public override void Configure()
	{
        HasKey(e => e.RecordId).HasName("PK_GlobalDnsRecord");

		Property(e => e.RecordType).IsUnicode(false);

		if (IsCore && IsSqlite) Property(e => e.RecordType).HasColumnType("TEXT COLLATE NOCASE");

#if NetCore
        HasOne(d => d.IpAddress).WithMany(p => p.GlobalDnsRecords).HasConstraintName("FK_GlobalDnsRecords_IPAddresses");

        HasOne(d => d.Package).WithMany(p => p.GlobalDnsRecords)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_GlobalDnsRecords_Packages");

        HasOne(d => d.Server).WithMany(p => p.GlobalDnsRecords).HasConstraintName("FK_GlobalDnsRecords_Servers");

        HasOne(d => d.Service).WithMany(p => p.GlobalDnsRecords)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_GlobalDnsRecords_Services");
#else
		HasRequired(d => d.IpAddress).WithMany(p => p.GlobalDnsRecords);
		HasRequired(d => d.Package).WithMany(p => p.GlobalDnsRecords);
		HasRequired(d => d.Server).WithMany(p => p.GlobalDnsRecords);
		HasRequired(d => d.Service).WithMany(p => p.GlobalDnsRecords);
#endif

    }
}
