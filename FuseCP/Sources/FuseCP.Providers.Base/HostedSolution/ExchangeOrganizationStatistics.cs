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
	public class ExchangeOrganizationStatistics
	{
		private int allocatedMailboxes;
		private int createdMailboxes;

		private int allocatedContacts;
		private int createdContacts;

		private int allocatedDistributionLists;
		private int createdDistributionLists;

		private int allocatedPublicFolders;
		private int createdPublicFolders;

		private int allocatedDomains;
		private int createdDomains;

		private int allocatedDiskSpace;
		private int usedDiskSpace;

		public int AllocatedMailboxes
		{
			get { return this.allocatedMailboxes; }
			set { this.allocatedMailboxes = value; }
		}

		public int CreatedMailboxes
		{
			get { return this.createdMailboxes; }
			set { this.createdMailboxes = value; }
		}

		public int AllocatedContacts
		{
			get { return this.allocatedContacts; }
			set { this.allocatedContacts = value; }
		}

		public int CreatedContacts
		{
			get { return this.createdContacts; }
			set { this.createdContacts = value; }
		}

		public int AllocatedDistributionLists
		{
			get { return this.allocatedDistributionLists; }
			set { this.allocatedDistributionLists = value; }
		}

		public int CreatedDistributionLists
		{
			get { return this.createdDistributionLists; }
			set { this.createdDistributionLists = value; }
		}

		public int AllocatedPublicFolders
		{
			get { return this.allocatedPublicFolders; }
			set { this.allocatedPublicFolders = value; }
		}

		public int CreatedPublicFolders
		{
			get { return this.createdPublicFolders; }
			set { this.createdPublicFolders = value; }
		}

		public int AllocatedDomains
		{
			get { return this.allocatedDomains; }
			set { this.allocatedDomains = value; }
		}

		public int CreatedDomains
		{
			get { return this.createdDomains; }
			set { this.createdDomains = value; }
		}

		public int AllocatedDiskSpace
		{
			get { return this.allocatedDiskSpace; }
			set { this.allocatedDiskSpace = value; }
		}

		public int UsedDiskSpace
		{
			get { return this.usedDiskSpace; }
			set { this.usedDiskSpace = value; }
		}
	}
}
