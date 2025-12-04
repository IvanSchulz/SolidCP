using System.Collections.Generic;
using FuseCP.WebDav.Core.Client;
using FuseCP.WebDav.Core.Entities.Account;
using FuseCP.WebDav.Core.Security.Authorization.Enums;
using FuseCP.WebDavPortal.Models.Common;

namespace FuseCP.WebDavPortal.Models
{
    public class ModelForWebDav 
    {
        public IEnumerable<IHierarchyItem> Items { get; set; }
        public string UrlSuffix { get; set; }
        public string Error { get; set; }
        public string SearchValue { get; set; }
        public WebDavPermissions Permissions { get; set; }
        public UserPortalSettings UserSettings { get; set; }
    }
}
