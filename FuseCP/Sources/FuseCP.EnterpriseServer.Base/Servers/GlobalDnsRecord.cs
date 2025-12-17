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
    public class GlobalDnsRecord
    {
        private int recordId;
        private string internalIP;
        private string externalIP;
        private int recordOrder;
        private int groupId;
        private int serviceId;
        private int serverId;
        private int packageId;
        private string recordType;
        private string recordName;
        private string recordData;
        private int recordTTL;
        private int mxPriority;
        private int ipAddressId;
        private int srvPriority;
        private int srvWeight;
        private int srvPort;


        public int RecordId
        {
            get { return recordId; }
            set { recordId = value; }
        }


        public int RecordOrder
        {
            get { return recordOrder; }
            set { recordOrder = value; }
        }

        public int GroupId
        {
            get { return groupId; }
            set { groupId = value; }
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

        public int PackageId
        {
            get { return packageId; }
            set { packageId = value; }
        }

        public string RecordType
        {
            get { return recordType; }
            set { recordType = value; }
        }

        public string RecordName
        {
            get { return recordName; }
            set { recordName = value; }
        }

        public string RecordData
        {
            get { return recordData; }
            set { recordData = value; }
        }

        public int RecordTTL
        {
            get { return recordTTL; }
            set { recordTTL = value; }
        }

        public int MxPriority
        {
            get { return mxPriority; }
            set { mxPriority = value; }
        }


        public int IpAddressId
        {
            get { return ipAddressId; }
            set { ipAddressId = value; }
        }

        public GlobalDnsRecord()
        {
        }

        public string InternalIP
        {
            get { return this.internalIP; }
            set { this.internalIP = value; }
        }

        public string ExternalIP
        {
            get { return this.externalIP; }
            set { this.externalIP = value; }
        }


        public int SrvPriority
        {
            get { return srvPriority; }
            set { srvPriority = value; }
        }

        public int SrvWeight
        {
            get { return srvWeight; }
            set { srvWeight = value; }
        }

        public int SrvPort
        {
            get { return srvPort; }
            set { srvPort = value; }
        }
    }
}
