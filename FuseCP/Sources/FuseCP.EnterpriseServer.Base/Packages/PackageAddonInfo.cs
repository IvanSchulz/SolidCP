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
	/// Summary description for PackageAddonInfo.
	/// </summary>
	[Serializable]
	public class PackageAddonInfo
	{
		int packageAddonId;
		int packageId;
		int planId;
		int quantity;
        int statusId;
		DateTime purchaseDate;
		string comments;
        string planName;
        string planDescription;


		public PackageAddonInfo()
		{
		}

		public int PackageAddonId
		{
			get { return packageAddonId; }
			set { packageAddonId = value; }
		}

		public int PackageId
		{
			get { return packageId; }
			set { packageId = value; }
		}

        public int PlanId
		{
            get { return planId; }
            set { planId = value; }
		}

		public int Quantity
		{
			get { return quantity; }
			set { quantity = value; }
		}

		public DateTime PurchaseDate
		{
			get { return purchaseDate; }
			set { purchaseDate = value; }
		}

		public string Comments
		{
			get { return comments; }
			set { comments = value; }
		}

        public int StatusId
        {
            get { return this.statusId; }
            set { this.statusId = value; }
        }

        public string PlanName
        {
            get { return planName; }
            set { planName = value; }
        }

        public string PlanDescription
        {
            get { return planDescription; }
            set { planDescription = value; }
        }

	}
}
