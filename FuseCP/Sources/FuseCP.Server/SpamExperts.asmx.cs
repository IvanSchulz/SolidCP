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
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FuseCP.Web.Services;
using System.ComponentModel;
using FuseCP.Providers;
using FuseCP.Providers.Filters;
using FuseCP.Server.Utils;

namespace FuseCP.Server
{
    /// <summary>
    /// Summary description for MailServer
    /// </summary>
    [WebService(Namespace = "http://smbsaas/fusecp/server/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [Policy("ServerPolicy")]
    [ToolboxItem(false)]
    public class SpamExperts : HostingServiceProviderWebService, ISpamExperts
    {
        private ISpamExperts SpamExpertsProvider
        {
            get { return (ISpamExperts)Provider; }
        }
        [WebMethod, SoapHeader("settings")]
        public SpamExpertsResult AddDomainFilter(string domain, string password, string email, string[] destinations)
        {
            return SpamExpertsProvider.AddDomainFilter(domain, password, email, destinations);
        }

        [WebMethod, SoapHeader("settings")]
        public SpamExpertsResult AddEmailFilter(string name, string domain, string password)
        {
            return SpamExpertsProvider.AddEmailFilter(name, domain, password);
        }

        [WebMethod, SoapHeader("settings")]
        public SpamExpertsResult DeleteDomainFilter(string domain)
        {
            return SpamExpertsProvider.DeleteDomainFilter(domain);
        }

        [WebMethod, SoapHeader("settings")]
        public SpamExpertsResult DeleteEmailFilter(string email)
        {
            return SpamExpertsProvider.DeleteEmailFilter(email);
        }

        [WebMethod, SoapHeader("settings")]
        public SpamExpertsResult SetDomainFilterDestinations(string name, string[] destinations)
        {
            return SpamExpertsProvider.SetDomainFilterDestinations(name, destinations);
        }

        [WebMethod, SoapHeader("settings")]
        public SpamExpertsResult SetDomainFilterUser(string domain, string password, string email)
        {
            return SpamExpertsProvider.SetDomainFilterUser(domain, password, email);
        }

        [WebMethod, SoapHeader("settings")]
        public SpamExpertsResult SetDomainFilterUserPassword(string name, string password)
        {
            return SpamExpertsProvider.SetDomainFilterUserPassword(name, password);
        }

        [WebMethod, SoapHeader("settings")]
        public SpamExpertsResult SetEmailFilterUserPassword(string email, string password)
        {
            return SpamExpertsProvider.SetEmailFilterUserPassword(email, password);
        }

        [WebMethod, SoapHeader("settings")]
        public SpamExpertsResult AddDomainFilterAlias(string domain, string alias)
        {
            return SpamExpertsProvider.AddDomainFilterAlias(domain,alias);
        }

        [WebMethod, SoapHeader("settings")]
        public SpamExpertsResult DeleteDomainFilterAlias(string domain, string alias)
        {
            return SpamExpertsProvider.DeleteDomainFilterAlias(domain, alias);
        }
    }
}
