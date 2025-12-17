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
using System.Data;
using System.Web;
using System.Collections;
using FuseCP.Web.Services;
using System.ComponentModel;
using FuseCP.Providers;
using FuseCP.Providers.FTP;
using FuseCP.Server.Utils;

namespace FuseCP.Server
{
    /// <summary>
    /// Summary description for FTPServer
    /// </summary>
    [WebService(Namespace = "http://smbsaas/fusecp/server/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [Policy("ServerPolicy")]
    [ToolboxItem(false)]
    public class FTPServer : HostingServiceProviderWebService, IFtpServer
    {
        private IFtpServer FtpProvider
        {
            get { return (IFtpServer)Provider; }
        }

        #region Sites

        [WebMethod, SoapHeader("settings")]
        public void ChangeSiteState(string siteId, ServerState state)
        {
            try
            {
                Log.WriteStart("'{0}' ChangeSiteState", ProviderSettings.ProviderName);
                FtpProvider.ChangeSiteState(siteId, state);
                Log.WriteEnd("'{0}' ChangeSiteState", ProviderSettings.ProviderName);
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("'{0}' ChangeSiteState", ProviderSettings.ProviderName), ex);
                throw;
            }
        }

        [WebMethod, SoapHeader("settings")]
        public ServerState GetSiteState(string siteId)
        {
            try
            {
                Log.WriteStart("'{0}' GetSiteState", ProviderSettings.ProviderName);
                ServerState result = FtpProvider.GetSiteState(siteId);
                Log.WriteEnd("'{0}' GetSiteState", ProviderSettings.ProviderName);
                return result;
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("'{0}' GetSiteState", ProviderSettings.ProviderName), ex);
                throw;
            }
        }

        [WebMethod, SoapHeader("settings")]
        public bool SiteExists(string siteId)
        {
            try
            {
                Log.WriteStart("'{0}' SiteIdExists", ProviderSettings.ProviderName);
                bool result = FtpProvider.SiteExists(siteId);
                Log.WriteEnd("'{0}' SiteIdExists", ProviderSettings.ProviderName);
                return result;
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("'{0}' SiteIdExists", ProviderSettings.ProviderName), ex);
                throw;
            }
        }

        [WebMethod, SoapHeader("settings")]
        public FtpSite[] GetSites()
        {
            try
            {
                Log.WriteStart("'{0}' GetSites", ProviderSettings.ProviderName);
                FtpSite[] result = FtpProvider.GetSites();
                Log.WriteEnd("'{0}' GetSites", ProviderSettings.ProviderName);
                return result;
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("'{0}' GetSites", ProviderSettings.ProviderName), ex);
                throw;
            }
        }

        [WebMethod, SoapHeader("settings")]
        public FtpSite GetSite(string siteId)
        {
            try
            {
                Log.WriteStart("'{0}' GetSite", ProviderSettings.ProviderName);
                FtpSite result = FtpProvider.GetSite(siteId);
                Log.WriteEnd("'{0}' GetSite", ProviderSettings.ProviderName);
                return result;
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("'{0}' GetSite", ProviderSettings.ProviderName), ex);
                throw;
            }
        }

        [WebMethod, SoapHeader("settings")]
        public string CreateSite(FtpSite site)
        {
            try
            {
                Log.WriteStart("'{0}' CreateSite", ProviderSettings.ProviderName);
                string result = FtpProvider.CreateSite(site);
                Log.WriteEnd("'{0}' CreateSite", ProviderSettings.ProviderName);
                return result;
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("'{0}' CreateSite", ProviderSettings.ProviderName), ex);
                throw;
            }
        }

        [WebMethod, SoapHeader("settings")]
        public void UpdateSite(FtpSite site)
        {
            try
            {
                Log.WriteStart("'{0}' UpdateSite", ProviderSettings.ProviderName);
                FtpProvider.UpdateSite(site);
                Log.WriteEnd("'{0}' UpdateSite", ProviderSettings.ProviderName);
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("'{0}' UpdateSite", ProviderSettings.ProviderName), ex);
                throw;
            }
        }

        [WebMethod, SoapHeader("settings")]
        public void DeleteSite(string siteId)
        {
            try
            {
                Log.WriteStart("'{0}' DeleteSite", ProviderSettings.ProviderName);
                FtpProvider.DeleteSite(siteId);
                Log.WriteEnd("'{0}' DeleteSite", ProviderSettings.ProviderName);
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("'{0}' DeleteSite", ProviderSettings.ProviderName), ex);
                throw;
            }
        }

        #endregion

        #region Accounts
        [WebMethod, SoapHeader("settings")]
        public bool AccountExists(string accountName)
        {
            try
            {
                Log.WriteStart("'{0}' AccountExists", ProviderSettings.ProviderName);
                bool result = FtpProvider.AccountExists(accountName);
                Log.WriteEnd("'{0}' AccountExists", ProviderSettings.ProviderName);
                return result;
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("'{0}' AccountExists", ProviderSettings.ProviderName), ex);
                throw;
            }
        }

        [WebMethod, SoapHeader("settings")]
        public FtpAccount[] GetAccounts()
        {
            try
            {
                Log.WriteStart("'{0}' GetAccounts", ProviderSettings.ProviderName);
                FtpAccount[] result = FtpProvider.GetAccounts();
                Log.WriteEnd("'{0}' GetAccounts", ProviderSettings.ProviderName);
                return result;
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("'{0}' GetAccounts", ProviderSettings.ProviderName), ex);
                throw;
            }
        }

        [WebMethod, SoapHeader("settings")]
        public FtpAccount GetAccount(string accountName)
        {
            try
            {
                Log.WriteStart("'{0}' GetAccount", ProviderSettings.ProviderName);
                FtpAccount result = FtpProvider.GetAccount(accountName);
                Log.WriteEnd("'{0}' GetAccount", ProviderSettings.ProviderName);
                return result;
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("'{0}' GetAccount", ProviderSettings.ProviderName), ex);
                throw;
            }
        }

        [WebMethod, SoapHeader("settings")]
        public void CreateAccount(FtpAccount account)
        {
            try
            {
                Log.WriteStart("'{0}' CreateAccount", ProviderSettings.ProviderName);
                FtpProvider.CreateAccount(account);
                Log.WriteEnd("'{0}' CreateAccount", ProviderSettings.ProviderName);
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("'{0}' CreateAccount", ProviderSettings.ProviderName), ex);
                throw;
            }
        }

        [WebMethod, SoapHeader("settings")]
        public void UpdateAccount(FtpAccount account)
        {
            try
            {
                Log.WriteStart("'{0}' UpdateAccount", ProviderSettings.ProviderName);
                FtpProvider.UpdateAccount(account);
                Log.WriteEnd("'{0}' UpdateAccount", ProviderSettings.ProviderName);
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("'{0}' UpdateAccount", ProviderSettings.ProviderName), ex);
                throw;
            }
        }

        [WebMethod, SoapHeader("settings")]
        public void DeleteAccount(string accountName)
        {
            try
            {
                Log.WriteStart("'{0}' DeleteAccount", ProviderSettings.ProviderName);
                FtpProvider.DeleteAccount(accountName);
                Log.WriteEnd("'{0}' DeleteAccount", ProviderSettings.ProviderName);
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("'{0}' DeleteAccount", ProviderSettings.ProviderName), ex);
                throw;
            }
        }
        #endregion
    }
}
