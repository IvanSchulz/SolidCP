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

namespace FuseCP.Portal.Lync
{
    public partial class LyncUsers : FuseCPModuleBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            BindStats();
        }

        private void BindStats()
        {            
            OrganizationStatistics stats = ES.Services.Organizations.GetOrganizationStatisticsByOrganization(PanelRequest.ItemID);
            int allocatedLyncUsers = stats.AllocatedLyncUsers;
            int usedUsers = stats.CreatedLyncUsers;
            usersQuota.QuotaUsedValue = usedUsers;
            usersQuota.QuotaValue = allocatedLyncUsers;

            if (stats.AllocatedLyncUsers != -1) usersQuota.QuotaAvailable = stats.AllocatedLyncUsers - stats.CreatedLyncUsers;
        }

        protected void btnCreateUser_Click(object sender, EventArgs e)
        {
            Response.Redirect(EditUrl("ItemID", PanelRequest.ItemID.ToString(), "create_new_lync_user",
                "SpaceID=" + PanelSecurity.PackageId));
        }

        public string GetAccountImage()
        {

            return GetThemedImage("Exchange/admin_16.png");
        }

        public string GetUserEditUrl(string accountId)
        {
            return EditUrl("SpaceID", PanelSecurity.PackageId.ToString(), "edit_lync_user",
                    "AccountID=" + accountId,
                    "ItemID=" + PanelRequest.ItemID);
        }



        protected void odsAccountsPaged_Selected(object sender, System.Web.UI.WebControls.ObjectDataSourceStatusEventArgs e)
        {

        }

        protected void gvUsers_RowCommand(object sender, System.Web.UI.WebControls.GridViewCommandEventArgs e)
        {
            if (e.CommandName == "DeleteItem")
            {
                try
                {
                    ResultObject res  = ES.Services.Lync.DeleteLyncUser(PanelRequest.ItemID, Convert.ToInt32(e.CommandArgument));

                    messageBox.ShowMessage(res, "DELETE_LYNC_USER", "LYNC");
                    
                    // rebind grid
                    gvUsers.DataBind();
                    BindStats();
                    
                }
                catch (Exception ex)
                {
                    messageBox.ShowErrorMessage("DELETE_LYNC_USERS", ex);
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



        public string GetOrganizationUserEditUrl(string accountId)
        {
            return EditUrl("SpaceID", PanelSecurity.PackageId.ToString(), "edit_user",
                    "AccountID=" + accountId,
                    "ItemID=" + PanelRequest.ItemID,
                    "Context=User");
        }

    }
}
