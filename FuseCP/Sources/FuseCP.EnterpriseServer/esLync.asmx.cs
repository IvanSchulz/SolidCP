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
    /// Summary description for esLync
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    [Policy("EnterpriseServerPolicy")]

    public class esLync : WebService
    {

        [WebMethod]
        public LyncUserResult CreateLyncUser(int itemId, int accountId, int lyncUserPlanId)
        {
            return LyncController.CreateLyncUser(itemId, accountId, lyncUserPlanId);
        }

        [WebMethod]
        public ResultObject DeleteLyncUser(int itemId, int accountId)
        {
            return LyncController.DeleteLyncUser(itemId, accountId);
        }

        [WebMethod]
        public LyncUsersPagedResult GetLyncUsersPaged(int itemId, string sortColumn, string sortDirection, int startRow, int maximumRows)
        {
            return LyncController.GetLyncUsersPaged(itemId, sortColumn, sortDirection, startRow, maximumRows);
        }

        [WebMethod]
        public List<LyncUser> GetLyncUsersByPlanId(int itemId, int planId)
        {
            return LyncController.GetLyncUsersByPlanId(itemId, planId);
        }

        [WebMethod]
        public IntResult GetLyncUserCount(int itemId)
        {
            return LyncController.GetLyncUsersCount(itemId);
        }


        #region Lync User Plans
        [WebMethod]
        public List<LyncUserPlan> GetLyncUserPlans(int itemId)
        {
            return LyncController.GetLyncUserPlans(itemId);
        }

        [WebMethod]
        public LyncUserPlan GetLyncUserPlan(int itemId, int lyncUserPlanId)
        {
            return LyncController.GetLyncUserPlan(itemId, lyncUserPlanId);
        }

        [WebMethod]
        public int AddLyncUserPlan(int itemId, LyncUserPlan lyncUserPlan)
        {
            return LyncController.AddLyncUserPlan(itemId, lyncUserPlan);
        }

        [WebMethod]
        public int UpdateLyncUserPlan(int itemId, LyncUserPlan lyncUserPlan)
        {
            return LyncController.UpdateLyncUserPlan(itemId, lyncUserPlan);
        }


        [WebMethod]
        public int DeleteLyncUserPlan(int itemId, int lyncUserPlanId)
        {
            return LyncController.DeleteLyncUserPlan(itemId, lyncUserPlanId);
        }

        [WebMethod]
        public int SetOrganizationDefaultLyncUserPlan(int itemId, int lyncUserPlanId)
        {
            return LyncController.SetOrganizationDefaultLyncUserPlan(itemId, lyncUserPlanId);
        }

        [WebMethod]
        public LyncUser GetLyncUserGeneralSettings(int itemId, int accountId)
        {
            return LyncController.GetLyncUserGeneralSettings(itemId, accountId);
        }

        [WebMethod]
        public LyncUserResult SetLyncUserGeneralSettings(int itemId, int accountId, string sipAddress, string lineUri)
        {
            return LyncController.SetLyncUserGeneralSettings(itemId, accountId, sipAddress, lineUri);
        }


        [WebMethod]
        public LyncUserResult SetUserLyncPlan(int itemId, int accountId, int lyncUserPlanId)
        {
            return LyncController.SetUserLyncPlan(itemId, accountId, lyncUserPlanId);
        }

        [WebMethod]
        public LyncFederationDomain[] GetFederationDomains(int itemId)
        {
            return LyncController.GetFederationDomains(itemId);
        }

        [WebMethod]
        public LyncUserResult AddFederationDomain(int itemId, string domainName, string proxyFqdn)
        {
            return LyncController.AddFederationDomain(itemId, domainName, proxyFqdn);
        }

        [WebMethod]
        public LyncUserResult RemoveFederationDomain(int itemId, string domainName)
        {
            return LyncController.RemoveFederationDomain(itemId, domainName);
        }

        #endregion

        [WebMethod]
        public string[] GetPolicyList(int itemId, LyncPolicyType type, string name)
        {
            return LyncController.GetPolicyList(itemId, type, name);
        }

    }
}
