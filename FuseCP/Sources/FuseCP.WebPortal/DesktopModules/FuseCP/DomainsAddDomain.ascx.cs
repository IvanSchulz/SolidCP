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
using System.Web;
using FuseCP.EnterpriseServer;
using System.Collections.Generic;
using FuseCP.Portal.UserControls;
//using System.Net.Http;
using System.Threading.Tasks;
using System.Xml;
using System.Net;
using System.IO;

namespace FuseCP.Portal
{
	public partial class DomainsAddDomain : FuseCPModuleBase
	{
		protected void Page_Load(object sender, EventArgs e)
		{
			try
			{ 
				// bind controls
				BindControls();
                DomainType type = GetDomainType(Request["DomainType"]);
                PackageContext cntx = PackagesHelper.GetCachedPackageContext(PanelSecurity.PackageId);
                if (type == DomainType.Domain && cntx.Quotas[Quotas.OS_DOMAINS].QuotaExhausted)
                {
                    btnAdd.Enabled = false;
                }
                if (type == DomainType.SubDomain && cntx.Quotas[Quotas.OS_SUBDOMAINS].QuotaExhausted)
                {
                    btnAdd.Enabled = false;
                }


                    if (Utils.CheckQouta(Quotas.WEB_ENABLEHOSTNAMESUPPORT, cntx))
                {
                    lblHostName.Visible = txtHostName.Visible = true;
                    UserSettings settings = ES.Services.Users.GetUserSettings(PanelSecurity.LoggedUserId, UserSettings.WEB_POLICY);
                    txtHostName.Text = String.IsNullOrEmpty(settings["HostName"]) ? "" : settings["HostName"];
                }
                else
                {
                    lblHostName.Visible = txtHostName.Visible = false;
                    txtHostName.Text = "";
                }

                if ((PanelSecurity.LoggedUser.Role == UserRole.User) & (type != DomainType.SubDomain))
                {
                    if (cntx.Groups.ContainsKey(ResourceGroups.Dns))
                    {
                        if (!PackagesHelper.CheckGroupQuotaEnabled(PanelSecurity.PackageId, ResourceGroups.Dns, Quotas.DNS_EDITOR))
                            this.DisableControls = true;
                    }
                }
			}
			catch (Exception ex)
			{
				ShowErrorMessage("DOMAIN_GET_DOMAIN", ex);
			}
		}

		private void BindControls()
		{
			// get domain type
			DomainType type = GetDomainType(Request["DomainType"]);
            // enable domain/sub-domain fields
            // load package context
            if (type == DomainType.Domain || type == DomainType.DomainPointer)
            {
                    // domains
                    DomainName.IsSubDomain = false;
            }
            else
            {
                    // sub-domains
                    DomainName.IsSubDomain = true;

                    // fill sub-domains
                    if (!IsPostBack)
                    {
                        if (type == DomainType.SubDomain)
                            BindUserDomains();
                        else
                            BindResellerDomains();
                    }
            }
			
			if ((type == DomainType.DomainPointer || (type == DomainType.Domain)) && !IsPostBack)
			{
                // bind web sites
                WebSitesList.DataSource = ES.Services.WebServers.GetWebSites(PanelSecurity.PackageId, false);
                WebSitesList.DataBind();
			}

            if ((type == DomainType.DomainPointer || (type == DomainType.Domain)) && !IsPostBack)
            {
                // bind mail domains
                MailDomainsList.DataSource = ES.Services.MailServers.GetMailDomains(PanelSecurity.PackageId, false);
                MailDomainsList.DataBind();
            }

            PackageContext cntx = PackagesHelper.GetCachedPackageContext(PanelSecurity.PackageId);
            // create web site option
            CreateFuseCP.Visible = (type == DomainType.Domain || type == DomainType.SubDomain)
				&& cntx.Groups.ContainsKey(ResourceGroups.Web);

            if (PointWebSite.Checked)
            {
                CreateWebSite.Checked = false;
                CreateWebSite.Enabled = false;
            }
            else
            {
                CreateWebSite.Enabled = true;
                CreateWebSite.Checked &= CreateFuseCP.Visible;
            }

            // point Web site
            PointFuseCP.Visible = (type == DomainType.DomainPointer || (type == DomainType.Domain))
                && cntx.Groups.ContainsKey(ResourceGroups.Web) && WebSitesList.Items.Count > 0;
            WebSitesList.Enabled = PointWebSite.Checked;

			// point mail domain
            PointMailDomainPanel.Visible = (type == DomainType.DomainPointer || (type == DomainType.Domain))
				&& cntx.Groups.ContainsKey(ResourceGroups.Mail) && MailDomainsList.Items.Count > 0;
			MailDomainsList.Enabled = PointMailDomain.Checked;

			// DNS option
			EnableDnsPanel.Visible = cntx.Groups.ContainsKey(ResourceGroups.Dns);
			EnableDns.Checked &= EnableDnsPanel.Visible;

            // Preview Domain
            // check if Preview Domain was setup
            bool instantAliasAllowed = false;
			PackageSettings settings = ES.Services.Packages.GetPackageSettings(PanelSecurity.PackageId, PackageSettings.INSTANT_ALIAS);
			instantAliasAllowed = (settings != null && !String.IsNullOrEmpty(settings["PreviewDomain"]));

			PreviewDomainPanel.Visible = instantAliasAllowed && (type != DomainType.DomainPointer) /*&& EnableDnsPanel.Visible*/;
			CreatePreviewDomain.Checked &= PreviewDomainPanel.Visible;

			// allow sub-domains
			AllowSubDomainsPanel.Visible = (type == DomainType.Domain) && PanelSecurity.EffectiveUser.Role != UserRole.User;

		    if (IsPostBack)
		    {
		        CheckForCorrectIdnDomainUsage(DomainName.Text);
		    }
		}

		private DomainType GetDomainType(string typeName)
		{
			DomainType type = DomainType.Domain;

			if (!String.IsNullOrEmpty(typeName))
				type = (DomainType)Enum.Parse(typeof(DomainType), typeName, true);

			return type;
		}

		private void BindUserDomains()
		{
			DomainInfo[] allDomains = ES.Services.Servers.GetMyDomains(PanelSecurity.PackageId);

			// filter domains
			List<DomainInfo> domains = new List<DomainInfo>();
			foreach (DomainInfo domain in allDomains)
				if (!domain.IsDomainPointer && !domain.IsSubDomain && !domain.IsPreviewDomain)
					domains.Add(domain);

            DomainName.DataSource = domains;
			DomainName.DataBind();
		}

		private void BindResellerDomains()
		{
            DomainInfo[] allDomains = ES.Services.Servers.GetResellerDomains(PanelSecurity.PackageId);

            // filter domains
            List<DomainInfo> domains = new List<DomainInfo>();
            foreach (DomainInfo domain in allDomains)
                if (!domain.IsDomainPointer && !domain.IsSubDomain && !domain.IsPreviewDomain)
                    domains.Add(domain);

            DomainName.DataSource = domains;
            DomainName.DataBind();
		}
   
        private void AddDomain()
		{
			if (!Page.IsValid)
				return;

			// get domain type
			DomainType type = GetDomainType(Request["DomainType"]);

			// get domain name
		    var domainName = DomainName.Text;

			int pointWebSiteId = 0;
			int pointMailDomainId = 0;

			// load package context
			PackageContext cntx = PackagesHelper.GetCachedPackageContext(PanelSecurity.PackageId);

			if (type == DomainType.DomainPointer || (type == DomainType.Domain))
			{

                if (PointWebSite.Checked && WebSitesList.Items.Count > 0)
                    pointWebSiteId = Utils.ParseInt(WebSitesList.SelectedValue, 0);
			}

            if (type == DomainType.DomainPointer || (type == DomainType.Domain))
            {
                if (PointMailDomain.Checked && MailDomainsList.Items.Count > 0)
                    pointMailDomainId = Utils.ParseInt(MailDomainsList.SelectedValue, 0);
            }


			// add domain
			int domainId = 0;
			try
			{
				domainId = ES.Services.Servers.AddDomainWithProvisioning(PanelSecurity.PackageId,
					domainName.ToLower(), type, CreateWebSite.Checked, pointWebSiteId, pointMailDomainId,
                    EnableDns.Checked, CreatePreviewDomain.Checked, AllowSubDomains.Checked, (PointWebSite.Checked && WebSitesList.Items.Count > 0) ? string.Empty : txtHostName.Text.ToLower());

				if (domainId < 0)
				{
					ShowResultMessage(domainId);
					return;
				}
            }
			catch (Exception ex)
			{
				ShowErrorMessage("DOMAIN_ADD_DOMAIN", ex);
				return;
			}

			// put created domain to the cookie
			HttpCookie domainCookie = new HttpCookie("CreatedDomainId", domainId.ToString());
			Response.Cookies.Add(domainCookie);

			// return
			RedirectBack();
		}

		private void RedirectBack()
		{
			RedirectSpaceHomePage();
		}

		protected void btnCancel_Click(object sender, EventArgs e)
		{
			// return
			RedirectBack();
		}
		
		protected void btnAdd_Click(object sender, EventArgs e)
		{
			
			// get domain type
			DomainType type = GetDomainType(Request["DomainType"]);
			PackageContext cntx = PackagesHelper.GetCachedPackageContext(PanelSecurity.PackageId);
			
		    if (type == DomainType.Domain && !cntx.Quotas[Quotas.OS_DOMAINS].QuotaExhausted)
            {
                    if (CheckForCorrectIdnDomainUsage(DomainName.Text))
					{
						AddDomain();
					}
            }
            if (type == DomainType.SubDomain && !cntx.Quotas[Quotas.OS_SUBDOMAINS].QuotaExhausted)
            {
                    if (CheckForCorrectIdnDomainUsage(DomainName.Text))
					{
						AddDomain();
					}
			}
            if (type == DomainType.ProviderSubDomain && !cntx.Quotas[Quotas.OS_SUBDOMAINS].QuotaExhausted)
            {
                if (CheckForCorrectIdnDomainUsage(DomainName.Text))
                {
                    AddDomain();
                }
            }
        }

	    private bool CheckForCorrectIdnDomainUsage(string domainName)
	    {
	        // If the choosen domain is a idn domain, don't allow to create mail
	        if (Utils.IsIdnDomain(domainName) && PointMailDomain.Checked)
	        {
	            ShowErrorMessage("IDNDOMAIN_NO_MAIL");
	            return false;
	        }

	        return true;
	    }
	}
}
