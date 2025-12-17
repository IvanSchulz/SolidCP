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
using FuseCP.EnterpriseServer;
using FuseCP.EnterpriseServer.Base.HostedSolution;
using FuseCP.Providers.HostedSolution;

namespace FuseCP.Portal.ExchangeServer
{
    public partial class OrganizationDomainNames : FuseCPModuleBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindStats();

                // bind domain names
                BindDomainNames();
            }

            
        }

        private void BindStats()
        {
            // set quotas
            OrganizationStatistics stats = ES.Services.Organizations.GetOrganizationStatisticsByOrganization(PanelRequest.ItemID);
            domainsQuota.QuotaUsedValue = stats.CreatedDomains;
            domainsQuota.QuotaValue = stats.AllocatedDomains;
            if (stats.AllocatedDomains != -1) domainsQuota.QuotaAvailable = stats.AllocatedDomains - stats.CreatedDomains;
        }

        public string GetDomainRecordsEditUrl(string domainId)
        {
            return EditUrl("SpaceID", PanelSecurity.PackageId.ToString(), "domain_records",
                    "DomainID=" + domainId,
                    "ItemID=" + PanelRequest.ItemID);
        }

        private void BindDomainNames()
        {
            OrganizationDomainName[] list = ES.Services.Organizations.GetOrganizationDomains(PanelRequest.ItemID);

            gvDomains.DataSource = list;
            gvDomains.DataBind();

            //check if organization has only one default domain
            if (gvDomains.Rows.Count == 1)
            {
                btnSetDefaultDomain.Enabled = false;
            }
        }

        public string IsChecked(bool val)
        {
            return val ? "checked" : "";
        }

        protected void btnAddDomain_Click(object sender, EventArgs e)
        {
            btnSetDefaultDomain.Enabled = true;
            Response.Redirect(EditUrl("ItemID", PanelRequest.ItemID.ToString(), "org_add_domain",
                "SpaceID=" + PanelSecurity.PackageId));
        }

        protected void gvDomains_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "DeleteItem")
            {
                // delete domain
                int domainId = Utils.ParseInt(e.CommandArgument.ToString(), 0);

                try
                {
                    int result = ES.Services.Organizations.DeleteOrganizationDomain(PanelRequest.ItemID, domainId);
                    if (result < 0)
                    {
                        Response.Redirect(EditUrl("ItemID", PanelRequest.ItemID.ToString(), "check_domain",
                            "SpaceID=" + PanelSecurity.PackageId, "DomainID=" + domainId));
                        return;
                    }

                    // rebind domains
                    BindDomainNames();

                    BindStats();
                }
                catch (Exception ex)
                {
                    ShowErrorMessage("EXCHANGE_DELETE_DOMAIN", ex);
                }
            }
        }

        protected void btnSetDefaultDomain_Click(object sender, EventArgs e)
        {
            // get domain
            int domainId = Utils.ParseInt(Request.Form["DefaultDomain"], 0);

            try
            {
                int result = ES.Services.Organizations.SetOrganizationDefaultDomain(PanelRequest.ItemID, domainId);
                if (result < 0)
                {
                    messageBox.ShowResultMessage(result);
                    if (BusinessErrorCodes.ERROR_USER_ACCOUNT_DEMO == result)
                        BindDomainNames();
                    return;
                }

                // rebind domains
                BindDomainNames();
            }
            catch (Exception ex)
            {
                ShowErrorMessage("EXCHANGE_SET_DEFAULT_DOMAIN", ex);
            }
        }

    }
}
