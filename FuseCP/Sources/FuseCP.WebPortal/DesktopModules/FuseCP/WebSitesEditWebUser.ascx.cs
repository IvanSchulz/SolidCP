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
    public partial class WebSitesEditWebUser : FuseCPModuleBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindGroups();

                // bind user
                BindUser();

				// set policies
				usernameControl.SetPackagePolicy(PanelSecurity.PackageId, UserSettings.WEB_POLICY, "SecuredUserNamePolicy");
				passwordControl.SetPackagePolicy(PanelSecurity.PackageId, UserSettings.WEB_POLICY, "SecuredUserPasswordPolicy");
            }
        }

        private void BindGroups()
        {
            dlGroups.DataSource = ES.Services.WebServers.GetSecuredGroups(PanelRequest.ItemID);
            dlGroups.DataBind();
        }

        private void BindUser()
        {
            if (String.IsNullOrEmpty(PanelRequest.Name))
                return;

            // read user
            WebUser user = ES.Services.WebServers.GetSecuredUser(PanelRequest.ItemID, PanelRequest.Name);
            if (user == null)
                ReturnBack();

            usernameControl.Text = user.Name;
            usernameControl.EditMode = true;
            passwordControl.EditMode = true;

            // groups
            foreach (string group in user.Groups)
            {
                ListItem li = dlGroups.Items.FindByValue(group);
                if (li != null) li.Selected = true;
            }
        }

        private void SaveUser()
        {
            WebUser user = new WebUser();
            user.Name = usernameControl.Text;
            user.Password = passwordControl.Password;

            List<string> groups = new List<string>();
            foreach (ListItem li in dlGroups.Items)
                if (li.Selected)
                    groups.Add(li.Value);

            user.Groups = groups.ToArray();

            try
            {
                int result = ES.Services.WebServers.UpdateSecuredUser(PanelRequest.ItemID, user);
                if (result < 0)
                {
                    ShowResultMessage(result);
                    return;
                }
            }
            catch (Exception ex)
            {
                ShowErrorMessage("WEB_UPDATE_SECURED_USER", ex);
                return;
            }

            ReturnBack();
        }

        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            SaveUser();
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
