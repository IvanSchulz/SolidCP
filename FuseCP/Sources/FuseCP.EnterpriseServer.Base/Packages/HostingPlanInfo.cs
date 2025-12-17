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

namespace FuseCP.EnterpriseServer
{
    [Serializable]
    public class HostingPlanInfo
    {
        int planId;
        int userId;
        int packageId;
        int serverId;
        string planName;
        string planDescription;
        bool available;
        bool isAddon;
        decimal setupPrice;
        decimal recurringPrice;
        int recurrenceUnit;
        int recurrenceLength;
        HostingPlanGroupInfo[] groups;
        HostingPlanQuotaInfo[] quotas;

        public int PlanId
        {
            get { return planId; }
            set { planId = value; }
        }

        public int PackageId
        {
            get { return packageId; }
            set { packageId = value; }
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

        public bool Available
        {
            get { return available; }
            set { available = value; }
        }

        public int ServerId
        {
            get { return serverId; }
            set { serverId = value; }
        }

        public bool IsAddon
        {
            get { return isAddon; }
            set { isAddon = value; }
        }

        public decimal SetupPrice
        {
            get { return this.setupPrice; }
            set { this.setupPrice = value; }
        }

        public decimal RecurringPrice
        {
            get { return this.recurringPrice; }
            set { this.recurringPrice = value; }
        }

        public int RecurrenceUnit
        {
            get { return this.recurrenceUnit; }
            set { this.recurrenceUnit = value; }
        }

        public int RecurrenceLength
        {
            get { return this.recurrenceLength; }
            set { this.recurrenceLength = value; }
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

        public int UserId
        {
            get { return this.userId; }
            set { this.userId = value; }
        }
    }
}
