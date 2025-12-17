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
