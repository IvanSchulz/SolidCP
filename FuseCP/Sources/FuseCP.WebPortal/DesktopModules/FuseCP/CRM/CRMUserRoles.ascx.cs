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
using System.Web.UI.WebControls;
using FuseCP.Providers.Common;
using FuseCP.Providers.HostedSolution;
using FuseCP.Providers.ResultObjects;
using FuseCP.EnterpriseServer;

namespace FuseCP.Portal.CRM
{
    public partial class CRMUserRoles : FuseCPModuleBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                PackageContext cntx = PackagesHelper.GetCachedPackageContext(PanelSecurity.PackageId);
                if (cntx.Groups.ContainsKey(ResourceGroups.HostedCRM2013))
                {
                    ddlLicenseType.Items.Add(new System.Web.UI.WebControls.ListItem(
                        GetSharedLocalizedString("HostedCRM.LicenseProfessional"), CRMUserLycenseTypes.PROFESSIONAL.ToString()));
                    ddlLicenseType.Items.Add(new System.Web.UI.WebControls.ListItem(
                        GetSharedLocalizedString("HostedCRM.LicenseBasic"), CRMUserLycenseTypes.BASIC.ToString()));
                    ddlLicenseType.Items.Add(new System.Web.UI.WebControls.ListItem(
                        GetSharedLocalizedString("HostedCRM.LicenseEssential"), CRMUserLycenseTypes.ESSENTIAL.ToString()));
                }
                else
                {
                    ddlLicenseType.Items.Add(new System.Web.UI.WebControls.ListItem(
                        GetSharedLocalizedString("HostedCRM.LicenseFull"), CRMUserLycenseTypes.FULL.ToString()));
                    ddlLicenseType.Items.Add(new System.Web.UI.WebControls.ListItem(
                        GetSharedLocalizedString("HostedCRM.LicenseLimited"), CRMUserLycenseTypes.LIMITED.ToString()));
                    ddlLicenseType.Items.Add(new System.Web.UI.WebControls.ListItem(
                        GetSharedLocalizedString("HostedCRM.LicenseESS"), CRMUserLycenseTypes.ESS.ToString()));
                }


                try
                {
                    OrganizationUser user =
                        ES.Services.Organizations.GetUserGeneralSettings(PanelRequest.ItemID, PanelRequest.AccountID);

                    CrmUserResult userResult = ES.Services.CRM.GetCrmUser(PanelRequest.ItemID, PanelRequest.AccountID);

                    if (userResult.IsSuccess)
                    {
                        btnActive.Visible = userResult.Value.IsDisabled;
                        locEnabled.Visible = !userResult.Value.IsDisabled;

                        btnDeactivate.Visible = !userResult.Value.IsDisabled;
                        locDisabled.Visible = userResult.Value.IsDisabled;
                        lblDisplayName.Text = user.DisplayName;
                        lblEmailAddress.Text = user.PrimaryEmailAddress;
                        lblDomainName.Text = user.DomainUserName;

                        int cALType = userResult.Value.CALType + ((int)userResult.Value.ClientAccessMode) * 10;

                        Utils.SelectListItem(ddlLicenseType, cALType);
                    }
                    else
                    {
                        messageBox.ShowMessage(userResult, "GET_CRM_USER", "HostedCRM");
                        return;
                    }
                    
                    CrmRolesResult res =
                        ES.Services.CRM.GetCrmRoles(PanelRequest.ItemID, PanelRequest.AccountID, PanelSecurity.PackageId);

                    if (res.IsSuccess)
                    {
                        gvRoles.DataSource = res.Value;
                        gvRoles.DataBind();
                    }
                    else
                    {
                        messageBox.ShowMessage(res, "GET_CRM_USER_ROLES", "HostedCRM");
                    }
                }
                catch(Exception ex)
                {
                    messageBox.ShowErrorMessage("GET_CRM_USER_ROLES", ex);
                }
            }
        }

        protected bool SaveSettings()
        {
            try
            {
                List<Guid> roles = new List<Guid>();
                foreach (GridViewRow row in gvRoles.Rows)
                {
                    CheckBox cbSelected = row.FindControl("cbSelected") as CheckBox;
                    string str = gvRoles.DataKeys[row.DataItemIndex].Value.ToString();
                    if (cbSelected != null && cbSelected.Checked)
                        roles.Add(new Guid(str));
                }


                ResultObject res =
                    ES.Services.CRM.SetUserRoles(PanelRequest.ItemID, PanelRequest.AccountID, PanelSecurity.PackageId,
                                                 roles.ToArray());


                int CALType = 0;
                int.TryParse(ddlLicenseType.SelectedValue, out CALType);

                ResultObject res2 =
                    ES.Services.CRM.SetUserCALType(PanelRequest.ItemID, PanelRequest.AccountID, PanelSecurity.PackageId,
                                                CALType);

                if (!res2.IsSuccess)
                    messageBox.ShowMessage(res2, "UPDATE_CRM_USER_ROLES", "HostedCRM");
                else if (!res.IsSuccess)
                    messageBox.ShowMessage(res, "UPDATE_CRM_USER_ROLES", "HostedCRM");
                else
                    messageBox.ShowMessage(res, "UPDATE_CRM_USER_ROLES", "HostedCRM");

                return res.IsSuccess && res2.IsSuccess;
            }
            catch (Exception ex)
            {
                messageBox.ShowErrorMessage("UPDATE_CRM_USER_ROLES", ex);
                return false;
            }

        }

        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            SaveSettings();
        }

        protected void btnSaveExit_Click(object sender, EventArgs e)
        {
            if (SaveSettings())
            {
                Response.Redirect(PortalUtils.EditUrl("ItemID", PanelRequest.ItemID.ToString(),
                    "CRMUsers",
                    "SpaceID=" + PanelSecurity.PackageId));
            }
        }


        private void ActivateUser()
        {
            ResultObject res = ES.Services.CRM.ChangeUserState(PanelRequest.ItemID, PanelRequest.AccountID, false);
            messageBox.ShowMessage(res, "CHANGE_USER_STATE", "HostedCRM");
            locDisabled.Visible = false;
            btnDeactivate.Visible = true;
            btnActive.Visible = false;
            locEnabled.Visible = true;
        }

        private void DeactivateUser()
        {
            ResultObject res = ES.Services.CRM.ChangeUserState(PanelRequest.ItemID, PanelRequest.AccountID, true);
            messageBox.ShowMessage(res, "CHANGE_USER_STATE", "HostedCRM");
            locDisabled.Visible = true;
            btnDeactivate.Visible = false;
            btnActive.Visible = true;
            locEnabled.Visible = false;
        }
        
        
        protected void btnActive_Click(object sender, EventArgs e)
        {
            ActivateUser();
        }

        protected void btnDeactivate_Click(object sender, EventArgs e)
        {
            DeactivateUser();
        }


        
    }
}
