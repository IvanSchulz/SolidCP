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
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

using FuseCP.Providers.SharePoint;

namespace FuseCP.Portal
{
    public partial class SharePointWebPartPackages : FuseCPModuleBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindSite();
            }
        }

        private void BindSite()
        {
            try
            {
                SharePointSite site = ES.Services.SharePointServers.GetSharePointSite(PanelRequest.ItemID);
                if (site == null)
                    RedirectToBrowsePage();

                litSiteName.Text = site.Name;

                BindWebPartPackages();
            }
            catch (Exception ex)
            {
                ShowErrorMessage("SHAREPOINT_GET_SITE", ex);
                return;
            }
        }

        private void BindWebPartPackages()
        {
            lbWebPartPackages.DataSource = ES.Services.SharePointServers.GetInstalledWebParts(PanelRequest.ItemID);
            lbWebPartPackages.DataBind();
        }

        protected void btnInstall_Click(object sender, EventArgs e)
        {
            Response.Redirect(EditUrl("ItemID", PanelRequest.ItemID.ToString(), "install_webparts",
                PortalUtils.SPACE_ID_PARAM + "=" + PanelSecurity.PackageId.ToString()));
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect(EditUrl("ItemID", PanelRequest.ItemID.ToString(), "edit_item",
                PortalUtils.SPACE_ID_PARAM + "=" + PanelSecurity.PackageId.ToString()));
        }

        protected void btnUninstall_Click(object sender, EventArgs e)
        {
            if (lbWebPartPackages.SelectedIndex != -1)
            {
                try
                {
                    int result = ES.Services.SharePointServers.DeleteWebPartsPackage(
                        PanelRequest.ItemID, lbWebPartPackages.SelectedValue);

                    if (result < 0)
                    {
                        ShowResultMessage(result);
                        return;
                    }

                    // bind packages
                    BindWebPartPackages();
                }
                catch (Exception ex)
                {
                    ShowErrorMessage("SHAREPOINT_UNINSTALL_WEBPARTS", ex);
                    return;
                }
            }
        }
    }
}
