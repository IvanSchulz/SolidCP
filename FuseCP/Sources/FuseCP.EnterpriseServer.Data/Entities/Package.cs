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

#if ScaffoldedEntities
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
#if NetCore
using Microsoft.EntityFrameworkCore;
#endif

namespace FuseCP.EnterpriseServer.Data.Entities;

#if NetCore
[Index("ParentPackageId", Name = "PackageIndex_ParentPackageID")]
[Index("PlanId", Name = "PackageIndex_PlanID")]
[Index("ServerId", Name = "PackageIndex_ServerID")]
[Index("UserId", Name = "PackageIndex_UserID")]
#endif
public partial class Package
{
    [Key]
    [Column("PackageID")]
    public int PackageId { get; set; }

    [Column("ParentPackageID")]
    public int? ParentPackageId { get; set; }

    [Column("UserID")]
    public int UserId { get; set; }

    [StringLength(300)]
    public string PackageName { get; set; }

    //[Column(TypeName = "ntext")]
    public string PackageComments { get; set; }

    [Column("ServerID")]
    public int? ServerId { get; set; }

    [Column("StatusID")]
    public int StatusId { get; set; }

    [Column("PlanID")]
    public int? PlanId { get; set; }

    //[Column(TypeName = "datetime")]
    public DateTime? PurchaseDate { get; set; }

    public bool OverrideQuotas { get; set; }

    //[Column(TypeName = "datetime")]
    public DateTime? BandwidthUpdated { get; set; }

    public bool DefaultTopPackage { get; set; }

	//[Column("StatusIDchangeDate", TypeName = "datetime")]
	[Column("StatusIDchangeDate")]
	public DateTime StatusIdChangeDate { get; set; }

    [InverseProperty("Package")]
    public virtual ICollection<Domain> Domains { get; set; } = new List<Domain>();

    [InverseProperty("Package")]
    public virtual ICollection<GlobalDnsRecord> GlobalDnsRecords { get; set; } = new List<GlobalDnsRecord>();

    /*[InverseProperty("Package")]
    public virtual ICollection<HostingPlan> HostingPlans { get; set; } = new List<HostingPlan>();*/

    [InverseProperty("ParentPackage")]
    public virtual ICollection<Package> ChildPackages { get; set; } = new List<Package>();

    [InverseProperty("Package")]
    public virtual ICollection<PackageAddon> PackageAddons { get; set; } = new List<PackageAddon>();

    [InverseProperty("Package")]
    public virtual ICollection<PackageIpAddress> PackageIpAddresses { get; set; } = new List<PackageIpAddress>();

    [InverseProperty("Package")]
    public virtual ICollection<PackageQuota> PackageQuota { get; set; } = new List<PackageQuota>();

    [InverseProperty("Package")]
    public virtual ICollection<PackageResource> PackageResources { get; set; } = new List<PackageResource>();

    [InverseProperty("Package")]
    public virtual ICollection<PackageVlan> PackageVlans { get; set; } = new List<PackageVlan>();

    [InverseProperty("Package")]
    public virtual ICollection<PackagesBandwidth> PackagesBandwidths { get; set; } = new List<PackagesBandwidth>();

    [InverseProperty("Package")]
    public virtual ICollection<PackagesDiskspace> PackagesDiskspaces { get; set; } = new List<PackagesDiskspace>();

    [ForeignKey("ParentPackageId")]
    [InverseProperty("ChildPackages")]
    public virtual Package ParentPackage { get; set; }

    [ForeignKey("PlanId")]
    [InverseProperty("Packages")]
    public virtual HostingPlan HostingPlan { get; set; }

    [InverseProperty("Package")]
    public virtual ICollection<Schedule> Schedules { get; set; } = new List<Schedule>();

    [ForeignKey("ServerId")]
    [InverseProperty("Packages")]
    public virtual Server Server { get; set; }

    [InverseProperty("Package")]
    public virtual ICollection<ServiceItem> ServiceItems { get; set; } = new List<ServiceItem>();

    [ForeignKey("UserId")]
    [InverseProperty("Packages")]
    public virtual User User { get; set; }

    /* [ForeignKey("PackageId")]
    [InverseProperty("Packages")]
    public virtual ICollection<Service> Services { get; set; } = new List<Service>(); */
}
#endif
