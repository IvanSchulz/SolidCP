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
    public partial class SpaceEditDetails : FuseCPModuleBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindSpace();
                BindSpaceAddons();
                BindRoles(PanelSecurity.EffectiveUserId);
                BindUsers();
            }
        }

        private void BindRoles(int userId)
        {
            // load selected user
            UserInfo user = UsersHelper.GetUser(userId);

            if (user != null)
            {
                if ((user.Role == UserRole.User) | 
                    (PanelSecurity.LoggedUser.Role == UserRole.ResellerCSR) |
                    (PanelSecurity.LoggedUser.Role == UserRole.ResellerHelpdesk) | 
                    (PanelSecurity.LoggedUser.Role == UserRole.PlatformCSR) |
                    (PanelSecurity.LoggedUser.Role == UserRole.PlatformHelpdesk))
                    this.rbPackageQuotas.Enabled = this.rbPlanQuotas.Enabled = false;
            }
        }


        private void BindSpace()
        {
            PackageInfo package = ES.Services.Packages.GetPackage(PanelSecurity.PackageId);
            if (package != null)
            {
                // bind plans
                BindHostingPlans();

                // bind space
                txtName.Text = PortalAntiXSS.DecodeOld(package.PackageName);
                txtComments.Text = PortalAntiXSS.DecodeOld(package.PackageComments);
                PurchaseDate.SelectedDate = package.PurchaseDate;
                serverDetails.ServerId = package.ServerId;
                Utils.SelectListItem(ddlPlan, package.PlanId);

                // bind quotas
                packageQuotas.BindQuotas(PanelRequest.PackageID);

                // bind override flag
                rbPlanQuotas.Checked = !package.OverrideQuotas;
                rbPackageQuotas.Checked = package.OverrideQuotas;

                // toggle quotas editor
                ToggleQuotasEditor();
            }
        }

        private void BindUsers()
        {
            System.Collections.Generic.List<UserInfo> userList = new System.Collections.Generic.List<UserInfo>(ES.Services.Users.GetUsers(PanelSecurity.EffectiveUserId, true));
            ddlUser.DataSource = userList;
            ddlUser.DataBind();
            ddlUser.SelectedValue = PanelSecurity.SelectedUserId.ToString();
            ddlUser.Enabled = false;
        }

        protected void chkMoveUser_CheckedChanged(object sender, EventArgs e)
        {
            ddlUser.Enabled = chkMoveUser.Checked;
        }

        private void BindHostingPlans()
        {
            ddlPlan.DataSource = ES.Services.Packages.GetUserAvailableHostingPlans(PanelSecurity.SelectedUserId);
            ddlPlan.DataBind();

            ddlPlan.Items.Insert(0, new ListItem(GetLocalizedString("SelectHostingPlan.Text"), ""));
        }

        private void BindSpaceAddons()
        {
            gvAddons.DataSource = ES.Services.Packages.GetPackageAddons(PanelSecurity.PackageId);
            gvAddons.DataBind();
        }

        private void ToggleQuotasEditor()
        {
            packageQuotas.Visible = rbPlanQuotas.Checked;
            editPackageQuotas.Visible = rbPackageQuotas.Checked;

            // bind quotas editor if required
            if (rbPackageQuotas.Checked)
                editPackageQuotas.BindPackageQuotas(PanelSecurity.PackageId);
            else
                packageQuotas.BindQuotas(PanelSecurity.PackageId);
        }

        private void SaveSpace()
        {
            if (!Page.IsValid)
                return;

            // gather form data
            PackageInfo package = new PackageInfo();

            // load package for update
            if (PanelSecurity.PackageId > 0)
                package = ES.Services.Packages.GetPackage(PanelSecurity.PackageId);

            package.PackageId = PanelSecurity.PackageId;
            package.PackageName = txtName.Text;
            package.PackageComments = txtComments.Text;
            package.PlanId = Utils.ParseInt(ddlPlan.SelectedValue, 0);
            package.PurchaseDate = PurchaseDate.SelectedDate;

            package.OverrideQuotas = rbPackageQuotas.Checked;
            if (package.OverrideQuotas)
            {
                package.Groups = editPackageQuotas.Groups;
                package.Quotas = editPackageQuotas.Quotas;
            }

            try
            {
                // update existing package
                PackageResult result = ES.Services.Packages.UpdatePackage(package);
                if (result.Result < 0)
                {
                    ShowResultMessage(result.Result);
                    lblMessage.Text = PortalAntiXSS.Encode(GetExceedingQuotasMessage(result.ExceedingQuotas));
                    return;
                }

                bool notUserRoleAndNotSelectedTheSameUser = (PanelSecurity.SelectedUserId != Convert.ToInt32(ddlUser.SelectedValue)
                    && PanelSecurity.LoggedUser.Role != UserRole.User);

                if (chkMoveUser.Checked  && notUserRoleAndNotSelectedTheSameUser)
                {
                    int changeResult = ES.Services.Packages.ChangePackageUser(PanelSecurity.PackageId, Convert.ToInt32(ddlUser.SelectedValue));
                    if (changeResult < 0)
                    {
                        ShowResultMessage(result.Result);
                        return;
                    }
                }                
            }
            catch (Exception ex)
            {
                ShowErrorMessage("PACKAGE_UPDATE_PACKAGE", ex);
                return;
            }

            // return
            RedirectSpaceHomePage();
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            SaveSpace();
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            RedirectSpaceHomePage();
        }

        protected void rbPlanQuotas_CheckedChanged(object sender, EventArgs e)
        {
            ToggleQuotasEditor();
        }

        protected void btnAddAddon_Click(object sender, EventArgs e)
        {
            Response.Redirect(EditUrl(PortalUtils.SPACE_ID_PARAM, PanelSecurity.PackageId.ToString(), "edit_addon"));
        }
    }
}
