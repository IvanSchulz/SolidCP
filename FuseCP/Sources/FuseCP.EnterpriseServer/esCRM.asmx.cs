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
using FuseCP.Providers.Common;
using FuseCP.Providers.HostedSolution;
using FuseCP.Providers.ResultObjects;

namespace FuseCP.EnterpriseServer
{
    /// <summary>
    /// Summary description for esApplicationsInstaller
    /// </summary>
    [WebService(Namespace = "http://smbsaas/fusecp/enterpriseserver")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [Policy("EnterpriseServerPolicy")]
    [ToolboxItem(false)]
    
    public class esCRM : WebService
    {

        [WebMethod]
        public OrganizationResult CreateOrganization(int organizationId, string baseCurrencyCode, string baseCurrencyName, string baseCurrencySymbol, string regionName, int userId, string collation, int baseLanguageCode)
        {
            return CRMController.CreateOrganization(organizationId, baseCurrencyCode, baseCurrencyName, baseCurrencySymbol, regionName, userId, collation, baseLanguageCode);
        }

        [WebMethod]
        public StringArrayResultObject GetCollation(int packageId)
        {            
            return CRMController.GetCollation(packageId);            
        }

        [WebMethod]
        public StringArrayResultObject GetCollationByServiceId(int serviceId)
        {
            return CRMController.GetCollationByServiceId(serviceId);            
        }

        [WebMethod]
        public CurrencyArrayResultObject GetCurrency(int packageId)
        {
            return CRMController.GetCurrency(packageId);
        }

        [WebMethod]
        public CurrencyArrayResultObject GetCurrencyByServiceId(int serviceId)
        {
            return CRMController.GetCurrencyByServiceId(serviceId);
        }
      
        [WebMethod]
        public ResultObject DeleteCRMOrganization(int organizationId)
        {
            return CRMController.DeleteOrganization(organizationId);
        }

        [WebMethod]
        public OrganizationUsersPagedResult GetCRMUsersPaged(int itemId, string sortColumn, string sortDirection, string name, string email,
            int startRow, int maximumRows)
        {
            return CRMController.GetCRMUsers(itemId, sortColumn, sortDirection, name, email, startRow, maximumRows);
        }

        [WebMethod]
        public IntResult GetCRMUserCount(int itemId, string name, string email, int CALType)
        {
            return CRMController.GetCRMUsersCount(itemId, name, email, CALType);
        }

        [WebMethod]
        public UserResult CreateCRMUser(OrganizationUser user, int packageId, int itemId, Guid businessUnitOrgId, int CALType)
        {
            return CRMController.CreateCRMUser(user, packageId, itemId, businessUnitOrgId, CALType);
        }


        [WebMethod]
        public CRMBusinessUnitsResult GetBusinessUnits(int itemId, int packageId)
        {
            return CRMController.GetCRMBusinessUnits(itemId, packageId);
        }


        [WebMethod]
        public CrmRolesResult GetCrmRoles(int itemId, int accountId, int packageId)
        {
            return CRMController.GetCRMRoles(itemId, accountId, packageId);
        }

        [WebMethod]
        public ResultObject SetUserRoles(int itemId, int accountId, int packageId, Guid[] roles)
        {
            return CRMController.SetUserRoles(itemId, accountId, packageId, roles);
        }

        [WebMethod]
        public ResultObject SetUserCALType(int itemId, int accountId, int packageId, int CALType)
        {
            return CRMController.SetUserCALType(itemId, accountId, packageId, CALType);
        }

        [WebMethod]
        public ResultObject ChangeUserState(int itemId, int accountId, bool disable)
        {
            return CRMController.ChangeUserState(itemId, accountId, disable);
        }


        [WebMethod]
        public CrmUserResult GetCrmUser(int itemId, int accountId)
        {
            return CRMController.GetCrmUser(itemId, accountId);
        }

        [WebMethod]
        public ResultObject SetMaxDBSize(int itemId, int packageId, long maxSize)
        {
            return CRMController.SetMaxDBSize(itemId, packageId, maxSize);
        }

        [WebMethod]
        public long GetDBSize(int itemId, int packageId)
        {
            return CRMController.GetDBSize(itemId, packageId);
        }

        [WebMethod]
        public long GetMaxDBSize(int itemId, int packageId)
        {
            return CRMController.GetMaxDBSize(itemId, packageId);
        }

        [WebMethod]
        public int[] GetInstalledLanguagePacks(int packageId)
        {
            return CRMController.GetInstalledLanguagePacks(packageId);
        }

        [WebMethod]
        public int[] GetInstalledLanguagePacksByServiceId(int serviceId)
        {
            return CRMController.GetInstalledLanguagePacksByServiceId(serviceId);
        }
        
    }
}
