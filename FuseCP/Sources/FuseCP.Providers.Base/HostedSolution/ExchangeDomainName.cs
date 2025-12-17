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
	public class ExchangeDomainName
	{
		int organizationDomainId;
		int itemId;
		int domainId;
		string domainName;
		bool isHost;
		bool isDefault;

		public bool IsHost
		{
			get { return this.isHost; }
			set { this.isHost = value; }
		}

		public bool IsDefault
		{
			get { return this.isDefault; }
			set { this.isDefault = value; }
		}

		public int DomainId
		{
			get { return this.domainId; }
			set { this.domainId = value; }
		}

		public int OrganizationDomainId
		{
			get { return this.organizationDomainId; }
			set { this.organizationDomainId = value; }
		}

		public int ItemId
		{
			get { return this.itemId; }
			set { this.itemId = value; }
		}

		public string DomainName
		{
			get { return this.domainName; }
			set { this.domainName = value; }
		}
	}
}
