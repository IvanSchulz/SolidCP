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
using FuseCP.EnterpriseServer;

namespace FuseCP.Import.Enterprise
{
	public class Controller : ControllerBase
	{
		public Controller(ControllerBase provider) : base(provider) { }
		public Controller() : base((DataProvider)null) { }

		public new SecurityContext SecurityContext => base.SecurityContext;
		public new UserController UserController => base.UserController;
		public new PackageController PackageController => base.PackageController;
		public new ServerController ServerController => base.ServerController;
		public new OrganizationController OrganizationController => base.OrganizationController;
		public new ExchangeServerController ExchangeServerController => base.ExchangeServerController;

		OrganizationImporter organizationImporter = null;
		public OrganizationImporter OrganizationImporter => organizationImporter ??= new OrganizationImporter(this);
	}
}
