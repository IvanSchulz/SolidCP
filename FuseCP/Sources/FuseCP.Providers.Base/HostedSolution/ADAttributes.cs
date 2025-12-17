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

namespace FuseCP.Providers.HostedSolution
{
    public class ADAttributes
    {
        public const string Initials = "initials";
        public const string JobTitle = "title";
        public const string Company = "company";
        public const string Department = "department";
        public const string Office = "physicalDeliveryOfficeName";
        public const string BusinessPhone = "telephoneNumber";
        public const string Fax = "facsimileTelephoneNumber";
        public const string HomePhone = "homePhone";
        public const string MobilePhone = "mobile";
        public const string Pager = "pager";
        public const string WebPage = "wWWHomePage";
        public const string Address = "streetAddress";
        public const string City = "l";
        public const string State = "st";
        public const string Zip = "postalCode";
        public const string Country = "c";
        public const string Notes = "info";
        public const string FirstName = "givenName";
        public const string LastName = "sn";
        public const string DisplayName = "displayName";
        public const string AccountDisabled = "AccountDisabled";
        public const string AccountLocked = "IsAccountLocked";
        public const string Manager = "manager";
        public const string SetPassword = "SetPassword";
        public const string SAMAccountName = "sAMAccountName";
        public const string UserPrincipalName = "UserPrincipalName";
        public const string GroupType = "GroupType";
        public const string Name = "Name";
        public const string ExternalEmail = "mail";
        public const string CustomAttribute2 = "extensionAttribute2";
        public const string DistinguishedName = "distinguishedName";
        public const string SID = "objectSid";
        public const string PwdLastSet = "pwdLastSet";
        public const string PasswordExpirationDateTime = "msDS-UserPasswordExpiryTimeComputed";
        public const string PasswordNeverExpires = "PasswordNeverExpires";
        public const string UserAccountControl = "UserAccountControl";
        public const string Description = "description";
        public const string dSHeuristics = "dSHeuristics";

        // ACL Attributes
        public const string ReadCn = @"bf96793f-0de6-11d0-a285-00aa003049e2";
        public const string ReadCanonicalName = @"9a7ad945-ca53-11d1-bbd0-0080c76670c0";
        public const string ReadDistinguishedName = @"bf9679e4-0de6-11d0-a285-00aa003049e2";
        public const string ReadGpLink = @"f30e3bbe-9ff0-11d1-b603-0000f80367c1";
        public const string ReadGpOption = @"f30e3bbf-9ff0-11d1-b603-0000f80367c1";
    }
}
