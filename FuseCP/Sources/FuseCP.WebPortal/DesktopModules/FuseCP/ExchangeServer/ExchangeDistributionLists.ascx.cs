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
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

using FuseCP.Providers.HostedSolution;
using FuseCP.EnterpriseServer;
using FuseCP.EnterpriseServer.Base.HostedSolution;

namespace FuseCP.Portal.ExchangeServer
{
    public partial class ExchangeDistributionLists : FuseCPModuleBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindStats();
            }


        }

        private void BindStats()
        {
            // quota values
            OrganizationStatistics stats = ES.Services.ExchangeServer.GetOrganizationStatisticsByOrganization(PanelRequest.ItemID);
            listsQuota.QuotaUsedValue = stats.CreatedDistributionLists;
            listsQuota.QuotaValue = stats.AllocatedDistributionLists;
            if (stats.AllocatedDistributionLists != -1) listsQuota.QuotaAvailable = stats.AllocatedDistributionLists - stats.CreatedDistributionLists;
        }

        protected void btnCreateList_Click(object sender, EventArgs e)
        {
            Response.Redirect(EditUrl("ItemID", PanelRequest.ItemID.ToString(), "create_dlist",
                "SpaceID=" + PanelSecurity.PackageId.ToString()));
        }

        public string GetListEditUrl(string accountId)
        {
            return EditUrl("SpaceID", PanelSecurity.PackageId.ToString(), "dlist_settings",
                    "AccountID=" + accountId,
                    "ItemID=" + PanelRequest.ItemID.ToString());
        }

        protected void odsAccountsPaged_Selected(object sender, ObjectDataSourceStatusEventArgs e)
        {
            if (e.Exception != null)
            {
                messageBox.ShowErrorMessage("EXCHANGE_GET_LISTS", e.Exception);
                e.ExceptionHandled = true;
            }
        }

        protected void gvLists_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "DeleteItem")
            {
                // delete distribution list
                int accountId = Utils.ParseInt(e.CommandArgument.ToString(), 0);

                try
                {
                    int result = ES.Services.ExchangeServer.DeleteDistributionList(PanelRequest.ItemID, accountId);
                    if (result < 0)
                    {
                        messageBox.ShowResultMessage(result);
                        return;
                    }

                    // rebind grid
                    gvLists.DataBind();

                    BindStats();
                }
                catch (Exception ex)
                {
                    messageBox.ShowErrorMessage("EXCHANGE_DELETE_DISTRIBUTION_LIST", ex);
                }
            }
        }

        protected void ddlPageSize_SelectedIndexChanged(object sender, EventArgs e)   
        {   
            gvLists.PageSize = Convert.ToInt32(ddlPageSize.SelectedValue);   
       
            // rebind grid   
            gvLists.DataBind();   
       
            // bind stats   
            BindStats();   
       
        }  

    }
}
