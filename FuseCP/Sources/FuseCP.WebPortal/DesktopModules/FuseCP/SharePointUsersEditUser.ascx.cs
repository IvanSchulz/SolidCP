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
    public partial class SharePointUsersEditUser : FuseCPModuleBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            btnDelete.Visible = (PanelRequest.ItemID > 0);

            if (!IsPostBack)
            {
                BindItem();
            }
        }

        private void BindGroups(int packageId)
        {
            try
            {
                SystemGroup[] groups = ES.Services.SharePointServers.GetSharePointGroups(packageId, false);
                dlGroups.DataSource = groups;
                dlGroups.DataBind();
            }
            catch (Exception ex)
            {
                ShowErrorMessage("SHAREPOINT_GET_GROUP", ex);
            }
        }

        private void BindItem()
        {
            if (PanelRequest.ItemID == 0)
            {
                usernameControl.SetPackagePolicy(PanelSecurity.PackageId, UserSettings.SHAREPOINT_POLICY, "UserNamePolicy");
                passwordControl.SetPackagePolicy(PanelSecurity.PackageId, UserSettings.SHAREPOINT_POLICY, "UserPasswordPolicy");
                BindGroups(PanelSecurity.PackageId);
                return;
            }

            // load item
            SystemUser item = null;
            try
            {
                item = ES.Services.SharePointServers.GetSharePointUser(PanelRequest.ItemID);
            }
            catch (Exception ex)
            {
                ShowErrorMessage("SHAREPOINT_GET_USER", ex);
                return;
            }

            if (item == null)
                RedirectToBrowsePage();

            BindGroups(item.PackageId);
            usernameControl.SetPackagePolicy(item.PackageId, UserSettings.SHAREPOINT_POLICY, "UserNamePolicy");
            passwordControl.SetPackagePolicy(item.PackageId, UserSettings.SHAREPOINT_POLICY, "UserPasswordPolicy");
            usernameControl.Text = item.Name;
            usernameControl.EditMode = true;
            passwordControl.EditMode = true;

            foreach (string group in item.MemberOf)
            {
                ListItem li = dlGroups.Items.FindByValue(group);
                if (li != null)
                    li.Selected = true;
            }
        }

        private void SaveItem()
        {
            if (!Page.IsValid)
                return;

            // get form data
            SystemUser item = new SystemUser();
            item.Id = PanelRequest.ItemID;
            item.PackageId = PanelSecurity.PackageId;
            item.Name = usernameControl.Text;
            item.Password = passwordControl.Password;

            List<string> memberOf = new List<string>();
            foreach (ListItem li in dlGroups.Items)
            {
                if (li.Selected)
                    memberOf.Add(li.Value);
            }
            item.MemberOf = memberOf.ToArray();

            if (PanelRequest.ItemID == 0)
            {
                // new item
                try
                {
                    int result = ES.Services.SharePointServers.AddSharePointUser(item);
                    if (result < 0)
                    {
                        ShowResultMessage(result);
                        return;
                    }
                }
                catch (Exception ex)
                {
                    ShowErrorMessage("SHAREPOINT_ADD_USER", ex);
                    return;
                }
            }
            else
            {
                // existing item
                try
                {
                    int result = ES.Services.SharePointServers.UpdateSharePointUser(item);
                    if (result < 0)
                    {
                        ShowResultMessage(result);
                        return;
                    }
                }
                catch (Exception ex)
                {
                    ShowErrorMessage("SHAREPOINT_UPDATE_USER", ex);
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
                int result = ES.Services.SharePointServers.DeleteSharePointUser(PanelRequest.ItemID);
                if (result < 0)
                {
                    ShowResultMessage(result);
                    return;
                }
            }
            catch (Exception ex)
            {
                ShowErrorMessage("SHAREPOINT_DELETE_USER", ex);
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
