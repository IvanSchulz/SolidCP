// This file is auto generated, do not edit.
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

public partial class HostingPlanQuotaConfiguration: EntityTypeConfiguration<HostingPlanQuota>
{
    public override void Configure() {
        HasKey(e => new { e.PlanId, e.QuotaId }).HasName("PK_HostingPlanQuota");

#if NetCore
        HasOne(d => d.Plan).WithMany(p => p.HostingPlanQuota).HasConstraintName("FK_HostingPlanQuotas_HostingPlans");

        HasOne(d => d.Quota).WithMany(p => p.HostingPlanQuota)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_HostingPlanQuotas_Quotas");
#else
        HasRequired(d => d.Plan).WithMany(p => p.HostingPlanQuota);
        HasRequired(d => d.Quota).WithMany(p => p.HostingPlanQuota);
#endif
    }
}
