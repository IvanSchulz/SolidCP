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

namespace FuseCP.EnterpriseServer
{
	/// <summary>
	/// Summary description for PackageInfo.
	/// </summary>
	[Serializable]
	public class PackageInfo
	{
		int packageId;
		int userId;
		int parentPackageId;
		int statusId;
		int planId;
        int serverId;
		DateTime purchaseDate;
        DateTime statusIDchangeDate;
        string packageName;
		string packageComments;
        int domains;
        int diskSpace;
        int bandWidth;
        int domainsQuota;
        int diskSpaceQuota;
        int bandWidthQuota;
        bool overrideQuotas;
        bool defaultTopPackage;
        HostingPlanGroupInfo[] groups;
        HostingPlanQuotaInfo[] quotas;

		public PackageInfo()
		{
		}

		public int PackageId
		{
			get { return packageId; }
			set { packageId = value; }
		}

		public int UserId
		{
			get { return userId; }
			set { userId = value; }
		}

		public int ParentPackageId
		{
			get { return parentPackageId; }
			set { parentPackageId = value; }
		}

		public int StatusId
		{
			get { return statusId; }
			set { statusId = value; }
		}

		public int PlanId
		{
			get { return planId; }
			set { planId = value; }
		}

		public DateTime PurchaseDate
		{
			get { return purchaseDate; }
			set { purchaseDate = value; }
		}

		public string PackageName
		{
			get { return packageName; }
			set { packageName = value; }
		}

		public string PackageComments
		{
			get { return packageComments; }
			set { packageComments = value; }
		}

        public int ServerId
        {
            get { return serverId; }
            set { serverId = value; }
        }

        public int DiskSpace
        {
            get { return diskSpace; }
            set { diskSpace = value; }
        }

        public int BandWidth
        {
            get { return bandWidth; }
            set { bandWidth = value; }
        }

        public int DiskSpaceQuota
        {
            get { return this.diskSpaceQuota; }
            set { this.diskSpaceQuota = value; }
        }

        public int BandWidthQuota
        {
            get { return this.bandWidthQuota; }
            set { this.bandWidthQuota = value; }
        }

        public int Domains
        {
            get { return this.domains; }
            set { this.domains = value; }
        }

        public int DomainsQuota
        {
            get { return this.domainsQuota; }
            set { this.domainsQuota = value; }
        }

        public bool OverrideQuotas
        {
            get { return this.overrideQuotas; }
            set { this.overrideQuotas = value; }
        }

        public bool DefaultTopPackage 
        {
            get { return this.defaultTopPackage;  }
            set { this.defaultTopPackage = value; }
        }

        public HostingPlanGroupInfo[] Groups
        {
            get { return this.groups; }
            set { this.groups = value; }
        }

        public HostingPlanQuotaInfo[] Quotas
        {
            get { return this.quotas; }
            set { this.quotas = value; }
        }

        public DateTime StatusIDchangeDate { get => statusIDchangeDate; set => statusIDchangeDate = value; }
    }
}
