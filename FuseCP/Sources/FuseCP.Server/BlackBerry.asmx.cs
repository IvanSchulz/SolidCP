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

﻿using System.ComponentModel;
using FuseCP.Web.Services;
using FuseCP.Providers;
using FuseCP.Providers.Common;
using FuseCP.Providers.HostedSolution;
using FuseCP.Providers.ResultObjects;

namespace FuseCP.Server
{
    /// </summary>
    [WebService(Namespace = "http://smbsaas/fusecp/server/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [Policy("ServerPolicy")]
    [ToolboxItem(false)]
    public class BlackBerry : HostingServiceProviderWebService, IBlackBerry
    {
        private IBlackBerry BlackBerryProvider
        {
            get { return (IBlackBerry)Provider; }
        }


        [WebMethod, SoapHeader("settings")]
        public ResultObject CreateBlackBerryUser(string primaryEmailAddress)
        {
            return BlackBerryProvider.CreateBlackBerryUser(primaryEmailAddress);
        }

        [WebMethod, SoapHeader("settings")]
        public ResultObject DeleteBlackBerryUser(string primaryEmailAddress)
        {
            return BlackBerryProvider.DeleteBlackBerryUser(primaryEmailAddress);
        }

        [WebMethod, SoapHeader("settings")] 
        public BlackBerryUserStatsResult GetBlackBerryUserStats(string primaryEmailAddress)
        {
            return BlackBerryProvider.GetBlackBerryUserStats(primaryEmailAddress);
        }

        [WebMethod, SoapHeader("settings")] 
        public ResultObject SetActivationPasswordWithExpirationTime(string primaryEmailAddress, string password, int time)
        {
            return BlackBerryProvider.SetActivationPasswordWithExpirationTime(primaryEmailAddress, password, time);
        }

        [WebMethod, SoapHeader("settings")]
        public ResultObject SetEmailActivationPassword(string primaryEmailAddress)
        {
            return BlackBerryProvider.SetEmailActivationPassword(primaryEmailAddress);
        }

        [WebMethod, SoapHeader("settings")]
        public ResultObject DeleteDataFromBlackBerryDevice(string primaryEmailAddress)
        {
            return BlackBerryProvider.DeleteDataFromBlackBerryDevice(primaryEmailAddress);
        }
                
    }
}
