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

namespace FuseCP.Providers.HostedSolution
{
    [LogParentProperty("Id", NameInLog = "Org ID")]
    [LogParentProperty("Name", NameInLog = "Org Name")]
    public class Organization : ServiceProviderItem
    {
        #region Fields

        private string distinguishedName;
        private string organizationId;
        private string defaultDomain;
        private string offlineAddressBook;
        private string addressList;
        private string roomsAddressList;
        private string globalAddressList;
        private string addressBookPolicy;

        private string database;
        private string securityGroup;
        private string lyncTenantId;
        private string sfbTenantId;
        private int diskSpace;
        private int keepDeletedItemsDays;


        private Guid crmOrganizationId;
        private int crmOrgState;
        private int crmAdministratorId;
        private string crmLanguadgeCode;
        private string crmCollation;
        private string crmCurrency;
        private string crmUrl;

        private int maxSharePointStorage;
        private int warningSharePointStorage;

        private int maxSharePointEnterpriseStorage;
        private int warningSharePointEnterpriseStorage;

        #endregion
 
        [Persistent]
        public bool IsDefault { get; set; }

        [Persistent]
        public int MaxSharePointStorage
        {
            get { return maxSharePointStorage; }
            set { maxSharePointStorage = value; }
        }

        [Persistent]
        public int WarningSharePointStorage
        {
            get { return warningSharePointStorage; }
            set { warningSharePointStorage = value; }
        }

        [Persistent]
        public int MaxSharePointEnterpriseStorage
        {
            get { return maxSharePointEnterpriseStorage; }
            set { maxSharePointEnterpriseStorage = value; }
        }

        [Persistent]
        public int WarningSharePointEnterpriseStorage
        {
            get { return warningSharePointEnterpriseStorage; }
            set { warningSharePointEnterpriseStorage = value; }
        }

        [Persistent]
        public string CrmUrl
        {
            get { return crmUrl; }
            set { crmUrl = value; }
        }

        [Persistent]
        public Guid CrmOrganizationId
        {
            get { return crmOrganizationId; }
            set { crmOrganizationId = value; }
        }

        [Persistent]
        public int CrmOrgState
        {
            get { return crmOrgState; }
            set { crmOrgState = value; }
        }

        [Persistent]
        public int CrmAdministratorId
        {
            get { return crmAdministratorId; }
            set { crmAdministratorId = value; }
        }

        [Persistent]
        public string CrmLanguadgeCode
        {
            get { return crmLanguadgeCode; }
            set { crmLanguadgeCode = value; }
        }

        [Persistent]
        public string CrmCollation
        {
            get { return crmCollation; }
            set { crmCollation = value; }
        }

        [Persistent]
        public string CrmCurrency
        {
            get { return crmCurrency; }
            set { crmCurrency = value; }
        }

        [Persistent]
        public string DistinguishedName
        {
            get { return distinguishedName; }
            set { distinguishedName = value; }
        }

        [Persistent]
        public string OrganizationId
        {
            get { return organizationId; }
            set { organizationId = value; }
        }

        [Persistent]
        public string DefaultDomain
        {
            set
            {
                defaultDomain = value;
            }
            get
            {
                return defaultDomain;
            }
        }


        [Persistent]
        public string OfflineAddressBook
        {
            get { return offlineAddressBook; }
            set { offlineAddressBook = value; }
        }

        [Persistent]
        public string AddressList
        {
            get { return addressList; }
            set { addressList = value; }
        }

        [Persistent]
        public string RoomsAddressList
        {
            get { return roomsAddressList; }
            set { roomsAddressList = value; }
        }

        [Persistent]
        public string GlobalAddressList
        {
            get { return globalAddressList; }
            set { globalAddressList = value; }
        }

        [Persistent]
        public string AddressBookPolicy
        {
            get { return addressBookPolicy; }
            set { addressBookPolicy = value; }
        }

        [Persistent]
        public string Database
        {
            get { return database; }
            set { database = value; }
        }

        [Persistent]
        public string SecurityGroup
        {
            get { return securityGroup; }
            set { securityGroup = value; }
        }

        [Persistent]
        public int DiskSpace
        {
            get { return diskSpace; }
            set { diskSpace = value; }
        }

        [Persistent]
        public int KeepDeletedItemsDays
        {
            get { return keepDeletedItemsDays; }
            set { keepDeletedItemsDays = value; }
        }

        [Persistent]
        public bool IsOCSOrganization { get; set; }


        [Persistent]
        public string LyncTenantId
        {
            get { return lyncTenantId; }
            set { lyncTenantId = value; }
        }
        [Persistent]
        public string SfBTenantId
        {
            get { return sfbTenantId; }
            set { sfbTenantId = value; }
        }


    }
}
