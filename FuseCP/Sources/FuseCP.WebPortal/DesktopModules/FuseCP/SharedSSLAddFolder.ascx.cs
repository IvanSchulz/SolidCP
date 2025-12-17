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
    public partial class SharedSSLAddFolder : FuseCPModuleBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindDomains();

                BindWebSites();

                fileLookup.PackageId = PanelSecurity.PackageId;
                virtDirName.SetPackagePolicy(PanelSecurity.PackageId, UserSettings.WEB_POLICY, "VirtDirNamePolicy");
            }
        }

        private void BindWebSites()
        {
            WebSite[] webSites = ES.Services.WebServers.GetWebSites(PanelSecurity.PackageId, true);

            ddlWebSites.DataSource = webSites;
            ddlWebSites.DataTextField = "Name";
            ddlWebSites.DataValueField = "Id";
            ddlWebSites.DataBind();
            ddlWebSites.Items.Insert(0, new ListItem(GetLocalizedString("SelectWebSite.Text"), ""));
        }


        private void BindDomains()
        {
            string[] sslDomains = ES.Services.WebServers.GetSharedSSLDomains(PanelSecurity.PackageId);
            ddlDomains.DataSource = sslDomains;
            ddlDomains.DataBind();
            ddlDomains.Items.Insert(0, new ListItem(GetLocalizedString("SelectDomain.Text"), ""));
            
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid)
                return;

            string dirName = virtDirName.Text;

            int siteId = 0;
            if (!Int32.TryParse(ddlWebSites.SelectedValue, out siteId))
            {
                siteId = 0;
            }

            int result = 0;
            try
            {
                

                result = ES.Services.WebServers.AddSharedSSLFolder(PanelSecurity.PackageId, ddlDomains.SelectedValue,
                    siteId, dirName, fileLookup.SelectedFile);

                if (result < 0)
                {
                    ShowResultMessage(result);
                    return;
                }
            }
            catch (Exception ex)
            {
                ShowErrorMessage("WEB_ADD_VDIR", ex);
                return;
            }

            // redirect to directory edit page
            Response.Redirect(EditUrl("ItemID", result.ToString(), "edit_item",
                PortalUtils.SPACE_ID_PARAM + "=" + PanelSecurity.PackageId.ToString()));
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            RedirectSpaceHomePage();
        }
    }
}
