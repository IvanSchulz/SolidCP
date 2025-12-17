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
    using Microsoft.Web.Administration;
	using FuseCP.Providers.FTP.IIs100.Config;
    using System;
    using System.Reflection;

    internal class AuthorizationRuleCollection : ConfigurationElementCollectionBase<AuthorizationRule>
    {
        public AuthorizationRule Add(AuthorizationRuleAccessType accessType, string users, string roles, PermissionsFlags permissions)
        {
            AuthorizationRule element = base.CreateElement();
            element.AccessType = accessType;
            if (!string.IsNullOrEmpty(users))
            {
                element.Users = users;
            }
            if (!string.IsNullOrEmpty(roles))
            {
                element.Roles = roles;
            }
            element.Permissions = permissions;
            return base.Add(element);
        }

        protected override AuthorizationRule CreateNewElement(string elementTagName)
        {
            return new AuthorizationRule();
        }

        public AuthorizationRule this[string users, string roles, PermissionsFlags permissions]
        {
            get
            {
                for (int i = 0; i < base.Count; i++)
                {
                    AuthorizationRule rule = base[i];
                    if ((string.Equals(rule.Users, users, StringComparison.OrdinalIgnoreCase) && string.Equals(rule.Roles, roles, StringComparison.OrdinalIgnoreCase)) && (rule.Permissions == permissions))
                    {
                        return rule;
                    }
                }
                return null;
            }
        }
    }
}

