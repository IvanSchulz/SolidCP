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
using System;
using System.Collections.Generic;
using FuseCP.EnterpriseServer.Data.Configuration;
using FuseCP.EnterpriseServer.Data.Entities;
using FuseCP.EnterpriseServer.Data.Extensions;
#if NetCore
using Microsoft.EntityFrameworkCore;
#endif
#if NetFX
using System.Data.Entity;
#endif

namespace FuseCP.EnterpriseServer.Data.Configuration;

public partial class ResourceGroupDnsRecordConfiguration: EntityTypeConfiguration<ResourceGroupDnsRecord>
{
    public override void Configure() {
        Property(e => e.RecordOrder).HasDefaultValue(1);

        HasOne(d => d.Group).WithMany(p => p.ResourceGroupDnsRecords).HasConstraintName("FK_ResourceGroupDnsRecords_ResourceGroups");

        #region Seed Data
        HasData(() => new ResourceGroupDnsRecord[] {
            new ResourceGroupDnsRecord() { RecordId = 1, GroupId = 2, MXPriority = 0, RecordData = "[IP]", RecordName = "", RecordOrder = 1,
                RecordType = "A" },
            new ResourceGroupDnsRecord() { RecordId = 2, GroupId = 2, MXPriority = 0, RecordData = "[IP]", RecordName = "*", RecordOrder = 2,
                RecordType = "A" },
            new ResourceGroupDnsRecord() { RecordId = 3, GroupId = 2, MXPriority = 0, RecordData = "[IP]", RecordName = "www", RecordOrder = 3,
                RecordType = "A" },
            new ResourceGroupDnsRecord() { RecordId = 4, GroupId = 3, MXPriority = 0, RecordData = "[IP]", RecordName = "ftp", RecordOrder = 1,
                RecordType = "A" },
            new ResourceGroupDnsRecord() { RecordId = 5, GroupId = 4, MXPriority = 0, RecordData = "[IP]", RecordName = "mail", RecordOrder = 1,
                RecordType = "A" },
            new ResourceGroupDnsRecord() { RecordId = 6, GroupId = 4, MXPriority = 0, RecordData = "[IP]", RecordName = "mail2", RecordOrder = 2,
                RecordType = "A" },
            new ResourceGroupDnsRecord() { RecordId = 7, GroupId = 4, MXPriority = 10, RecordData = "mail.[DOMAIN_NAME]", RecordName = "", RecordOrder = 3,
                RecordType = "MX" },
            new ResourceGroupDnsRecord() { RecordId = 9, GroupId = 4, MXPriority = 21, RecordData = "mail2.[DOMAIN_NAME]", RecordName = "", RecordOrder = 4,
                RecordType = "MX" },
            new ResourceGroupDnsRecord() { RecordId = 10, GroupId = 5, MXPriority = 0, RecordData = "[IP]", RecordName = "mssql", RecordOrder = 1,
                RecordType = "A" },
            new ResourceGroupDnsRecord() { RecordId = 11, GroupId = 6, MXPriority = 0, RecordData = "[IP]", RecordName = "mysql", RecordOrder = 1,
                RecordType = "A" },
            new ResourceGroupDnsRecord() { RecordId = 12, GroupId = 8, MXPriority = 0, RecordData = "[IP]", RecordName = "stats", RecordOrder = 1,
                RecordType = "A" },
            new ResourceGroupDnsRecord() { RecordId = 13, GroupId = 4, MXPriority = 0, RecordData = "v=spf1 a mx -all", RecordName = "", RecordOrder = 5,
                RecordType = "TXT" },
            new ResourceGroupDnsRecord() { RecordId = 14, GroupId = 12, MXPriority = 0, RecordData = "[IP]", RecordName = "smtp", RecordOrder = 1,
                RecordType = "A" },
            new ResourceGroupDnsRecord() { RecordId = 15, GroupId = 12, MXPriority = 10, RecordData = "smtp.[DOMAIN_NAME]", RecordName = "", RecordOrder = 2,
                RecordType = "MX" },
            new ResourceGroupDnsRecord() { RecordId = 16, GroupId = 12, MXPriority = 0, RecordData = "", RecordName = "autodiscover", RecordOrder = 3,
                RecordType = "CNAME" },
            new ResourceGroupDnsRecord() { RecordId = 17, GroupId = 12, MXPriority = 0, RecordData = "", RecordName = "owa", RecordOrder = 4,
                RecordType = "CNAME" }
        });
        #endregion

    }
}
