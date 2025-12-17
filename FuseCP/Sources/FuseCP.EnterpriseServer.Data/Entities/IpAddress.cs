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

[Table("IPAddresses")]
#if NetCore
[Index("ServerId", Name = "IPAddressesIdx_ServerID")]
#endif
public partial class IpAddress
{
    [Key]
    [Column("AddressID")]
    public int AddressId { get; set; }

    [Required]
    [Column("ExternalIP")]
    [StringLength(24)]
#if NetCore
    [Unicode(false)]
#endif
    public string ExternalIp { get; set; }

    [Column("InternalIP")]
    [StringLength(24)]
#if NetCore
    [Unicode(false)]
#endif
    public string InternalIp { get; set; }

    [Column("ServerID")]
    public int? ServerId { get; set; }

    //[Column(TypeName = "ntext")]
    public string Comments { get; set; }

    [StringLength(15)]
#if NetCore
    [Unicode(false)]
#endif
    public string SubnetMask { get; set; }

    [StringLength(15)]
#if NetCore
    [Unicode(false)]
#endif
    public string DefaultGateway { get; set; }

    [Column("PoolID")]
    public int? PoolId { get; set; }

    [Column("VLAN")]
    public int? Vlan { get; set; }

    [InverseProperty("IpAddress")]
    public virtual ICollection<GlobalDnsRecord> GlobalDnsRecords { get; set; } = new List<GlobalDnsRecord>();

    [InverseProperty("Address")]
    public virtual ICollection<PackageIpAddress> PackageIpAddresses { get; set; } = new List<PackageIpAddress>();

    [ForeignKey("ServerId")]
    [InverseProperty("IpAddresses")]
    public virtual Server Server { get; set; }
}
#endif
