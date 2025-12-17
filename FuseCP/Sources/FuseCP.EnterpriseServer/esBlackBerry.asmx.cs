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
using FuseCP.Providers.ResultObjects;

namespace FuseCP.EnterpriseServer
{
    /// <summary>
    /// Summary description for esBlackBerry
    /// </summary>
    [WebService(Namespace = "http://smbsaas/fusecp/enterpriseserver")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [Policy("EnterpriseServerPolicy")]
    public class esBlackBerry : WebService
    {

        [WebMethod]
        public ResultObject CreateBlackBerryUser(int itemId, int accountId)
        {
            return BlackBerryController.CreateBlackBerryUser(itemId, accountId);
        }

        [WebMethod]
        public ResultObject DeleteBlackBerryUser(int itemId, int accountId)
        {
            return BlackBerryController.DeleteBlackBerryUser(itemId, accountId);
        }

        [WebMethod]
        public OrganizationUsersPagedResult GetBlackBerryUsersPaged(int itemId, string sortColumn, string sortDirection, string name, string email,
            int startRow, int maximumRows)
        {
            return BlackBerryController.GetBlackBerryUsers(itemId, sortColumn, sortDirection, name, email, startRow, maximumRows);
        }

        [WebMethod]
        public IntResult GetBlackBerryUserCount(int itemId, string name, string email)
        {
            return BlackBerryController.GetBlackBerryUsersCount(itemId, name, email);
        }

        [WebMethod]
        public BlackBerryUserStatsResult GetBlackBerryUserStats(int itemId, int accountId)
        {                        
            return BlackBerryController.GetBlackBerryUserStats(itemId, accountId);
        }


        [WebMethod]
        public ResultObject DeleteDataFromBlackBerryDevice(int itemId, int accountId)
        {
            return BlackBerryController.DeleteDataFromBlackBerryDevice(itemId, accountId);
        }

        [WebMethod]
        public ResultObject SetEmailActivationPassword(int itemId, int accountId)
        {
            return BlackBerryController.SetEmailActivationPassword(itemId, accountId);
        }


        [WebMethod]
        public ResultObject SetActivationPasswordWithExpirationTime(int itemId, int accountId, string password, int time)
        {
            return BlackBerryController.SetActivationPasswordWithExpirationTime(itemId, accountId, password, time);
        }


    }
}
