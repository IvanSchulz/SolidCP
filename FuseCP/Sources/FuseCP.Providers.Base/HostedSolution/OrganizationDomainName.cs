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
    public class OrganizationDomainName
    {
        int organizationDomainId;
        int itemId;
        int domainId;
        int domainTypeId;
        string domainName;
        bool isHost;
        bool isDefault;

        public bool IsHost
        {
            get { return isHost; }
            set { isHost = value; }
        }

        public bool IsDefault
        {
            get { return isDefault; }
            set { isDefault = value; }
        }

        public int DomainId
        {
            get { return domainId; }
            set { domainId = value; }
        }

        public int DomainTypeId
        {
            get { return domainTypeId; }
            set { domainTypeId = value; }
        }

        public ExchangeAcceptedDomainType DomainType
        {
            get
            {
                ExchangeAcceptedDomainType type = (ExchangeAcceptedDomainType)domainTypeId;
                return type;
            }
        }

        public int OrganizationDomainId
        {
            get { return organizationDomainId; }
            set { organizationDomainId = value; }
        }

        public int ItemId
        {
            get { return itemId; }
            set { itemId = value; }
        }

        public string DomainName
        {
            get { return domainName; }
            set { domainName = value; }
        }
    }
}
