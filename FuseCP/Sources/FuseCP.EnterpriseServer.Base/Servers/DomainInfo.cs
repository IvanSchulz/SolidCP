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
using FuseCP.Providers;

namespace FuseCP.EnterpriseServer
{
    [Serializable]
    public class DomainInfo
    {
        private int domainId;
        private int packageId;
        private int zoneItemId;
        private int domainItemId;
        private string domainName;
        private bool hostingAllowed;
        private int webSiteId;
        private int mailDomainId;
        private string webSiteName;
        private string mailDomainName;
        private string zoneName;
        private bool isSubDomain;
        private bool isPreviewDomain;
        private bool isDomainPointer;
        private int previewDomainId;
        private string previewDomainName;
        //private int recorddefaultTTL;
        //private int zoneServiceID;
        //private int recordminimumTTL;
        //private int minimumTTL;

        [LogProperty]
        public int DomainId
        {
            get { return domainId; }
            set { domainId = value; }
        }

        public int PackageId
        {
            get { return packageId; }
            set { packageId = value; }
        }

        public int ZoneItemId
        {
            get { return zoneItemId; }
            set { zoneItemId = value; }
        }

        public int DomainItemId
        {
            get { return domainItemId; }
            set { domainItemId = value; }
        }

        [LogProperty]
        public string DomainName
        {
            get { return domainName; }
            set { domainName = value; }
        }

        public bool HostingAllowed
        {
            get { return hostingAllowed; }
            set { hostingAllowed = value; }
        }

        public int WebSiteId
        {
            get { return webSiteId; }
            set { webSiteId = value; }
        }

        public int MailDomainId
        {
            get { return mailDomainId; }
            set { mailDomainId = value; }
        }

        public string WebSiteName
        {
            get { return this.webSiteName; }
            set { this.webSiteName = value; }
        }

        public string MailDomainName
        {
            get { return this.mailDomainName; }
            set { this.mailDomainName = value; }
        }

        public string ZoneName
        {
            get { return this.zoneName; }
            set { this.zoneName = value; }
        }

        public bool IsSubDomain
        {
            get { return this.isSubDomain; }
            set { this.isSubDomain = value; }
        }

        public bool IsPreviewDomain
        {
            get { return this.isPreviewDomain; }
            set { this.isPreviewDomain = value; }
        }

        public bool IsDomainPointer
        {
            get { return this.isDomainPointer; }
            set { this.isDomainPointer = value; }
        }

        public int PreviewDomainId
        {
            get { return this.previewDomainId; }
            set { this.previewDomainId = value; }
        }

        public string PreviewDomainName
        {
            get { return this.previewDomainName; }
            set { this.previewDomainName = value; }
        }

        public DateTime? CreationDate { get; set; }
        public DateTime? ExpirationDate { get; set; }
        public DateTime? LastUpdateDate { get; set; }
        public string RegistrarName { get; set; }
        public int RecordDefaultTTL { get; set; }
        public int ZoneServiceID { get; set; }
        public int RecordMinimumTTL { get; set; }
        public int MinimumTTL { get; set; }
    }
}
