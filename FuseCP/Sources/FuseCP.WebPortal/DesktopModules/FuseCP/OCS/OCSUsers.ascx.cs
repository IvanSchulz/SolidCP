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
﻿using FuseCP.EnterpriseServer.Base.HostedSolution;
﻿using FuseCP.Providers.Common;
using FuseCP.Providers.HostedSolution;

namespace FuseCP.Portal.OCS
{
    public partial class OCSUsers : FuseCPModuleBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            BindStats();
        }

        private void BindStats()
        {            
            OrganizationStatistics stats = ES.Services.Organizations.GetOrganizationStatisticsByOrganization(PanelRequest.ItemID);
            int allocatedOCSUsers = stats.AllocatedOCSUsers;
            int usedUsers = stats.CreatedOCSUsers;
            usersQuota.QuotaUsedValue = usedUsers;
            usersQuota.QuotaValue = allocatedOCSUsers;

            if (stats.AllocatedOCSUsers != -1) usersQuota.QuotaAvailable = stats.AllocatedOCSUsers - stats.CreatedOCSUsers;
        }

        protected void btnCreateUser_Click(object sender, EventArgs e)
        {
            Response.Redirect(EditUrl("ItemID", PanelRequest.ItemID.ToString(), "create_new_ocs_user",
                "SpaceID=" + PanelSecurity.PackageId));
        }

        public string GetAccountImage()
        {

            return GetThemedImage("Exchange/admin_16.png");
        }

        public string GetUserEditUrl(string accountId, string instanceId)
        {
            return EditUrl("SpaceID", PanelSecurity.PackageId.ToString(), "edit_ocs_user",
                    "AccountID=" + accountId,
                    "ItemID=" + PanelRequest.ItemID, 
                    "InstanceID=" + instanceId);
        }



        protected void odsAccountsPaged_Selected(object sender, System.Web.UI.WebControls.ObjectDataSourceStatusEventArgs e)
        {
            if (e.Exception != null)
            {
                messageBox.ShowErrorMessage("OCS_GET_USERS", e.Exception);
                e.ExceptionHandled = true;
            }
        }

        protected void gvUsers_RowCommand(object sender, System.Web.UI.WebControls.GridViewCommandEventArgs e)
        {
            if (e.CommandName == "DeleteItem")
            {
                try
                {
                    ResultObject res  = ES.Services.OCS.DeleteOCSUser(PanelRequest.ItemID, e.CommandArgument.ToString());

                    messageBox.ShowMessage(res, "DELETE_OCS_USER", "OCS");
                    
                    // rebind grid
                    gvUsers.DataBind();
                    BindStats();
                    
                }
                catch (Exception ex)
                {
                    messageBox.ShowErrorMessage("DELETE_OCS_USERS", ex);
                }
            }
        }

        protected void ddlPageSize_SelectedIndexChanged(object sender, EventArgs e)
        {
            gvUsers.PageSize = Convert.ToInt32(ddlPageSize.SelectedValue);

            // rebind grid
            gvUsers.DataBind();

            // bind stats
            BindStats();

        }

    }
}
