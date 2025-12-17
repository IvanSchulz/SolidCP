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
using FuseCP.Providers.Web;

namespace FuseCP.Portal
{
    public partial class WebSitesEditVirtualDir : FuseCPModuleBase
    {

        class Tab
        {
            int index;
            string id;
            string name;

            public Tab(int index, string id, string name)
            {
                this.index = index;
                this.id = id;
                this.name = name;
            }

            public int Index
            {
                get { return this.index; }
                set { this.index = value; }
            }

            public string Id
            {
                get { return this.id; }
                set { this.id = value; }
            }

            public string Name
            {
                get { return this.name; }
                set { this.name = value; }
            }
        }

        private int PackageId
        {
            get { return (int)ViewState["PackageId"]; }
            set { ViewState["PackageId"] = value; }
        }

        private bool IIs7
        {
            get { return (bool)ViewState["IIs7"]; }
            set { ViewState["IIs7"] = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindVirtualDir();

                BindTabs();
            }
        }

        private void BindTabs()
        {
            List<Tab> tabsList = new List<Tab>();
            tabsList.Add(new Tab(0, "home", GetLocalizedString("Tab.HomeFolder")));
            //tabsList.Add(new Tab(1, "extensions", GetLocalizedString("Tab.Extensions")));
            //if (PackagesHelper.CheckGroupQuotaEnabled(PackageId, ResourceGroups.Web, Quotas.WEB_ERRORS))
            //    tabsList.Add(new Tab(2, "errors", GetLocalizedString("Tab.CustomErrors")));
            //if (PackagesHelper.CheckGroupQuotaEnabled(PackageId, ResourceGroups.Web, Quotas.WEB_HEADERS))
            //    tabsList.Add(new Tab(3, "headers", GetLocalizedString("Tab.CustomHeaders")));
            //if (PackagesHelper.CheckGroupQuotaEnabled(PackageId, ResourceGroups.Web, Quotas.WEB_MIME))
            //    tabsList.Add(new Tab(4, "mime", GetLocalizedString("Tab.MIMETypes")));

            if (dlTabs.SelectedIndex == -1)
                dlTabs.SelectedIndex = 0;

            dlTabs.DataSource = tabsList.ToArray();
            dlTabs.DataBind();

            tabs.ActiveViewIndex = tabsList[dlTabs.SelectedIndex].Index;
        }

        protected void dlTabs_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindTabs();
        }

        private void BindVirtualDir()
        {
            WebVirtualDirectory vdir = null;
            try
            {
                vdir = ES.Services.WebServers.GetVirtualDirectory(PanelRequest.ItemID, PanelRequest.VirtDir);
            }
            catch (Exception ex)
            {
                ShowErrorMessage("WEB_GET_VDIR", ex);
                return;
            }

            if (vdir == null)
                RedirectToBrowsePage();

            // IIS 7.0 mode
            IIs7 = vdir.IIs7;

            PackageId = vdir.PackageId;

            // bind site
            string fullName = vdir.ParentSiteName + "/" + vdir.Name;
            lnkSiteName.Text = fullName;
            lnkSiteName.NavigateUrl = "http://" + fullName;

            // bind controls
            VirtualDirectoryHomeFolderControl.BindWebItem(PackageId, vdir);
            //webSitesExtensionsControl.BindWebItem(PackageId, vdir);
            //webSitesMimeTypesControl.BindWebItem(vdir);
            //webSitesCustomHeadersControl.BindWebItem(vdir);
            //webSitesCustomErrorsControl.BindWebItem(vdir);
        }

        private void SaveVirtualDir()
        {
            if (!Page.IsValid)
                return;

            // load original web site item
            WebVirtualDirectory vdir = ES.Services.WebServers.GetVirtualDirectory(PanelRequest.ItemID, PanelRequest.VirtDir);

            // other controls
            //webSitesExtensionsControl.SaveWebItem(vdir);
            VirtualDirectoryHomeFolderControl.SaveWebItem(vdir);
            //webSitesMimeTypesControl.SaveWebItem(vdir);
            //webSitesCustomHeadersControl.SaveWebItem(vdir);
            //webSitesCustomErrorsControl.SaveWebItem(vdir);

            // update web site
            try
            {
                int result = ES.Services.WebServers.UpdateVirtualDirectory(PanelRequest.ItemID, vdir);
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

            ReturnBack();
        }

        private void DeleteVirtualDir()
        {
            try
            {
                int result = ES.Services.WebServers.DeleteVirtualDirectory(PanelRequest.ItemID, PanelRequest.VirtDir);
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

            ReturnBack();
        }

        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            SaveVirtualDir();
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            ReturnBack();
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            DeleteVirtualDir();
        }

        private void ReturnBack()
        {
            Response.Redirect(EditUrl("ItemID", PanelRequest.ItemID.ToString(), "edit_item",
                "MenuID=vdirs",
                PortalUtils.SPACE_ID_PARAM + "=" + PanelSecurity.PackageId.ToString()));
        }
    }
}
