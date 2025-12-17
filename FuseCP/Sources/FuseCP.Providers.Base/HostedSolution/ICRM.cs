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

using System;
using FuseCP.Providers.Common;
using FuseCP.Providers.ResultObjects;

namespace FuseCP.Providers.HostedSolution
{
    public interface ICRM
    {
        OrganizationResult CreateOrganization(Guid organizationId, string organizationUniqueName, string organizationFriendlyName,
                    int baseLanguageCode,
                    string ou,
                    string baseCurrencyCode, string baseCurrencyName, string baseCurrencySymbol,
                    string initialUserDomainName, string initialUserFirstName, string initialUserLastName, string initialUserPrimaryEmail,                  
                    string organizationCollation,
                    long maxSize);

        string[] GetSupportedCollationNames();

        Currency[] GetCurrencyList();

        int[] GetInstalledLanguagePacks();
       
        ResultObject DeleteOrganization(Guid orgId);

        UserResult CreateCRMUser(OrganizationUser user, string orgName, Guid organizationId, Guid baseUnitId, int CALType);

        CRMBusinessUnitsResult GetOrganizationBusinessUnits(Guid organizationId, string orgName);

        CrmRolesResult GetAllCrmRoles(string orgName, Guid businessUnitId);

        CrmRolesResult GetCrmUserRoles(string orgName, Guid userId);

        ResultObject SetUserRoles(string orgName, Guid userId, Guid[] roles);

        ResultObject SetUserCALType(string orgName, Guid userId, int CALType);

        CrmUserResult GetCrmUserByDomainName(string domainName, string orgName);

        CrmUserResult GetCrmUserById(Guid crmUserId, string orgName);

        ResultObject ChangeUserState(bool disable, string orgName, Guid crmUserId);

        long GetDBSize(Guid organizationId);

        long GetMaxDBSize(Guid organizationId);

        ResultObject SetMaxDBSize(Guid organizationId, long maxSize);

    }
}
