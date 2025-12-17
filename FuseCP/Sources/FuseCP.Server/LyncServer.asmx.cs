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
    public class LyncServer : HostingServiceProviderWebService
    {
        private ILyncServer Lync
        {
            get { return (ILyncServer)Provider; }
        }


        #region Organization
        [WebMethod, SoapHeader("settings")]
        public string CreateOrganization(string organizationId, string sipDomain, bool enableConferencing, bool enableConferencingVideo, int maxConferenceSize, bool enabledFederation, bool enabledEnterpriseVoice)
        {
            try
            {
                Log.WriteStart("{0}.CreateOrganization", ProviderSettings.ProviderName);
                string ret = Lync.CreateOrganization(organizationId, sipDomain, enableConferencing, enableConferencingVideo, maxConferenceSize, enabledFederation, enabledEnterpriseVoice);
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
                string ret = Lync.GetOrganizationTenantId(organizationId);
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
                bool ret = Lync.DeleteOrganization(organizationId, sipDomain);
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
        public bool CreateUser(string organizationId, string userUpn, LyncUserPlan plan)
        {
            try
            {
                Log.WriteStart("{0}.CreateUser", ProviderSettings.ProviderName);
                bool ret = Lync.CreateUser(organizationId, userUpn, plan);
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
        public LyncUser GetLyncUserGeneralSettings(string organizationId, string userUpn)
        {
            try
            {
                Log.WriteStart("{0}.GetLyncUserGeneralSettings", ProviderSettings.ProviderName);
                LyncUser ret = Lync.GetLyncUserGeneralSettings(organizationId, userUpn);
                Log.WriteEnd("{0}.GetLyncUserGeneralSettings", ProviderSettings.ProviderName);
                return ret;
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("Error: {0}.GetLyncUserGeneralSettings", ProviderSettings.ProviderName), ex);
                throw;
            }
        }

        [WebMethod, SoapHeader("settings")]
        public bool SetLyncUserGeneralSettings(string organizationId, string userUpn, LyncUser lyncUser)
        {
            try
            {
                Log.WriteStart("{0}.SetLyncUserGeneralSettings", ProviderSettings.ProviderName);
                bool ret = Lync.SetLyncUserGeneralSettings(organizationId, userUpn, lyncUser);
                Log.WriteEnd("{0}.SetLyncUserGeneralSettings", ProviderSettings.ProviderName);
                return ret;
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("Error: {0}.SetLyncUserGeneralSettings", ProviderSettings.ProviderName), ex);
                throw;
            }
        }


        [WebMethod, SoapHeader("settings")]
        public bool SetLyncUserPlan(string organizationId, string userUpn, LyncUserPlan plan)
        {
            try
            {
                Log.WriteStart("{0}.SetLyncUserPlan", ProviderSettings.ProviderName);
                bool ret = Lync.SetLyncUserPlan(organizationId, userUpn, plan);
                Log.WriteEnd("{0}.SetLyncUserPlan", ProviderSettings.ProviderName);
                return ret;
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("Error: {0}.SetLyncUserPlan", ProviderSettings.ProviderName), ex);
                throw;
            }
        }

        [WebMethod, SoapHeader("settings")]
        public bool DeleteUser(string userUpn)
        {
            try
            {
                Log.WriteStart("{0}.DeleteUser", ProviderSettings.ProviderName);
                bool ret = Lync.DeleteUser(userUpn);
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
        public LyncFederationDomain[] GetFederationDomains(string organizationId)
        {
            try
            {
                Log.WriteStart("{0}.GetFederationDomains", ProviderSettings.ProviderName);
                LyncFederationDomain[] ret = Lync.GetFederationDomains(organizationId);
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
                bool ret = Lync.AddFederationDomain(organizationId, domainName, proxyFqdn);
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
                bool ret = Lync.RemoveFederationDomain(organizationId, domainName);
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
                Lync.ReloadConfiguration();
                Log.WriteEnd("{0}.ReloadConfiguration", ProviderSettings.ProviderName);
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("Error: {0}.ReloadConfiguration", ProviderSettings.ProviderName), ex);
                throw;
            }
        }

        [WebMethod, SoapHeader("settings")]
        public string[] GetPolicyList(LyncPolicyType type, string name)
        {
            string[] ret = null;

            try
            {
                Log.WriteStart("{0}.GetPolicyList", ProviderSettings.ProviderName);
                ret = Lync.GetPolicyList(type, name);
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
