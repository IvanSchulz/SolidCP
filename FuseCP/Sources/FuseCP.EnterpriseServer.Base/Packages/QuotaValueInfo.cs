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
    public class QuotaValueInfo
    {
        private int quotaId;
        private int groupId;
        private string quotaName;
        private string quotaDescription;
        private int quotaAllocatedValue;
        private int quotaAllocatedValuePerOrganization;
        private int quotaTypeId;
        private int quotaUsedValue;
        private bool quotaExhausted;

        public QuotaValueInfo()
        {
        }

        public int QuotaAllocatedValue
        {
            get { return this.quotaAllocatedValue; }
            set { this.quotaAllocatedValue = value; }
        }

        public int QuotaAllocatedValuePerOrganization
        {
            get { return this.quotaAllocatedValuePerOrganization; }
            set { this.quotaAllocatedValuePerOrganization = value; }
        }

        public int QuotaUsedValue
        {
            get { return this.quotaUsedValue; }
            set { this.quotaUsedValue = value; }
        }

        public bool QuotaExhausted
        {
            get { return this.quotaExhausted; }
            set { this.quotaExhausted = value; }
        }

        public string QuotaName
        {
            get { return this.quotaName; }
            set { this.quotaName = value; }
        }

        public string QuotaDescription
        {
            get { return this.quotaDescription; }
            set { this.quotaDescription = value; }
        }

        public int QuotaId
        {
            get { return this.quotaId; }
            set { this.quotaId = value; }
        }

        public int QuotaTypeId
        {
            get { return this.quotaTypeId; }
            set { this.quotaTypeId = value; }
        }

        public int GroupId
        {
            get { return this.groupId; }
            set { this.groupId = value; }
        }

        public int GetQuotaAllocatedValue(bool byOrganization)
        {
            return byOrganization ? QuotaAllocatedValuePerOrganization : QuotaAllocatedValue;
        }
    }
}
