using System.Collections.Generic;
using FuseCP.Providers.HostedSolution;
using FuseCP.WebDav.Core.Security.Authentication.Principals;
using FuseCP.WebDav.Core.Security.Authorization.Enums;

namespace FuseCP.WebDav.Core.Interfaces.Security
{
    public interface IWebDavAuthorizationService
    {
        bool HasAccess(ScpPrincipal principal, string path);
        WebDavPermissions GetPermissions(ScpPrincipal principal, string path);
        IEnumerable<ExchangeAccount> GetUserSecurityGroups(ScpPrincipal principal);
    }
}
