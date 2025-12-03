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

public partial class VirtualServiceConfiguration : EntityTypeConfiguration<VirtualService>
{
	public override void Configure()
	{

#if NetCore
        HasOne(d => d.Server).WithMany(p => p.VirtualServices).HasConstraintName("FK_VirtualServices_Servers");

        HasOne(d => d.Service).WithMany(p => p.VirtualServices)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_VirtualServices_Services");
#else
		HasRequired(d => d.Server).WithMany(p => p.VirtualServices);
		HasRequired(d => d.Service).WithMany(p => p.VirtualServices);
#endif

	}
}
