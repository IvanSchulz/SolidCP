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
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

using FuseCP.Providers.Web;

namespace FuseCP.Portal
{
    public partial class WebSitesSecuredFoldersControl : FuseCPControlBase
    {
        private bool IsSecuredFoldersInstalled
        {
            get { return ViewState["IsSecuredFoldersInstalled"] != null ? (bool)ViewState["IsSecuredFoldersInstalled"] : false; }
            set { ViewState["IsSecuredFoldersInstalled"] = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
        }

		public void BindSecuredFolders(WebSite site)
        {
            // save initial state
            IsSecuredFoldersInstalled = site.SecuredFoldersInstalled;
			// Render a warning message about the automatic site's settings change
			if (!IsSecuredFoldersInstalled && site.IIs7)
			{
				// Ensure the message is displayed only when neccessary
				if (site.EnableWindowsAuthentication || !site.AspNetInstalled.EndsWith("I"))
				{
					string warningStr = GetLocalizedString("EnableFoldersIIs7Warning.Text");
					// Render a warning only if specified
					if (!String.IsNullOrEmpty(warningStr))
						btnToggleSecuredFolders.OnClientClick = String.Format("return confirm('{0}')", warningStr);
				}
			}
            // toggle
            ToggleControls();
        }

        private void ToggleControls()
        {
            // toggle button
            btnToggleSecuredFolders.Text = GetLocalizedString(
                IsSecuredFoldersInstalled ? "DisableFolders.Text" : "EnableFolders.Text");

            // show hide panel
            SecuredFoldersPanel.Visible = IsSecuredFoldersInstalled;

            // bind items
            if (IsSecuredFoldersInstalled)
            {
                BindFolders();
                BindUsers();
                BindGroups();
            }
        }

        private void BindFolders()
        {
            gvFolders.DataSource = ES.Services.WebServers.GetSecuredFolders(PanelRequest.ItemID);
            gvFolders.DataBind();
        }

        private void BindUsers()
        {
            gvUsers.DataSource = ES.Services.WebServers.GetSecuredUsers(PanelRequest.ItemID);
            gvUsers.DataBind();
        }

        private void BindGroups()
        {
            gvGroups.DataSource = ES.Services.WebServers.GetSecuredGroups(PanelRequest.ItemID);
            gvGroups.DataBind();
        }

        protected void btnToggleSecuredFolders_Click(object sender, EventArgs e)
        {
            try
            {
                int result = 0;
                if (IsSecuredFoldersInstalled)
                {
                    // uninstall folders
                    result = ES.Services.WebServers.UninstallSecuredFolders(PanelRequest.ItemID);
                }
                else
                {
                    // install folders
                    result = ES.Services.WebServers.InstallSecuredFolders(PanelRequest.ItemID);
                }

                if (result < 0)
                {
                    HostModule.ShowResultMessage(result);
                    return;
                }
            }
            catch (Exception ex)
            {
                HostModule.ShowErrorMessage("WEB_INSTALL_FOLDERS", ex);
                return;
            }

            // change state
            IsSecuredFoldersInstalled = !IsSecuredFoldersInstalled;

            // bind items
            ToggleControls();
        }

        public string GetEditUrl(string ctrlKey, string name)
        {
            name = Server.UrlEncode(name);
            return HostModule.EditUrl("ItemID", PanelRequest.ItemID.ToString(), ctrlKey,
                "Name=" + name);
        }

        protected void btnAddFolder_Click(object sender, EventArgs e)
        {
            RedirectToEditControl("edit_webfolder", "");
        }

        protected void btnAddUser_Click(object sender, EventArgs e)
        {
            RedirectToEditControl("edit_webuser", "");
        }

        protected void btnAddGroup_Click(object sender, EventArgs e)
        {
            RedirectToEditControl("edit_webgroup", "");
        }

        public void RedirectToEditControl(string ctrlKey, string name)
        {
            Response.Redirect(GetEditControlUrl(ctrlKey, name));
        }

        public string GetEditControlUrl(string ctrlKey, string name)
        {
            return HostModule.EditUrl("ItemID", PanelRequest.ItemID.ToString(), ctrlKey,
                "Name=" + name,
                PortalUtils.SPACE_ID_PARAM + "=" + PanelSecurity.PackageId.ToString());
        }

        private bool DeleteFolder(string name)
        {
            try
            {
                int result = ES.Services.WebServers.DeleteSecuredFolder(PanelRequest.ItemID, name);
                if (result < 0)
                {
                    HostModule.ShowResultMessage(result);
                    return false;
                }
            }
            catch (Exception ex)
            {
                HostModule.ShowErrorMessage("WEB_DELETE_SECURED_FOLDER", ex);
                return false;
            }
            return true;
        }

        private bool DeleteUser(string name)
        {
            try
            {
                int result = ES.Services.WebServers.DeleteSecuredUser(PanelRequest.ItemID, name);
                if (result < 0)
                {
                    HostModule.ShowResultMessage(result);
                    return false;
                }
            }
            catch (Exception ex)
            {
                HostModule.ShowErrorMessage("WEB_DELETE_SECURED_USER", ex);
                return false;
            }
            return true;
        }

        private bool DeleteGroup(string name)
        {
            try
            {
                int result = ES.Services.WebServers.DeleteSecuredGroup(PanelRequest.ItemID, name);
                if (result < 0)
                {
                    HostModule.ShowResultMessage(result);
                    return false;
                }
            }
            catch (Exception ex)
            {
                HostModule.ShowErrorMessage("WEB_DELETE_SECURED_GROUP", ex);
                return false;
            }
            return true;
        }

        protected void gvFolders_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            // delete folder
            string folderName = (string)gvFolders.DataKeys[e.RowIndex][0];

            if (DeleteFolder(folderName))
            {
                // reb-bind folders
                BindFolders();
            }

            // cancel command
            e.Cancel = true;
        }

        protected void gvUsers_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            // delete user
            string name = (string)gvUsers.DataKeys[e.RowIndex][0];

            if (DeleteUser(name))
            {
                // reb-bind users
                BindUsers();
            }

            // cancel command
            e.Cancel = true;
        }

        protected void gvGroups_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            // delete group
            string name = (string)gvGroups.DataKeys[e.RowIndex][0];

            if (DeleteGroup(name))
            {
                // reb-bind groups
                BindGroups();
            }

            // cancel command
            e.Cancel = true;
        }
    }
}
