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
using System.Collections.Generic;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Collections.Specialized;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using FuseCP.EnterpriseServer;

namespace FuseCP.Portal.ProviderControls
{
    public partial class MSFTP70_Settings : FuseCPControlBase, IHostingServiceProviderSettings
    {
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        public void BindSettings(StringDictionary settings)
        {
			int selectedAddressid = this.FindAddressByText(settings["SharedIP"]);
			ipAddress.AddressId = (selectedAddressid > 0) ? selectedAddressid : 0;
            BindSiteId(settings);
            txtAdFtpRoot.Text = settings["AdFtpRoot"];
            txtFtpGroupName.Text = settings["FtpGroupName"];
			chkBuildUncFilesPath.Checked = Utils.ParseBool(settings["BuildUncFilesPath"], false);
            ActiveDirectoryIntegration.BindSettings(settings);
        }

        public void SaveSettings(StringDictionary settings)
        {
			if (ipAddress.AddressId > 0)
			{
				IPAddressInfo address = ES.Services.Servers.GetIPAddress(ipAddress.AddressId);
				if (String.IsNullOrEmpty(address.InternalIP))
				{
					settings["SharedIP"] = address.ExternalIP;
				}
				else
				{
					settings["SharedIP"] = address.InternalIP;
				}
			}
			else
			{
				settings["SharedIP"] = String.Empty;
			}
        	settings["SiteId"] = ddlSite.SelectedValue;
            if (!string.IsNullOrWhiteSpace(txtAdFtpRoot.Text))
            {
                settings["AdFtpRoot"] = txtAdFtpRoot.Text.Trim();
            }
            settings["FtpGroupName"] = txtFtpGroupName.Text.Trim();
			settings["BuildUncFilesPath"] = chkBuildUncFilesPath.Checked.ToString();
            ActiveDirectoryIntegration.SaveSettings(settings);
        }

		private int FindAddressByText(string address)
		{
		    if (string.IsNullOrEmpty(address))
		    {
		        return 0;
		    }

            foreach (IPAddressInfo addressInfo in ES.Services.Servers.GetIPAddresses(IPAddressPool.General, PanelRequest.ServerId))
			{
				if (addressInfo.InternalIP == address || addressInfo.ExternalIP == address)
				{
					return addressInfo.AddressId;
				}
			}
			return 0;
		}

        private void BindSiteId(StringDictionary settings)
        {
            var sites = ES.Services.FtpServers.GetFtpSites(PanelRequest.ServiceId);

            foreach (var site in sites)
            {
                var item = new ListItem(site.Name + " (User Isolation Mode: " + site["UserIsolationMode"] + ")", site.Name);

                if (item.Value == settings["SiteId"])
                {
                    item.Selected = true;
                }

                ddlSite.Items.Add(item);
            }

            if (ddlSite.Items.Count == 0)
            {
                ddlSite.Items.Add(new ListItem("Default FTP Site (not yet created)", "Default FTP Site"));
            }
            else
            {
                ddlSite_SelectedIndexChanged(this, null);
            }
        }

        protected void ddlSite_SelectedIndexChanged(object sender, EventArgs e)
        {
            var isActiveDirectoryUserIsolated = ddlSite.SelectedItem.Text.Contains("ActiveDirectory");
            FtpRootRow.Visible = isActiveDirectoryUserIsolated;
            txtAdFtpRootReqValidator.Enabled= isActiveDirectoryUserIsolated;
        }
    }
}
