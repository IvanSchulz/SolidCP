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
using System.DirectoryServices.AccountManagement;
using System.Threading;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.Security;
using FuseCP.EnterpriseServer.Base.HostedSolution;
using FuseCP.Server.Utils;
using FuseCP.WebDav.Core.Config;
using FuseCP.WebDav.Core.Interfaces.Security;
using FuseCP.WebDav.Core.Security.Authentication.Principals;
using FuseCP.WebDav.Core.Security.Cryptography;
using FuseCP.WebDav.Core.Scp.Framework;

namespace FuseCP.WebDav.Core.Security.Authentication
{
    public class FormsAuthenticationService : IAuthenticationService
    {
        private readonly ICryptography _cryptography;
        private readonly PrincipalContext _principalContext;

        public FormsAuthenticationService(ICryptography cryptography)
        {
            Log.WriteStart("FormsAuthenticationService");

            _cryptography = cryptography;

            try
            {
                _principalContext = new PrincipalContext(ContextType.Domain, WebDavAppConfigManager.Instance.UserDomain);
            }
            catch (Exception ex)
            {

                Log.WriteError(ex);
            }
            

            Log.WriteEnd("FormsAuthenticationService");
        }

        public ScpPrincipal LogIn(string login, string password)
        {
            Log.WriteStart("Login");

            if (ValidateAuthenticationData(login, password) == false)
            {
                return null;
            }

            var principal = new ScpPrincipal(login);
            
            var exchangeAccount = FCP.Services.ExchangeServer.GetAccountByAccountNameWithoutItemId(login);
            var organization = FCP.Services.Organizations.GetOrganization(exchangeAccount.ItemId);

            principal.AccountId = exchangeAccount.AccountId;
            principal.ItemId = exchangeAccount.ItemId;
            principal.OrganizationId = organization.OrganizationId;
            principal.DisplayName = exchangeAccount.DisplayName;
            principal.AccountName = exchangeAccount.AccountName;
            principal.EncryptedPassword = _cryptography.Encrypt(password);

            if (HttpContext.Current != null)
            {
                HttpContext.Current.User = principal;
            }

            Thread.CurrentPrincipal = principal;

            Log.WriteEnd("Login");

            return principal;
        }

        public void CreateAuthenticationTicket(ScpPrincipal principal)
        {
            var serializer = new JavaScriptSerializer();
            string userData = serializer.Serialize(principal);

            var authTicket = new FormsAuthenticationTicket(1, principal.Identity.Name, DateTime.Now, DateTime.Now.Add(FormsAuthentication.Timeout),
                FormsAuthentication.SlidingExpiration, userData);

            var encTicket = FormsAuthentication.Encrypt(authTicket);

            var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, encTicket);

            if (FormsAuthentication.SlidingExpiration)
            {
                cookie.Expires = authTicket.Expiration;
            }

            HttpContext.Current.Response.Cookies.Add(cookie);
        }

        public void LogOut()
        {
            FormsAuthentication.SignOut();
        }

        public bool ValidateAuthenticationData(string login, string password)
        {
            Log.WriteStart("ValidateAuthenticationData");

            if (string.IsNullOrEmpty(login) || string.IsNullOrEmpty(password))
            {
                return false;
            }

            var user = UserPrincipal.FindByIdentity(_principalContext, IdentityType.UserPrincipalName, login);

            if (user == null || _principalContext.ValidateCredentials(login, password) == false)
            {
                return false;
            }

            Log.WriteEnd("ValidateAuthenticationData");

            return true;
        }
    }
}
