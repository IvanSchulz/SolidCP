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

namespace FuseCP.Providers.Web.Iis.Common
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Web;

    using Microsoft.Web.Administration;
    using Microsoft.Web.Management.Server;

	public abstract class ConfigurationModuleService
    {
        private const string ServerManagerContextKey = "ServerManagerContextKey";
		/// <summary>
		/// We'll use it in the future to implement management of web farm with shared configuration enabled
		/// </summary>
		/// <returns></returns>
		protected internal ServerManager GetServerManager()
		{
			return new ServerManager();
		}
    }
}
