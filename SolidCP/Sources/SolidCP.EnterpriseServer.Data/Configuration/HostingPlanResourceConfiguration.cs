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

public partial class HostingPlanResourceConfiguration: EntityTypeConfiguration<HostingPlanResource>
{
    public override void Configure() {

#if NetCore
        HasOne(d => d.Group).WithMany(p => p.HostingPlanResources)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_HostingPlanResources_ResourceGroups");

        HasOne(d => d.Plan).WithMany(p => p.HostingPlanResources).HasConstraintName("FK_HostingPlanResources_HostingPlans");
#else
        HasRequired(d => d.Group).WithMany(p => p.HostingPlanResources);
        HasRequired(d => d.Plan).WithMany(p => p.HostingPlanResources);
#endif
    }
}
