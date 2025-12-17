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
using System.Web;

namespace FuseCP.Portal
{
	public class DbHelper
	{

		static EnterpriseServer.Data.DbType? dbType = null;
		public static EnterpriseServer.Data.DbType DbType => dbType ??= ES.Services.System.GetDatabaseType();

		static bool? useEntityFramework = null;
		public static bool UseEntityFramework
		{
			get
			{
				return useEntityFramework ??= ES.Services.System.GetUseEntityFramework();
            }
			set
			{
				ES.Services.System.SetUseEntityFramework(value);
				useEntityFramework = value;
			}
		}

		public static int SetUseEntityFramework(bool useEntityFramework)
		{
			var result = ES.Services.System.SetUseEntityFramework(useEntityFramework);
			DbHelper.useEntityFramework = useEntityFramework;
			if (useEntityFramework) showUseEntityFrameworkCheckbox = true;
			return result;
		}

		static bool? showUseEntityFrameworkCheckbox = null;
		public static bool ShowUseEntityFrameworkCheckbox => PortalUtils.AuthTicket != null && PortalUtils.AuthTicket.Name == "serveradmin" && (showUseEntityFrameworkCheckbox ??=
			DbType == EnterpriseServer.Data.DbType.SqlServer && UseEntityFramework);
	}
}
