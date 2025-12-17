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
using FuseCP.Providers.Filters;
using FuseCP.EnterpriseServer.Base;
using FuseCP.Server.Client;
using System;
using System.Collections.Specialized;

namespace FuseCP.EnterpriseServer
{
    public class SpamExpertsController: ControllerBase
    {
        public SpamExpertsController(ControllerBase provider): base(provider) { }

        public Server.Client.SpamExperts GetServer(int serviceId)
        {
            Server.Client.SpamExperts ws = new Server.Client.SpamExperts();

            ServiceProviderProxy.Init(ws, serviceId);

            /*
            string[] settings = ws.ServiceProviderSettingsSoapHeaderValue.Settings;

            List<string> resSettings = new List<string>(settings);

            ws.ServiceProviderSettingsSoapHeaderValue.Settings = resSettings.ToArray();*/

            return ws;
        }

        private int GetServiceId(int packageId)
        {
            return PackageController.GetPackageServiceId(packageId, ResourceGroups.Filters);
        }

        private bool IsPackageServiceEnabled(int packageId, int serviceId)
        {
            QuotaValueInfo quota = PackageController.GetPackageQuota(packageId, Quotas.FILTERS_ENABLE);
            return (serviceId != 0 && Convert.ToBoolean(quota.QuotaAllocatedValue));
        }

        public SpamExpertsResult AddDomainFilter(SpamExpertsRoute route)
        {
            int serviceId = GetServiceId(route.PackageId);

            if (!IsPackageServiceEnabled(route.PackageId, serviceId))
                return new SpamExpertsResult(SpamExpertsStatus.Error,"Service not enabled");

            Server.Client.SpamExperts server = GetServer(serviceId);

            return server.AddDomainFilter(route.DomainName, "", "postmaster@" + route.DomainName, route.Destinations);
        }

        public void DeleteDomainFilter(DomainInfo domain)
        {
            int serviceId = GetServiceId(domain.PackageId);

            if (IsPackageServiceEnabled(domain.PackageId, serviceId))
            {
                Server.Client.SpamExperts server = GetServer(serviceId);
                var res = server.DeleteDomainFilter(domain.DomainName);
            }
        }

        public SpamExpertsResult AddDomainFilterAlias(DomainInfo domain, string alias)
        {
            int serviceId = GetServiceId(domain.PackageId);

            if (!IsPackageServiceEnabled(domain.PackageId, serviceId))
                return new SpamExpertsResult(SpamExpertsStatus.Error, "Service not enabled");

            Server.Client.SpamExperts server = GetServer(serviceId);

            return server.AddDomainFilterAlias(domain.DomainName, alias);
        }

        public void DeleteDomainFilterAlias(DomainInfo domain, string alias)
        {
            int serviceId = GetServiceId(domain.PackageId);

            if (IsPackageServiceEnabled(domain.PackageId, serviceId))
            {
                Server.Client.SpamExperts server = GetServer(serviceId);
                var res = server.DeleteDomainFilterAlias(domain.DomainName,alias);
            }
        }

        public SpamExpertsResult AddEmailFilter(int packageId, string username, string password, string domain)
        {
            int serviceId = GetServiceId(packageId);

            if (!IsPackageServiceEnabled(packageId, serviceId))
                return new SpamExpertsResult(SpamExpertsStatus.Error, "Service not enabled");
            if (Convert.ToBoolean(PackageController.GetPackageQuota(packageId, Quotas.FILTERS_ENABLE_EMAIL_USERS).QuotaAllocatedValue))
            {
                Server.Client.SpamExperts server = GetServer(serviceId);

                return server.AddEmailFilter(username, domain, password);
            }
            return new SpamExpertsResult(SpamExpertsStatus.Error, "Service not enabled for users");
        }

        public void DeleteEmailFilter(int packageId, string email)
        {
            int serviceId = GetServiceId(packageId);

            if (IsPackageServiceEnabled(packageId, serviceId))
            {
                if (Convert.ToBoolean(PackageController.GetPackageQuota(packageId, Quotas.FILTERS_ENABLE_EMAIL_USERS).QuotaAllocatedValue))
                {
                    Server.Client.SpamExperts server = GetServer(serviceId);
                    var res = server.DeleteEmailFilter(email);
                }
            }
        }

        public void SetEmailFilterPassword(int packageId, string email, string password)
        {
            int serviceId = GetServiceId(packageId);

            if (IsPackageServiceEnabled(packageId, serviceId))
            {
                if (Convert.ToBoolean(PackageController.GetPackageQuota(packageId, Quotas.FILTERS_ENABLE_EMAIL_USERS).QuotaAllocatedValue))
                {
                    Server.Client.SpamExperts server = GetServer(serviceId);
                    var res = server.SetEmailFilterUserPassword(email, password);
                }
            }
        }

        public bool IsSpamExpertsEnabled(int packageId, string group)
        {
            int serviceId = GetServiceId(packageId);
            if (IsPackageServiceEnabled(packageId, serviceId))
            {
                int mailServiceId = PackageController.GetPackageServiceId(packageId, group);
                StringDictionary exSettings = ServerController.GetServiceSettings(mailServiceId);
                return (exSettings != null && Convert.ToBoolean(exSettings["EnableMailFilter"]));
            }
            return false;
        }
    }
}
