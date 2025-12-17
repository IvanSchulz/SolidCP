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

using AjaxControlToolkit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;
using FuseCP.EnterpriseServer;
using FuseCP.EnterpriseServer.Base.HostedSolution;
using FuseCP.Providers.Common;
using FuseCP.Providers.HostedSolution;
using FuseCP.Providers.OS;

namespace FuseCP.Portal.RDS
{
    public partial class RDSEditCollectionUsers : FuseCPModuleBase
    {
        
        protected void Page_Load(object sender, EventArgs e)
        {
            users.Module = Module;
            users.OnRefreshClicked -= OnRefreshClicked;
            users.OnRefreshClicked += OnRefreshClicked;

            if (!IsPostBack)
            {
                var collection = ES.Services.RDS.GetRdsCollection(PanelRequest.CollectionID, true);
                litCollectionName.Text = collection.DisplayName;
                BindQuota();
                users.BindUsers();
            }
        }        

        private void BindQuota()
        {
            var quota = GetQuota();

            if (quota != null)
            {
                int rdsUsersCount = ES.Services.RDS.GetOrganizationRdsUsersCount(PanelRequest.ItemID);
                users.ButtonAddEnabled = (!(quota.QuotaAllocatedValue <= rdsUsersCount) || (quota.QuotaAllocatedValue == -1));
            }
        }

        private QuotaValueInfo GetQuota()
        {
            PackageContext cntx = PackagesHelper.GetCachedPackageContext(PanelSecurity.PackageId);
            OrganizationStatistics stats = ES.Services.Organizations.GetOrganizationStatisticsByOrganization(PanelRequest.ItemID);
            usersQuota.QuotaUsedValue = stats.CreatedRdsUsers;
            usersQuota.QuotaValue = stats.AllocatedRdsUsers;

            if (stats.AllocatedUsers != -1)
            {
                usersQuota.QuotaAvailable = stats.AllocatedRdsUsers - stats.CreatedRdsUsers;
            }
            
            if (cntx.Quotas.ContainsKey(Quotas.RDS_USERS))
            {
                return cntx.Quotas[Quotas.RDS_USERS];
            }

            return null;
        }

        private void OnRefreshClicked(object sender, EventArgs e)
        {           
            ((ModalPopupExtender)asyncTasks.FindControl("ModalPopupProperties")).Hide();
            var users = (List<string>)sender;

            if (users.Any())
            {
                messageBox.Visible = true;
                messageBox.ShowErrorMessage("RDS_USERS_NOT_DELETED", new Exception(string.Join(", ", users)));
            }
            else
            {
                messageBox.Visible = false;
            }
        }

        private bool SaveRdsUsers()
        {
            try
            {
                var quota = GetQuota();
                var rdsUsers = users.GetUsers();

                if (quota.QuotaAllocatedValue == -1 || quota.QuotaAllocatedValue >= rdsUsers.Count())
                {
                    messageBox.Visible = false;
                    ES.Services.RDS.SetUsersToRdsCollection(PanelRequest.ItemID, PanelRequest.CollectionID, users.GetUsers());                
                }   
                else
                {
                    throw new Exception("Too many RDS users added");
                }
            }
            catch (Exception ex)
            {
                messageBox.Visible = true;
                messageBox.ShowErrorMessage("RDS_USERS_NOT_UPDATED", ex);
                return false;
            }

            return true;
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid)
            {
                return;
            }

            SaveRdsUsers();
            BindQuota();
        }

        protected void btnSaveExit_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid)
            {
                return;
            }

            if (SaveRdsUsers())
            {
                Response.Redirect(EditUrl("ItemID", PanelRequest.ItemID.ToString(), "rds_collections", "SpaceID=" + PanelSecurity.PackageId));
            }
        }
    }
}
