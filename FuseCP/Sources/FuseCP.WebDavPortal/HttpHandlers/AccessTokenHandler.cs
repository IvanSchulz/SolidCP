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
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using FuseCP.WebDav.Core.Interfaces.Managers;
using FuseCP.WebDav.Core.Interfaces.Security;
using FuseCP.WebDav.Core.Security.Cryptography;
using FuseCP.WebDav.Core.Scp.Framework;

namespace FuseCP.WebDavPortal.HttpHandlers
{
    public class AccessTokenHandler : DelegatingHandler
    {
        private const string Bearer = "Bearer ";

        protected override async Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request, CancellationToken cancellationToken)
        {
            if (request.Headers.Contains("Authorization"))
            {
                var tokenString = request.Headers.GetValues("Authorization").First();
                if (!string.IsNullOrEmpty(tokenString) && tokenString.StartsWith(Bearer))
                {
                    try
                    {
                        var accessToken = tokenString.Substring(Bearer.Length - 1);

                        var tokenManager = DependencyResolver.Current.GetService<IAccessTokenManager>();

                        var guid = Guid.Parse(accessToken);
                        tokenManager.ClearExpiredTokens();

                        var token = tokenManager.GetToken(guid);

                        if (token != null)
                        {
                            var authenticationService = DependencyResolver.Current.GetService<IAuthenticationService>();
                            var cryptography = DependencyResolver.Current.GetService<ICryptography>();


                            var user = FCP.Services.ExchangeServer.GetAccount(token.ItemId, token.AccountId);

                            authenticationService.LogIn(user.UserPrincipalName, cryptography.Decrypt(token.AuthData));
                        }
                    }
                    catch (Exception)
                    {
                    }
                }
            }

            return await
                base.SendAsync(request, cancellationToken);
        }
    }
}
