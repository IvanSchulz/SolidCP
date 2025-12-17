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
    public partial class EnterpriseStorageSpaces : FuseCPModuleBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindStats();

                // bind domain names
                BindEnterpriseStorageSpaces();
            }

            
        }

        private void BindStats()
        {
            // set quotas
            OrganizationStatistics stats = ES.Services.Organizations.GetOrganizationStatisticsByOrganization(PanelRequest.ItemID);
            OrganizationStatistics tenantStats = ES.Services.Organizations.GetOrganizationStatistics(PanelRequest.ItemID);
            spacesQuota.QuotaUsedValue = stats.CreatedEnterpriseStorageFolders;
            spacesQuota.QuotaValue = stats.AllocatedEnterpriseStorageFolders;
            if (stats.AllocatedEnterpriseStorageFolders != -1) spacesQuota.QuotaAvailable = tenantStats.AllocatedEnterpriseStorageFolders - tenantStats.CreatedEnterpriseStorageFolders;
        }

        public string GetSpaceRecordsEditUrl(string spaceName)
        {
            return EditUrl("SpaceID", PanelSecurity.PackageId.ToString(), "enterprisestorage_edit",
                    "SpaceName=" + spaceName,
                    "ItemID=" + PanelRequest.ItemID);
        }

        private void BindEnterpriseStorageSpaces()
        {
            FuseCP.Providers.OS.SystemFile[] list = ES.Services.EnterpriseStorage.GetEnterpriseFolders(PanelRequest.ItemID);

            gvSpaces.DataSource = list;
            gvSpaces.DataBind();
        }

        public string IsPublished(bool val)
        {
            return val ? "UnPublish" : "Publish";
        }

        protected void btnAddSpace_Click(object sender, EventArgs e)
        {
     
            Response.Redirect(EditUrl("ItemID", PanelRequest.ItemID.ToString(), "enterprisestorage_add",
                "SpaceID=" + PanelSecurity.PackageId));
        }

        protected void gvSpaces_RowCommand(object sender, GridViewCommandEventArgs e)
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
                        messageBox.ShowErrorMessage("EXCHANGE_UNABLE_TO_DELETE_DOMAIN");
                    }

                    // rebind domains
                    //BindDomainNames();

                    BindStats();
                }
                catch (Exception ex)
                {
                    ShowErrorMessage("EXCHANGE_DELETE_DOMAIN", ex);
                }
            }
            else if (e.CommandName == "Change")
            {
                string[] commandArgument = e.CommandArgument.ToString().Split('|');
                int domainId = Utils.ParseInt(commandArgument[0].ToString(), 0);
                ExchangeAcceptedDomainType acceptedDomainType = (ExchangeAcceptedDomainType)Enum.Parse(typeof(ExchangeAcceptedDomainType), commandArgument[1]);


                try
                {

                    ExchangeAcceptedDomainType newDomainType = ExchangeAcceptedDomainType.Authoritative;
                    if (acceptedDomainType == ExchangeAcceptedDomainType.Authoritative)
                        newDomainType = ExchangeAcceptedDomainType.InternalRelay;

                    int result = ES.Services.Organizations.ChangeOrganizationDomainType(PanelRequest.ItemID, domainId, newDomainType);
                    if (result < 0)
                    {
                        messageBox.ShowResultMessage(result);
                        return;
                    }

                    // rebind domains
                    //BindDomainNames();

                    BindStats();
                }
                catch (Exception ex)
                {
                    ShowErrorMessage("EXCHANGE_CHANGE_DOMAIN", ex);
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
                        //BindDomainNames();
                    return;
                }

                // rebind domains
                //BindDomainNames();
            }
            catch (Exception ex)
            {
                ShowErrorMessage("EXCHANGE_SET_DEFAULT_DOMAIN", ex);
            }
        }

    }
}
