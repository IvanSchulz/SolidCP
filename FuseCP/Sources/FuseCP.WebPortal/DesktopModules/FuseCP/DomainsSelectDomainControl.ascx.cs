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
using System.Linq;
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
    public partial class DomainsSelectDomainControl : FuseCPControlBase
    {
        public bool HideIdnDomains
        {
            get { return (ViewState["HideIdnDomains"] != null) && (bool)ViewState["HideIdnDomains"]; }
            set { ViewState["HideIdnDomains"] = value; }
        }

        public bool HideWebSites
        {
            get { return (ViewState["HideWebSites"] != null) ? (bool)ViewState["HideWebSites"] : false; }
            set { ViewState["HideWebSites"] = value; }
        }

        public bool HidePreviewDomain
        {
            get { return (ViewState["HidePreviewDomain"] != null) ? (bool)ViewState["HidePreviewDomain"] : false; }
            set { ViewState["HidePreviewDomain"] = value; }
        }

        public bool HideMailDomains
        {
            get { return (ViewState["HideMailDomains"] != null) ? (bool)ViewState["HideMailDomains"] : false; }
            set { ViewState["HideMailDomains"] = value; }
        }

        public bool HideMailDomainPointers
        {
            get { return (ViewState["HideMailDomainPointers"] != null) ? (bool)ViewState["HideMailDomainPointers"] : false; }
            set { ViewState["HideMailDomainPointers"] = value; }
        }


        public bool HideDomainPointers
        {
            get { return (ViewState["HideDomainPointers"] != null) ? (bool)ViewState["HideDomainPointers"] : false; }
            set { ViewState["HideDomainPointers"] = value; }
        }

        public bool HideDomainsSubDomains
        {
            get { return (ViewState["HideDomainsSubDomains"] != null) ? (bool)ViewState["HideDomainsSubDomains"] : false; }
            set { ViewState["HideDomainsSubDomains"] = value; }
        }

        public int PackageId
        {
            get { return (ViewState["PackageId"] != null) ? (int)ViewState["PackageId"] : 0; }
            set { ViewState["PackageId"] = value; }
        }

        public int DomainId
        {
            get
            {
                return Utils.ParseInt(ddlDomains.SelectedValue, 0);
            }
        }

        public string DomainName
        {
            get
            {
                return ddlDomains.SelectedItem.Text.ToLower();
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindDomains();
            }

        }

        private void BindDomains()
        {
            DomainInfo[] domains = ES.Services.Servers.GetMyDomains(PackageId);

            if (HideIdnDomains)
            {
                domains = domains.Where(d => !Utils.IsIdnDomain(d.DomainName)).ToArray();
            }

            WebSite[] sites = null;
            Hashtable htSites = new Hashtable();
            Hashtable htMailDomainPointers = new Hashtable();
            if (HideWebSites)
            {
                sites = ES.Services.WebServers.GetWebSites(PackageId, false);

                foreach (WebSite w in sites)
                {
                    if (htSites[w.Name.ToLower()] == null) htSites.Add(w.Name.ToLower(), 1);

                    DomainInfo[] pointers = ES.Services.WebServers.GetWebSitePointers(w.Id);
                    foreach (DomainInfo p in pointers)
                    {
                        if (htSites[p.DomainName.ToLower()] == null) htSites.Add(p.DomainName.ToLower(), 1);
                    }
                }
            }

            if (HideMailDomainPointers)
            {
                Providers.Mail.MailDomain[] mailDomains = ES.Services.MailServers.GetMailDomains(PackageId, false);

                foreach (Providers.Mail.MailDomain mailDomain in mailDomains)
                {
                    DomainInfo[] pointers = ES.Services.MailServers.GetMailDomainPointers(mailDomain.Id);
                    if (pointers != null)
                    {
                        foreach (DomainInfo p in pointers)
                        {
                            if (htMailDomainPointers[p.DomainName.ToLower()] == null) htMailDomainPointers.Add(p.DomainName.ToLower(), 1);

                        }
                    }
                }
            }


            ddlDomains.Items.Clear();

            // add "select" item
            ddlDomains.Items.Insert(0, new ListItem(GetLocalizedString("Text.SelectDomain"), ""));

            foreach (DomainInfo domain in domains)
            {
                if (HideWebSites)
                {
                    if (domain.WebSiteId > 0)
                    {
                        continue;
                    }
                    else
                    {
                        if (htSites != null)
                        {
                            if (htSites[domain.DomainName.ToLower()] != null) continue;
                        }
                    }
                }


                if (HideMailDomainPointers)
                {
                    if (htMailDomainPointers[domain.DomainName.ToLower()] != null) continue;
                }

                
                if (HidePreviewDomain && domain.IsPreviewDomain)
                    continue;
                else if (HideMailDomains && domain.MailDomainId > 0)
                    continue;
                else if (HideDomainPointers && (domain.IsDomainPointer))
                    continue;
                else if (HideDomainsSubDomains && !(domain.IsDomainPointer))
                    continue;

                ddlDomains.Items.Add(new ListItem(domain.DomainName.ToLower(), domain.DomainId.ToString()));
            }

            if (Request.Cookies["CreatedDomainId"] != null)
                Utils.SelectListItem(ddlDomains, Request.Cookies["CreatedDomainId"].Value);
        }
    }
}
