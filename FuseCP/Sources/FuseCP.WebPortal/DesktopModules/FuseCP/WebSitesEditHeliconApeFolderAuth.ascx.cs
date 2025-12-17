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

namespace FuseCP.Portal
{
    public partial class WebSitesEditHeliconApeFolderAuth : FuseCPModuleBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindUsers();
                BindGroups();

                // bind folder
                BindFolder();
            }
        }

        private void BindFolder()
        {
            // read web site
            WebSite site = ES.Services.WebServers.GetWebSite(PanelRequest.ItemID);
            if (site == null)
                RedirectToBrowsePage();

            folderPath.RootFolder = site.ContentPath;
            folderPath.PackageId = site.PackageId;

            if (String.IsNullOrEmpty(PanelRequest.Name))
                return;

            // read folder
            HtaccessFolder folder = ES.Services.WebServers.GetHeliconApeFolder(PanelRequest.ItemID, PanelRequest.Name);
            if(folder == null)
                ReturnBack();

            txtTitle.Text = folder.AuthName;
            folderPath.SelectedFile = folder.Path;
            folderPath.Enabled = false;

            // AuthType
            rblAuthType.Items.Clear();
            foreach (string authType in HtaccessFolder.AUTH_TYPES)
            {
                rblAuthType.Items.Add(new ListItem(authType, authType));
            }

            for (int i = 0; i < rblAuthType.Items.Count; i++)
            {
                ListItem item = rblAuthType.Items[i];
                if (item.Value == folder.AuthType)
                {
                    rblAuthType.SelectedIndex = i;
                    break;
                }
            }

            // users
            foreach (string user in folder.Users)
            {
                ListItem li = dlUsers.Items.FindByValue(user);
                if (li != null) li.Selected = true;
            }
            if (folder.ValidUser)
            {
                ListItem li = dlUsers.Items.FindByText(HtaccessFolder.VALID_USER);
                if (li != null) li.Selected = true;
            }

            // groups
            foreach (string group in folder.Groups)
            {
                ListItem li = dlGroups.Items.FindByValue(group);
                if (li != null) li.Selected = true;
            }
        }

        private void BindUsers()
        {
            List<WebUser> webUsers = new List<WebUser>(ES.Services.WebServers.GetHeliconApeUsers(PanelRequest.ItemID));
            webUsers.Add(new WebUser{Name = HtaccessFolder.VALID_USER});
            dlUsers.DataSource = webUsers;
            dlUsers.DataBind();
        }

        private void BindGroups()
        {
            dlGroups.DataSource = ES.Services.WebServers.GetHeliconApeGroups(PanelRequest.ItemID);
            dlGroups.DataBind();
        }

        private void SaveFolder()
        {
            HtaccessFolder folder;
            WebSite site = ES.Services.WebServers.GetWebSite(PanelRequest.ItemID);
            if (null != site && !String.IsNullOrEmpty(PanelRequest.Name))
            {
                folder = ES.Services.WebServers.GetHeliconApeFolder(PanelRequest.ItemID, PanelRequest.Name);
            }
            else
            {
                folder = new HtaccessFolder();
            }

            folder.AuthName = txtTitle.Text.Trim();
            folder.AuthType = rblAuthType.SelectedItem.Value;
            
            // readonly
            // folder.Path = folderPath.SelectedFile;

            List<string> users = new List<string>();
            foreach (ListItem li in dlUsers.Items)
                if (li.Selected)
                    users.Add(li.Value);

            List<string> groups = new List<string>();
            foreach (ListItem li in dlGroups.Items)
                if (li.Selected)
                    groups.Add(li.Value);

            folder.Users = users;//.ToArray();
            folder.Groups = groups;//.ToArray();

            folder.DoAuthUpdate = true;

            try
            {
                int result = ES.Services.WebServers.UpdateHeliconApeFolder(PanelRequest.ItemID, folder);
                if (result < 0)
                {
                    ShowResultMessage(result);
                    return;
                }
            }
            catch (Exception ex)
            {
                ShowErrorMessage("WEB_UPDATE_HeliconApe_FOLDER", ex);
                return;
            }

            ReturnBack();
        }

        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            SaveFolder();
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
