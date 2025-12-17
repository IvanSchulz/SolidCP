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
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Collections.Generic;
using FuseCP.EnterpriseServer.Base.HostedSolution;
using FuseCP.Providers.SharePoint;
using FuseCP.Providers.HostedSolution;

namespace FuseCP.Portal
{
	public partial class HostedSharePointSiteCollections :  FuseCPModuleBase
	{
 
		protected void Page_Load(object sender, EventArgs e)
		{
			this.BindStats();
		}

		private void BindStats()
		{
			// quota values
			OrganizationStatistics stats = ES.Services.Organizations.GetOrganizationStatisticsByOrganization(PanelRequest.ItemID);

			siteCollectionsQuota.QuotaUsedValue = stats.CreatedSharePointSiteCollections;
			siteCollectionsQuota.QuotaValue = stats.AllocatedSharePointSiteCollections;
            if (stats.AllocatedSharePointSiteCollections != -1) siteCollectionsQuota.QuotaAvailable = stats.AllocatedSharePointSiteCollections - stats.CreatedSharePointSiteCollections;
		}

		protected void btnCreateSiteCollection_Click(object sender, EventArgs e)
		{
			Response.Redirect(EditUrl("ItemID", PanelRequest.ItemID.ToString(), "sharepoint_edit_sitecollection", "SpaceID=" + PanelSecurity.PackageId.ToString()));
		}

		public string GetSiteCollectionEditUrl(string siteCollectionId)
		{
			return EditUrl("SpaceID", PanelSecurity.PackageId.ToString(), "sharepoint_edit_sitecollection",
					"SiteCollectionID=" + siteCollectionId,
					"ItemID=" + PanelRequest.ItemID.ToString());
		}

		protected void odsSharePointSiteCollectionPaged_Selected(object sender, ObjectDataSourceStatusEventArgs e)
		{
			if (e.Exception != null)
			{
				messageBox.ShowErrorMessage("HOSTEDSHAREPOINT_GET_SITECOLLECTIONS", e.Exception);
				e.ExceptionHandled = true;
			}
		}

		protected void gvSiteCollections_RowCommand(object sender, GridViewCommandEventArgs e)
		{
			if (e.CommandName == "DeleteItem")
			{
				int siteCollectionId = Utils.ParseInt(e.CommandArgument.ToString(), 0);

				try
				{
					int result = ES.Services.HostedSharePointServers.DeleteSiteCollection(siteCollectionId);
					if (result < 0)
					{
						messageBox.ShowResultMessage(result);
						return;
					}

					gvSiteCollections.DataBind();
					this.BindStats();
				}
				catch (Exception ex)
				{
					messageBox.ShowErrorMessage("HOSTEDSHAREPOINT_DELETE_SITECOLLECTION", ex);
				}
			}
		}
	}
}
