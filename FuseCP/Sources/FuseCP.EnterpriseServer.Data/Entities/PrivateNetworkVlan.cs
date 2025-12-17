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

[Table("PrivateNetworkVLANs")]
#if NetCore
[Index("ServerId", Name = "PrivateNetworkVLANsIdx_ServerID")]
#endif
public partial class PrivateNetworkVlan
{
    [Key]
    [Column("VlanID")]
    public int VlanId { get; set; }

    public int Vlan { get; set; }

    [Column("ServerID")]
    public int? ServerId { get; set; }

    //[Column(TypeName = "ntext")]
    public string Comments { get; set; }

    [InverseProperty("Vlan")]
    public virtual ICollection<PackageVlan> PackageVlans { get; set; } = new List<PackageVlan>();

    [ForeignKey("ServerId")]
    [InverseProperty("PrivateNetworkVlans")]
    public virtual Server Server { get; set; }
}
#endif
