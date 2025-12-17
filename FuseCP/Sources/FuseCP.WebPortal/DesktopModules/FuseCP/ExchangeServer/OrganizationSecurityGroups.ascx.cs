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
    public partial class OrganizationSecurityGroups : FuseCPModuleBase
    {
        protected const int _NotesMaxLength = 100;

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
            OrganizationStatistics stats = ES.Services.Organizations.GetOrganizationStatisticsByOrganization(PanelRequest.ItemID);
            groupsQuota.QuotaUsedValue = stats.CreatedGroups;
            groupsQuota.QuotaValue = stats.AllocatedGroups;

            if (stats.AllocatedGroups != -1)
            {
                int groupsAvailable = groupsQuota.QuotaAvailable = stats.AllocatedGroups - stats.CreatedGroups;

                if (groupsAvailable <= 0)
                {
                    btnCreateGroup.Enabled = false;
                }
            }
        }

        protected void btnCreateGroup_Click(object sender, EventArgs e)
        {
            Response.Redirect(EditUrl("ItemID", PanelRequest.ItemID.ToString(), "create_secur_group",
                "SpaceID=" + PanelSecurity.PackageId.ToString()));
        }

        public string GetListEditUrl(string accountId)
        {
            return EditUrl("SpaceID", PanelSecurity.PackageId.ToString(), "secur_group_settings",
                    "AccountID=" + accountId,
                    "ItemID=" + PanelRequest.ItemID.ToString());
        }

        public bool IsNotDefault(string accountType)
        {
            return (ExchangeAccountType)Enum.Parse(typeof(ExchangeAccountType), accountType) != ExchangeAccountType.DefaultSecurityGroup;
        }

        protected void odsSecurityGroupsPaged_Selected(object sender, ObjectDataSourceStatusEventArgs e)
        {
            if (e.Exception != null)
            {
                messageBox.ShowErrorMessage("ORGANIZATION_GET_SECURITY_GROUP", e.Exception);
                e.ExceptionHandled = true;
            }
        }

        protected void gvSecurityGroups_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "DeleteItem")
            {
                // delete security group
                int accountId = Utils.ParseInt(e.CommandArgument.ToString(), 0);

                try
                {
                    int result = ES.Services.Organizations.DeleteSecurityGroup(PanelRequest.ItemID, accountId);
                    if (result < 0)
                    {
                        messageBox.ShowResultMessage(result);
                        return;
                    }

                    // rebind grid
                    gvGroups.DataBind();

                    // bind stats
                    BindStats();
                }
                catch (Exception ex)
                {
                    messageBox.ShowErrorMessage("ORGANIZATION_DELETE_SECURITY_GROUP", ex);
                }
            }
        }

        protected void gvSecurityGroups_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow && e.Row.Cells[1].Text.Length > _NotesMaxLength)
            {
                e.Row.Cells[1].Text = e.Row.Cells[1].Text.Substring(0, _NotesMaxLength - 3) + "...";
            }
        }

        protected void ddlPageSize_SelectedIndexChanged(object sender, EventArgs e)   
        {
            gvGroups.PageSize = Convert.ToInt32(ddlPageSize.SelectedValue);   
       
            // rebind grid   
            gvGroups.DataBind();

            // bind stats   
            BindStats();  
                
        }  
    }
}
