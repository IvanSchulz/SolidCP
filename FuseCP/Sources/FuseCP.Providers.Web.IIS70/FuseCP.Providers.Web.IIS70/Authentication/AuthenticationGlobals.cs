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

namespace FuseCP.Providers.Web.Iis.Authentication
{
    using System;

    internal static class AuthenticationGlobals
    {
        public const string ActiveDirectoryAuthenticationModuleName = "ActiveDirectoryAuthentication";
        public const string ActiveDirectoryAuthenticationSectionName = "system.webServer/security/authentication/clientCertificateMappingAuthentication";
        
        public const string AnonymousAuthenticationModuleName = "AnonymousAuthentication";
        public const string AnonymousAuthenticationSectionName = "system.webServer/security/authentication/anonymousAuthentication";
        
        public const string AuthenticationModuleName = "Authentication";
        public const string AuthenticationSectionName = "system.web/authentication";
        
        public const string BasicAuthenticationModuleName = "BasicAuthentication";
        public const string BasicAuthenticationSectionName = "system.webServer/security/authentication/basicAuthentication";
        public const string DigestAuthenticationModuleName = "DigestAuthentication";
        public const string DigestAuthenticationSectionName = "system.webServer/security/authentication/digestAuthentication";

        public const string PasswordProperty = "password";

        public const string WindowsAuthenticationModuleName = "WindowsAuthentication";
        public const string WindowsAuthenticationSectionName = "system.webServer/security/authentication/windowsAuthentication";


        public const int AnonymousAuthenticationUserName = 0;
        public const int AnonymousAuthenticationPassword = 1;

        public const int BasicAuthenticationDefaultLogonDomain = 2;
        public const int BasicAuthenticationRealm = 3;

        public const int DigestAuthenticationRealm = 4;
        public const int DigestAuthenticationHasDomain = 5;

        public const int FormsAuthenticationLoginPageUrl = 6;
        public const int FormsAuthenticationExpiration = 7;
        public const int FormsAuthenticationCookieMode = 8;
        public const int FormsAuthenticationCookieName = 9;
        public const int FormsAuthenticationProtectionMode = 10;
        public const int FormsAuthenticationRequireSsl = 11;
        public const int FormsAuthenticationSlidingExpiration = 12;

        public const int ActiveDirectoryAuthenticationHasSslRequirements = 13;

        public const int ModuleName = 14;

        public const int Enabled = 15;

        public const int IsLocked = 16;
    }
}

