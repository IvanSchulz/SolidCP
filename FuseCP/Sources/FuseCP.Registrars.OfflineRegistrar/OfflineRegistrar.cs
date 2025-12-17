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
using System.Text;

using FuseCP.EnterpriseServer;

namespace FuseCP.Ecommerce.EnterpriseServer
{
	public class OfflineRegistrar : SystemPluginBase, IDomainRegistrar
	{
		private string registrarName;

		public OfflineRegistrar()
		{
		}

		#region IDomainRegistrar Members

		public bool SubAccountRequired
		{
			get { return false; }
		}

		public string RegistrarName
		{
			get { return registrarName; }
			set { registrarName = value; }
		}

		public bool CheckSubAccountExists(string account, string emailAddress)
		{
			return true;
		}

		public AccountResult GetSubAccount(string account, string emailAddress)
		{
			AccountResult result = new AccountResult();

			return result;
		}

		public AccountResult CreateSubAccount(CommandParams args)
		{
			AccountResult result = new AccountResult();

			return result;
		}

		public DomainStatus CheckDomain(string domain)
		{
			return DomainStatus.NotFound;
		}

		public void RegisterDomain(DomainNameSvc domainSvc, ContractAccount accountInfo, string[] nameServers)
		{
			domainSvc["OrderID"] = DateTime.Now.ToString("yyyy-MM-dd") + "-" + domainSvc.Fqdn;
		}

		public void RenewDomain(DomainNameSvc domainSvc, ContractAccount accountInfo, string[] nameServers)
		{
			domainSvc["OrderID"] = DateTime.Now.ToString("yyyy-MM-dd") + "-" + domainSvc.Fqdn;
		}

		public TransferDomainResult TransferDomain(CommandParams args, DomainContacts contacts)
		{
			TransferDomainResult result = new TransferDomainResult();

			result[TransferDomainResult.TRANSFER_ORDER_NUMBER] = DateTime.Now.ToString("yyyy-MM-dd") + "-" + args[CommandParams.DOMAIN_NAME];

			return result;
		}

		#endregion
	}
}
