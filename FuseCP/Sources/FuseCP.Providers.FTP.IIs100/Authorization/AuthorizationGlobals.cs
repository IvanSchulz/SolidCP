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

namespace FuseCP.Providers.FTP.IIs100.Authorization
{
    using System;

    public static class AuthorizationGlobals
    {
        public const int AccessType = 0;
        public const string AuthorizationRuleAlreadyExistsError = "AuthorizationRuleAlreadyExistsError";
        public const int AuthorizationRuleCollection = 0;
        public const string AuthorizationRuleDoesNotExistError = "AuthorizationRuleDoesNotExistError";
        public const string AuthorizationSectionName = "system.ftpServer/security/authorization";
        public const string ConfigurationError = "ConfigurationError";
        public const int Permissions = 3;
        public const int ReadOnly = 1;
        public const int Roles = 1;
        public const int Users = 2;
    }
}

