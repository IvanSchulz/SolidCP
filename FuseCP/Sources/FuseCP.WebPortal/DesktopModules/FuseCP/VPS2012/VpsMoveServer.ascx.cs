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
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using FuseCP.EnterpriseServer;
using FuseCP.Providers.Virtualization;
using FuseCP.Providers.ResultObjects;

namespace FuseCP.Portal.VPS2012
{
    public partial class VpsMoveServer : FuseCPModuleBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            bool isAdmin = (PanelSecurity.EffectiveUser.Role == UserRole.Administrator);
            if (!isAdmin) 
                Response.Redirect(EditUrl("SpaceID", PanelSecurity.PackageId.ToString(), ""));

            if (!IsPostBack)
            {
                BindHyperVServices();
                BindSourceService();
            }
        }

        public void BindHyperVServices()
        {
            // bind
            HyperVServices.DataSource = ES.Services.Servers.GetRawServicesByGroupName(ResourceGroups.VPS2012).Tables[0].DefaultView;
            HyperVServices.DataBind();

            // add select value
            HyperVServices.Items.Insert(0, new ListItem(GetLocalizedString("SelectHyperVService.Text"), ""));
        }

        private void BindSourceService()
        {
            VirtualMachine vm = ES.Services.VPS2012.GetVirtualMachineItem(PanelRequest.ItemID);
            if (vm == null)
                ReturnBack();

            ListItem sourceItem = null;
            foreach (ListItem item in HyperVServices.Items)
            {
                if (item.Value == vm.ServiceId.ToString())
                {
                    sourceItem = item;
                    SourceHyperVService.Text = item.Text;
                    break;
                }
            }

            if (sourceItem != null)
                HyperVServices.Items.Remove(sourceItem);
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            ReturnBack();
        }

        private void ReturnBack()
        {
            Response.Redirect(EditUrl("SpaceID", PanelSecurity.PackageId.ToString(), ""));
        }

        protected void btnMove_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid)
                return;

            // move item
            int destinationServiceId = Utils.ParseInt(HyperVServices.SelectedValue);
            int result = ES.Services.Packages.MovePackageItem(PanelRequest.ItemID, destinationServiceId);
            if (result < 0)
            {
                ShowResultMessage(result);
                return;
            }

            // redirect to properties screen
            Response.Redirect(EditUrl("ItemID", PanelRequest.ItemID.ToString(), "vps_general",
                "SpaceID=" + PanelSecurity.PackageId.ToString()));
        }
    }
}
