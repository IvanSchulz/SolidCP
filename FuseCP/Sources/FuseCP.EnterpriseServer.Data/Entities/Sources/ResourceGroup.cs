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
#if ScaffoldedEntities
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
#if NetCore
using Microsoft.EntityFrameworkCore;
#endif

namespace FuseCP.EnterpriseServer.Data.Entities.Sources;

public partial class ResourceGroup
{
    [Key]
    [Column("GroupID")]
    public int GroupId { get; set; }

    [Required]
    [StringLength(100)]
    public string GroupName { get; set; }

    public int GroupOrder { get; set; }

    [StringLength(1000)]
    public string GroupController { get; set; }

    public bool? ShowGroup { get; set; }

    [InverseProperty("Group")]
    public virtual ICollection<HostingPlanResource> HostingPlanResources { get; set; } = new List<HostingPlanResource>();

    [InverseProperty("Group")]
    public virtual ICollection<PackageResource> PackageResources { get; set; } = new List<PackageResource>();

    [InverseProperty("Group")]
    public virtual ICollection<PackagesBandwidth> PackagesBandwidths { get; set; } = new List<PackagesBandwidth>();

    [InverseProperty("Group")]
    public virtual ICollection<PackagesDiskspace> PackagesDiskspaces { get; set; } = new List<PackagesDiskspace>();

    [InverseProperty("Group")]
    public virtual ICollection<Provider> Providers { get; set; } = new List<Provider>();

    [InverseProperty("Group")]
    public virtual ICollection<Quota> Quota { get; set; } = new List<Quota>();

    [InverseProperty("Group")]
    public virtual ICollection<ResourceGroupDnsRecord> ResourceGroupDnsRecords { get; set; } = new List<ResourceGroupDnsRecord>();

    [InverseProperty("PrimaryGroup")]
    public virtual ICollection<Server> Servers { get; set; } = new List<Server>();

    [InverseProperty("Group")]
    public virtual ICollection<ServiceItemType> ServiceItemTypes { get; set; } = new List<ServiceItemType>();

    [InverseProperty("Group")]
    public virtual ICollection<StorageSpaceLevelResourceGroup> StorageSpaceLevelResourceGroups { get; set; } = new List<StorageSpaceLevelResourceGroup>();

    [InverseProperty("Group")]
    public virtual ICollection<VirtualGroup> VirtualGroups { get; set; } = new List<VirtualGroup>();
}
#endif
