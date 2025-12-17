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
﻿using FuseCP.Providers.HostedSolution;

namespace FuseCP.Portal.BlackBerry
{
    public partial class BlackBerryUsers : FuseCPModuleBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            OrganizationStatistics stats = ES.Services.Organizations.GetOrganizationStatisticsByOrganization(PanelRequest.ItemID);
            int allocatedCrmUsers = stats.AllocatedBlackBerryUsers;
            int usedUsers = stats.CreatedBlackBerryUsers;
            usersQuota.QuotaUsedValue = usedUsers;
            usersQuota.QuotaValue = allocatedCrmUsers;

            if (stats.AllocatedBlackBerryUsers != -1) usersQuota.QuotaAvailable = allocatedCrmUsers - usedUsers;
        }

        protected void btnCreateNewBlackBerryUser_Click(object sender, EventArgs e)
        {
            Response.Redirect(EditUrl("ItemID", PanelRequest.ItemID.ToString(), "create_new_blackberry_user",
             "SpaceID=" + PanelSecurity.PackageId));
        }

       
        public string GetAccountImage(int accountTypeId)
        {

            return GetThemedImage("Exchange/admin_16.png");
        }

        public string GetUserEditUrl(string accountId)
        {
            return EditUrl("SpaceID", PanelSecurity.PackageId.ToString(), "edit_blackberry_user",
                    "AccountID=" + accountId,
                    "ItemID=" + PanelRequest.ItemID);
        }

        protected void ddlPageSize_SelectedIndexChanged(object sender, EventArgs e)
        {
            gvUsers.PageSize = Convert.ToInt32(ddlPageSize.SelectedValue);

            // rebind grid
            gvUsers.DataBind();

        }

    }
}
