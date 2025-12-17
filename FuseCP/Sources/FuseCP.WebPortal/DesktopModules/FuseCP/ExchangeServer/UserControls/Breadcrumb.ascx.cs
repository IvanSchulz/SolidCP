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
using FuseCP.Providers.HostedSolution;

namespace FuseCP.Portal.ExchangeServer.UserControls
{
	public partial class Breadcrumb : FuseCPControlBase
	{
		private string pageName;
		public string PageName
		{
			get { return pageName; }
			set { pageName = value; }
		}

		protected void Page_Load(object sender, EventArgs e)
		{
			// home element
			lnkHome.NavigateUrl = HostModule.NavigateURL(PortalUtils.SPACE_ID_PARAM, PanelSecurity.PackageId.ToString());

			// organization element
			bool orgVisible = (PanelRequest.ItemID > 0);
			spanOrg.Visible = orgVisible;
			if (orgVisible)
			{
				// load organization details
				Organization org = ES.Services.Organizations.GetOrganization(PanelRequest.ItemID);

				lnkOrg.NavigateUrl = HostModule.EditUrl(
					"ItemID", PanelRequest.ItemID.ToString(), "organization_home",
					"SpaceID=" + PanelSecurity.PackageId.ToString());
				lnkOrg.Text = org.Name;
			}

			// page name
			string localizedPageName = HostModule.GetLocalizedString(pageName);
			litPage.Text = localizedPageName != null ? localizedPageName : pageName;
		}
	}
}
