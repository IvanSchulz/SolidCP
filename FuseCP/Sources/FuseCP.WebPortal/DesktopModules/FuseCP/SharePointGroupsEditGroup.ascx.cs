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

using FuseCP.EnterpriseServer;
using FuseCP.Providers.OS;

namespace FuseCP.Portal
{
    public partial class SharePointGroupsEditGroup : FuseCPModuleBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            btnDelete.Visible = (PanelRequest.ItemID > 0);

            if (!IsPostBack)
            {
                BindItem();
            }
        }

        private void BindUsers(int packageId)
        {
            try
            {
                SystemUser[] users = ES.Services.SharePointServers.GetSharePointUsers(packageId, false);
                dlUsers.DataSource = users;
                dlUsers.DataBind();
            }
            catch (Exception ex)
            {
                ShowErrorMessage("SHAREPOINT_GET_USER", ex);
                return;
            }
        }

        private void BindItem()
        {
            if (PanelRequest.ItemID == 0)
            {
                usernameControl.SetPackagePolicy(PanelSecurity.PackageId, UserSettings.SHAREPOINT_POLICY, "GroupNamePolicy");
                BindUsers(PanelSecurity.PackageId);
                return;
            }

            // load item
            SystemGroup item = null;
            try
            {
                item = ES.Services.SharePointServers.GetSharePointGroup(PanelRequest.ItemID);
            }
            catch (Exception ex)
            {
                ShowErrorMessage("SHAREPOINT_GET_GROUP", ex);
                return;
            }

            if (item == null)
                RedirectToBrowsePage();

            BindUsers(item.PackageId);
            usernameControl.SetPackagePolicy(item.PackageId, UserSettings.SHAREPOINT_POLICY, "GroupNamePolicy");
            usernameControl.Text = item.Name;
            usernameControl.EditMode = true;

            foreach (string user in item.Members)
            {
                ListItem li = dlUsers.Items.FindByValue(user);
                if (li != null)
                    li.Selected = true;
            }
        }

        private void SaveItem()
        {
            if (!Page.IsValid)
                return;

            // get form data
            SystemGroup item = new SystemGroup();
            item.Id = PanelRequest.ItemID;
            item.PackageId = PanelSecurity.PackageId;
            item.Name = usernameControl.Text;

            List<string> members = new List<string>();
            foreach (ListItem li in dlUsers.Items)
            {
                if (li.Selected)
                    members.Add(li.Value);
            }
            item.Members = members.ToArray();

            if (PanelRequest.ItemID == 0)
            {
                // new item
                try
                {
                    int result = ES.Services.SharePointServers.AddSharePointGroup(item);
                    if (result < 0)
                    {
                        ShowResultMessage(result);
                        return;
                    }
                }
                catch (Exception ex)
                {
                    ShowErrorMessage("SHAREPOINT_ADD_GROUP", ex);
                    return;
                }
            }
            else
            {
                // existing item
                try
                {
                    int result = ES.Services.SharePointServers.UpdateSharePointGroup(item);
                    if (result < 0)
                    {
                        ShowResultMessage(result);
                        return;
                    }
                }
                catch (Exception ex)
                {
                    ShowErrorMessage("SHAREPOINT_UPDATE_GROUP", ex);
                    return;
                }
            }

            // return
            RedirectSpaceHomePage();
        }

        private void DeleteItem()
        {
            // delete
            try
            {
                int result = ES.Services.SharePointServers.DeleteSharePointGroup(PanelRequest.ItemID);
                if (result < 0)
                {
                    ShowResultMessage(result);
                    return;
                }
            }
            catch (Exception ex)
            {
                ShowErrorMessage("SHAREPOINT_DELETE_GROUP", ex);
                return;
            }

            // return
            RedirectSpaceHomePage();
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            SaveItem();
        }
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            RedirectSpaceHomePage();
        }
        protected void btnDelete_Click(object sender, EventArgs e)
        {
            DeleteItem();
        }
    }
}
