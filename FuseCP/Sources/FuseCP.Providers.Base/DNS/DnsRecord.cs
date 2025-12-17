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

namespace FuseCP.Providers.DNS
{
    public class DnsRecord
    {
        private string recordName;
        private DnsRecordType recordType;
        private string recordData;
        private int mxPriority;
        private string recordText;
        private int srvPriority;
        private int srvWeight;
        private int srvPort;
        private int recordTTL;


        public string RecordName
        {
            get { return this.recordName; }
            set { this.recordName = value; }
        }

        public DnsRecordType RecordType
        {
            get { return this.recordType; }
            set { this.recordType = value; }
        }

        public string RecordData
        {
            get { return this.recordData; }
            set { this.recordData = value; }
        }

        public int RecordTTL
        {
            get { return this.recordTTL; }
            set { this.recordTTL = value; }
        }

        public int MxPriority
        {
            get { return this.mxPriority; }
            set { this.mxPriority = value; }
        }

        public string RecordText
        {
            get { return this.recordText; }
            set { this.recordText = value; }
        }


        public int SrvPriority
        {
            get { return this.srvPriority; }
            set { this.srvPriority = value; }
        }

        public int SrvWeight
        {
            get { return this.srvWeight; }
            set { this.srvWeight = value; }
        }

        public int SrvPort
        {
            get { return this.srvPort; }
            set { this.srvPort = value; }
        }

    }
}
