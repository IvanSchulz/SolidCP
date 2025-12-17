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
using System.Collections.Generic;
using FuseCP.Web.Services;
using System.ComponentModel;

using FuseCP.Providers.Mail;

namespace FuseCP.EnterpriseServer
{
    /// <summary>
    /// Summary description for esApplicationsInstaller
    /// </summary>
    [WebService(Namespace = "http://smbsaas/fusecp/enterpriseserver")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [Policy("EnterpriseServerPolicy")]
    [ToolboxItem(false)]
    public class esMailServers: WebService
    {
        #region Mail Accounts

        [WebMethod]
        public DataSet GetRawMailAccountsPaged(int packageId,
            string filterColumn, string filterValue, string sortColumn, int startRow, int maximumRows)
        {
            return MailServerController.GetRawMailAccountsPaged(packageId, filterColumn, filterValue,
                sortColumn, startRow, maximumRows);
        }

        [WebMethod]
        public List<MailAccount> GetMailAccounts(int packageId, bool recursive)
        {
            return MailServerController.GetMailAccounts(packageId, recursive);
        }

        [WebMethod]
        public MailAccount GetMailAccount(int itemId)
        {
            return MailServerController.GetMailAccount(itemId);
        }

        [WebMethod]
        public int AddMailAccount(MailAccount item)
        {
            return MailServerController.AddMailAccount(item);
        }

        [WebMethod]
        public int UpdateMailAccount(MailAccount item)
        {
            return MailServerController.UpdateMailAccount(item);
        }

        [WebMethod]
        public int DeleteMailAccount(int itemId)
        {
            return MailServerController.DeleteMailAccount(itemId);
        }
        #endregion

        #region Mail Forwardings
        [WebMethod]
        public DataSet GetRawMailForwardingsPaged(int packageId,
            string filterColumn, string filterValue, string sortColumn, int startRow, int maximumRows)
        {
            return MailServerController.GetRawMailForwardingsPaged(packageId, filterColumn, filterValue,
                sortColumn, startRow, maximumRows);
        }

        [WebMethod]
        public List<MailAlias> GetMailForwardings(int packageId, bool recursive)
        {
            return MailServerController.GetMailForwardings(packageId, recursive);
        }

        [WebMethod]
        public MailAlias GetMailForwarding(int itemId)
        {
            return MailServerController.GetMailForwarding(itemId);
        }

        [WebMethod]
        public int AddMailForwarding(MailAlias item)
        {
            return MailServerController.AddMailForwarding(item);
        }

        [WebMethod]
        public int UpdateMailForwarding(MailAlias item)
        {
            return MailServerController.UpdateMailForwarding(item);
        }

        [WebMethod]
        public int DeleteMailForwarding(int itemId)
        {
            return MailServerController.DeleteMailForwarding(itemId);
        }
        #endregion

        #region Mail Groups
        [WebMethod]
        public DataSet GetRawMailGroupsPaged(int packageId,
            string filterColumn, string filterValue, string sortColumn, int startRow, int maximumRows)
        {
            return MailServerController.GetRawMailGroupsPaged(packageId, filterColumn, filterValue,
                sortColumn, startRow, maximumRows);
        }

        [WebMethod]
        public List<MailGroup> GetMailGroups(int packageId, bool recursive)
        {
            return MailServerController.GetMailGroups(packageId, recursive);
        }

        [WebMethod]
        public MailGroup GetMailGroup(int itemId)
        {
            return MailServerController.GetMailGroup(itemId);
        }

        [WebMethod]
        public int AddMailGroup(MailGroup item)
        {
            return MailServerController.AddMailGroup(item);
        }

        [WebMethod]
        public int UpdateMailGroup(MailGroup item)
        {
            return MailServerController.UpdateMailGroup(item);
        }

        [WebMethod]
        public int DeleteMailGroup(int itemId)
        {
            return MailServerController.DeleteMailGroup(itemId);
        }
        #endregion

        #region Mail Lists
        [WebMethod]
        public DataSet GetRawMailListsPaged(int packageId,
            string filterColumn, string filterValue, string sortColumn, int startRow, int maximumRows)
        {
            return MailServerController.GetRawMailListsPaged(packageId, filterColumn, filterValue,
                sortColumn, startRow, maximumRows);
        }

        [WebMethod]
        public List<MailList> GetMailLists(int packageId, bool recursive)
        {
            return MailServerController.GetMailLists(packageId, recursive);
        }

        [WebMethod]
        public MailList GetMailList(int itemId)
        {
            return MailServerController.GetMailList(itemId);
        }

        [WebMethod]
        public int AddMailList(MailList item)
        {
            return MailServerController.AddMailList(item);
        }

        [WebMethod]
        public int UpdateMailList(MailList item)
        {
            return MailServerController.UpdateMailList(item);
        }

        [WebMethod]
        public int DeleteMailList(int itemId)
        {
            return MailServerController.DeleteMailList(itemId);
        }
        #endregion

        #region Mail Domains
        [WebMethod]
        public DataSet GetRawMailDomainsPaged(int packageId,
            string filterColumn, string filterValue, string sortColumn, int startRow, int maximumRows)
        {
            return MailServerController.GetRawMailDomainsPaged(packageId, filterColumn, filterValue,
                sortColumn, startRow, maximumRows);
        }

        [WebMethod]
        public List<MailDomain> GetMailDomains(int packageId, bool recursive)
        {
            return MailServerController.GetMailDomains(packageId, recursive);
        }

        [WebMethod]
        public List<DomainInfo> GetMailDomainPointers(int itemId)
        {
            return MailServerController.GetMailDomainPointers(itemId);
        }

        [WebMethod]
        public MailDomain GetMailDomain(int itemId)
        {
            return MailServerController.GetMailDomain(itemId);
        }

        [WebMethod]
        public int AddMailDomain(MailDomain item)
        {
            return MailServerController.AddMailDomain(item);
        }

        [WebMethod]
        public int UpdateMailDomain(MailDomain item)
        {
            return MailServerController.UpdateMailDomain(item);
        }

        [WebMethod]
        public int DeleteMailDomain(int itemId)
        {
            return MailServerController.DeleteMailDomain(itemId);
        }

        [WebMethod]
        public int AddMailDomainPointer(int itemId, int domainId)
        {
            return MailServerController.AddMailDomainPointer(itemId, domainId);
        }

        [WebMethod]
        public int DeleteMailDomainPointer(int itemId, int domainId)
        {
            return MailServerController.DeleteMailDomainPointer(itemId, domainId);
        }
        #endregion
    }
}
