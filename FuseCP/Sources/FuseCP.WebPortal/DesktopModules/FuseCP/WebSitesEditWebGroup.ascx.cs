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
using System.Collections.Generic;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

using FuseCP.Providers.Web;
using FuseCP.EnterpriseServer;

namespace FuseCP.Portal
{
    public partial class WebSitesEditWebGroup : FuseCPModuleBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindUsers();

                // bind group
                BindGroup();

				// set policies
				usernameControl.SetPackagePolicy(PanelSecurity.PackageId, UserSettings.WEB_POLICY, "SecuredGroupNamePolicy");

            }
        }

        private void BindUsers()
        {
            dlUsers.DataSource = ES.Services.WebServers.GetSecuredUsers(PanelRequest.ItemID);
            dlUsers.DataBind();
        }

        private void BindGroup()
        {
            if (String.IsNullOrEmpty(PanelRequest.Name))
                return;

            // read group
            WebGroup group = ES.Services.WebServers.GetSecuredGroup(PanelRequest.ItemID, PanelRequest.Name);
            if (group == null)
                ReturnBack();

            usernameControl.Text = group.Name;
            usernameControl.EditMode = true;

            // users
            foreach (string user in group.Users)
            {
                ListItem li = dlUsers.Items.FindByValue(user);
                if (li != null) li.Selected = true;
            }
        }

        private void SaveGroup()
        {
            WebGroup group = new WebGroup();
            group.Name = usernameControl.Text;

            List<string> users = new List<string>();
            foreach (ListItem li in dlUsers.Items)
                if (li.Selected)
                    users.Add(li.Value);

            group.Users = users.ToArray();

            try
            {
                int result = ES.Services.WebServers.UpdateSecuredGroup(PanelRequest.ItemID, group);
                if (result < 0)
                {
                    ShowResultMessage(result);
                    return;
                }
            }
            catch (Exception ex)
            {
                ShowErrorMessage("WEB_UPDATE_SECURED_GROUP", ex);
                return;
            }

            ReturnBack();
        }

        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            SaveGroup();
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            ReturnBack();
        }

        private void ReturnBack()
        {
            Response.Redirect(EditUrl("ItemID", PanelRequest.ItemID.ToString(), "edit_item",
                "MenuID=securedfolders",
                PortalUtils.SPACE_ID_PARAM + "=" + PanelSecurity.PackageId.ToString()));
        }
    }
}
