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
    public partial class RDSEditCollectionApps : FuseCPModuleBase
    {        
        protected void Page_Load(object sender, EventArgs e)
        {
            remoreApps.Module = Module;

            if (!IsPostBack)
            {
                RdsCollection collection = ES.Services.RDS.GetRdsCollection(PanelRequest.CollectionID, true);
                var collectionApps = ES.Services.RDS.GetCollectionRemoteApplications(PanelRequest.ItemID, collection.Name);
                
                litCollectionName.Text = collection.DisplayName;
                remoreApps.SetApps(collectionApps);
            }            
        }

        private bool SaveRemoteApplications()
        {
            try
            {
                ES.Services.RDS.SetRemoteApplicationsToRdsCollection(PanelRequest.ItemID, PanelRequest.CollectionID, remoreApps.GetApps());
            }
            catch (Exception ex)
            {
                messageBox.ShowErrorMessage(ex.Message);
                return false;
            }

            return true;
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid)
            {
                return;
            }

            SaveRemoteApplications();
        }

        protected void btnSaveExit_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid)
            {
                return;
            }

            if (SaveRemoteApplications())
            {
                Response.Redirect(EditUrl("ItemID", PanelRequest.ItemID.ToString(), "rds_collections", "SpaceID=" + PanelSecurity.PackageId));
            }
        }
    }
}
