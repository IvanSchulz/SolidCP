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
using FuseCP.EnterpriseServer;
﻿using FuseCP.EnterpriseServer.Base.HostedSolution;
﻿using FuseCP.Providers.HostedSolution;

namespace FuseCP.Portal.CRM
{
    public partial class CRMUsers : FuseCPModuleBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Organization org = ES.Services.Organizations.GetOrganization(PanelRequest.ItemID);
            if (org.CrmOrganizationId == Guid.Empty)
            {
                messageBox.ShowErrorMessage("NOT_CRM_ORGANIZATION");
                btnCreateUser.Enabled = false;
                CRM2011Panel.Visible = false;
                CRM2013Panel.Visible = false;
            }
            else
            {
                OrganizationStatistics stats = ES.Services.Organizations.GetOrganizationStatisticsByOrganization(PanelRequest.ItemID);

                PackageContext cntx = PackagesHelper.GetCachedPackageContext(PanelSecurity.PackageId);
                if (cntx.Groups.ContainsKey(ResourceGroups.HostedCRM2013))
                {
                    CRM2011Panel.Visible = false;
                    CRM2013Panel.Visible = true;

                    professionalusersQuota.QuotaUsedValue = stats.CreatedProfessionalCRMUsers;
                    professionalusersQuota.QuotaValue = stats.AllocatedProfessionalCRMUsers;

                    basicusersQuota.QuotaUsedValue = stats.CreatedBasicCRMUsers;
                    basicusersQuota.QuotaValue = stats.AllocatedBasicCRMUsers;

                    essentialusersQuota.QuotaUsedValue = stats.CreatedEssentialCRMUsers;
                    essentialusersQuota.QuotaValue = stats.AllocatedEssentialCRMUsers;
                }
                else
                {
                    CRM2011Panel.Visible = true;
                    CRM2013Panel.Visible = false;

                    usersQuota.QuotaUsedValue = stats.CreatedCRMUsers;
                    usersQuota.QuotaValue = stats.AllocatedCRMUsers;

                    limitedusersQuota.QuotaUsedValue = stats.CreatedLimitedCRMUsers;
                    limitedusersQuota.QuotaValue = stats.AllocatedLimitedCRMUsers;

                    essusersQuota.QuotaUsedValue = stats.CreatedESSCRMUsers;
                    essusersQuota.QuotaValue = stats.AllocatedESSCRMUsers;
                }

            }
        }

        protected void btnCreateUser_Click(object sender, EventArgs e)
        {
            Response.Redirect(EditUrl("ItemID", PanelRequest.ItemID.ToString(), "create_crm_user",
                "SpaceID=" + PanelSecurity.PackageId));
        }

        public string GetAccountImage(int accountTypeId)
        {
            
            return GetThemedImage("Exchange/admin_16.png");
        }

        public string GetUserEditUrl(string accountId)
        {
            return EditUrl("SpaceID", PanelSecurity.PackageId.ToString(), "CRMUserRoles",
                    "AccountID=" + accountId,
                    "ItemID=" + PanelRequest.ItemID);
        }

      

        protected void odsAccountsPaged_Selected(object sender, System.Web.UI.WebControls.ObjectDataSourceStatusEventArgs e)
        {

        }

        protected void ddlPageSize_SelectedIndexChanged(object sender, EventArgs e)
        {
            gvUsers.PageSize = Convert.ToInt32(ddlPageSize.SelectedValue);

            // rebind grid
            gvUsers.DataBind();

        }

    }
}
