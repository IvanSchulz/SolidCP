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

public partial class UserConfiguration: EntityTypeConfiguration<User>
{
    public override void Configure() {
        Property(e => e.HtmlMail).HasDefaultValue(true);

        HasOne(d => d.Owner).WithMany(p => p.InverseOwner).HasConstraintName("FK_Users_Users");

        #region Seed Data
        HasData(() => new User[] {
            new User() { UserId = 1, Address = "", Changed = DateTime.Parse("2010-07-16T10:53:02.4530000Z").ToUniversalTime(), City = "", Comments = "", Country = "",
                Created = DateTime.Parse("2010-07-16T10:53:02.4530000Z").ToUniversalTime(), EcommerceEnabled = true, Email = "serveradmin@myhosting.com", Fax = "", FirstName = "Enterprise", HtmlMail = true,
                InstantMessenger = "", LastName = "Administrator", Password = "", PrimaryPhone = "", RoleId = 1, SecondaryEmail = "",
                SecondaryPhone = "", State = "", StatusId = 1, Username = "serveradmin", Zip = "" }
        });
        #endregion

    }
}
