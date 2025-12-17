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
using System;
using System.Collections.Generic;
using FuseCP.EnterpriseServer.Data.Configuration;
using FuseCP.EnterpriseServer.Data.Entities;
using FuseCP.EnterpriseServer.Data.Extensions;
#if NetCore
using Microsoft.EntityFrameworkCore;
#endif
#if NetFX
using System.Data.Entity;
#endif

namespace FuseCP.EnterpriseServer.Data.Configuration;

public partial class PackageConfiguration: EntityTypeConfiguration<Package>
{
    public override void Configure() {
        ToTable(tb => tb.HasTrigger("Update_StatusIDchangeDate"));

        Property(e => e.StatusIdchangeDate).HasDefaultValueSql("(getdate())");

        HasOne(d => d.ParentPackage).WithMany(p => p.InverseParentPackage).HasConstraintName("FK_Packages_Packages");

        HasOne(d => d.Plan).WithMany(p => p.Packages).HasConstraintName("FK_Packages_HostingPlans");

        HasOne(d => d.Server).WithMany(p => p.Packages).HasConstraintName("FK_Packages_Servers");

        HasOne(d => d.User).WithMany(p => p.Packages)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Packages_Users");

        HasMany(d => d.Services).WithMany(p => p.Packages)
            .UsingEntity<Dictionary<string, object>>(
                "PackageService",
                r => r.HasOne<Service>().WithMany()
                        .HasForeignKey("ServiceId")
                        .HasConstraintName("FK_PackageServices_Services"),
                l => l.HasOne<Package>().WithMany()
                        .HasForeignKey("PackageId")
                        .HasConstraintName("FK_PackageServices_Packages"),
                j => {
                    j.HasKey("PackageId", "ServiceId");
                    j.ToTable("PackageServices");
                    j.IndexerProperty<int>("PackageId").HasColumnName("PackageID");
                    j.IndexerProperty<int>("ServiceId").HasColumnName("ServiceID");
                });

        #region Seed Data
        HasData(() => new Package[] {
            new Package() { PackageId = 1, PackageComments = "", PackageName = "System", StatusId = 1, StatusIdChangeDate = DateTime.Parse("2024-10-12T19:29:19.9270000Z").ToUniversalTime(), UserId = 1 }
        });
        #endregion

    }
}
