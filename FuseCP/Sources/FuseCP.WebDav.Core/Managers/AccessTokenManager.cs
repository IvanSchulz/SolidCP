using System;
using FuseCP.EnterpriseServer.Base.HostedSolution;
using FuseCP.WebDav.Core.Interfaces.Managers;
using FuseCP.WebDav.Core.Security.Authentication.Principals;
using FuseCP.WebDav.Core.Scp.Framework;

namespace FuseCP.WebDav.Core.Managers
{
    public class AccessTokenManager : IAccessTokenManager
    {
        public WebDavAccessToken CreateToken(ScpPrincipal principal, string filePath)
        {
            var token = new WebDavAccessToken();

            token.AccessToken = Guid.NewGuid();
            token.AccountId = principal.AccountId;
            token.ItemId = principal.ItemId;
            token.AuthData = principal.EncryptedPassword;
            token.ExpirationDate = DateTime.Now.AddHours(3);
            token.FilePath = filePath;

            token.Id = FCP.Services.EnterpriseStorage.AddWebDavAccessToken(token);

            return token;
        }

        public WebDavAccessToken GetToken(int id)
        {
            return FCP.Services.EnterpriseStorage.GetWebDavAccessTokenById(id);
        }

        public WebDavAccessToken GetToken(Guid guid)
        {
            return FCP.Services.EnterpriseStorage.GetWebDavAccessTokenByAccessToken(guid);
        }

        public void ClearExpiredTokens()
        {
            FCP.Services.EnterpriseStorage.DeleteExpiredWebDavAccessTokens();
        }
    }
}
