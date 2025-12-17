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
    public partial class WebSitesEditHeliconApeUser : FuseCPModuleBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindGroups();

                BindAuthControls();

                // bind user
                BindUser();

				// set policies
				usernameControl.SetPackagePolicy(PanelSecurity.PackageId, UserSettings.WEB_POLICY, "SecuredUserNamePolicy");
				passwordControl.SetPackagePolicy(PanelSecurity.PackageId, UserSettings.WEB_POLICY, "SecuredUserPasswordPolicy");
            }
        }

        private void BindAuthControls()
        {
            // AuthType
            rblAuthType.Items.Clear();
            foreach (string authType in HtaccessFolder.AUTH_TYPES)
            {
                rblAuthType.Items.Add(new ListItem(authType, authType));
            }
            rblAuthType.SelectedIndex = 0;

            // Encoding types
            rblEncType.Items.Clear();
            foreach (string encType in HtaccessUser.ENCODING_TYPES)
            {
                rblEncType.Items.Add(new ListItem(encType, encType));
            }
            rblEncType.SelectedIndex = 0;
        }

        private void BindGroups()
        {
            dlGroups.DataSource = ES.Services.WebServers.GetHeliconApeGroups(PanelRequest.ItemID);
            dlGroups.DataBind();
        }

        private void BindUser()
        {
            if (String.IsNullOrEmpty(PanelRequest.Name))
                return;

            // read user
            HtaccessUser user = ES.Services.WebServers.GetHeliconApeUser(PanelRequest.ItemID, PanelRequest.Name);
            if (user == null)
                ReturnBack();

            usernameControl.Text = user.Name;
            usernameControl.EditMode = true;
            passwordControl.EditMode = true;

            // AuthType
            for (int i = 0; i < rblAuthType.Items.Count; i++)
            {
                ListItem item = rblAuthType.Items[i];
                if (item.Value == user.AuthType)
                {
                    rblAuthType.SelectedIndex = i;
                    break;
                }
            }

            // Encoding types
            for (int i = 0; i < rblEncType.Items.Count; i++)
            {
                ListItem item = rblEncType.Items[i];
                if (item.Value == user.EncType)
                {
                    rblEncType.SelectedIndex = i;
                    break;
                }
            }

            // groups
            foreach (string group in user.Groups)
            {
                ListItem li = dlGroups.Items.FindByValue(group);
                if (li != null) li.Selected = true;
            }
        }

        private void SaveUser()
        {
            HtaccessUser user = new HtaccessUser();
            user.Name = usernameControl.Text;
            user.Password = passwordControl.Password;
            user.AuthType = rblAuthType.SelectedItem.Value;
            user.EncType  = rblEncType.SelectedItem.Value;
            user.Realm = tbDigestRealm.Text;

            List<string> groups = new List<string>();
            foreach (ListItem li in dlGroups.Items)
                if (li.Selected)
                    groups.Add(li.Value);

            user.Groups = groups.ToArray();

            try
            {
                int result = ES.Services.WebServers.UpdateHeliconApeUser(PanelRequest.ItemID, user);
                if (result < 0)
                {
                    ShowResultMessage(result);
                    return;
                }
            }
            catch (Exception ex)
            {
                ShowErrorMessage("WEB_UPDATE_HeliconApe_USER", ex);
                return;
            }

        }

        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            SaveUser();
            ReturnBack();
        }

        protected void btnSaveAndAddAnother_Click(object sender, EventArgs e)
        {
            SaveUser();
            ClearControls();
        }

        private void ClearControls()
        {
            usernameControl.Text = string.Empty;
            usernameControl.EditMode = false;
            
            passwordControl.Password = string.Empty;
            passwordControl.EditMode = false;
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            ReturnBack();
        }

        private void ReturnBack()
        {
            Response.Redirect(EditUrl("ItemID", PanelRequest.ItemID.ToString(), "edit_item",
                "MenuID=htaccessfolders",
                PortalUtils.SPACE_ID_PARAM + "=" + PanelSecurity.PackageId.ToString()));
        }
    }
}
