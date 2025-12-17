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

namespace FuseCP.EnterpriseServer
{
    /// <summary>
    /// Summary description for esOCS
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    [Policy("EnterpriseServerPolicy")]
    public class esOCS : WebService
    {

        [WebMethod]
        public OCSUserResult CreateOCSUser(int itemId, int accountId)
        {
            return OCSController.CreateOCSUser(itemId, accountId);
        }

        [WebMethod]
        public ResultObject DeleteOCSUser(int itemId, string instanceId)
        {
            return OCSController.DeleteOCSUser(itemId, instanceId);
        }

        [WebMethod]
        public OCSUsersPagedResult GetOCSUsersPaged(int itemId, string sortColumn, string sortDirection, string name, string email,
            int startRow, int maximumRows)
        {
            return OCSController.GetOCSUsers(itemId, sortColumn, sortDirection, name, email, startRow, maximumRows);            
        }

        [WebMethod]
        public IntResult GetOCSUserCount(int itemId, string name, string email)
        {
            return OCSController.GetOCSUsersCount(itemId, name, email);            
        }

        [WebMethod]
        public OCSUser GetUserGeneralSettings(int itemId, string instanceId)
        {
            return OCSController.GetUserGeneralSettings(itemId, instanceId);
        }

        [WebMethod]
        public void SetUserGeneralSettings(int itemId, string instanceId, bool enabledForFederation, bool enabledForPublicIMConnectivity, bool archiveInternalCommunications, bool archiveFederatedCommunications, bool enabledForEnhancedPresence)
        {
            OCSController.SetUserGeneralSettings(itemId, instanceId, enabledForFederation, enabledForPublicIMConnectivity, archiveInternalCommunications, archiveFederatedCommunications, enabledForEnhancedPresence);    
        }
    }

}
