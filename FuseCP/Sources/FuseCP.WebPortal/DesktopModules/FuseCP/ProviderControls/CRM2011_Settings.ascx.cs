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
using System.Web.UI.WebControls;
using System.Globalization;
using FuseCP.EnterpriseServer;
using FuseCP.Providers.Common;
using FuseCP.Providers.ResultObjects;
using FuseCP.Providers.HostedSolution;

namespace FuseCP.Portal.ProviderControls
{
    public partial class CRM2011_Settings : FuseCPControlBase, IHostingServiceProviderSettings
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
            txtOrganizationWebService.Text = settings[Constants.OrganizationWebService];
            txtDiscoveryWebService.Text = settings[Constants.DiscoveryWebService];
            txtDeploymentWebService.Text = settings[Constants.DeploymentWebService];

            txtPassword.Text = settings[Constants.Password];
            ViewState["PWD"] = settings[Constants.Password];
            txtUserName.Text = settings[Constants.UserName];

            int selectedAddressid = FindAddressByText(settings[Constants.CRMWebsiteIP]);
            ddlCrmIpAddress.AddressId = (selectedAddressid > 0) ? selectedAddressid : 0; 
            
            Utils.SelectListItem(ddlSchema, settings[Constants.UrlSchema]);

            // Collation
            StringArrayResultObject res = ES.Services.CRM.GetCollationByServiceId(PanelRequest.ServiceId);
            if (res.IsSuccess)
            {
                ddlCollation.DataSource = res.Value;
                ddlCollation.DataBind();
                Utils.SelectListItem(ddlCollation, "Latin1_General_CI_AI"); // default
            }
            Utils.SelectListItem(ddlCollation, settings[Constants.Collation]);

            // Currency
            ddlCurrency.Items.Clear();
            CurrencyArrayResultObject cres = ES.Services.CRM.GetCurrencyByServiceId(PanelRequest.ServiceId);
            if (cres.IsSuccess)
            {
                foreach (Currency currency in cres.Value)
                {
                    ListItem item = new ListItem(string.Format("{0} ({1})",
                                                               currency.RegionName, currency.CurrencyName),
                                                 string.Join("|",
                                                             new string[]
                                                                 {
                                                                     currency.CurrencyCode, currency.CurrencyName,
                                                                     currency.CurrencySymbol, currency.RegionName
                                                                 }));

                    ddlCurrency.Items.Add(item);
                }
                Utils.SelectListItem(ddlCurrency, "USD|US Dollar|$|United States"); // default
            }
            Utils.SelectListItem(ddlCurrency, settings[Constants.Currency]);

            // Base Language
            ddlBaseLanguage.Items.Clear();
            int[] langPacksId = ES.Services.CRM.GetInstalledLanguagePacksByServiceId(PanelRequest.ServiceId);
            if (langPacksId != null)
            {
                foreach (int langId in langPacksId)
                {
                    CultureInfo ci = CultureInfo.GetCultureInfo(langId);
                    ListItem item = new ListItem(ci.EnglishName, langId.ToString());
                    ddlBaseLanguage.Items.Add(item);
                }
                Utils.SelectListItem(ddlBaseLanguage, "1033"); // default
            }
            Utils.SelectListItem(ddlBaseLanguage, settings[Constants.BaseLanguage]);
        }

        public void SaveSettings(System.Collections.Specialized.StringDictionary settings)
        {
            settings[Constants.ReportingServer] = txtReportingService.Text;
            settings[Constants.SqlServer] = txtSqlServer.Text;
            settings[Constants.IFDWebApplicationRootDomain] = txtDomainName.Text;
            settings[Constants.Port] = txtPort.Text;

            settings[Constants.AppRootDomain] = txtAppRootDomain.Text;
            settings[Constants.OrganizationWebService] = txtOrganizationWebService.Text;
            settings[Constants.DiscoveryWebService] = txtDiscoveryWebService.Text;
            settings[Constants.DeploymentWebService] = txtDeploymentWebService.Text;

            settings[Constants.Password] = (txtPassword.Text.Length > 0) ? txtPassword.Text : (string)ViewState["PWD"];
            settings[Constants.UserName] = txtUserName.Text;

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

            settings[Constants.Collation] = ddlCollation.SelectedValue;
            settings[Constants.Currency] = ddlCurrency.SelectedValue;
            settings[Constants.BaseLanguage] = ddlBaseLanguage.SelectedValue;

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
