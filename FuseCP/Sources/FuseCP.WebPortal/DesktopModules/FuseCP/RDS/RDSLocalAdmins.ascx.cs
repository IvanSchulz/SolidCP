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
using FuseCP.Providers.HostedSolution;
using FuseCP.Providers.RemoteDesktopServices;

namespace FuseCP.Portal.RDS
{
    public partial class RdsLocalAdmins : FuseCPModuleBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            users.OnRefreshClicked -= OnRefreshClicked;
            users.OnRefreshClicked += OnRefreshClicked;
            users.Module = Module;

            if (!IsPostBack)
            {                
                var collectionLocalAdmins = ES.Services.RDS.GetRdsCollectionLocalAdmins(PanelRequest.CollectionID);
                var collection = ES.Services.RDS.GetRdsCollection(PanelRequest.CollectionID, true);

                litCollectionName.Text = collection.DisplayName;

                foreach(var user in collectionLocalAdmins)
                {
                    user.IsVIP = false;
                }

                users.SetUsers(collectionLocalAdmins);
            }
        }

        private void OnRefreshClicked(object sender, EventArgs e)
        {
            ((ModalPopupExtender)asyncTasks.FindControl("ModalPopupProperties")).Hide();            
        }

        private bool SaveLocalAdmins()
        {
            try
            {
                ES.Services.RDS.SaveRdsCollectionLocalAdmins(users.GetUsers(), PanelRequest.CollectionID);
            }
            catch (Exception ex)
            {
                ShowErrorMessage("RDSLOCALADMINS_NOT_ADDED", ex);
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

            SaveLocalAdmins();
        }

        protected void btnSaveExit_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid)
            {
                return;
            }

            if (SaveLocalAdmins())
            {
                Response.Redirect(EditUrl("ItemID", PanelRequest.ItemID.ToString(), "rds_collections", "SpaceID=" + PanelSecurity.PackageId));
            }
        }
    }
}
