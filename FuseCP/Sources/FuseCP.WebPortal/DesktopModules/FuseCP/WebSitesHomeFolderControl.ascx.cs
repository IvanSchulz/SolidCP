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
using FuseCP.Providers.Web;

namespace FuseCP.Portal
{
    public partial class WebSitesHomeFolderControl : FuseCPControlBase
    {
        private bool isAppVirtualDirectory;
        public bool IsAppVirtualDirectory
        {
            get { return isAppVirtualDirectory; }
            set { isAppVirtualDirectory = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            rowDedicatedPool.Visible = !IsAppVirtualDirectory;
        }

        public void BindWebItemNonApp(int packageId, WebVirtualDirectory item)
        {
            fileLookup.PackageId = item.PackageId;
            fileLookup.SelectedFile = item.ContentPath;

            string resSuffix = item.IIs7 ? "IIS7" : "";

            chkDirectoryBrowsing.Checked = item.EnableDirectoryBrowsing;

            chkWrite.Checked = item.EnableWritePermissions;


            chkAuthAnonymous.Checked = item.EnableAnonymousAccess;
            chkAuthWindows.Checked = item.EnableWindowsAuthentication;
            chkAuthBasic.Checked = item.EnableBasicAuthentication;
            chkDynamicCompression.Checked = item.EnableDynamicCompression;
            chkStaticCompression.Checked = item.EnableStaticCompression;

            ToggleLocationControls();
           

        }

        public void BindWebItem(int packageId, WebAppVirtualDirectory item)
        {
            fileLookup.PackageId = item.PackageId;
            fileLookup.SelectedFile = item.ContentPath;

            string resSuffix = item.IIs7 ? "IIS7" : "";

            chkRedirectExactUrl.Text = GetLocalizedString("chkRedirectExactUrl.Text" + resSuffix);
            chkRedirectDirectoryBelow.Text = GetLocalizedString("chkRedirectDirectoryBelow.Text" + resSuffix);
            chkRedirectPermanent.Text = GetLocalizedString("chkRedirectPermanent.Text" + resSuffix);

            chkRedirectExactUrl.Checked = item.RedirectExactUrl;
            chkRedirectDirectoryBelow.Checked = item.RedirectDirectoryBelow;
            chkRedirectPermanent.Checked = item.RedirectPermanent;

            chkDirectoryBrowsing.Checked = item.EnableDirectoryBrowsing;
            chkParentPaths.Checked = item.EnableParentPaths;
            chkWrite.Checked = item.EnableWritePermissions;

            chkDedicatedPool.Checked = item.DedicatedApplicationPool;
            chkDedicatedPool.Enabled = !item.SharePointInstalled;

            chkAuthAnonymous.Checked = item.EnableAnonymousAccess;
            chkAuthWindows.Checked = item.EnableWindowsAuthentication;
            chkAuthBasic.Checked = item.EnableBasicAuthentication;
            chkDynamicCompression.Checked = item.EnableDynamicCompression;
            chkStaticCompression.Checked = item.EnableStaticCompression;

            // default documents
            txtDefaultDocs.Text = String.Join("\n", item.DefaultDocs.Split(',', ';'));

            // redirection
            txtRedirectUrl.Text = item.HttpRedirect;
            bool redirectionEnabled = !String.IsNullOrEmpty(item.HttpRedirect);
            rbLocationFolder.Checked = !redirectionEnabled;
            rbLocationRedirect.Checked = redirectionEnabled;
            valRedirection.Enabled = redirectionEnabled;

            // store app pool value
            ViewState["ApplicationPool"] = item.ApplicationPool;

            ToggleLocationControls();

            // toggle controls by quotas
            fileLookup.Enabled = PackagesHelper.CheckGroupQuotaEnabled(packageId, ResourceGroups.Web, Quotas.WEB_HOMEFOLDERS);
            rbLocationRedirect.Visible = PackagesHelper.CheckGroupQuotaEnabled(packageId, ResourceGroups.Web, Quotas.WEB_REDIRECTIONS);
            bool customSecurity = PackagesHelper.CheckGroupQuotaEnabled(packageId, ResourceGroups.Web, Quotas.WEB_SECURITY);
            chkWrite.Visible = customSecurity;
            // hide authentication options if not allowed
            pnlCustomAuth.Visible = customSecurity;
            //
            chkDedicatedPool.Visible = PackagesHelper.CheckGroupQuotaEnabled(packageId, ResourceGroups.Web, Quotas.WEB_APPPOOLS);
            pnlDefaultDocuments.Visible = PackagesHelper.CheckGroupQuotaEnabled(packageId, ResourceGroups.Web, Quotas.WEB_DEFAULTDOCS);

            UserSettings settings = ES.Services.Users.GetUserSettings(PanelSecurity.SelectedUserId, "WebPolicy");
            if (Utils.ParseBool(settings["EnableDedicatedPool"], false) == true)
                chkDedicatedPool.Checked = true;

            chkDedicatedPool.Enabled = !(Utils.ParseBool(settings["EnableDedicatedPool"], false));

        }

        public void SaveWebItemNonApp(WebVirtualDirectory item)
        {
            item.ContentPath = fileLookup.SelectedFile;

            item.EnableDirectoryBrowsing = chkDirectoryBrowsing.Checked;

            item.EnableWritePermissions = chkWrite.Checked;

            item.EnableAnonymousAccess = chkAuthAnonymous.Checked;
            item.EnableWindowsAuthentication = chkAuthWindows.Checked;
            item.EnableBasicAuthentication = chkAuthBasic.Checked;
            item.EnableDynamicCompression = chkDynamicCompression.Checked;
            item.EnableStaticCompression = chkStaticCompression.Checked;

        }

        public void SaveWebItem(WebAppVirtualDirectory item)
        {
            item.ContentPath = fileLookup.SelectedFile;
            item.RedirectExactUrl = chkRedirectExactUrl.Checked;
            item.RedirectDirectoryBelow = chkRedirectDirectoryBelow.Checked;
            item.RedirectPermanent = chkRedirectPermanent.Checked;

            item.EnableDirectoryBrowsing = chkDirectoryBrowsing.Checked;
            item.EnableParentPaths = chkParentPaths.Checked;
            item.EnableWritePermissions = chkWrite.Checked;
            item.DedicatedApplicationPool = chkDedicatedPool.Checked;

            item.EnableAnonymousAccess = chkAuthAnonymous.Checked;
            item.EnableWindowsAuthentication = chkAuthWindows.Checked;
            item.EnableBasicAuthentication = chkAuthBasic.Checked;
            item.EnableDynamicCompression = chkDynamicCompression.Checked;
            item.EnableStaticCompression = chkStaticCompression.Checked;


            // default documents
            item.DefaultDocs = String.Join(",", Utils.ParseDelimitedString(txtDefaultDocs.Text, '\n', '\r', ';', ','));
            
            // redirection
            item.HttpRedirect = rbLocationRedirect.Checked ? txtRedirectUrl.Text : "";

            // set app pool
            item.ApplicationPool = (string)ViewState["ApplicationPool"];
        }

        private void ToggleLocationControls()
        {
            tblFolder.Visible = rbLocationFolder.Checked;
            tblRedirect.Visible = rbLocationRedirect.Checked;
        }

        protected void rbLocationFolder_CheckedChanged(object sender, EventArgs e)
        {
            txtRedirectUrl.Text = "";
            ToggleLocationControls();
        }
        protected void rbLocationRedirect_CheckedChanged(object sender, EventArgs e)
        {
            txtRedirectUrl.Text = "http://";
            ToggleLocationControls();
        }
    }
}
