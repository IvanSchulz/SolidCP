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
using FuseCP.EnterpriseServer;

namespace FuseCP.Portal
{
    public partial class SpaceDeleteSpace : FuseCPModuleBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                try
                {
                    BindPackageItems();
                    BindPackagePackages();
                }
                catch (Exception ex)
                {
                    ShowErrorMessage("PACKAGE_GET_PACKAGE", ex);
                    return;
                }
            }
        }

        private void BindPackageItems()
        {
            try
            {
                gvItems.DataSource = ES.Services.Packages.GetRawPackageItems(PanelSecurity.PackageId);
                gvItems.DataBind();
            }
            catch (Exception ex)
            {
                ShowErrorMessage("INIT_SERVICE_ITEM_FORM", ex);
            }
        }

        private void BindPackagePackages()
        {
            gvPackages.DataSource = ES.Services.Packages.GetPackagePackages(PanelSecurity.PackageId);
            gvPackages.DataBind();
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect(NavigateURL(PortalUtils.SPACE_ID_PARAM, PanelSecurity.PackageId.ToString()));
        }
        
        protected void btnDelete_Click(object sender, EventArgs e)
        {
            int ownerId = PanelSecurity.SelectedUserId;

            //old temp fix, not need more.
            //PackageInfo package = ES.Services.Packages.GetPackage(PanelSecurity.PackageId);
            //if ((PackageStatus)package.StatusId != PackageStatus.Active)
            //{
            //    ShowErrorMessage("PACKAGE_CHANGE_STATUS");
            //    return;
            //}

            // delete package
            if (chkConfirm.Checked)
            {
                try
                {
                    

                    int result = ES.Services.Packages.DeletePackage(PanelSecurity.PackageId);
                    if (result < 0)
                    {
                        ShowResultMessage(result);
                        return;
                    }
                }
                catch (Exception ex)
                {
                    ShowErrorMessage("PACKAGE_DELETE_PACKAGE", ex);
                    return;
                }

                // return to the listgv
                Response.Redirect(PortalUtils.GetUserHomePageUrl(ownerId));
            }
            else
            {
                ShowWarningMessage("PACKAGE_DELETE_CONFIRM");
            }
        }

        private void RedirectBack(int spaceId)
        {
            
        }
    }
}
