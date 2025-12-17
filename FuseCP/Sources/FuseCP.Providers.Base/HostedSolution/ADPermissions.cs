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

using System.Collections.Generic;
using System.Linq;

namespace FuseCP.Providers.HostedSolution
{
    public enum AclPermission
    {
        InheritanceEnabled,
        EveryoneAllowed,
        AuthenticatedUsersAllowed,
        PreWindows2000Allowed,
        ManyRulesForOrgGroup,
        ListObjectPermission,
        ReadCnPropertyAccess,
        ReadDistinguishedNamePropertyAccess,
        ReadGpLinkPropertyAccess,
        ReadGpOptionPropertyAccess,
        RecipientManagementReadGeneric,
        RecipientManagementFullControl,
        PublicFolderFullControl,
        RdsManyRulesForOrgGroup,
        RdsListObjectPermission,
        RdsReadCnPropertyAccess,
        RdsReadDistinguishedNamePropertyAccess,
        RdsReadGpLinkPropertyAccess,
        RdsReadGpOptionPropertyAccess,
        AdManyRulesForOrgGroup,
        AdListObjectPermission,
        AdReadCnPropertyAccess,
        AdReadCanonicalNamePropertyAccess,
        AdReadDistinguishedNamePropertyAccess,
        AdReadGpLinkPropertyAccess,
        AdReadGpOptionPropertyAccess,
        AdDsHueristics,
        AuthenticatedUsersNotInheritedPermissions,
        ExchangeServersReadGeneric,
        ExchangeServersFullControl,
        AuthenticatedUsersReadGeneric,
        PrivelegedServersNotExists,
        PrivelegedServersReadGeneric,
    }

    public class ADPermissions
    {
        public string Name { get; set; }

        public string OuPath { get; set; }

        public List<AclPermission> AclPermissions { get; set; } = new List<AclPermission>();

        public bool HasCorrectPermissions => !AclPermissions.Any();

    }
}
