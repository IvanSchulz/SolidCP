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

using Version = FuseCP.EnterpriseServer.Data.Entities.Version;

public partial class VersionConfiguration: EntityTypeConfiguration<Version>
{
    public override void Configure() {

        #region Seed Data
        HasData(() => new Version[] {
            new Version() { DatabaseVersion = "1.0", BuildDate = DateTime.Parse("2010-04-09T22:00:00.0000000Z").ToUniversalTime() },
            new Version() { DatabaseVersion = "1.0.1.0", BuildDate = DateTime.Parse("2010-07-16T10:53:03.5630000Z").ToUniversalTime() },
            new Version() { DatabaseVersion = "1.0.2.0", BuildDate = DateTime.Parse("2010-09-02T22:00:00.0000000Z").ToUniversalTime() },
            new Version() { DatabaseVersion = "1.1.0.9", BuildDate = DateTime.Parse("2010-11-15T23:00:00.0000000Z").ToUniversalTime() },
            new Version() { DatabaseVersion = "1.1.2.13", BuildDate = DateTime.Parse("2011-04-14T22:00:00.0000000Z").ToUniversalTime() },
            new Version() { DatabaseVersion = "1.2.0.38", BuildDate = DateTime.Parse("2011-07-12T22:00:00.0000000Z").ToUniversalTime() },
            new Version() { DatabaseVersion = "1.2.1.6", BuildDate = DateTime.Parse("2012-03-28T22:00:00.0000000Z").ToUniversalTime() },
            new Version() { DatabaseVersion = "2.0.0.228", BuildDate = DateTime.Parse("2012-12-06T23:00:00.0000000Z").ToUniversalTime() }
        });
        #endregion

    }
}
