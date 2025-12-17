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
using FuseCP.EnterpriseServer;

namespace FuseCP.Portal
{
    public partial class SharedSSLEditFolder : FuseCPModuleBase
    {
        class Tab
        {
            string name;
            bool enabled;

            public Tab(string name, bool enabled)
            {
                this.name = name;
                this.enabled = enabled;
            }

            public string Name
            {
                get { return this.name; }
                set { this.name = value; }
            }

            public bool Enabled
            {
                get { return this.enabled; }
                set { this.enabled = value; }
            }
        }

        private int PackageId
        {
            get { return (int)ViewState["PackageId"]; }
            set { ViewState["PackageId"] = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindVirtualDir();
            }
        }

        private void BindTabs()
        {
            Tab[] tabsArray = new Tab[]
            {
                new Tab(GetLocalizedString("Tab.HomeFolder"), true),
                new Tab(GetLocalizedString("Tab.Extensions"), true),
                new Tab(GetLocalizedString("Tab.CustomErrors"),
                    PackagesHelper.CheckGroupQuotaEnabled(PackageId, ResourceGroups.Web, Quotas.WEB_ERRORS)),
                new Tab(GetLocalizedString("Tab.CustomHeaders"),
                    PackagesHelper.CheckGroupQuotaEnabled(PackageId, ResourceGroups.Web, Quotas.WEB_HEADERS)),
                new Tab(GetLocalizedString("Tab.MIMETypes"),
                    PackagesHelper.CheckGroupQuotaEnabled(PackageId, ResourceGroups.Web, Quotas.WEB_MIME))
            };

            if (dlTabs.SelectedIndex == -1)
                dlTabs.SelectedIndex = 0;

            dlTabs.DataSource = tabsArray;
            dlTabs.DataBind();

            tabs.ActiveViewIndex = dlTabs.SelectedIndex;
        }

        protected void dlTabs_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindTabs();
        }

        private void BindVirtualDir()
        {
            SharedSSLFolder vdir = null;
            try
            {
                vdir = ES.Services.WebServers.GetSharedSSLFolder(PanelRequest.ItemID);
            }
            catch (Exception ex)
            {
                ShowErrorMessage("WEB_GET_VDIR", ex);
                return;
            }

            if (vdir == null)
                RedirectSpaceHomePage();

            PackageId = vdir.PackageId;

            // bind site
            string fullName = vdir.Name;
            lnkSiteName.Text = "https://" + fullName;
            lnkSiteName.NavigateUrl = "https://" + fullName;

            // bind controls
            webSitesHomeFolderControl.BindWebItem(PackageId, vdir);
            webSitesExtensionsControl.BindWebItem(PackageId, vdir);
            webSitesMimeTypesControl.BindWebItem(vdir);
            webSitesCustomHeadersControl.BindWebItem(vdir);
            webSitesCustomErrorsControl.BindWebItem(vdir);

			// bind tabs
			BindTabs();
        }

        private void SaveVirtualDir()
        {
            if (!Page.IsValid)
                return;

            // load original web site item
            SharedSSLFolder vdir = ES.Services.WebServers.GetSharedSSLFolder(PanelRequest.ItemID);

            // other controls
            webSitesExtensionsControl.SaveWebItem(vdir);
            webSitesHomeFolderControl.SaveWebItem(vdir);
            webSitesMimeTypesControl.SaveWebItem(vdir);
            webSitesCustomHeadersControl.SaveWebItem(vdir);
            webSitesCustomErrorsControl.SaveWebItem(vdir);

            // update web site
            try
            {
                int result = ES.Services.WebServers.UpdateSharedSSLFolder(vdir);
                if (result < 0)
                {
                    ShowResultMessage(result);
                    return;
                }
            }
            catch (Exception ex)
            {
                ShowErrorMessage("WEB_UPDATE_VDIR", ex);
                return;
            }

            RedirectSpaceHomePage();
        }

        private void DeleteVirtualDir()
        {
            try
            {
                int result = ES.Services.WebServers.DeleteSharedSSLFolder(PanelRequest.ItemID);
                if (result < 0)
                {
                    ShowResultMessage(result);
                    return;
                }
            }
            catch (Exception ex)
            {
                ShowErrorMessage("WEB_DELETE_VDIR", ex);
                return;
            }

            RedirectSpaceHomePage();
        }

        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            SaveVirtualDir();
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            RedirectSpaceHomePage();
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            DeleteVirtualDir();
        }
    }
}
