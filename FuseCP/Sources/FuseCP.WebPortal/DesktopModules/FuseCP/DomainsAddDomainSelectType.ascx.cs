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
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FuseCP.EnterpriseServer;

namespace FuseCP.Portal
{
    public partial class DomainsAddDomainSelectType : FuseCPControlBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            BindControls();
        }

        private void BindControls()
        {
            // set navigate URLs
            DomainLink.NavigateUrl = GetAddDomainLink("Domain");
            SubDomainLink.NavigateUrl = GetAddDomainLink("SubDomain");
            ProviderSubDomainLink.NavigateUrl = GetAddDomainLink("ProviderSubDomain");
            DomainPointerLink.NavigateUrl = GetAddDomainLink("DomainPointer");

            // load package context
            PackageContext cntx = PackagesHelper.GetCachedPackageContext(PanelSecurity.PackageId);

            DomainLink.Enabled = (cntx.Quotas.ContainsKey(Quotas.OS_DOMAINS) && !cntx.Quotas[Quotas.OS_DOMAINS].QuotaExhausted);

            if (DomainLink.Enabled)
            {
                UserInfo user = UsersHelper.GetUser(PanelSecurity.EffectiveUserId);

                if (user != null)
                {
                    if (user.Role == UserRole.User)
                    {
                        DomainLink.Enabled = !Utils.CheckQouta(Quotas.OS_NOTALLOWTENANTCREATEDOMAINS, cntx);
                    }
                }
            }

            

            DomainInfo[] myDomains = ES.Services.Servers.GetMyDomains(PanelSecurity.PackageId);
            bool enableSubDomains = false;
            foreach(DomainInfo domain in myDomains)
            {
                if(!domain.IsSubDomain && !domain.IsPreviewDomain && !domain.IsDomainPointer)
                {
                    enableSubDomains = true;
                    break;
                }
            }
            SubDomainLink.Enabled = (cntx.Quotas.ContainsKey(Quotas.OS_SUBDOMAINS) && !cntx.Quotas[Quotas.OS_SUBDOMAINS].QuotaExhausted
                && enableSubDomains);

            ProviderSubDomainPanel.Visible = (cntx.Quotas.ContainsKey(Quotas.OS_SUBDOMAINS) && !cntx.Quotas[Quotas.OS_SUBDOMAINS].QuotaExhausted
                && ES.Services.Servers.GetResellerDomains(PanelSecurity.PackageId).Length > 0);

            DomainPointerLink.Enabled = (cntx.Quotas.ContainsKey(Quotas.OS_DOMAINPOINTERS) && !cntx.Quotas[Quotas.OS_DOMAINPOINTERS].QuotaExhausted);
        }

        private string GetAddDomainLink(string domainType)
        {
            string returnUrl = (ViewState["ReturnURL"] != null) ? Server.UrlEncode(ViewState["ReturnURL"].ToString()) : "";

            return EditUrl("DomainType", domainType, "add_domain_step2",
                "SpaceID=" + PanelSecurity.PackageId.ToString(),
                "ReturnURL=" + returnUrl);
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            // return
            RedirectBack();
        }

        private void RedirectBack()
        {
            if (ViewState["ReturnURL"] != null)
                Response.Redirect((string)ViewState["ReturnURL"]);
            else
                RedirectSpaceHomePage();
        }
    }
}
