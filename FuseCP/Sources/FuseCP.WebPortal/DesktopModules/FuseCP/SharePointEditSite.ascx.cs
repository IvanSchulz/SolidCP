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
using FuseCP.Providers.SharePoint;

namespace FuseCP.Portal
{
    public partial class SharePointEditSite : FuseCPModuleBase
    {
        SharePointSite item = null;

        protected void Page_Load(object sender, EventArgs e)
        {
            bool newItem = (PanelRequest.ItemID == 0);

            tblEditItem.Visible = newItem;
            tblViewItem.Visible = !newItem;

            btnUpdate.Visible = newItem;
            btnDelete.Visible = !newItem;
            btnUpdate.Text = newItem ? GetLocalizedString("Text.Add") : GetLocalizedString("Text.Update");
            btnBackup.Enabled = btnRestore.Enabled = btnWebParts.Enabled = !newItem;

            // bind item
            BindItem();
        }

        private void BindDatabaseVersions()
        {
            List<string> versions = new List<string>();
            versions.Add(ResourceGroups.MsSql2000);
            versions.Add(ResourceGroups.MsSql2005);
            versions.Add(ResourceGroups.MsSql2008);
            versions.Add(ResourceGroups.MsSql2012);
            versions.Add(ResourceGroups.MsSql2014);
            versions.Add(ResourceGroups.MsSql2016);
            versions.Add(ResourceGroups.MsSql2017);
            versions.Add(ResourceGroups.MsSql2019);
			versions.Add(ResourceGroups.MsSql2022);
			versions.Add(ResourceGroups.MsSql2025);

			FillDatabaseVersions(PanelSecurity.PackageId, ddlDatabaseVersion.Items, versions);
        }

        private void BindItem()
        {
            try
            {
                if (!IsPostBack)
                {
                    // load item if required
                    if (PanelRequest.ItemID > 0)
                    {
                        // existing item
                        item = ES.Services.SharePointServers.GetSharePointSite(PanelRequest.ItemID);
                        if (item != null)
                        {
                            // save package info
                            ViewState["PackageId"] = item.PackageId;
                        }
                        else
                            RedirectToBrowsePage();
                    }
                    else
                    {
                        // new item
                        ViewState["PackageId"] = PanelSecurity.PackageId;
                        databaseName.SetPackagePolicy(PanelSecurity.PackageId, UserSettings.MSSQL_POLICY, "DatabaseNamePolicy");
                        databaseUser.SetPackagePolicy(PanelSecurity.PackageId, UserSettings.MSSQL_POLICY, "UserNamePolicy");
                        databasePassword.SetPackagePolicy(PanelSecurity.PackageId, UserSettings.MSSQL_POLICY, "UserPasswordPolicy");
                        BindDatabaseVersions();
                        BindWebSites(PanelSecurity.PackageId);
                        BindUsers(PanelSecurity.PackageId);
                    }
                }

                if (!IsPostBack)
                {
                    // bind item to controls
                    if (item != null)
                    {
                        // bind item to controls
                        litWebSite.Text = item.Name;
                        litLocaleID.Text = (item.LocaleID == 0) ? "1033" : item.LocaleID.ToString();
                        litSiteOwner.Text = item.OwnerLogin;
                        litOwnerEmail.Text = item.OwnerEmail;
                        litDatabaseName.Text = item.DatabaseName;
                        litDatabaseUser.Text = item.DatabaseUser;
                    }
                }

            }
            catch
            {
                ShowWarningMessage("INIT_SERVICE_ITEM_FORM");
                DisableFormControls(this, btnCancel);
                return;
            }
        }

        private void BindWebSites(int packageId)
        {
            ddlWebSites.DataSource = ES.Services.WebServers.GetWebSites(packageId, false);
            ddlWebSites.DataBind();
            ddlWebSites.Items.Insert(0, new ListItem(GetLocalizedString("Text.SelectSite"), ""));
        }

        private void BindUsers(int packageId)
        {
            ddlSiteOwner.DataSource = ES.Services.SharePointServers.GetSharePointUsers(packageId, false);
            ddlSiteOwner.DataBind();
            ddlSiteOwner.Items.Insert(0, new ListItem(GetLocalizedString("Text.SelectUser"), ""));
        }

        private void SaveItem()
        {
            if (!Page.IsValid)
                return;

            // get form data
            item = new SharePointSite();
            item.Id = PanelRequest.ItemID;
            item.PackageId = PanelSecurity.PackageId;
            item.Name = ddlWebSites.SelectedValue;
            item.LocaleID = Utils.ParseInt(txtLocaleID.Text.Trim(), 0);
            item.OwnerLogin = ddlSiteOwner.SelectedValue;
            item.OwnerEmail = txtOwnerEmail.Text;
            item.DatabaseGroupName = ddlDatabaseVersion.SelectedValue;
            item.DatabaseName = databaseName.Text;
            item.DatabaseUser = databaseUser.Text;
            item.DatabasePassword = databasePassword.Password;

            if (PanelRequest.ItemID == 0)
            {
                // new item
                try
                {
                    int result = ES.Services.SharePointServers.AddSharePointSite(item);
                    if (result < 0)
                    {
                        ShowResultMessage(result);
                        return;
                    }

                }
                catch (Exception ex)
                {
                    ShowErrorMessage("SHAREPOINT_ADD_SITE", ex);
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
                int result = ES.Services.SharePointServers.DeleteSharePointSite(PanelRequest.ItemID);
                if (result < 0)
                {
                    ShowResultMessage(result);
                    return;
                }
            }
            catch (Exception ex)
            {
                ShowErrorMessage("SHAREPOINT_DELETE_SITE", ex);
                return;
            }

            // return
            RedirectSpaceHomePage();
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            // return
            RedirectSpaceHomePage();
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            DeleteItem();
        }
        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            SaveItem();
        }

        protected void btnBackup_Click(object sender, EventArgs e)
        {
            Response.Redirect(EditUrl("ItemID", PanelRequest.ItemID.ToString(), "backup",
                PortalUtils.SPACE_ID_PARAM + "=" + PanelSecurity.PackageId.ToString()));
        }

        protected void btnRestore_Click(object sender, EventArgs e)
        {
            Response.Redirect(EditUrl("ItemID", PanelRequest.ItemID.ToString(), "restore",
                PortalUtils.SPACE_ID_PARAM + "=" + PanelSecurity.PackageId.ToString()));
        }

        protected void btnWebParts_Click(object sender, EventArgs e)
        {
            Response.Redirect(EditUrl("ItemID", PanelRequest.ItemID.ToString(), "webparts",
                PortalUtils.SPACE_ID_PARAM + "=" + PanelSecurity.PackageId.ToString()));
        }
    }
}
