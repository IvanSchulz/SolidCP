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
using System.Web.UI.WebControls;
using FuseCP.EnterpriseServer;
using FuseCP.Providers.ResultObjects;
using FuseCP.Providers.HostedSolution;

namespace FuseCP.Portal.SfB
{
	public partial class SfBFederationDomains : FuseCPModuleBase
	{
		protected void Page_Load(object sender, EventArgs e)
		{
			if (!IsPostBack)
			{
                // bind domain names
                BindDomainNames();
			}

		}
        
        private void BindDomainNames()
        {
            SfBFederationDomain[] list = ES.Services.SfB.GetFederationDomains(PanelRequest.ItemID);

            gvDomains.DataSource = list;
            gvDomains.DataBind();

        }

        protected void btnAddDomain_Click(object sender, EventArgs e)
        {
            Response.Redirect(EditUrl("ItemID", PanelRequest.ItemID.ToString(), "add_sfbfederation_domain",
                "SpaceID=" + PanelSecurity.PackageId));
        }

        protected void gvDomains_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "DeleteItem")
            {
                try
                {
                    SfBUserResult res = ES.Services.SfB.RemoveFederationDomain(PanelRequest.ItemID, e.CommandArgument.ToString());
                    if (!(res.IsSuccess && res.ErrorCodes.Count == 0))
                    {
                        messageBox.ShowMessage(res, "DELETE_SFB_FEDERATIONDOMAIN", "SFB");
                        return;
                    }

                    // rebind domains
                    BindDomainNames();
                }
                catch (Exception ex)
                {
                    ShowErrorMessage("SFB_DELETE_DOMAIN", ex);
                }
            }
        }
	}
}
