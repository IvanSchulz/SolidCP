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

using AjaxControlToolkit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FuseCP.Providers.RemoteDesktopServices;

namespace FuseCP.Portal.RDS
{
    public partial class RDSEditCollection : FuseCPModuleBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            servers.Module = Module;
            servers.OnRefreshClicked -= OnRefreshClicked;
            servers.OnRefreshClicked += OnRefreshClicked;

            if (!Page.IsPostBack)
            {
                var collection = ES.Services.RDS.GetRdsCollection(PanelRequest.CollectionID, true);
                litCollectionName.Text = collection.DisplayName;                
            }
        }

        private void OnRefreshClicked(object sender, EventArgs e)
        {
            var rdsServers = (List<RdsServer>)sender;

            foreach (var rdsServer in rdsServers)
            {
                rdsServer.Status = ES.Services.RDS.GetRdsServerStatus(PanelRequest.ItemID, rdsServer.FqdName);
            }

            servers.BindServers(rdsServers.ToArray());
            ((ModalPopupExtender)asyncTasks.FindControl("ModalPopupProperties")).Hide();
        }

        private bool SaveRdsServers(bool exit = false)
        {
            try
            {
                if (servers.GetServers().Count < 1)
                {
                    messageBox.ShowErrorMessage("RDS_CREATE_COLLECTION_RDSSERVER_REQUAIRED");
                    return false;
                }

                RdsCollection collection = ES.Services.RDS.GetRdsCollection(PanelRequest.CollectionID, false);
                collection.Servers = servers.GetServers();

                ES.Services.RDS.EditRdsCollection(PanelRequest.ItemID, collection);

                if (!exit)
                {
                    foreach(var rdsServer in collection.Servers)
                    {
                        rdsServer.Status = ES.Services.RDS.GetRdsServerStatus(PanelRequest.ItemID, rdsServer.FqdName);
                    }

                    servers.BindServers(collection.Servers.ToArray());
                }
            }
            catch(Exception ex)
            {
                messageBox.ShowErrorMessage(ex.Message);
                return false;
            }

            return true;
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid)
                return;

            SaveRdsServers();
        }

        protected void btnSaveExit_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid)
                return;

            if (SaveRdsServers())
            {
                Response.Redirect(EditUrl("ItemID", PanelRequest.ItemID.ToString(), "rds_collections", "SpaceID=" + PanelSecurity.PackageId));
            }
        }
    }
}
