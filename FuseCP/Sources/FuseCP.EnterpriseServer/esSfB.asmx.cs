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

﻿using FuseCP.Web.Services;
using FuseCP.EnterpriseServer.Code.HostedSolution;
using FuseCP.Providers.Common;
using FuseCP.Providers.HostedSolution;
using FuseCP.Providers.ResultObjects;
using System.Collections.Generic;

namespace FuseCP.EnterpriseServer
{
    /// <summary>
    /// Summary description for esSfB
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    [Policy("EnterpriseServerPolicy")]
    public class esSfB : WebService
    {

        [WebMethod]
        public SfBUserResult CreateSfBUser(int itemId, int accountId, int sfbUserPlanId)
        {
            return SfBController.CreateSfBUser(itemId, accountId, sfbUserPlanId);
        }

        [WebMethod]
        public ResultObject DeleteSfBUser(int itemId, int accountId)
        {
            return SfBController.DeleteSfBUser(itemId, accountId);
        }

        [WebMethod]
        public SfBUsersPagedResult GetSfBUsersPaged(int itemId, string sortColumn, string sortDirection, int startRow, int maximumRows)
        {
            return SfBController.GetSfBUsersPaged(itemId, sortColumn, sortDirection, startRow, maximumRows);
        }

        [WebMethod]
        public List<SfBUser> GetSfBUsersByPlanId(int itemId, int planId)
        {
            return SfBController.GetSfBUsersByPlanId(itemId, planId);
        }

        [WebMethod]
        public IntResult GetSfBUserCount(int itemId)
        {
            return SfBController.GetSfBUsersCount(itemId);
        }


        #region SfB User Plans
        [WebMethod]
        public List<SfBUserPlan> GetSfBUserPlans(int itemId)
        {
            return SfBController.GetSfBUserPlans(itemId);
        }

        [WebMethod]
        public SfBUserPlan GetSfBUserPlan(int itemId, int sfbUserPlanId)
        {
            return SfBController.GetSfBUserPlan(itemId, sfbUserPlanId);
        }

        [WebMethod]
        public int AddSfBUserPlan(int itemId, SfBUserPlan sfbUserPlan)
        {
            return SfBController.AddSfBUserPlan(itemId, sfbUserPlan);
        }

        [WebMethod]
        public int UpdateSfBUserPlan(int itemId, SfBUserPlan sfbUserPlan)
        {
            return SfBController.UpdateSfBUserPlan(itemId, sfbUserPlan);
        }


        [WebMethod]
        public int DeleteSfBUserPlan(int itemId, int sfbUserPlanId)
        {
            return SfBController.DeleteSfBUserPlan(itemId, sfbUserPlanId);
        }

        [WebMethod]
        public int SetOrganizationDefaultSfBUserPlan(int itemId, int sfbUserPlanId)
        {
            return SfBController.SetOrganizationDefaultSfBUserPlan(itemId, sfbUserPlanId);
        }

        [WebMethod]
        public SfBUser GetSfBUserGeneralSettings(int itemId, int accountId)
        {
            return SfBController.GetSfBUserGeneralSettings(itemId, accountId);
        }

        [WebMethod]
        public SfBUserResult SetSfBUserGeneralSettings(int itemId, int accountId, string sipAddress, string lineUri)
        {
            return SfBController.SetSfBUserGeneralSettings(itemId, accountId, sipAddress, lineUri);
        }


        [WebMethod]
        public SfBUserResult SetUserSfBPlan(int itemId, int accountId, int sfbUserPlanId)
        {
            return SfBController.SetUserSfBPlan(itemId, accountId, sfbUserPlanId);
        }

        [WebMethod]
        public SfBFederationDomain[] GetFederationDomains(int itemId)
        {
            return SfBController.GetFederationDomains(itemId);
        }

        [WebMethod]
        public SfBUserResult AddFederationDomain(int itemId, string domainName, string proxyFqdn)
        {
            return SfBController.AddFederationDomain(itemId, domainName, proxyFqdn);
        }

        [WebMethod]
        public SfBUserResult RemoveFederationDomain(int itemId, string domainName)
        {
            return SfBController.RemoveFederationDomain(itemId, domainName);
        }

        #endregion

        [WebMethod]
        public string[] GetPolicyList(int itemId, SfBPolicyType type, string name)
        {
            return SfBController.GetPolicyList(itemId, type, name);
        }

    }
}
