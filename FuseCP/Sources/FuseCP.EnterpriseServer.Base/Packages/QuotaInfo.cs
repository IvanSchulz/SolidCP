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
    public class QuotaInfo
    {
		#region Quotas types constants

		public const int BooleanQuota = 1;
		public const int NumericQuota = 2;
		public const int MaximumValueQuota = 3;
		
		#endregion

        int quotaId;

        public int QuotaId
        {
            get { return quotaId; }
            set { quotaId = value; }
        }
        int groupId;

        public int GroupId
        {
            get { return groupId; }
            set { groupId = value; }
        }
        string quotaName;

        public string QuotaName
        {
            get { return quotaName; }
            set { quotaName = value; }
        }
        
        string quotaDescription;
        public string QuotaDescription
        {
            get { return quotaDescription; }
            set { quotaDescription = value; }
        }

        int quotaTypeId;
        public int QuotaTypeId
        {
            get { return quotaTypeId; }
            set { quotaTypeId = value; }
        }
        int serviceQuota;

        public int ServiceQuota
        {
            get { return serviceQuota; }
            set { serviceQuota = value; }
        }
    }
}
