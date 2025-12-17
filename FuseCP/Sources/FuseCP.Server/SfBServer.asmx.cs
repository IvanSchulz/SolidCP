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
using FuseCP.Providers.HostedSolution;
using FuseCP.Server.Utils;

namespace FuseCP.Server
{
    /// <summary>
    /// OCS Web Service
    /// </summary>
    [WebService(Namespace = "http://smbsaas/fusecp/server/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [Policy("ServerPolicy")]
    [ToolboxItem(false)]
    public class SfBServer : HostingServiceProviderWebService
    {
        private ISfBServer SfB
        {
            get { return (ISfBServer)Provider; }
        }


        #region Organization
        [WebMethod, SoapHeader("settings")]
        public string CreateOrganization(string organizationId, string sipDomain, bool enableConferencing, bool enableConferencingVideo, int maxConferenceSize, bool enabledFederation, bool enabledEnterpriseVoice)
        {
            try
            {
                Log.WriteStart("{0}.CreateOrganization", ProviderSettings.ProviderName);
                string ret = SfB.CreateOrganization(organizationId, sipDomain, enableConferencing, enableConferencingVideo, maxConferenceSize, enabledFederation, enabledEnterpriseVoice);
                Log.WriteEnd("{0}.CreateOrganization", ProviderSettings.ProviderName);
                return ret;
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("Error: {0}.CreateOrganization", ProviderSettings.ProviderName), ex);
                throw;
            }
        }

        [WebMethod, SoapHeader("settings")]
        public string GetOrganizationTenantId(string organizationId)
        {
            try
            {
                Log.WriteStart("{0}.GetOrganizationTenantId", ProviderSettings.ProviderName);
                string ret = SfB.GetOrganizationTenantId(organizationId);
                Log.WriteEnd("{0}.GetOrganizationTenantId", ProviderSettings.ProviderName);
                return ret;
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("Error: {0}.GetOrganizationTenantId", ProviderSettings.ProviderName), ex);
                throw;
            }
        }

        [WebMethod, SoapHeader("settings")]
        public bool DeleteOrganization(string organizationId, string sipDomain)
        {
            try
            {
                Log.WriteStart("{0}.DeleteOrganization", ProviderSettings.ProviderName);
                bool ret = SfB.DeleteOrganization(organizationId, sipDomain);
                Log.WriteEnd("{0}.DeleteOrganization", ProviderSettings.ProviderName);
                return ret;
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("Error: {0}.DeleteOrganization", ProviderSettings.ProviderName), ex);
                throw;
            }
        }
        #endregion

        #region Users

        [WebMethod, SoapHeader("settings")]
        public bool CreateUser(string organizationId, string userUpn, SfBUserPlan plan)
        {
            try
            {
                Log.WriteStart("{0}.CreateUser", ProviderSettings.ProviderName);
                bool ret = SfB.CreateUser(organizationId, userUpn, plan);
                Log.WriteEnd("{0}.CreateUser", ProviderSettings.ProviderName);
                return ret;
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("Error: {0}.CreateUser", ProviderSettings.ProviderName), ex);
                throw;
            }
        }

        [WebMethod, SoapHeader("settings")]
        public SfBUser GetSfBUserGeneralSettings(string organizationId, string userUpn)
        {
            try
            {
                Log.WriteStart("{0}.GetSfBUserGeneralSettings", ProviderSettings.ProviderName);
                SfBUser ret = SfB.GetSfBUserGeneralSettings(organizationId, userUpn);
                Log.WriteEnd("{0}.GetSfBUserGeneralSettings", ProviderSettings.ProviderName);
                return ret;
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("Error: {0}.GetSfBUserGeneralSettings", ProviderSettings.ProviderName), ex);
                throw;
            }
        }

        [WebMethod, SoapHeader("settings")]
        public bool SetSfBUserGeneralSettings(string organizationId, string userUpn, SfBUser sfbUser)
        {
            try
            {
                Log.WriteStart("{0}.SetSfBUserGeneralSettings", ProviderSettings.ProviderName);
                bool ret = SfB.SetSfBUserGeneralSettings(organizationId, userUpn, sfbUser);
                Log.WriteEnd("{0}.SetSfBUserGeneralSettings", ProviderSettings.ProviderName);
                return ret;
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("Error: {0}.SetSfBUserGeneralSettings", ProviderSettings.ProviderName), ex);
                throw;
            }
        }


        [WebMethod, SoapHeader("settings")]
        public bool SetSfBUserPlan(string organizationId, string userUpn, SfBUserPlan plan)
        {
            try
            {
                Log.WriteStart("{0}.SetSfBUserPlan", ProviderSettings.ProviderName);
                bool ret = SfB.SetSfBUserPlan(organizationId, userUpn, plan);
                Log.WriteEnd("{0}.SetSfBUserPlan", ProviderSettings.ProviderName);
                return ret;
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("Error: {0}.SetSfBUserPlan", ProviderSettings.ProviderName), ex);
                throw;
            }
        }

        [WebMethod, SoapHeader("settings")]
        public bool DeleteUser(string userUpn)
        {
            try
            {
                Log.WriteStart("{0}.DeleteUser", ProviderSettings.ProviderName);
                bool ret = SfB.DeleteUser(userUpn);
                Log.WriteEnd("{0}.DeleteUser", ProviderSettings.ProviderName);
                return ret;
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("Error: {0}.DeleteUser", ProviderSettings.ProviderName), ex);
                throw;
            }
        }

        #endregion

        #region Federation
        [WebMethod, SoapHeader("settings")]
        public SfBFederationDomain[] GetFederationDomains(string organizationId)
        {
            try
            {
                Log.WriteStart("{0}.GetFederationDomains", ProviderSettings.ProviderName);
                SfBFederationDomain[] ret = SfB.GetFederationDomains(organizationId);
                Log.WriteEnd("{0}.GetFederationDomains", ProviderSettings.ProviderName);
                return ret;
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("Error: {0}.GetFederationDomains", ProviderSettings.ProviderName), ex);
                throw;
            }
        }

        [WebMethod, SoapHeader("settings")]
        public bool AddFederationDomain(string organizationId, string domainName, string proxyFqdn)
        {
            try
            {
                Log.WriteStart("{0}.AddFederationDomain", ProviderSettings.ProviderName);
                bool ret = SfB.AddFederationDomain(organizationId, domainName, proxyFqdn);
                Log.WriteEnd("{0}.AddFederationDomain", ProviderSettings.ProviderName);
                return ret;
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("Error: {0}.AddFederationDomain", ProviderSettings.ProviderName), ex);
                throw;
            }
        }

        [WebMethod, SoapHeader("settings")]
        public bool RemoveFederationDomain(string organizationId, string domainName)
        {
            try
            {
                Log.WriteStart("{0}.RemoveFederationDomain", ProviderSettings.ProviderName);
                bool ret = SfB.RemoveFederationDomain(organizationId, domainName);
                Log.WriteEnd("{0}.RemoveFederationDomain", ProviderSettings.ProviderName);
                return ret;
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("Error: {0}.RemoveFederationDomain", ProviderSettings.ProviderName), ex);
                throw;
            }
        }
        #endregion

        [WebMethod, SoapHeader("settings")]
        public void ReloadConfiguration()
        {
            try
            {
                Log.WriteStart("{0}.ReloadConfiguration", ProviderSettings.ProviderName);
                SfB.ReloadConfiguration();
                Log.WriteEnd("{0}.ReloadConfiguration", ProviderSettings.ProviderName);
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("Error: {0}.ReloadConfiguration", ProviderSettings.ProviderName), ex);
                throw;
            }
        }

        [WebMethod, SoapHeader("settings")]
        public string[] GetPolicyList(SfBPolicyType type, string name)
        {
            string[] ret = null;

            try
            {
                Log.WriteStart("{0}.GetPolicyList", ProviderSettings.ProviderName);
                ret = SfB.GetPolicyList(type, name);
                Log.WriteEnd("{0}.GetPolicyList", ProviderSettings.ProviderName);
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("Error: {0}.GetPolicyList", ProviderSettings.ProviderName), ex);
                throw;
            }

            return ret;
        }


    }
}
