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
	public class ExchangeMailboxStatistics : BaseStatistics
	{
		public string DisplayName{ get; set; }
		public DateTime AccountCreated { get; set; }
		public string PrimaryEmailAddress { get; set; }
        public bool LitigationHoldEnabled { get; set; }
		public bool POPEnabled { get; set; }
		public bool IMAPEnabled { get; set; }
		public bool OWAEnabled { get; set; }
		public bool MAPIEnabled { get; set; }
		public bool ActiveSyncEnabled { get; set; }
		public int TotalItems { get; set; }
		public long TotalSize { get; set; }
		public long MaxSize { get; set; }
        public long LitigationHoldTotalSize { get; set; }
        public long LitigationHoldTotalItems { get; set; }
        public long LitigationHoldMaxSize { get; set; }
		public DateTime LastLogon { get; set; }
		public DateTime LastLogoff { get; set; }
		public bool Enabled { get; set; }
		public ExchangeAccountType MailboxType { get; set; }
        public bool BlackberryEnabled { get; set; }
        public string MailboxPlan { get; set; }


        public long ArchivingTotalSize { get; set; }
        public long ArchivingTotalItems { get; set; }
        public long ArchivingMaxSize { get; set; }

	}
}
