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

﻿using System;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls;
using FuseCP.EnterpriseServer;
using FuseCP.Providers.Virtualization;

namespace FuseCP.Portal.VPS2012
{
    public partial class VdcHome : FuseCPModuleBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                searchBox.AddCriteria("ItemName", GetLocalizedString("SearchField.ItemName"));
                searchBox.AddCriteria("Username", GetLocalizedString("SearchField.Username"));
                searchBox.AddCriteria("ExternalIP", GetLocalizedString("SearchField.ExternalIP"));
                searchBox.AddCriteria("IPAddress", GetLocalizedString("SearchField.IPAddress"));
                searchBox.AddCriteria("DmzIP", GetLocalizedString("SearchField.DmzIP"));
            }
            searchBox.AjaxData = this.GetSearchBoxAjaxData();

            // toggle columns
            gvServers.Columns[3].Visible = PackagesHelper.IsQuotaEnabled(PanelSecurity.PackageId, Quotas.VPS2012_DMZ_NETWORK_ENABLED);
            bool isUserSelected = PanelSecurity.SelectedUser.Role == FuseCP.EnterpriseServer.UserRole.User;
            gvServers.Columns[4].Visible = !isUserSelected;
            gvServers.Columns[5].Visible = !isUserSelected;

            // replication
            gvServers.Columns[6].Visible = false;
            btnReplicaStates.Visible = PackagesHelper.IsQuotaEnabled(PanelSecurity.PackageId, Quotas.VPS2012_REPLICATION_ENABLED);

            // check package quotas
            bool manageAllowed = VirtualMachines2012Helper.IsVirtualMachineManagementAllowed(PanelSecurity.PackageId);
            bool reinstallAlloew = VirtualMachines2012Helper.IsReinstallAllowed(PanelSecurity.PackageId);

            btnCreate.Visible = manageAllowed;
            btnImport.Visible = (PanelSecurity.EffectiveUser.Role == UserRole.Administrator);
            gvServers.Columns[7].Visible = manageAllowed; // delete column
            gvServers.Columns[8].Visible = reinstallAlloew; // reinstall column

            // admin operations column
            gvServers.Columns[9].Visible = (PanelSecurity.EffectiveUser.Role == UserRole.Administrator);

        }

        public bool IsServerDeleting(string itemID)
        {
            bool isDeleting = false;
            VirtualMachine _vm = VirtualMachines2012Helper.GetCachedVirtualMachine(Convert.ToInt32(itemID));
            if(_vm != null)
            {
                if (_vm.ProvisioningStatus == VirtualMachineProvisioningStatus.DeletionProgress)
                    isDeleting = true;
            }
            return isDeleting;
        }

        public string GetServerEditUrl(string itemID)
        {
            return EditUrl("SpaceID", PanelSecurity.PackageId.ToString(), "vps_general",
                    "ItemID=" + itemID);
        }

        public string GetSpaceHomeUrl(string spaceId)
        {
            return EditUrl("SpaceID", spaceId, "");
        }

        public string GetUserHomeUrl(int userId)
        {
            return PortalUtils.GetUserHomePageUrl(userId);
        }

        private VirtualMachine[] _machines;
        public string GetReplicationStatus(int itemID)
        {
            if (!gvServers.Columns[6].Visible)
                return "";

            if (_machines == null)
            {
                var packageVm = ES.Services.VPS2012.GetVirtualMachineItem(itemID);
                _machines = ES.Services.VPS2012.GetVirtualMachinesByServiceId(packageVm.ServiceId);
            }

            var vmItem = ES.Services.VPS2012.GetVirtualMachineItem(itemID);
            if (vmItem == null) return "";

            var vm = _machines.FirstOrDefault(v => v.VirtualMachineId == vmItem.VirtualMachineId);
            return vm != null ? vm.ReplicationState.ToString() : "";
        }
        
        protected void odsServersPaged_Selected(object sender, ObjectDataSourceStatusEventArgs e)
        {
            if (e.Exception != null)
            {
                messageBox.ShowErrorMessage("VPS_ERROR_GET_VDC_HOME", e.Exception);
                e.ExceptionHandled = true;
            }
        }

        protected void btnCreate_Click(object sender, EventArgs e)
        {
            Response.Redirect(EditUrl("SpaceID", PanelSecurity.PackageId.ToString(), "vdc_create_server"));
        }

        protected void btnImport_Click(object sender, EventArgs e)
        {
            Response.Redirect(EditUrl("SpaceID", PanelSecurity.PackageId.ToString(), "vdc_import_server"));
        }

        protected void btnReplicaStates_Click(object sender, EventArgs e)
        {
            gvServers.Columns[6].Visible = PackagesHelper.IsQuotaEnabled(PanelSecurity.PackageId, Quotas.VPS2012_REPLICATION_ENABLED);
        }

        protected void gvServers_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "DeleteItem")
            {
                // get server ID
                int itemId = Utils.ParseInt(e.CommandArgument.ToString(), 0);

                // go to delete page
                Response.Redirect(EditUrl("SpaceID", PanelSecurity.PackageId.ToString(), "vps_tools_delete",
                    "ItemID=" + itemId));
            }
            else if (e.CommandName == "ReinstallItem")
            {
                // get server ID
                int itemId = Utils.ParseInt(e.CommandArgument.ToString(), 0);

                // go to delete page
                Response.Redirect(EditUrl("SpaceID", PanelSecurity.PackageId.ToString(), "vps_tools_reinstall",
                    "ItemID=" + itemId));
            }
            else if (e.CommandName == "Detach")
            {
                // remove item from meta base
                int itemId = Utils.ParseInt(e.CommandArgument.ToString(), 0);

                int result = ES.Services.Packages.DetachPackageItem(itemId);
                if (result < 0)
                {
                    ShowResultMessage(result);
                    return;
                }

                // refresh the list
                gvServers.DataBind();
            }
            else if (e.CommandName == "Move")
            {
                // get server ID
                int itemId = Utils.ParseInt(e.CommandArgument.ToString(), 0);

                // go to delete page
                Response.Redirect(EditUrl("SpaceID", PanelSecurity.PackageId.ToString(), "vps_tools_move",
                    "ItemID=" + itemId));
            }
        }

        protected void ddlPageSize_SelectedIndexChanged(object sender, EventArgs e)
        {
            gvServers.PageSize = Convert.ToInt32(ddlPageSize.SelectedValue);
            gvServers.DataBind();
        }

        public string GetSearchBoxAjaxData()
        {
            StringBuilder res = new StringBuilder();
            res.Append("PagedStored: 'VirtualMachines'");
            res.Append(", RedirectUrl: '" + GetServerEditUrl("{0}").Substring(2) + "'");
            res.Append(", PackageID: " + (String.IsNullOrEmpty(Request["SpaceID"]) ? "0" : Request["SpaceID"]));
            res.Append(", Recursive: true");
            res.Append(", VPSTypeID: 'VPS2012'");
            return res.ToString();
        }
    }
}
