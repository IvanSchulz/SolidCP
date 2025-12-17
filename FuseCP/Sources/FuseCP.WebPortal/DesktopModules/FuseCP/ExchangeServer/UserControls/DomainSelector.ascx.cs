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
using FuseCP.Providers.HostedSolution;

namespace FuseCP.Portal.ExchangeServer.UserControls
{
    public partial class DomainSelector : System.Web.UI.UserControl
    {
        public string DomainName
        {
            get { return ddlDomain.SelectedItem.Text; }
            set
            {
                foreach (ListItem li in ddlDomain.Items)
                {
                    if (li.Value == value)
                    {
                        ddlDomain.ClearSelection();
                        li.Selected = true;
                        break;
                    }
                }
            }
        }

        public int DomainId
        {
            get
            {
                return Convert.ToInt32(ddlDomain.SelectedValue);
            }
        }

        public int DomainsCount
        {
            get
            {
                return this.ddlDomain.Items.Count;
            }
        }

        public bool ShowAt
        {
            get
            {
                return this.litAt.Visible;
            }
            set
            {
                this.litAt.Visible = value;
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
            // get domains
            OrganizationDomainName[] domains = ES.Services.Organizations.GetOrganizationDomains(PanelRequest.ItemID);

            // bind domains
            foreach (OrganizationDomainName domain in domains)
            {
                ListItem li = new ListItem(domain.DomainName, domain.DomainId.ToString());
                li.Selected = domain.IsDefault;
                ddlDomain.Items.Add(li);
            }
        }
    }
}
