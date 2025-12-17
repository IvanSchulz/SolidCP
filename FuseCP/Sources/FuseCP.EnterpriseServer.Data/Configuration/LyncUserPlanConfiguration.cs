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

public partial class LyncUserPlanConfiguration: EntityTypeConfiguration<LyncUserPlan>
{
    public override void Configure() {

        if (IsCore && IsSqlite)
        {
			Property(e => e.ArchivePolicy).HasColumnType("TEXT COLLATE NOCASE");
			Property(e => e.LyncUserPlanName).HasColumnType("TEXT COLLATE NOCASE");
			Property(e => e.TelephonyDialPlanPolicy).HasColumnType("TEXT COLLATE NOCASE");
			Property(e => e.TelephonyVoicePolicy).HasColumnType("TEXT COLLATE NOCASE");
			Property(e => e.VoicePolicy).HasColumnType("TEXT COLLATE NOCASE");
		}

#if NetCore
		Property(e => e.RemoteUserAccess).HasDefaultValue(false);
		Property(e => e.PublicIMConnectivity).HasDefaultValue(false);
		Property(e => e.AllowOrganizeMeetingsWithExternalAnonymous).HasDefaultValue(false);

		HasOne(d => d.Item).WithMany(p => p.LyncUserPlans).HasConstraintName("FK_LyncUserPlans_ExchangeOrganizations");
#else
        HasRequired(d => d.Item).WithMany(p => p.LyncUserPlans);
#endif
    }
}
