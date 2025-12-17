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
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FuseCP.Providers.HostedSolution;
using FuseCP.Providers.OS;

namespace FuseCP.Portal.ExchangeServer
{
    public partial class EnterpriseStorageFolderSettingsFolderPermissions : FuseCPModuleBase
    {
        #region Constants

        private const int OneGb = 1024;

        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (!ES.Services.EnterpriseStorage.CheckUsersDomainExists(PanelRequest.ItemID))
                {
                    Response.Redirect(EditUrl("SpaceID", PanelSecurity.PackageId.ToString(), "enterprisestorage_folders",
                        "ItemID=" + PanelRequest.ItemID));
                }

                BindSettings();
            }
        }

        private void BindSettings()
        {
            try
            {
                // get settings
                Organization org = ES.Services.Organizations.GetOrganization(PanelRequest.ItemID);

                SystemFile folder = ES.Services.EnterpriseStorage.GetEnterpriseFolder(
                    PanelRequest.ItemID, PanelRequest.FolderID);

                litFolderName.Text = string.Format("{0}", folder.Name);

                // bind form
                var esPermissions = ES.Services.EnterpriseStorage.GetEnterpriseFolderPermissions(PanelRequest.ItemID,folder.Name);

                permissions.SetPermissions(esPermissions);
            }
            catch (Exception ex)
            {
                messageBox.ShowErrorMessage("ENETERPRISE_STORAGE_GET_FOLDER_SETTINGS", ex);
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid)
                return;

            try
            {
                SystemFile folder = new SystemFile { Name = PanelRequest.FolderID };

                if (!ES.Services.EnterpriseStorage.CheckEnterpriseStorageInitialization(PanelSecurity.PackageId, PanelRequest.ItemID))
                {
                    ES.Services.EnterpriseStorage.CreateEnterpriseStorage(PanelSecurity.PackageId, PanelRequest.ItemID);
                }

                ES.Services.EnterpriseStorage.SetEnterpriseFolderPermissionSettings(
                    PanelRequest.ItemID,
                    folder,
                    permissions.GetPemissions());


                Response.Redirect(EditUrl("SpaceID", PanelSecurity.PackageId.ToString(), "enterprisestorage_folders",
                        "ItemID=" + PanelRequest.ItemID));
            }
            catch (Exception ex)
            {
                messageBox.ShowErrorMessage("ENTERPRISE_STORAGE_UPDATE_FOLDER_SETTINGS", ex);
            }
        }
    }
}
