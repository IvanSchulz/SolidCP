using System;
using FuseCP.EnterpriseServer.Base.HostedSolution;
using FuseCP.WebDav.Core.Security.Authentication.Principals;

namespace FuseCP.WebDav.Core.Interfaces.Managers
{
    public interface IAccessTokenManager
    {
        WebDavAccessToken CreateToken(ScpPrincipal principal, string filePath);
        WebDavAccessToken GetToken(int id);
        WebDavAccessToken GetToken(Guid guid);
        void ClearExpiredTokens();
    }
}
