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
    public partial class WebSitesAddSite : FuseCPModuleBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // set controls
                domainsSelectDomainControl.PackageId = PanelSecurity.PackageId;

                // bind IP Addresses
                BindIPAddresses();

                BindIgnoreZoneTemplate();

                // toggle
                ToggleControls();
            }
        }

        private void ToggleControls()
        {
            PackageContext cntx = PackagesHelper.GetCachedPackageContext(PanelSecurity.PackageId);

            rowDedicatedIP.Visible = rbDedicatedIP.Checked;
            
            if (Utils.CheckQouta(Quotas.WEB_ENABLEHOSTNAMESUPPORT, cntx))
            {
                txtHostName.Visible = chkIgnoreGlobalDNSRecords.Visible = lblIgnoreGlobalDNSRecords.Visible = lblTheDotInTheMiddle.Visible = true;
                UserSettings settings = ES.Services.Users.GetUserSettings(PanelSecurity.LoggedUserId, UserSettings.WEB_POLICY);
                txtHostName.Text = String.IsNullOrEmpty(settings["HostName"]) ? "" : settings["HostName"];
                chkIgnoreGlobalDNSRecords.Checked = false;
            }
            else
            {
                txtHostName.Visible = chkIgnoreGlobalDNSRecords.Visible = lblIgnoreGlobalDNSRecords.Visible = lblTheDotInTheMiddle.Visible = false;
                chkIgnoreGlobalDNSRecords.Checked = true;
                txtHostName.Text = "";
                domainsSelectDomainControl.HideWebSites = true;
            }

        }

        private void BindIgnoreZoneTemplate()
        {
            chkIgnoreGlobalDNSRecords.Checked = false;
        }

        private void BindIPAddresses()
        {
            ddlIpAddresses.Items.Add(new ListItem("<Select IP>", ""));

            PackageIPAddress[] ips = ES.Services.Servers.GetPackageUnassignedIPAddresses(PanelSecurity.PackageId, 0, IPAddressPool.WebSites);
            foreach (PackageIPAddress ip in ips)
            {
                string fullIP = ip.ExternalIP;
                if (ip.InternalIP != null &&
                    ip.InternalIP != "" &&
                    ip.InternalIP != ip.ExternalIP)
                    fullIP += " (" + ip.InternalIP + ")";

                ddlIpAddresses.Items.Add(new ListItem(fullIP, ip.PackageAddressID.ToString()));
            }

            rowSiteIP.Visible = (ddlIpAddresses.Items.Count > 1);
            rbDedicatedIP.Enabled = (ddlIpAddresses.Items.Count > 1);
        }

        private void AddWebSite()
        {
            int siteItemId = 0;

            try
            {
                int packageAddressId = rbDedicatedIP.Checked ? Utils.ParseInt(ddlIpAddresses.SelectedValue, 0) : 0;

                siteItemId = ES.Services.WebServers.AddWebSite(PanelSecurity.PackageId, txtHostName.Text.ToLower(), domainsSelectDomainControl.DomainId,
                    packageAddressId, !chkIgnoreGlobalDNSRecords.Checked);

                if (siteItemId < 0)
                {
                    ShowResultMessage(siteItemId);
                    return;
                }
            }
            catch (Exception ex)
            {
                ShowErrorMessage("WEB_ADD_SITE", ex);
                return;
            }

            // go to edit site
            Response.Redirect(EditUrl("ItemID", siteItemId.ToString(), "edit_item",
                PortalUtils.SPACE_ID_PARAM + "=" + PanelSecurity.PackageId.ToString()));
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            RedirectSpaceHomePage();
        }
        protected void rbIP_CheckedChanged(object sender, EventArgs e)
        {
            ToggleControls();
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            AddWebSite();
        }
    }
}
