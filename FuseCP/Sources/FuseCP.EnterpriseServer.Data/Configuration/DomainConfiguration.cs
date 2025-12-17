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

public partial class DomainConfiguration: EntityTypeConfiguration<Domain>
{
    public DomainConfiguration(): base() { }
    public DomainConfiguration(DbType dbType, bool initSeedData = false) : base(dbType, initSeedData) { }

    public override void Configure() {

        if (IsSqlServer)
        {
            Property(e => e.CreationDate).HasColumnType("datetime");
            Property(e => e.ExpirationDate).HasColumnType("datetime");
            Property(e => e.LastUpdateDate).HasColumnType("datetime");
        }
        else if (IsCore && IsSqlite) Property(e => e.DomainName).HasColumnType("TEXT COLLATE NOCASE");

#if NetCore
		Property(e => e.HostingAllowed).HasDefaultValue(false);
		Property(e => e.IsSubDomain).HasDefaultValue(false);
		Property(e => e.IsPreviewDomain).HasDefaultValue(false);

		HasOne(d => d.MailDomain).WithMany(p => p.DomainMailDomains).HasConstraintName("FK_Domains_ServiceItems_MailDomain");

        HasOne(d => d.Package).WithMany(p => p.Domains).HasConstraintName("FK_Domains_Packages");

        HasOne(d => d.WebSite).WithMany(p => p.DomainWebSites).HasConstraintName("FK_Domains_ServiceItems_WebSite");

        HasOne(d => d.Zone).WithMany(p => p.DomainZones).HasConstraintName("FK_Domains_ServiceItems_ZoneItem");
#else
		HasOptional(d => d.MailDomain).WithMany(p => p.DomainMailDomains);
        HasRequired(d => d.Package).WithMany(p => p.Domains);
        HasOptional(d => d.WebSite).WithMany(p => p.DomainWebSites);
        HasOptional(d => d.Zone).WithMany(p => p.DomainZones);
#endif
    }
}
