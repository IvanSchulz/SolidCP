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
using FuseCP.EnterpriseServer;

namespace FuseCP.Portal
{
	public class WebPortalPageTitleProvider : FuseCP.WebPortal.PageTitleProvider
	{
		public override string ProcessPageTitle(string title)
		{
			string result = title;

			// user
			if (title.IndexOf("{user}") != -1)
			{
				UserInfo user = PanelSecurity.SelectedUser;
				if (user != null)
					result = result.Replace("{user}", user.Username);
			}

			// space
			if (title.IndexOf("{space}") != -1)
			{
				if (PanelSecurity.PackageId > 0)
				{
					// space
					PackageInfo package = ES.Services.Packages.GetPackage(PanelSecurity.PackageId);
					if (package != null)
						result = result.Replace("{space}", package.PackageName);
				}
			}

			return result;
		}
	}
}
