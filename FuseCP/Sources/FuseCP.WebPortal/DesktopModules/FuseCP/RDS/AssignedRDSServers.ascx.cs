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
using FuseCP.EnterpriseServer.Base.HostedSolution;
using FuseCP.Providers.Common;
using FuseCP.Providers.HostedSolution;
using FuseCP.Providers.OS;
using FuseCP.Providers.RemoteDesktopServices;
using FuseCP.WebPortal;

namespace FuseCP.Portal.RDS
{
    public partial class AssignedRDSServers : FuseCPModuleBase
    {
        public bool VisableDeleteServer;

        protected void Page_Load(object sender, EventArgs e)
        {
            PackageContext cntx = PackagesHelper.GetCachedPackageContext(PanelSecurity.PackageId);

            if (!IsPostBack)
            {
                BindQuota(cntx);
            }
            
            if (cntx.Quotas.ContainsKey(Quotas.RDS_SERVERS))
            {
                //If logged in person is not Administrator
                if (PanelSecurity.LoggedUser.Role != UserRole.Administrator )
                {
                    // Check if User is allowed to add server
                    if (!Utils.CheckQouta("RDS.DisableUserAddServer", cntx))
                    {
                        btnAddServerToOrg.Enabled = (!(cntx.Quotas[Quotas.RDS_SERVERS].QuotaAllocatedValue <= gvRDSAssignedServers.Rows.Count) || (cntx.Quotas[Quotas.RDS_SERVERS].QuotaAllocatedValue == -1));
                    }
                    else
                    {
                        btnAddServerToOrg.Enabled = false;
                    }

                    // Check if User is allowed to add server
                    if (!Utils.CheckQouta("RDS.DisableUserDeleteServer", cntx))
                    {
                        VisableDeleteServer = true;
                    }
                    else
                    {
                        VisableDeleteServer = false;
                    }
                }
                else
                {
                    btnAddServerToOrg.Enabled = (!(cntx.Quotas[Quotas.RDS_SERVERS].QuotaAllocatedValue <= gvRDSAssignedServers.Rows.Count) || (cntx.Quotas[Quotas.RDS_SERVERS].QuotaAllocatedValue == -1));
                    VisableDeleteServer = true;
                }
            }


        }

        private void BindQuota(PackageContext cntx)
        {
            OrganizationStatistics stats = ES.Services.Organizations.GetOrganizationStatisticsByOrganization(PanelRequest.ItemID);
            rdsServersQuota.QuotaUsedValue = stats.CreatedRdsServers;
            rdsServersQuota.QuotaValue = stats.AllocatedRdsServers;

            if (stats.AllocatedUsers != -1)
            {
                rdsServersQuota.QuotaAvailable = stats.AllocatedRdsServers - stats.CreatedRdsServers;
            }
        }

        protected void btnAddServerToOrg_Click(object sender, EventArgs e)
        {
            Response.Redirect(EditUrl("ItemID", PanelRequest.ItemID.ToString(), "rds_add_server",
                "SpaceID=" + PanelSecurity.PackageId));
        }

        protected void gvRDSAssignedServers_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int rdsServerId = int.Parse(e.CommandArgument.ToString());


            RdsServer rdsServer = ES.Services.RDS.GetRdsServer(rdsServerId);

            switch (e.CommandName)
            {
                case "DeleteItem":
                    //If logged in person is not Administrator
                    if (PanelSecurity.LoggedUser.Role != UserRole.Administrator)
                    {
                        PackageContext cntx = PackagesHelper.GetCachedPackageContext(PanelSecurity.PackageId);
                        if (!Utils.CheckQouta("RDS.DisableUserDeleteServer", cntx))
                        {
                            if (rdsServer.RdsCollectionId != null)
                            {
                                messageBox.ShowErrorMessage("RDS_UNASSIGN_SERVER_FROM_ORG_SERVER_IS_IN_COLLECTION");
                                return;
                            }
                            DeleteItem(rdsServerId);
                        }
                        else
                        {
                            messageBox.ShowErrorMessage("RDS_YOU_DO_NOT_HAVE_PERMISSION_TO_PREFORM_ACTION");
                        }
                    }
                    else
                    {
                        DeleteItem(rdsServerId);
                    }
                    break;
                case "EnableItem":
                    ChangeConnectionState("yes", rdsServer);
                    break;
                case "DisableItem":
                    ChangeConnectionState("no", rdsServer);
                    break;
            }

            gvRDSAssignedServers.DataBind();
        }

        protected void ddlPageSize_SelectedIndexChanged(object sender, EventArgs e)
        {
            gvRDSAssignedServers.PageSize = Convert.ToInt32(ddlPageSize.SelectedValue);

            gvRDSAssignedServers.DataBind();
        }        

        #region Methods

        private void DeleteItem(int rdsServerId)
        {
            ResultObject result = ES.Services.RDS.RemoveRdsServerFromOrganization(PanelRequest.ItemID, rdsServerId);

            if (!result.IsSuccess)
            {
                messageBox.ShowMessage(result, "REMOTE_DESKTOP_SERVICES_UNASSIGN_SERVER_FROM_ORG", "RDS");
                return;
            }
        }

        private void ChangeConnectionState(string enabled, RdsServer rdsServer)
        {
            ES.Services.RDS.SetRDServerNewConnectionAllowed(rdsServer.ItemId.Value, enabled, rdsServer.Id);
        }

        #endregion
    }
}
