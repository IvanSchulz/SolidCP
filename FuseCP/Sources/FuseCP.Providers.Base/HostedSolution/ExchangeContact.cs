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
    public class ExchangeContact
    {
        string displayName;
        string accountName;
        string emailAddress;
        bool hideFromAddressBook;

        string firstName;
        string initials;
        string lastName;

        string jobTitle;
        string company;
        string department;
        string office;
        ExchangeAccount managerAccount;

        string businessPhone;
        string fax;
        string homePhone;
        string mobilePhone;
        string pager;
        string webPage;

        string address;
        string city;
        string state;
        string zip;
        string country;

        string notes;
        string sAMAccountName;
        private int useMapiRichTextFormat;

        ExchangeAccount[] acceptAccounts;
        ExchangeAccount[] rejectAccounts;
        bool requireSenderAuthentication;

        [LogProperty]
        public string DisplayName
        {
            get { return this.displayName; }
            set { this.displayName = value; }
        }

        [LogProperty]
        public string AccountName
        {
            get { return this.accountName; }
            set { this.accountName = value; }
        }

        [LogProperty]
        public string EmailAddress
        {
            get { return this.emailAddress; }
            set { this.emailAddress = value; }
        }

        public bool HideFromAddressBook
        {
            get { return this.hideFromAddressBook; }
            set { this.hideFromAddressBook = value; }
        }

        public string FirstName
        {
            get { return this.firstName; }
            set { this.firstName = value; }
        }

        public string Initials
        {
            get { return this.initials; }
            set { this.initials = value; }
        }

        public string LastName
        {
            get { return this.lastName; }
            set { this.lastName = value; }
        }

        public string JobTitle
        {
            get { return this.jobTitle; }
            set { this.jobTitle = value; }
        }

        public string Company
        {
            get { return this.company; }
            set { this.company = value; }
        }

        public string Department
        {
            get { return this.department; }
            set { this.department = value; }
        }

        public string Office
        {
            get { return this.office; }
            set { this.office = value; }
        }

        public ExchangeAccount ManagerAccount
        {
            get { return this.managerAccount; }
            set { this.managerAccount = value; }
        }

        public string BusinessPhone
        {
            get { return this.businessPhone; }
            set { this.businessPhone = value; }
        }

        public string Fax
        {
            get { return this.fax; }
            set { this.fax = value; }
        }

        public string HomePhone
        {
            get { return this.homePhone; }
            set { this.homePhone = value; }
        }

        public string MobilePhone
        {
            get { return this.mobilePhone; }
            set { this.mobilePhone = value; }
        }

        public string Pager
        {
            get { return this.pager; }
            set { this.pager = value; }
        }

        public string WebPage
        {
            get { return this.webPage; }
            set { this.webPage = value; }
        }

        public string Address
        {
            get { return this.address; }
            set { this.address = value; }
        }

        public string City
        {
            get { return this.city; }
            set { this.city = value; }
        }

        public string State
        {
            get { return this.state; }
            set { this.state = value; }
        }

        public string Zip
        {
            get { return this.zip; }
            set { this.zip = value; }
        }

        public string Country
        {
            get { return this.country; }
            set { this.country = value; }
        }

        public string Notes
        {
            get { return this.notes; }
            set { this.notes = value; }
        }

        public ExchangeAccount[] AcceptAccounts
        {
            get { return this.acceptAccounts; }
            set { this.acceptAccounts = value; }
        }

        public ExchangeAccount[] RejectAccounts
        {
            get { return this.rejectAccounts; }
            set { this.rejectAccounts = value; }
        }

        public bool RequireSenderAuthentication
        {
            get { return requireSenderAuthentication; }
            set { requireSenderAuthentication = value; }
        }

        public int UseMapiRichTextFormat
        {
            get { return useMapiRichTextFormat; }
            set { useMapiRichTextFormat = value; }
        }

        [LogProperty]
        public string SAMAccountName
        {
            get { return sAMAccountName; }
            set { sAMAccountName = value; }
        }



    }
}
