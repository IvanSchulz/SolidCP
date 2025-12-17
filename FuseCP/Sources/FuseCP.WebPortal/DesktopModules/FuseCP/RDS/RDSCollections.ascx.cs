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
using System.Collections.Specialized;
using System.Linq;
using System.Web.UI.WebControls;
using FuseCP.EnterpriseServer;
using FuseCP.EnterpriseServer.Base.HostedSolution;
using FuseCP.EnterpriseServer.Base.RDS;
using FuseCP.Providers.Common;
using FuseCP.Providers.HostedSolution;
using FuseCP.Providers.OS;
using FuseCP.Providers.RemoteDesktopServices;
using FuseCP.WebPortal;

namespace FuseCP.Portal.RDS
{
    public partial class RDSCollections : FuseCPModuleBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            PackageContext cntx = PackagesHelper.GetCachedPackageContext(PanelSecurity.PackageId);

            if (!IsPostBack)
            {
                BindQuota(cntx);
            }
            
            if (cntx.Quotas.ContainsKey(Quotas.RDS_COLLECTIONS))
            {
                btnAddCollection.Enabled = (!(cntx.Quotas[Quotas.RDS_COLLECTIONS].QuotaAllocatedValue <= gvRDSCollections.Rows.Count) || (cntx.Quotas[Quotas.RDS_COLLECTIONS].QuotaAllocatedValue == -1));
            }

            var serviceId = ES.Services.RDS.GetRemoteDesktopServiceId(PanelRequest.ItemID);
            var settings = ConvertArrayToDictionary(ES.Services.Servers.GetServiceSettingsRDS(serviceId));
            
            var allowImport = Convert.ToBoolean(settings[RdsServerSettings.ALLOWCOLLECTIONSIMPORT]);

            if (!allowImport)
            {
                btnImportCollection.Visible = false;
            }
        }



        private void BindQuota(PackageContext cntx)
        {            
            OrganizationStatistics stats = ES.Services.Organizations.GetOrganizationStatisticsByOrganization(PanelRequest.ItemID);
            collectionsQuota.QuotaUsedValue = stats.CreatedRdsCollections;
            collectionsQuota.QuotaValue = stats.AllocatedRdsCollections;

            if (stats.AllocatedUsers != -1)
            {
                collectionsQuota.QuotaAvailable = stats.AllocatedRdsCollections - stats.CreatedRdsCollections;
            }            
        }

        public string GetServerName(string collectionId)
        {
            int id = int.Parse(collectionId);

            RdsServer[] servers =  ES.Services.RDS.GetCollectionRdsServers(id);

            return servers.FirstOrDefault().FqdName;
        }

        protected void btnAddCollection_Click(object sender, EventArgs e)
        {
            Response.Redirect(EditUrl("ItemID", PanelRequest.ItemID.ToString(), "rds_create_collection",
                "SpaceID=" + PanelSecurity.PackageId));
        }

        protected void btnImportCollection_Click(object sender, EventArgs e)
        {
            Response.Redirect(EditUrl("ItemID", PanelRequest.ItemID.ToString(), "rds_import_collection",
                "SpaceID=" + PanelSecurity.PackageId));
        }

        protected void gvRDSCollections_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "DeleteItem")
            {
                // delete RDS Collection
                int rdsCollectionId = int.Parse(e.CommandArgument.ToString());

                try
                {
                    RdsCollection collection = ES.Services.RDS.GetRdsCollection(rdsCollectionId, false);

                    ResultObject result = ES.Services.RDS.RemoveRdsCollection(PanelRequest.ItemID, collection);

                    if (!result.IsSuccess)
                    {
                        messageBox.ShowMessage(result, "REMOTE_DESKTOP_SERVICES_REMOVE_COLLECTION", "RDS");
                        return;
                    }

                    gvRDSCollections.DataBind();

                }
                catch (Exception ex)
                {
                    ShowErrorMessage("REMOTE_DESKTOP_SERVICES_REMOVE_COLLECTION", ex);
                }
            }
            else if (e.CommandName == "EditCollection")
            {
                Response.Redirect(GetCollectionEditUrl(e.CommandArgument.ToString()));
            }
        }

        protected void ddlPageSize_SelectedIndexChanged(object sender, EventArgs e)
        {
            gvRDSCollections.PageSize = Convert.ToInt32(ddlPageSize.SelectedValue);

            gvRDSCollections.DataBind();
        }

        public string GetCollectionEditUrl(string collectionId)
        {
            return EditUrl("SpaceID", PanelSecurity.PackageId.ToString(), "rds_edit_collection", "CollectionId=" + collectionId, "ItemID=" + PanelRequest.ItemID);
        }

        private StringDictionary ConvertArrayToDictionary(string[] settings)
        {
            StringDictionary r = new StringDictionary();
            foreach (string setting in settings)
            {
                int idx = setting.IndexOf('=');
                r.Add(setting.Substring(0, idx), setting.Substring(idx + 1));
            }
            return r;
        }
    }
}
