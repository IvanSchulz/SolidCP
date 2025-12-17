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
using FuseCP.Providers.Mail;
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
    public class MailServer : HostingServiceProviderWebService, IMailServer
    {
        private IMailServer MailProvider
        {
            get { return (IMailServer)Provider; }
        }

        #region Domains
        [WebMethod, SoapHeader("settings")]
        public bool DomainExists(string domainName)
        {
            try
            {
                Log.WriteStart("'{0}' DomainExists", ProviderSettings.ProviderName);
                bool result = MailProvider.DomainExists(domainName);
                Log.WriteEnd("'{0}' DomainExists", ProviderSettings.ProviderName);
                return result;
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("'{0}' DomainExists", ProviderSettings.ProviderName), ex);
                throw;
            }
        }

        [WebMethod, SoapHeader("settings")]
        public MailDomain GetDomain(string domainName)
        {
            try
            {
                Log.WriteStart("'{0}' GetDomain", ProviderSettings.ProviderName);
                MailDomain result = MailProvider.GetDomain(domainName);
                Log.WriteEnd("'{0}' GetDomain", ProviderSettings.ProviderName);
                return result;
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("'{0}' GetDomain", ProviderSettings.ProviderName), ex);
                throw;
            }
        }

		[WebMethod, SoapHeader("settings")]
		public string[] GetDomains()
		{
			try
			{
				Log.WriteStart("'{0}' GetDomains", ProviderSettings.ProviderName);
				string[] result = MailProvider.GetDomains();
				Log.WriteEnd("'{0}' GetDomains", ProviderSettings.ProviderName);
				return result;
			}
			catch (Exception ex)
			{
				Log.WriteError(String.Format("'{0}' GetDomain", ProviderSettings.ProviderName), ex);
				throw;
			}
		}

        [WebMethod, SoapHeader("settings")]
        public void CreateDomain(MailDomain domain)
        {
            try
            {
                Log.WriteStart("'{0}' CreateDomain", ProviderSettings.ProviderName);
                MailProvider.CreateDomain(domain);
                Log.WriteEnd("'{0}' CreateDomain", ProviderSettings.ProviderName);
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("'{0}' CreateDomain", ProviderSettings.ProviderName), ex);
                throw;
            }
        }

        [WebMethod, SoapHeader("settings")]
        public void UpdateDomain(MailDomain domain)
        {
            try
            {
                Log.WriteStart("'{0}' UpdateDomain", ProviderSettings.ProviderName);
                MailProvider.UpdateDomain(domain);
                Log.WriteEnd("'{0}' UpdateDomain", ProviderSettings.ProviderName);
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("'{0}' UpdateDomain", ProviderSettings.ProviderName), ex);
                throw;
            }
        }

        [WebMethod, SoapHeader("settings")]
        public void DeleteDomain(string domainName)
        {
            try
            {
                Log.WriteStart("'{0}' DeleteDomain", ProviderSettings.ProviderName);
                MailProvider.DeleteDomain(domainName);
                Log.WriteEnd("'{0}' DeleteDomain", ProviderSettings.ProviderName);
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("'{0}' DeleteDomain", ProviderSettings.ProviderName), ex);
                throw;
            }
        }
        #endregion

        #region Domain Aliases
        [WebMethod, SoapHeader("settings")]
        public bool DomainAliasExists(string domainName, string aliasName)
        {
            try
            {
                Log.WriteStart("'{0}' DomainAliasExists", ProviderSettings.ProviderName);
                bool result = MailProvider.DomainAliasExists(domainName, aliasName);
                Log.WriteEnd("'{0}' DomainAliasExists", ProviderSettings.ProviderName);
                return result;
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("'{0}' DomainAliasExists", ProviderSettings.ProviderName), ex);
                throw;
            }
        }

        [WebMethod, SoapHeader("settings")]
        public string[] GetDomainAliases(string domainName)
        {
            try
            {
                Log.WriteStart("'{0}' GetDomainAliases", ProviderSettings.ProviderName);
                string[] result = MailProvider.GetDomainAliases(domainName);
                Log.WriteEnd("'{0}' GetDomainAliases", ProviderSettings.ProviderName);
                return result;
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("'{0}' DomainAliasExists", ProviderSettings.ProviderName), ex);
                throw;
            }
        }

        [WebMethod, SoapHeader("settings")]
        public void AddDomainAlias(string domainName, string aliasName)
        {
            try
            {
                Log.WriteStart("'{0}' AddDomainAlias", ProviderSettings.ProviderName);
                MailProvider.AddDomainAlias(domainName, aliasName);
                Log.WriteEnd("'{0}' AddDomainAlias", ProviderSettings.ProviderName);
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("'{0}' AddDomainAlias", ProviderSettings.ProviderName), ex);
                throw;
            }
        }

        [WebMethod, SoapHeader("settings")]
        public void DeleteDomainAlias(string domainName, string aliasName)
        {
            try
            {
                Log.WriteStart("'{0}' DeleteDomainAlias", ProviderSettings.ProviderName);
                MailProvider.DeleteDomainAlias(domainName, aliasName);
                Log.WriteEnd("'{0}' DeleteDomainAlias", ProviderSettings.ProviderName);
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("'{0}' DeleteDomainAlias", ProviderSettings.ProviderName), ex);
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
                bool result = MailProvider.AccountExists(accountName);
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
        public MailAccount[] GetAccounts(string domainName)
        {
            try
            {
                Log.WriteStart("'{0}' GetAccounts", ProviderSettings.ProviderName);
                MailAccount[] result = MailProvider.GetAccounts(domainName);
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
        public MailAccount GetAccount(string accountName)
        {
            try
            {
                Log.WriteStart("'{0}' GetAccount", ProviderSettings.ProviderName);
                MailAccount result = MailProvider.GetAccount(accountName);
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
        public void CreateAccount(MailAccount account)
        {
            try
            {
                Log.WriteStart("'{0}' CreateAccount", ProviderSettings.ProviderName);
                MailProvider.CreateAccount(account);
                Log.WriteEnd("'{0}' CreateAccount", ProviderSettings.ProviderName);
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("'{0}' CreateAccount", ProviderSettings.ProviderName), ex);
                throw;
            }
        }

        [WebMethod, SoapHeader("settings")]
        public void UpdateAccount(MailAccount account)
        {
            try
            {
                Log.WriteStart("'{0}' UpdateAccount", ProviderSettings.ProviderName);
                MailProvider.UpdateAccount(account);
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
                MailProvider.DeleteAccount(accountName);
                Log.WriteEnd("'{0}' DeleteAccount", ProviderSettings.ProviderName);
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("'{0}' DeleteAccount", ProviderSettings.ProviderName), ex);
                throw;
            }
        }
        #endregion

        #region Mail Aliases

        [WebMethod, SoapHeader("settings")]
        public bool MailAliasExists(string mailAliasName)
        {
            try
            {
                Log.WriteStart("'{0}' MailAliasExists", ProviderSettings.ProviderName);
                bool result = MailProvider.MailAliasExists(mailAliasName);
                Log.WriteEnd("'{0}' MailAliasExists", ProviderSettings.ProviderName);
                return result;
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("'{0}' MailAliasExists", ProviderSettings.ProviderName), ex);
                throw;
            }
        }

        [WebMethod, SoapHeader("settings")]
        public MailAlias[] GetMailAliases(string domainName)
        {
            try
            {
                Log.WriteStart("'{0}' GetMailAliases", ProviderSettings.ProviderName);
                MailAlias[] result = MailProvider.GetMailAliases(domainName);
                Log.WriteEnd("'{0}' GetMailAliases", ProviderSettings.ProviderName);
                return result;
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("'{0}' GetMailAliases", ProviderSettings.ProviderName), ex);
                throw;
            }
        }

        [WebMethod, SoapHeader("settings")]
        public MailAlias GetMailAlias(string mailAliasName)
        {
            try
            {
                Log.WriteStart("'{0}' GetMailAlias", ProviderSettings.ProviderName);
                MailAlias result = MailProvider.GetMailAlias(mailAliasName);
                Log.WriteEnd("'{0}' GetMailAlias", ProviderSettings.ProviderName);
                return result;
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("'{0}' GetMailAlias", ProviderSettings.ProviderName), ex);
                throw;
            }
        }

        [WebMethod, SoapHeader("settings")]
        public void CreateMailAlias(MailAlias mailAlias)
        {
            try
            {
                Log.WriteStart("'{0}' CreateMailAlias", ProviderSettings.ProviderName);
                MailProvider.CreateMailAlias(mailAlias);
                Log.WriteEnd("'{0}' CreateMailAlias", ProviderSettings.ProviderName);
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("'{0}' CreateMailAlias", ProviderSettings.ProviderName), ex);
                throw;
            }
        }

        [WebMethod, SoapHeader("settings")]
        public void UpdateMailAlias(MailAlias mailAlias)
        {
            try
            {
                Log.WriteStart("'{0}' UpdateMailAlias", ProviderSettings.ProviderName);
                MailProvider.UpdateMailAlias(mailAlias);
                Log.WriteEnd("'{0}' UpdateMailAlias", ProviderSettings.ProviderName);
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("'{0}' UpdateMailAlias", ProviderSettings.ProviderName), ex);
                throw;
            }
        }

        [WebMethod, SoapHeader("settings")]
        public void DeleteMailAlias(string mailAliasName)
        {
            try
            {
                Log.WriteStart("'{0}' DeleteMailAlias", ProviderSettings.ProviderName);
                MailProvider.DeleteMailAlias(mailAliasName);
                Log.WriteEnd("'{0}' DeleteMailAlias", ProviderSettings.ProviderName);
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("'{0}' DeleteMailAlias", ProviderSettings.ProviderName), ex);
                throw;
            }
        }

        #endregion

        #region Groups
        [WebMethod, SoapHeader("settings")]
        public bool GroupExists(string groupName)
        {
            try
            {
                Log.WriteStart("'{0}' GroupExists", ProviderSettings.ProviderName);
                bool result = MailProvider.GroupExists(groupName);
                Log.WriteEnd("'{0}' GroupExists", ProviderSettings.ProviderName);
                return result;
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("'{0}' GroupExists", ProviderSettings.ProviderName), ex);
                throw;
            }
        }

        [WebMethod, SoapHeader("settings")]
        public MailGroup[] GetGroups(string domainName)
        {
            try
            {
                Log.WriteStart("'{0}' GetGroups", ProviderSettings.ProviderName);
                MailGroup[] result = MailProvider.GetGroups(domainName);
                Log.WriteEnd("'{0}' GetGroups", ProviderSettings.ProviderName);
                return result;
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("'{0}' GetGroups", ProviderSettings.ProviderName), ex);
                throw;
            }
        }

        [WebMethod, SoapHeader("settings")]
        public MailGroup GetGroup(string groupName)
        {
            try
            {
                Log.WriteStart("'{0}' GetGroup", ProviderSettings.ProviderName);
                MailGroup result = MailProvider.GetGroup(groupName);
                Log.WriteEnd("'{0}' GetGroup", ProviderSettings.ProviderName);
                return result;
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("'{0}' GetGroup", ProviderSettings.ProviderName), ex);
                throw;
            }
        }

        [WebMethod, SoapHeader("settings")]
        public void CreateGroup(MailGroup group)
        {
            try
            {
                Log.WriteStart("'{0}' CreateGroup", ProviderSettings.ProviderName);
                MailProvider.CreateGroup(group);
                Log.WriteEnd("'{0}' CreateGroup", ProviderSettings.ProviderName);
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("'{0}' CreateGroup", ProviderSettings.ProviderName), ex);
                throw;
            }
        }

        [WebMethod, SoapHeader("settings")]
        public void UpdateGroup(MailGroup group)
        {
            try
            {
                Log.WriteStart("'{0}' UpdateGroup", ProviderSettings.ProviderName);
                MailProvider.UpdateGroup(group);
                Log.WriteEnd("'{0}' UpdateGroup", ProviderSettings.ProviderName);
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("'{0}' UpdateGroup", ProviderSettings.ProviderName), ex);
                throw;
            }
        }

        [WebMethod, SoapHeader("settings")]
        public void DeleteGroup(string groupName)
        {
            try
            {
                Log.WriteStart("'{0}' DeleteGroup", ProviderSettings.ProviderName);
                MailProvider.DeleteGroup(groupName);
                Log.WriteEnd("'{0}' DeleteGroup", ProviderSettings.ProviderName);
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("'{0}' DeleteGroup", ProviderSettings.ProviderName), ex);
                throw;
            }
        }
        #endregion

        #region Lists
        [WebMethod, SoapHeader("settings")]
        public bool ListExists(string listName)
        {
            try
            {
                Log.WriteStart("'{0}' ListExists", ProviderSettings.ProviderName);
                bool result = MailProvider.ListExists(listName);
                Log.WriteEnd("'{0}' ListExists", ProviderSettings.ProviderName);
                return result;
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("'{0}' ListExists", ProviderSettings.ProviderName), ex);
                throw;
            }
        }

        [WebMethod, SoapHeader("settings")]
        public MailList[] GetLists(string domainName)
        {
            try
            {
                Log.WriteStart("'{0}' GetLists", ProviderSettings.ProviderName);
                MailList[] result = MailProvider.GetLists(domainName);
                Log.WriteEnd("'{0}' GetLists", ProviderSettings.ProviderName);
                return result;
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("'{0}' GetLists", ProviderSettings.ProviderName), ex);
                throw;
            }
        }

        [WebMethod, SoapHeader("settings")]
        public MailList GetList(string listName)
        {
            try
            {
                Log.WriteStart("'{0}' GetList", ProviderSettings.ProviderName);
                MailList result = MailProvider.GetList(listName);
                Log.WriteEnd("'{0}' GetList", ProviderSettings.ProviderName);
                return result;
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("'{0}' GetList", ProviderSettings.ProviderName), ex);
                throw;
            }
        }

        [WebMethod, SoapHeader("settings")]
        public void CreateList(MailList list)
        {
            try
            {
                Log.WriteStart("'{0}' CreateList", ProviderSettings.ProviderName);
                MailProvider.CreateList(list);
                Log.WriteEnd("'{0}' CreateList", ProviderSettings.ProviderName);
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("'{0}' CreateList", ProviderSettings.ProviderName), ex);
                throw;
            }
        }

        [WebMethod, SoapHeader("settings")]
        public void UpdateList(MailList list)
        {
            try
            {
                Log.WriteStart("'{0}' UpdateList", ProviderSettings.ProviderName);
                MailProvider.UpdateList(list);
                Log.WriteEnd("'{0}' UpdateList", ProviderSettings.ProviderName);
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("'{0}' UpdateList", ProviderSettings.ProviderName), ex);
                throw;
            }
        }

        [WebMethod, SoapHeader("settings")]
        public void DeleteList(string listName)
        {
            try
            {
                Log.WriteStart("'{0}' DeleteList", ProviderSettings.ProviderName);
                MailProvider.DeleteList(listName);
                Log.WriteEnd("'{0}' DeleteList", ProviderSettings.ProviderName);
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("'{0}' DeleteList", ProviderSettings.ProviderName), ex);
                throw;
            }
        }
        #endregion
    }
}
