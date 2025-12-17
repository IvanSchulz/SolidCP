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
using FuseCP.Providers.RemoteDesktopServices;

namespace FuseCP.Portal.RDS
{
    public partial class RDSCreateCollection : FuseCPModuleBase
    {
        
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                servers.HideRefreshButton();
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid)
            {
                return;
            }

            try
            {
                if (servers.GetServers().Count < 1)
                {
                    messageBox.ShowErrorMessage("RDS_CREATE_COLLECTION_RDSSERVER_REQUAIRED");
                    return;
                }

                RdsCollection collection = new RdsCollection{ Name = txtCollectionName.Text, DisplayName = txtCollectionName.Text, Servers = servers.GetServers(), Description = "" };
                int collectionId = ES.Services.RDS.AddRdsCollection(PanelRequest.ItemID, collection);                

                Response.Redirect(EditUrl("SpaceID", PanelSecurity.PackageId.ToString(), "rds_edit_collection", "CollectionId=" + collectionId, "ItemID=" + PanelRequest.ItemID));
            }
            catch (Exception ex)
            {
                ShowErrorMessage("RDSCOLLECTION_NOT_CREATED", ex);
            }
        }
    }
}
