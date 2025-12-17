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
using FuseCP.WebPortal;

namespace FuseCP.Portal.ExchangeServer
{
    public partial class EnterpriseStorageDriveMaps : FuseCPModuleBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (!ES.Services.EnterpriseStorage.CheckUsersDomainExists(PanelRequest.ItemID))
                {
                    btnAddDriveMap.Enabled = false;

                    messageBox.ShowWarningMessage("WEB_SITE_IS_NOT_CREATED");
                }
            }
        }

        protected void btnAddDriveMap_Click(object sender, EventArgs e)
        {
            Response.Redirect(EditUrl("ItemID", PanelRequest.ItemID.ToString(), "create_enterprisestorage_drive_map",
                "SpaceID=" + PanelSecurity.PackageId));
        }

        protected void gvDriveMaps_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "DeleteItem")
            {
                // delete drive map
                string driveLetter = e.CommandArgument.ToString();

                try
                {
                    ResultObject result = ES.Services.EnterpriseStorage.DeleteMappedDrive(PanelRequest.ItemID, driveLetter);
                    if (!result.IsSuccess)
                    {
                        messageBox.ShowMessage(result, "ENTERPRISE_STORAGE_DRIVE_MAP", "EnterpriseStorage");
                        return;
                    }

                    gvDriveMaps.DataBind();
                }
                catch (Exception ex)
                {
                    ShowErrorMessage("ENTERPRISE_STORAGE_DELETE_DRIVE_MAP", ex);
                }
            }
        }

        protected void ddlPageSize_SelectedIndexChanged(object sender, EventArgs e)
        {
            gvDriveMaps.PageSize = Convert.ToInt32(ddlPageSize.SelectedValue);

            gvDriveMaps.DataBind();
        }

        protected string GetDriveImage()
        {
            return String.Concat("~/", DefaultPage.THEMES_FOLDER, "/", Page.Theme, "/", "Images/Exchange", "/", "net_drive16.png");
        }
    }
}
