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

namespace FuseCP.Providers.HostedSolution
{
	public class ExchangePublicFolder
	{
		string name;
        string netbios;
		string displayName;
        string sAMAccountName;
		bool hideFromAddressBook;
		bool mailEnabled;

		ExchangeAccount[] accounts;	
		
		ExchangeAccount[] acceptAccounts;
		ExchangeAccount[] rejectAccounts;
		bool requireSenderAuthentication;

		public string Name
		{
			get { return this.name; }
			set { this.name = value; }
		}

		public bool HideFromAddressBook
		{
			get { return this.hideFromAddressBook; }
			set { this.hideFromAddressBook = value; }
		}

		public bool MailEnabled
		{
			get { return this.mailEnabled; }
			set { this.mailEnabled = value; }
		}

		        
		public FuseCP.Providers.HostedSolution.ExchangeAccount[] Accounts
        {
            get { return this.accounts; }
            set { this.accounts = value; }
        }
        
		public FuseCP.Providers.HostedSolution.ExchangeAccount[] AcceptAccounts
		{
			get { return this.acceptAccounts; }
			set { this.acceptAccounts = value; }
		}

		public FuseCP.Providers.HostedSolution.ExchangeAccount[] RejectAccounts
		{
			get { return this.rejectAccounts; }
			set { this.rejectAccounts = value; }
		}

		public string DisplayName
		{
			get { return this.displayName; }
			set { this.displayName = value; }
		}

		public bool RequireSenderAuthentication
		{
			get { return requireSenderAuthentication; }
			set { requireSenderAuthentication = value; }
		}

        public string SAMAccountName
        {
            get { return this.sAMAccountName; }
            set { this.sAMAccountName = value; }
        }

        public string NETBIOS
        {
            get { return this.netbios; }
            set { this.netbios = value; }
        }


	}
}
