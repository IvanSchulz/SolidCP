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

﻿using System;
using System.Collections.Generic;
using System.Text;

namespace FuseCP.Providers.HostedSolution
{
    public interface ISfBServer
    {
        string CreateOrganization(string organizationId, string sipDomain, bool enableConferencing, bool enableConferencingVideo, int maxConferenceSize, bool enabledFederation, bool enabledEnterpriseVoice);
        string GetOrganizationTenantId(string organizationId);
        bool DeleteOrganization(string organizationId, string sipDomain);

        bool CreateUser(string organizationId, string userUpn, SfBUserPlan plan);
        SfBUser GetSfBUserGeneralSettings(string organizationId, string userUpn);
        bool SetSfBUserGeneralSettings(string organizationId, string userUpn, SfBUser sfbUser);
        bool SetSfBUserPlan(string organizationId, string userUpn, SfBUserPlan plan);
        bool DeleteUser(string userUpn);

        SfBFederationDomain[] GetFederationDomains(string organizationId);
        bool AddFederationDomain(string organizationId, string domainName, string proxyFqdn);
        bool RemoveFederationDomain(string organizationId, string domainName);

        void ReloadConfiguration();

        string[] GetPolicyList(SfBPolicyType type, string name);
    }
}
