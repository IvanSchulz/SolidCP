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

public partial class SfBUserPlanConfiguration: EntityTypeConfiguration<SfBUserPlan>
{
    public override void Configure() {

        if (IsCore && IsSqlite)
        {
            Property(e => e.ArchivePolicy).HasColumnType("TEXT COLLATE NOCASE");
			Property(e => e.TelephonyDialPlanPolicy).HasColumnType("TEXT COLLATE NOCASE");
			Property(e => e.TelephonyVoicePolicy).HasColumnType("TEXT COLLATE NOCASE");
			Property(e => e.VoicePolicy).HasColumnType("TEXT COLLATE NOCASE");
		}

#if NetCore
		Property(e => e.RemoteUserAccess).HasDefaultValue(false);
		Property(e => e.PublicIMConnectivity).HasDefaultValue(false);
		Property(e => e.AllowOrganizeMeetingsWithExternalAnonymous).HasDefaultValue(false);
#endif

	}
}
