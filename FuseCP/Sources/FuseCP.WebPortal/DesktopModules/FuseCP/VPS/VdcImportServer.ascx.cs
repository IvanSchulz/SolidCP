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

namespace FuseCP.Portal.VPS
{
    public partial class VdcImportServer : FuseCPModuleBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // bind hyper-V services
                BindHyperVServices();

                // bind virtual machines
                BindVirtualMachines();

                // bind OS templates
                BindOsTemplates();

                // bind IP addresses
                BindExternalAddresses();
                BindManagementAddresses();
            }

            ToggleControls();
        }

        private void ToggleControls()
        {
            AdminPasswordPanel.Visible = EnableRemoteDesktop.Checked;
            RequiredAdminPassword.Enabled = EnableRemoteDesktop.Checked;
            VirtualMachinePanel.Visible = (VirtualMachines.SelectedValue != "");
            ExternalAddressesRow.Visible = (ExternalAdapters.SelectedIndex != 0);
            ManagementAddressesRow.Visible = (ManagementAdapters.SelectedIndex != 0);
        }

        public void BindHyperVServices()
        {
            // bind
            HyperVServices.DataSource = ES.Services.Servers.GetRawServicesByGroupName(ResourceGroups.VPS).Tables[0].DefaultView;
            HyperVServices.DataBind();

            // add select value
            HyperVServices.Items.Insert(0, new ListItem(GetLocalizedString("SelectHyperVService.Text"), ""));
        }

        public void BindVirtualMachines()
        {
            // clear list
            VirtualMachines.Items.Clear();

            // bind
            int serviceId = Utils.ParseInt(HyperVServices.SelectedValue, 0);
            if (serviceId > 0)
            {
                VirtualMachines.DataSource = ES.Services.VPS.GetVirtualMachinesByServiceId(serviceId);
                VirtualMachines.DataBind();
            }

            // add select value
            VirtualMachines.Items.Insert(0, new ListItem(GetLocalizedString("SelectVirtualMachine.Text"), ""));
        }

        public void BindOsTemplates()
        {
            // clear list
            OsTemplates.Items.Clear();

            int serviceId = Utils.ParseInt(HyperVServices.SelectedValue, 0);
            if (serviceId > 0)
            {
                OsTemplates.DataSource = ES.Services.VPS.GetOperatingSystemTemplatesByServiceId(serviceId);
                OsTemplates.DataBind();
            }
            OsTemplates.Items.Insert(0, new ListItem(GetLocalizedString("SelectOsTemplate.Text"), ""));
        }

        public void BindVirtualMachineDetails()
        {
            int serviceId = Utils.ParseInt(HyperVServices.SelectedValue, 0);
            string vmId = VirtualMachines.SelectedValue;
            if (serviceId > 0 && vmId != "")
            {
                VirtualMachine vm = ES.Services.VPS.GetVirtualMachineExtendedInfo(serviceId, vmId);
                if (vm != null)
                {
                    // bind VM
                    CpuCores.Text = vm.CpuCores.ToString();
                    RamSize.Text = vm.RamSize.ToString();
                    HddSize.Text = vm.HddSize[0].ToString();
                    VhdPath.Text = vm.VirtualHardDrivePath[0];

                    // other settings
                    NumLockEnabled.Value = vm.NumLockEnabled;
                    BootFromCd.Value = vm.BootFromCD;
                    DvdInstalled.Value = vm.DvdDriveInstalled;

                    // network adapters
                    ExternalAdapters.DataSource = vm.Adapters;
                    ExternalAdapters.DataBind();
                    ExternalAdapters.Items.Insert(0, new ListItem(GetLocalizedString("SelectNetworkAdapter.Text"), ""));

                    ManagementAdapters.DataSource = vm.Adapters;
                    ManagementAdapters.DataBind();
                    ManagementAdapters.Items.Insert(0, new ListItem(GetLocalizedString("SelectNetworkAdapter.Text"), ""));
                }
            }
        }

        public void BindExternalAddresses()
        {
            BindAddresses(ExternalAddresses, IPAddressPool.VpsExternalNetwork);
        }

        public void BindManagementAddresses()
        {
            BindAddresses(ManagementAddresses, IPAddressPool.VpsManagementNetwork);
        }

        public void BindAddresses(ListBox list, IPAddressPool pool)
        {
            IPAddressInfo[] ips = ES.Services.Servers.GetUnallottedIPAddresses(PanelSecurity.PackageId, ResourceGroups.VPS, pool);
            foreach (IPAddressInfo ip in ips)
            {
                string txt = ip.ExternalIP;
                if (!String.IsNullOrEmpty(ip.DefaultGateway))
                    txt += "/" + ip.DefaultGateway;
                list.Items.Add(new ListItem(txt, ip.AddressId.ToString()));
            }
        }

        protected void btnImport_Click(object sender, EventArgs e)
        {
            try
            {
                // external IPs
                List<int> extIps = new List<int>();
                foreach (ListItem li in ExternalAddresses.Items)
                    if (li.Selected) extIps.Add(Utils.ParseInt(li.Value));

                // management IPs
                int manIp = 0;
                foreach (ListItem li in ManagementAddresses.Items)
                    if (li.Selected)
                    {
                        manIp = Utils.ParseInt(li.Value);
                        break;
                    }

                // create virtual machine
                IntResult res = ES.Services.VPS.ImportVirtualMachine(PanelSecurity.PackageId,
                    Utils.ParseInt(HyperVServices.SelectedValue),
                    VirtualMachines.SelectedValue,
                    OsTemplates.SelectedValue, adminPassword.Text,
                    AllowStartShutdown.Checked, AllowPause.Checked, AllowReboot.Checked, AllowReset.Checked, false,
                    ExternalAdapters.SelectedValue, extIps.ToArray(),
                    ManagementAdapters.SelectedValue, manIp);

                if (res.IsSuccess)
                {
                    Response.Redirect(EditUrl("ItemID", res.Value.ToString(), "vps_general",
                        "SpaceID=" + PanelSecurity.PackageId.ToString()));
                }
                else
                {
                    messageBox.ShowMessage(res, "VPS_ERROR_IMPORT", "VPS");
                }
            }
            catch (Exception ex)
            {
                messageBox.ShowErrorMessage("VPS_ERROR_IMPORT", ex);
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect(EditUrl("SpaceID", PanelSecurity.PackageId.ToString(), ""));
        }

        protected void HyperVServices_SelectedIndexChanged(object sender, EventArgs e)
        {
            // bind VMs
            BindVirtualMachines();

            // bind OS templates
            BindOsTemplates();
        }

        protected void VirtualMachines_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindVirtualMachineDetails();
            ToggleControls();
        }
    }
}
