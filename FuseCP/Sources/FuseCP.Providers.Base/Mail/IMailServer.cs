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
using System.Collections;

namespace FuseCP.Providers.Mail
{ 
	public interface IMailServer 
	{
		// mail domains
		bool DomainExists(string domainName);
		string[] GetDomains();
		MailDomain GetDomain(string domainName); 
		void CreateDomain(MailDomain domain); 
		void UpdateDomain(MailDomain domain); 
		void DeleteDomain(string domainName);
 
		// mail aliases
		bool DomainAliasExists(string domainName, string aliasName);
		string[] GetDomainAliases(string domainName);
		void AddDomainAlias(string domainName, string aliasName);
		void DeleteDomainAlias(string domainName, string aliasName);

		// mailboxes
		bool AccountExists(string mailboxName);
		MailAccount[] GetAccounts(string domainName);
        MailAccount GetAccount(string mailboxName);
        void CreateAccount(MailAccount mailbox);
        void UpdateAccount(MailAccount mailbox);
        void DeleteAccount(string mailboxName);

        //forwardings (mail aliases)
	    bool MailAliasExists(string mailAliasName);
	    MailAlias[] GetMailAliases(string domainName);
	    MailAlias GetMailAlias(string mailAliasName);
	    void CreateMailAlias(MailAlias mailAlias);
	    void UpdateMailAlias(MailAlias mailAlias);
        void DeleteMailAlias(string mailAliasName);


		// groups
		bool GroupExists(string groupName);
		MailGroup[] GetGroups(string domainName); 
		MailGroup GetGroup(string groupName); 
		void CreateGroup(MailGroup group); 
		void UpdateGroup(MailGroup group); 
		void DeleteGroup(string groupName);

		// mailing lists
		bool ListExists(string maillistName);
        MailList[] GetLists(string domainName);
        MailList GetList(string maillistName);
        void CreateList(MailList maillist);
        void UpdateList(MailList maillist);
        void DeleteList(string maillistName);
	} 
}
