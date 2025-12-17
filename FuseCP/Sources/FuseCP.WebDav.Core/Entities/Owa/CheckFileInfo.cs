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

using System.Runtime.Serialization;

namespace FuseCP.WebDav.Core.Entities.Owa
{
    [DataContract]
    public class CheckFileInfo
    {
        [DataMember]
        public string BaseFileName { get; set; }
        [DataMember]
        public string OwnerId { get; set; }
        [DataMember]
        public long Size { get; set; }
        [DataMember]
        public string Version { get; set; }
        [DataMember]
        public bool SupportsCoauth { get; set; }
        [DataMember]
        public bool SupportsCobalt { get; set; }
        [DataMember]
        public bool SupportsFolders { get; set; }
        [DataMember]
        public bool SupportsLocks { get; set; }
        [DataMember]
        public bool SupportsScenarioLinks { get; set; }
        [DataMember]
        public bool SupportsSecureStore { get; set; }
        [DataMember]
        public bool SupportsUpdate { get; set; }
        [DataMember]
        public bool UserCanWrite { get; set; }
        [DataMember]
        public string DownloadUrl { get; set; }
        [DataMember]
        public bool ReadOnly { get; set; }
        [DataMember]
        public bool RestrictedWebViewOnly { get; set; }
        [DataMember]
        public string ClientUrl { get; set; }
        [DataMember]
        public bool CloseButtonClosesWindow { get; set; }
        //[DataMember]
        //public string CloseUrl { get; set; }
        //[DataMember]
        //public bool UserCanNotWriteRelative { get; set; }
        

        //[DataMember]
        //public string SHA256 { get; set; }
        //[DataMember]
        //public bool AllowExternalMarketplace { get; set; }
        //[DataMember]
        //public string BreadcrumbBrandName { get; set; }
        //[DataMember]
        //public string BreadcrumbBrandUrl { get; set; }
        //[DataMember]
        //public string BreadcrumbDocName { get; set; }
        //[DataMember]
        //public string BreadcrumbDocUrl { get; set; }
        //[DataMember]
        //public string BreadcrumbFolderName { get; set; }
        //[DataMember]
        //public string BreadcrumbFolderUrl { get; set; }
        //[DataMember]
        //public string ClientUrl { get; set; }
    
        //[DataMember]
        //public string CloseUrl { get; set; }
        //[DataMember]
        //public bool DisableBrowserCachingOfUserContent { get; set; }
        //[DataMember]
        //public bool DisablePrint { get; set; }
        //[DataMember]
        //public bool DisableTranslation { get; set; }
        //[DataMember]
        //public string FileSharingUrl { get; set; }
        //[DataMember]
        //public string FileUrl { get; set; }
        //[DataMember]
        //public string HostAuthenticationId { get; set; }
        //[DataMember]
        //public string HostEditUrl { get; set; }
        //[DataMember]
        //public string HostEmbeddedEditUrl { get; set; }
        //[DataMember]
        //public string HostEmbeddedViewUrl { get; set; }
        //[DataMember]
        //public string HostName { get; set; }
        //[DataMember]
        //public string HostNotes { get; set; }
        //[DataMember]
        //public string HostRestUrl { get; set; }
        //[DataMember]
        //public string HostViewUrl { get; set; }
        //[DataMember]
        //public string IrmPolicyDescription { get; set; }
        //[DataMember]
        //public string IrmPolicyTitle { get; set; }

        //[DataMember]
        //public string PresenceProvider { get; set; }
        //[DataMember]
        //public string PresenceUserId { get; set; }
        //[DataMember]
        //public string PrivacyUrl { get; set; }
        //[DataMember]
        //public bool ProtectInClient { get; set; }
        //[DataMember]
        //public bool ReadOnly { get; set; }


        //[DataMember]
        //public string SignoutUrl { get; set; }


        //[DataMember]
        //public string TenantId { get; set; }
        //[DataMember]
        //public string TermsOfUseUrl { get; set; }
        //[DataMember]
        //public string TimeZone { get; set; }
        //[DataMember]
        //public bool UserCanAttend { get; set; }

        //[DataMember]
        //public bool UserCanPresent { get; set; }
        //[DataMember]
        //public bool UserCanWrite { get; set; }
        //[DataMember]
        //public string UserFriendlyName { get; set; }
        //[DataMember]
        //public string UserId { get; set; }

        //[DataMember]
        //public bool WebEditingDisabled { get; set; }
    }
}
