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
	[Serializable]
	public class ServiceInfo
	{
		private int serviceId;
		private int serverId;
		private int providerId;
		private string serviceName;
		private string comments;
		private int serviceQuotaValue;
        private int clusterId;	

		public ServiceInfo()
		{
		}

		public int ServiceId
		{
			get { return serviceId; }
			set { serviceId = value; }
		}

		public int ServerId
		{
			get { return serverId; }
			set { serverId = value; }
		}

		public int ProviderId
		{
			get { return providerId; }
			set { providerId = value; }
		}

		public string ServiceName
		{
			get { return serviceName; }
			set { serviceName = value; }
		}

		public string Comments
		{
			get { return comments; }
			set { comments = value; }
		}

		public int ServiceQuotaValue
		{
			get { return serviceQuotaValue; }
			set { serviceQuotaValue = value; }
		}

        public int ClusterId
        {
            get { return clusterId; }
            set { clusterId = value; }
        }

        public string ServerName { get; set; }
	}
}
