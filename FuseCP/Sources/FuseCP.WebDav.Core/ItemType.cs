using FuseCP.WebDav.Core.Attributes.Resources;
using FuseCP.WebDav.Core.Resources;

namespace FuseCP.WebDav.Core
{
    namespace Client
    {
        public enum ItemType
        {
            [LocalizedDescription(typeof(WebDavResources), "ItemTypeResource")]
            Resource,
            [LocalizedDescription(typeof(WebDavResources), "ItemTypeFolder")]
            Folder,
            Version,
            VersionHistory
        }
    }
}
