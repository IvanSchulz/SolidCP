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
using System.Text;
using System.Threading.Tasks;

namespace FuseCP.Web.Services
{
	public enum Policies { Encrypted, ServerAuthenticated, EnterpriseServerAuthenticated }
	public class PolicyAttribute : Attribute
	{
		public const string Encrypted = "CommonPolicy";
		public const string ServerAuthenticated = "ServerPolicy";
		public const string EnterpriseServerAuthenticated = "EnterpriseServerPolicy";

		public const bool AllowInsecureHttp = true;
		public const bool UseMessageSecurityOverHttp = true;
		public string Policy { get; set; }
		public PolicyAttribute(string policy) { Policy = policy; }
		public PolicyAttribute(Policies policy)
		{
			switch (policy)
			{
				case Policies.Encrypted: Policy = Encrypted; break;
				case Policies.ServerAuthenticated: Policy = ServerAuthenticated; break;
				case Policies.EnterpriseServerAuthenticated: Policy = EnterpriseServerAuthenticated; break;
			}
		}
	}

}
