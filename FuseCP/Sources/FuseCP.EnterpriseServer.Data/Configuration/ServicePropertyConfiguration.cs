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

using FuseCP.EnterpriseServer.Data.Entities;
#if NetCore
using Microsoft.EntityFrameworkCore;
#endif
#if NetFX
using System.Data.Entity;
#endif

namespace FuseCP.EnterpriseServer.Data.Configuration;

public partial class ServicePropertyConfiguration: EntityTypeConfiguration<ServiceProperty>
{
    public override void Configure() {

        if (IsCore && IsSqlite)
        {
            Property(e => e.PropertyName).HasColumnType("TEXT COLLATE NOCASE");
            Property(e => e.PropertyValue).HasColumnType("TEXT COLLATE NOCASE");
        }

        HasKey(e => new { e.ServiceId, e.PropertyName }).HasName("PK_ServiceProperties");

#if NetCore
        HasOne(d => d.Service).WithMany(p => p.ServiceProperties).HasConstraintName("FK_ServiceProperties_Services");
#else
        HasRequired(d => d.Service).WithMany(p => p.ServiceProperties);
#endif
    }
}
