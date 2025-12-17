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
using System.Xml.Serialization;
using System.Runtime.Serialization;

namespace FuseCP.EnterpriseServer
{
    public class PackageContext
    {
        private PackageInfo package;
        private HostingPlanGroupInfo[] groupsArray;
        private QuotaValueInfo[] quotasArray;
        private Dictionary<string, HostingPlanGroupInfo> groups = new Dictionary<string, HostingPlanGroupInfo>();
        private Dictionary<string, QuotaValueInfo> quotas = new Dictionary<string, QuotaValueInfo>();

        public PackageInfo Package
        {
            get { return this.package; }
            set { this.package = value; }
        }

        public HostingPlanGroupInfo[] GroupsArray
        {
            get { return this.groupsArray; }
            set { this.groupsArray = value; }
        }

        public QuotaValueInfo[] QuotasArray
        {
            get { return this.quotasArray; }
            set { this.quotasArray = value; }
        }

        [XmlIgnore, IgnoreDataMember]
        public Dictionary<string, HostingPlanGroupInfo> Groups
        {
            get { return groups; }
        }

        [XmlIgnore, IgnoreDataMember]
        public Dictionary<string, QuotaValueInfo> Quotas
        {
            get { return quotas; }
        }
    }
}
