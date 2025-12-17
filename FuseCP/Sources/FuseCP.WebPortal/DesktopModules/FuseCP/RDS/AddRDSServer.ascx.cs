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
using FuseCP.Providers.Common;
using FuseCP.Providers.HostedSolution;
using FuseCP.Providers.OS;

namespace FuseCP.Portal.RDS
{
    public partial class AddRDSServer : FuseCPModuleBase
    {
        
        protected void Page_Load(object sender, EventArgs e)
        {
            PackageContext cntx = PackagesHelper.GetCachedPackageContext(PanelSecurity.PackageId);
            if (PanelSecurity.LoggedUser.Role == UserRole.User)
            {
                if (!Utils.CheckQouta("RDS.DisableUserAddServer", cntx))
                {
                    if (!IsPostBack)
                    {
                        BindRDSServers();
                    }

                    btnAdd.Enabled = ddlServers.Items.Count > 0;
                }
                else
                {
                    btnAdd.Enabled = false;
                }
            }
            else
            {
                if (!IsPostBack)
                {
                    BindRDSServers();
                }

                btnAdd.Enabled = ddlServers.Items.Count > 0;
            }
        }

        private void BindRDSServers()
        {
            ddlServers.DataSource = new RDSHelper().GetFreeRDSServers(PanelRequest.ItemID, PanelSecurity.PackageId.ToString());
            ddlServers.DataTextField = "Name";
            ddlServers.DataValueField = "Id";
            ddlServers.DataBind();
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid)
                return;

            try
            {
                ES.Services.RDS.AddRdsServerToOrganization(PanelRequest.ItemID, int.Parse(ddlServers.SelectedValue));

                Response.Redirect(EditUrl("ItemID", PanelRequest.ItemID.ToString(), "rds_servers",
                                  "SpaceID=" + PanelSecurity.PackageId));
            }
            catch (Exception ex)
            {
                ShowErrorMessage("RDSSERVER_NOT_ASSIGNED", ex);
            }
        }
    }
}
