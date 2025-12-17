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
using FuseCP.EnterpriseServer;
using FuseCP.Providers.Common;

namespace FuseCP.Portal.ProviderControls
{
    public partial class CRM_Settings : FuseCPControlBase, IHostingServiceProviderSettings
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

     

        public void BindSettings(System.Collections.Specialized.StringDictionary settings)
        {
            txtReportingService.Text = settings[Constants.ReportingServer];
            txtSqlServer.Text = settings[Constants.SqlServer];
            txtDomainName.Text = settings[Constants.IFDWebApplicationRootDomain];
            txtPort.Text = settings[Constants.Port];
            txtAppRootDomain.Text = settings[Constants.AppRootDomain];
            int selectedAddressid = FindAddressByText(settings[Constants.CRMWebsiteIP]);
            ddlCrmIpAddress.AddressId = (selectedAddressid > 0) ? selectedAddressid : 0; 
            
            ddlSchema.SelectedValue = settings[Constants.UrlSchema];
            
        }

        public void SaveSettings(System.Collections.Specialized.StringDictionary settings)
        {
            settings[Constants.ReportingServer] = txtReportingService.Text;
            settings[Constants.SqlServer] = txtSqlServer.Text;
            settings[Constants.IFDWebApplicationRootDomain] = txtDomainName.Text;
            settings[Constants.Port] = txtPort.Text;
            settings[Constants.AppRootDomain] = txtAppRootDomain.Text;
            if (ddlCrmIpAddress.AddressId > 0)
			{
				IPAddressInfo address = ES.Services.Servers.GetIPAddress(ddlCrmIpAddress.AddressId);
                if (String.IsNullOrEmpty(address.ExternalIP))
				{
                    settings[Constants.CRMWebsiteIP] = address.InternalIP;
				}
				else
				{
                    settings[Constants.CRMWebsiteIP] = address.ExternalIP;
				}
			}
			else
			{
                settings[Constants.CRMWebsiteIP] = String.Empty;
			}
             
            settings[Constants.UrlSchema] = ddlSchema.SelectedValue;
        }

        private static int FindAddressByText(string address)
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
