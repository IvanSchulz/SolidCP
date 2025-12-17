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

namespace FuseCP.Portal
{
    public partial class SpaceEditAddon : FuseCPModuleBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            btnDelete.Visible = (PanelRequest.PackageAddonID != 0);

            if (!IsPostBack)
            {
                try
                {
                    BindPackageAddon();
                }
                catch (Exception ex)
                {
                    ShowErrorMessage("PACKAGE_GET_ADDON", ex);
                    return;
                }
            }
        }

        private void BindAddons(int userId)
        {
            HostingPlanInfo[] hpi = ES.Services.Packages.GetUserAvailableHostingAddons(userId);

            // Next code is user for decoding incorectly stored plan names and descriptions with pre 1.2.2 installations
            for (int i = 0; i < hpi.Length; i++)
            {
                hpi[i].PlanDescription = PortalAntiXSS.DecodeOld(hpi[i].PlanDescription);
                hpi[i].PlanName = PortalAntiXSS.DecodeOld(hpi[i].PlanName);
            }

            ddlPlan.DataSource = hpi;
            ddlPlan.DataBind();

            ddlPlan.Items.Insert(0, new ListItem(GetLocalizedString("SelectHostingPlan.Text"), ""));
        }

        private void BindPackageAddon()
        {
            try
            {
                int packageId = PanelSecurity.PackageId;

                PackageInfo package = null;
                PackageAddonInfo addon = null;

                if (PanelRequest.PackageAddonID != 0)
                {
                    // load package addon
                    addon = ES.Services.Packages.GetPackageAddon(PanelRequest.PackageAddonID);

                    if (addon == null)
                        // package not found
                        RedirectBack();

                    packageId = addon.PackageId;
                }

                // load addon package
                package = ES.Services.Packages.GetPackage(packageId);
                if (package == null)
                    RedirectBack();

                // bind addons list
                BindAddons(package.UserId);

                // init other fields
                PurchaseDate.SelectedDate = DateTime.Now;

                if (PanelRequest.PackageAddonID == 0)
                    return;

                Utils.SelectListItem(ddlPlan, addon.PlanId);

                txtComments.Text = addon.Comments;

                PurchaseDate.SelectedDate = addon.PurchaseDate;
                Utils.SelectListItem(ddlPlan, addon.PlanId);
                txtQuantity.Text = addon.Quantity.ToString();
                Utils.SelectListItem(ddlStatus, addon.StatusId);

            }
            catch (Exception ex)
            {
                ShowErrorMessage("PACKAGE_GET_ADDON", ex);
                return;
            }
        }

        private void SaveAddon()
        {
            if (!Page.IsValid)
                return;

            // gather form data
            PackageAddonInfo addon = new PackageAddonInfo();
            addon.PackageAddonId = PanelRequest.PackageAddonID;
            addon.PackageId = PanelSecurity.PackageId;
            addon.Comments = txtComments.Text;
            addon.PlanId = Utils.ParseInt(ddlPlan.SelectedValue, 0);
            addon.StatusId = Utils.ParseInt(ddlStatus.SelectedValue, 0);
            addon.PurchaseDate = PurchaseDate.SelectedDate;
            addon.Quantity = Utils.ParseInt(txtQuantity.Text, 1);

            if (PanelRequest.PackageAddonID == 0)
            {
                // add a new package addon
                try
                {
                    PackageResult result = ES.Services.Packages.AddPackageAddon(addon);
                    if (result.Result < 0)
                    {
                        ShowResultMessage(result.Result);
                        lblMessage.Text = PortalAntiXSS.Encode(GetExceedingQuotasMessage(result.ExceedingQuotas));
                        return;
                    }
                }
                catch (Exception ex)
                {
                    ShowErrorMessage("PACKAGE_ADD_ADDON", ex);
                    return;
                }
            }
            else
            {
                // update existing package addon
                try
                {
                    PackageResult result = ES.Services.Packages.UpdatePackageAddon(addon);
                    if (result.Result < 0)
                    {
                        ShowResultMessage(result.Result);
                        lblMessage.Text = PortalAntiXSS.Encode(GetExceedingQuotasMessage(result.ExceedingQuotas));
                        return;
                    }
                }
                catch (Exception ex)
                {
                    ShowErrorMessage("PACKAGE_UPDATE_ADDON", ex);
                    return;
                }
            }

            RedirectBack();
        }

        private void DeleteAddon()
        {
            try
            {
                int result = ES.Services.Packages.DeletePackageAddon(PanelRequest.PackageAddonID);
                if (result < 0)
                {
                    ShowResultMessage(result);
                    return;
                }
            }
            catch (Exception ex)
            {
                ShowErrorMessage("PACKAGE_DELETE_ADDON", ex);
                return;
            }

            RedirectBack();
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            SaveAddon();
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            RedirectBack();
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            DeleteAddon();
        }

        private void RedirectBack()
        {
            Response.Redirect(EditUrl(PortalUtils.SPACE_ID_PARAM, PanelSecurity.PackageId.ToString(), "edit_details"));
        }
    }
}
