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
using System.Linq;
using System.Text;
using System.Collections.Generic;

using FuseCP.EnterpriseServer;
using FuseCP.Providers.HostedSolution;

namespace FuseCP.Portal.ExchangeServer
{
    public partial class OrganizationAddDomainName : FuseCPModuleBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            DomainInfo[] domains = ES.Services.Servers.GetMyDomains(PanelSecurity.PackageId).Where(d => !Utils.IsIdnDomain(d.DomainName)).ToArray();

            Organization[] orgs = ES.Services.Organizations.GetOrganizations(PanelSecurity.PackageId, false);

            List<OrganizationDomainName> list = new List<OrganizationDomainName>();

            foreach (Organization o in orgs)
            {
                OrganizationDomainName[] tmpList = ES.Services.Organizations.GetOrganizationDomains(o.Id);

                foreach (OrganizationDomainName name in tmpList) list.Add(name);
            }

            foreach (DomainInfo d in domains)
            {
                if (!d.IsDomainPointer)
                {
                    bool bAdd = true;
                    foreach (OrganizationDomainName acceptedDomain in list)
                    {
                        if (d.DomainName.ToLower() == acceptedDomain.DomainName.ToLower())
                        {
                            bAdd = false;
                            break;
                        }

                    }
                    if (bAdd) ddlDomains.Items.Add(d.DomainName.ToLower());
                }
            }

            if (ddlDomains.Items.Count == 0)
            {
                ddlDomains.Visible= btnAdd.Enabled = false;
            }

            

        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            AddDomain();
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect(EditUrl("ItemID", PanelRequest.ItemID.ToString(), "org_domains", "SpaceID=" + PanelSecurity.PackageId));

        }


        private void AddDomain()
        {
            if (!Page.IsValid)
                return;

            try
            {

                int result = ES.Services.Organizations.AddOrganizationDomain(PanelRequest.ItemID,
                    ddlDomains.SelectedValue.Trim());

                if (result < 0)
                {
                    messageBox.ShowResultMessage(result);
                    return;
                }

                Response.Redirect(EditUrl("ItemID", PanelRequest.ItemID.ToString(), "org_domains",
                    "SpaceID=" + PanelSecurity.PackageId));
            }
            catch (Exception ex)
            {
                messageBox.ShowErrorMessage("EXCHANGE_ADD_DOMAIN", ex);
            }
        }
    }
}
