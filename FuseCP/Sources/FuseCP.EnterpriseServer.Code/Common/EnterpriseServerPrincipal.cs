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
using System.Text;
using System.Security;
using System.Security.Principal;

namespace FuseCP.EnterpriseServer
{
    public class EnterpriseServerPrincipal : IPrincipal
    {
        private int userId;
        private int ownerId;
        private bool isPeer;
        private bool isDemo;
        private UserStatus status;

        private List<string> roles = new List<string>();
        private IIdentity identity;

        public EnterpriseServerPrincipal(IIdentity identity, string[] roles)
        {
            this.identity = identity;
            this.roles.AddRange(roles);
        }

        #region IPrincipal Members

        public IIdentity Identity
        {
            get { return identity; }
        }

        public bool IsInRole(string role)
        {
            return roles.Contains(role);
        }

        #endregion

        #region Public properties
        public int UserId
        {
            get { return this.userId; }
            set { this.userId = value; }
        }

        public int OwnerId
        {
            get { return this.ownerId; }
            set { this.ownerId = value; }
        }

        public bool IsPeer
        {
            get { return this.isPeer; }
            set { this.isPeer = value; }
        }

        public bool IsDemo
        {
            get { return this.isDemo; }
            set { this.isDemo = value; }
        }

        public UserStatus Status
        {
            get { return this.status; }
            set { this.status = value; }
        }
        #endregion
    }
}
