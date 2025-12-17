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
using System.Collections.Specialized;
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

namespace FuseCP.Portal.ProviderControls
{
    public partial class HostedSharePoint30_Settings : FuseCPControlBase, IHostingServiceProviderSettings
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        public void BindSettings(StringDictionary settings)
        {
            this.txtRootWebApplication.Text = settings["RootWebApplicationUri"];
            int selectedAddressid = this.FindAddressByText(settings["RootWebApplicationIpAddress"]);
            this.ddlRootWebApplicationIpAddress.AddressId = (selectedAddressid > 0) ? selectedAddressid : 0;
            chkLocalHostFile.Checked = Utils.ParseBool(settings["LocalHostFile"], false);
            this.txtSharedSSLRoot.Text = settings["SharedSSLRoot"];
        }

        public void SaveSettings(StringDictionary settings)
        {
            settings["RootWebApplicationUri"] = this.txtRootWebApplication.Text;
            settings["LocalHostFile"] = chkLocalHostFile.Checked.ToString();
            settings["RootWebApplicationInteralIpAddress"] = String.Empty;
            settings["SharedSSLRoot"] = this.txtSharedSSLRoot.Text;

            if (ddlRootWebApplicationIpAddress.AddressId > 0)
            {
                IPAddressInfo address = ES.Services.Servers.GetIPAddress(ddlRootWebApplicationIpAddress.AddressId);
                if (String.IsNullOrEmpty(address.ExternalIP))
                {
                    settings["RootWebApplicationIpAddress"] = address.InternalIP;
                }
                else
                {
                    settings["RootWebApplicationIpAddress"] = address.ExternalIP;
                }

                if (!String.IsNullOrEmpty(address.InternalIP))
                    settings["RootWebApplicationInteralIpAddress"] = address.InternalIP;
            }
            else
            {
                settings["RootWebApplicationIpAddress"] = String.Empty;
            }

        }

        private int FindAddressByText(string address)
        {
            foreach (IPAddressInfo addressInfo in ES.Services.Servers.GetIPAddresses(IPAddressPool.General, PanelRequest.ServerId))
            {
                if (addressInfo.InternalIP == address || addressInfo.ExternalIP == address)
                {
                    return addressInfo.AddressId;
                }
            }
            return 0;
        }
    }
}
