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

namespace FuseCP.Providers.HostedSolution
{
    public class ExchangeDistributionList
    {
        public string DisplayName
        {
            get;
            set;
        }

        public string AccountName
        {
            get;
            set;
        }

        public bool HideFromAddressBook
        {
            get;
            set;
        }

        public ExchangeAccount[] MembersAccounts
        {
            get;
            set;
        }

        public ExchangeAccount ManagerAccount
        {
            get;
            set;
        }

        public string Notes
        {
            get;
            set;
        }

        public ExchangeAccount[] AcceptAccounts
        {
            get;
            set;
        }

        public ExchangeAccount[] RejectAccounts
        {
            get;
            set;
        }

        public bool RequireSenderAuthentication
        {
            get;
            set;
        }

        public ExchangeAccount[] SendAsAccounts
        {
            get;
            set;
        }

        public ExchangeAccount[] SendOnBehalfAccounts
        {
            get;
            set;
        }

        public string SAMAccountName
        {
            get;
            set;
        }

    }
}
