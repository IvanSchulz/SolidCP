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

public partial class UsersDetailedConfiguration: EntityTypeConfiguration<UsersDetailed>
{
    public override void Configure() {
        ToView("UsersDetailed");

        #region Seed Data
        HasData(() => new UsersDetailed[] {
            new UsersDetailed() { UserId = 1, Changed = DateTime.Parse("2010-07-16T10:53:02.4530000Z").ToUniversalTime(), Comments = "", Created = DateTime.Parse("2010-07-16T10:53:02.4530000Z").ToUniversalTime(), EcommerceEnabled = true, Email = "serveradmin@myhosting.com",
                FirstName = "Enterprise", FullName = "Enterprise Administrator", LastName = "Administrator", PackagesNumber = 1, RoleId = 1, StatusId = 1,
                Username = "serveradmin" }
        });
        #endregion

    }
}
