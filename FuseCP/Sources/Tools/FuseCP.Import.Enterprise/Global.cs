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
using System.DirectoryServices;

namespace FuseCP.Import.Enterprise
{
	class Global
	{
		private static string rootOU;
		public static string RootOU
		{
			get { return rootOU; }
			set { rootOU = value; }
		}
	
		private static string primaryDomainController;
		public static string PrimaryDomainController
		{
			get { return primaryDomainController; }
			set { primaryDomainController = value; }
		}
	
		private static string aDRootDomain;

		public static string ADRootDomain
		{
			get { return aDRootDomain; }
			set { aDRootDomain = value; }
		}

        private static string netBiosDomain;
        public static string NetBiosDomain
        {
            get { return netBiosDomain; }
            set { netBiosDomain = value; }
        }

        public static bool IsExchange;

        public static PackageInfo Space;
		public static string TempDomain;

		public static string MailboxCluster;
		public static string StorageGroup;
		public static string MailboxDatabase;
		public static string KeepDeletedMailboxesDays;
		public static string KeepDeletedItemsDays;
		public static DirectoryEntry OrgDirectoryEntry;
		public static List<DirectoryEntry> SelectedAccounts;
		public static string OrganizationId;
		public static string OrganizationName;
		public static int ItemId;
		public static string ErrorMessage;
		public static bool ImportAccountsOnly;
		public static bool HasErrors;
        public static int defaultMailboxPlanId;
	
	}
}
