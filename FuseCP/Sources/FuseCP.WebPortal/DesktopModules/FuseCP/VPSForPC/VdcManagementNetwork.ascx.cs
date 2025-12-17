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

﻿using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FuseCP.EnterpriseServer;

namespace FuseCP.Portal.VPSForPC
{
    public partial class VdcManagementNetwork : FuseCPModuleBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
                BindAccount();
        }

        private void BindAccount()
        {
            // load user
            UserInfo user = UsersHelper.GetUser(PanelSecurity.SelectedUserId);
            if (user != null)
            {
                // Allow edit
                gvVlans.Columns[2].Visible = btnAddVlan.Visible = (PanelSecurity.EffectiveUser.Role == UserRole.Administrator);

                gvVlans.DataSource = user.Vlans;
                gvVlans.DataBind();
            }
        }

        protected void gvVlans_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "DeleteItem")
            {
                UsersHelper.DeleteUserVlan(PanelSecurity.SelectedUserId, ushort.Parse(e.CommandArgument.ToString(), 0));
                BindAccount();
            }
        }

        protected void btnAddVlan_Click(object sender, EventArgs e)
        {
            Response.Redirect(EditUrl("SpaceID", PanelSecurity.PackageId.ToString(), "vdc_account_vLan_add"));
        }


    }
}
