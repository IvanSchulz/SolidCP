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
