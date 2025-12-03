using AutoMapper;
using FuseCP.WebDavPortal.Mapping.Profiles.Account;
using FuseCP.WebDavPortal.Mapping.Profiles.Webdav;

namespace FuseCP.WebDavPortal.Mapping
{
    public class AutoMapperPortalConfiguration
    {
        public static void Configure()
        {
            Mapper.Initialize(
                config =>
                {
                    config.AddProfile<UserProfileProfile>();
                    config.AddProfile<ResourceTableItemProfile>();
                });
        } 
    }
}
