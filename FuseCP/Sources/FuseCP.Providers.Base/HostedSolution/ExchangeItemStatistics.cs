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
	public class ExchangeItemStatistics
	{
		private string itemName;
		private int totalItems;
		private int totalSizeMB;
		private DateTime lastLogon;
		private DateTime lastLogoff;
		private DateTime lastAccessTime;
		private DateTime lastModificationTime;

		public string ItemName
		{
			get { return this.itemName; }
			set { this.itemName = value; }
		}

		public int TotalItems
		{
			get { return this.totalItems; }
			set { this.totalItems = value; }
		}

		public int TotalSizeMB
		{
			get { return this.totalSizeMB; }
			set { this.totalSizeMB = value; }
		}

		public DateTime LastLogon
		{
			get { return this.lastLogon; }
			set { this.lastLogon = value; }
		}

		public DateTime LastLogoff
		{
			get { return this.lastLogoff; }
			set { this.lastLogoff = value; }
		}

		public DateTime LastAccessTime
		{
			get { return lastAccessTime; }
			set { lastAccessTime = value; }
		}

		public DateTime LastModificationTime
		{
			get { return lastModificationTime; }
			set { lastModificationTime = value; }
		}
	}
}
