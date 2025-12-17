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

using FuseCP.EnterpriseServer;

namespace FuseCP.Portal
{
    public partial class RestoreWizard : FuseCPModuleBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // save return page url
                if (Request.UrlReferrer != null)
                    ViewState["ReturnURL"] = Request.UrlReferrer.ToString();

                BindForm();
            }
        }

        private void BindForm()
        {
            if (PanelSecurity.SelectedUser.Role == UserRole.Administrator
                && PanelSecurity.PackageId < 2)
                ddlLocation.Items.Remove(ddlLocation.Items.FindByValue("1"));

            if (PanelSecurity.LoggedUser.Role != UserRole.Administrator)
                ddlLocation.Items.Remove(ddlLocation.Items.FindByValue("2"));

            string modeText = "{0}";
            string modeValue = "";

            if (PanelSecurity.PackageId > 0)
            {
                // load a single package
                PackageInfo restorePackage = ES.Services.Packages.GetPackage(PanelSecurity.PackageId);

                // load "store" packages
                PackageInfo[] packages = ES.Services.Packages.GetMyPackages(PanelSecurity.SelectedUser.UserId);
                foreach (PackageInfo package in packages)
                    ddlSpace.Items.Add(new ListItem(package.PackageName, package.PackageId.ToString()));
                ddlSpace.SelectedValue = PanelSecurity.PackageId.ToString();

                modeText = "Text.SpaceRestoreMode";
                modeValue = restorePackage.PackageName;
            }
            else if (PanelRequest.ServiceId > 0)
            {
                ddlLocation.Items.Remove(ddlLocation.Items.FindByValue("1"));

                ServiceInfo service = ES.Services.Servers.GetServiceInfo(PanelRequest.ServiceId);

                modeText = "Text.ServiceRestoreMode";
                modeValue = service.ServiceName;
            }
            else if (PanelRequest.ServerId > 0)
            {
                ddlLocation.Items.Remove(ddlLocation.Items.FindByValue("1"));

                ServerInfo server = ES.Services.Servers.GetServerById(PanelRequest.ServerId);

                modeText = "Text.ServerRestoreMode";
                modeValue = server.ServerName;
            }
            else if (PanelSecurity.SelectedUserId > 0)
            {
                // load user spaces
                PackageInfo[] packages = ES.Services.Packages.GetMyPackages(PanelSecurity.SelectedUserId);
                foreach (PackageInfo package in packages)
                    ddlSpace.Items.Add(new ListItem(package.PackageName, package.PackageId.ToString()));

                modeText = "Text.UserRestoreMode";
                modeValue = PanelSecurity.SelectedUser.Username;
            }

            // restore type
            litRestoreType.Text = String.Format(GetLocalizedString(modeText), modeValue);

            ToggleFormControls();
            InitFolderBrowser();
        }

        private void ToggleFormControls()
        {
            SpaceFolderPanel.Visible = (ddlLocation.SelectedValue == "1");
            ServerFolderPanel.Visible = (ddlLocation.SelectedValue == "2");
        }

        private void InitFolderBrowser()
        {
            int packageId = Utils.ParseInt(ddlSpace.SelectedValue, -1);
            if (packageId != -1)
                spaceFile.PackageId = packageId;
        }

        private void Restore()
        {
            if (!Page.IsValid)
                return;

            int userId = 0;
            int packageId = 0;
            int serviceId = 0;
            int serverId = 0;

            packageId = PanelSecurity.PackageId;
            if (packageId == -1)
                userId = PanelSecurity.SelectedUserId;

            serviceId = PanelRequest.ServiceId;
            if (serviceId == 0)
                serverId = PanelRequest.ServerId;

            // perform restore
            try
            {
                int result = ES.Services.Backup.Restore(true, TaskID, userId, packageId, serviceId, serverId,
                    Utils.ParseInt(ddlSpace.SelectedValue, 0),
                    spaceFile.SelectedFile,
                    txtServerPath.Text.Trim());

                if (result < 0)
                {
                    ShowResultMessage(result);
                    return;
                }
            }
            catch (Exception ex)
            {
                ShowErrorMessage("RESTORE_WIZARD", ex);
                return;
            }

            // show progress dialog
			AsyncTaskID = TaskID;
			AsyncTaskTitle = GetLocalizedString("Text.RestoreItems");
        }

        protected void ddlLocation_SelectedIndexChanged(object sender, EventArgs e)
        {
            ToggleFormControls();
        }

        protected void ddlSpace_SelectedIndexChanged(object sender, EventArgs e)
        {
            InitFolderBrowser();
        }

        protected void btnRestore_Click(object sender, EventArgs e)
        {
            Restore();
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            RedirectBack();
        }

        private void RedirectBack()
        {
            if (ViewState["ReturnURL"] != null)
                Response.Redirect((string)ViewState["ReturnURL"]);
            else
                RedirectToBrowsePage();
        }
    }
}
