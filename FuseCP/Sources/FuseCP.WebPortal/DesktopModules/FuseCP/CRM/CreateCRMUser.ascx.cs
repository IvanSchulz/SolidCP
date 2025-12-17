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
using FuseCP.Providers.HostedSolution;
using FuseCP.Providers.ResultObjects;
using FuseCP.EnterpriseServer;

namespace FuseCP.Portal.CRM
{
    public partial class CreateCRMUser : FuseCPModuleBase
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
                    CRMBusinessUnitsResult res =
                        ES.Services.CRM.GetBusinessUnits(PanelRequest.ItemID, PanelSecurity.PackageId);
                    if (res.IsSuccess)
                    {
                        ddlBusinessUnits.DataSource = res.Value;
                        ddlBusinessUnits.DataValueField = "BusinessUnitId";
                        ddlBusinessUnits.DataTextField = "BusinessUnitName";
                        ddlBusinessUnits.DataBind();
                    }
                    else
                    {
                        messageBox.ShowMessage(res, "CREATE_CRM_USER", "HostedCRM");
                    }
                }
                catch(Exception ex)
                {
                    messageBox.ShowErrorMessage("CREATE_CRM_USER", ex);
                }
            }
        }

        protected void btnCreate_Click(object sender, EventArgs e)
        {
            int accountId = userSelector.GetAccountId();
            UserResult res;
            try
            {
                OrganizationUser user = ES.Services.Organizations.GetUserGeneralSettings(PanelRequest.ItemID, accountId);
                user.AccountId = accountId;

                int cALType = 0;
                int.TryParse(ddlLicenseType.SelectedValue, out cALType);

                res = ES.Services.CRM.CreateCRMUser(user, PanelSecurity.PackageId, PanelRequest.ItemID, new Guid(ddlBusinessUnits.SelectedValue), cALType);
                if (res.IsSuccess)
                {

                    Response.Redirect(EditUrl("AccountID", user.AccountId.ToString(), "CRMUserRoles",
                    "SpaceID=" + PanelSecurity.PackageId,
                    "ItemID=" + PanelRequest.ItemID));                    
                }
                else
                {
                    messageBox.ShowMessage(res, "CREATE_CRM_USER", "HostedCRM");
                }
                    
            }
            catch(Exception ex)
            {
                messageBox.ShowErrorMessage("CREATE_CRM_USER", ex);
            }
        }
    }
}
