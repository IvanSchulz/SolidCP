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
using System.ComponentModel;
using FuseCP.Web.Services;
using FuseCP.Providers;
using FuseCP.Providers.Common;
using FuseCP.Providers.HostedSolution;
using FuseCP.Providers.ResultObjects;

namespace FuseCP.Server
{
    
    /// <summary>
    /// 
    /// </summary>
    [WebService(Namespace = "http://smbsaas/fusecp/server/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [Policy("ServerPolicy")]
    [ToolboxItem(false)]
    public class CRM : HostingServiceProviderWebService, ICRM
    {              
        private ICRM CrmProvider
        {
            get { return (ICRM)Provider; }
        }


        [WebMethod, SoapHeader("settings")]
        public OrganizationResult CreateOrganization(Guid organizationId, string organizationUniqueName, string organizationFriendlyName, int baseLanguageCode, string ou, string baseCurrencyCode, string baseCurrencyName, string baseCurrencySymbol, string initialUserDomainName, string initialUserFirstName, string initialUserLastName, string initialUserPrimaryEmail, string organizationCollation, long maxSize)
        {                                  
           return CrmProvider.CreateOrganization(organizationId, organizationUniqueName, organizationFriendlyName, baseLanguageCode, ou, baseCurrencyCode, baseCurrencyName, baseCurrencySymbol, initialUserDomainName, initialUserFirstName, initialUserLastName, initialUserPrimaryEmail, organizationCollation, maxSize);
        }

        [WebMethod, SoapHeader("settings")] 
        public string[] GetSupportedCollationNames()
        {
            return CrmProvider.GetSupportedCollationNames();
        }
       
        [WebMethod, SoapHeader("settings")]
        public Currency[] GetCurrencyList()
        {
            return CrmProvider.GetCurrencyList();
        }

        [WebMethod, SoapHeader("settings")]
        public int[] GetInstalledLanguagePacks()
        {
            return CrmProvider.GetInstalledLanguagePacks();
        }

                
        [WebMethod, SoapHeader("settings")]
        public ResultObject DeleteOrganization(Guid orgId)
        {
            return CrmProvider.DeleteOrganization(orgId);
        }

        [WebMethod, SoapHeader("settings")]
        public UserResult CreateCRMUser(OrganizationUser user, string orgName, Guid organizationId, Guid baseUnitId, int CALType)
        {
            return CrmProvider.CreateCRMUser(user, orgName, organizationId, baseUnitId, CALType);
        }

        [WebMethod, SoapHeader("settings")]
        public CRMBusinessUnitsResult GetOrganizationBusinessUnits(Guid organizationId, string orgName)
        {
            return CrmProvider.GetOrganizationBusinessUnits(organizationId, orgName);
        }

        [WebMethod, SoapHeader("settings")]
        public CrmRolesResult GetAllCrmRoles(string orgName, Guid businessUnitId)
        {
            return CrmProvider.GetAllCrmRoles(orgName, businessUnitId);
        }
        
        [WebMethod, SoapHeader("settings")]
        public CrmRolesResult GetCrmUserRoles(string orgName, Guid userId)
        {
            return CrmProvider.GetCrmUserRoles(orgName, userId);
        }
        
        [WebMethod, SoapHeader("settings")]
        public ResultObject SetUserRoles(string orgName, Guid userId, Guid []roles)
        {
            return CrmProvider.SetUserRoles(orgName, userId, roles);
        }

        [WebMethod, SoapHeader("settings")]
        public ResultObject SetUserCALType(string orgName, Guid userId, int CALType)
        {
            return CrmProvider.SetUserCALType(orgName, userId, CALType);
        }
        
        [WebMethod, SoapHeader("settings")]
        public CrmUserResult GetCrmUserByDomainName(string domainName, string orgName)
        {
            return CrmProvider.GetCrmUserByDomainName(domainName, orgName);
        }


        [WebMethod, SoapHeader("settings")]
        public CrmUserResult GetCrmUserById(Guid crmUserId, string orgName)
        {
            return CrmProvider.GetCrmUserById(crmUserId, orgName);
        }

        [WebMethod, SoapHeader("settings")]
        public ResultObject ChangeUserState(bool disable, string orgName, Guid crmUserId)
        {
            return CrmProvider.ChangeUserState(disable, orgName, crmUserId);
        }

        [WebMethod, SoapHeader("settings")]
        public long GetDBSize(Guid organizationId)
        {
            return CrmProvider.GetDBSize(organizationId);
        }

        [WebMethod, SoapHeader("settings")]
        public long GetMaxDBSize(Guid organizationId)
        {
            return CrmProvider.GetMaxDBSize(organizationId);
        }

        [WebMethod, SoapHeader("settings")]
        public ResultObject SetMaxDBSize(Guid organizationId, long maxSize)
        {
            return CrmProvider.SetMaxDBSize(organizationId, maxSize);
        }
    
    }
}
