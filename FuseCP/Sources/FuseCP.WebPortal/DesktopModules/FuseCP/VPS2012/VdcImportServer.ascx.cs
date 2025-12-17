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
﻿using FuseCP.Portal.Code.Helpers;
﻿using FuseCP.Providers.Virtualization;
using FuseCP.Providers.ResultObjects;

namespace FuseCP.Portal.VPS2012
{
    public partial class VdcImportServer : FuseCPModuleBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            bool isAdmin = (PanelSecurity.EffectiveUser.Role == UserRole.Administrator);
            if (!isAdmin)
                Response.Redirect(EditUrl("SpaceID", PanelSecurity.PackageId.ToString(), ""));

            if (!IsPostBack)
            {
                // bind hyper-V services
                BindHyperVServices();

                // bind virtual machines
                //BindVirtualMachines();

                // bind OS templates
                //BindOsTemplates();

                // bind IP addresses
                //BindExternalAddresses();
                //BindManagementAddresses();
            }

            ToggleControls();
        }

        private void ToggleControls()
        {
            VMsRow.Visible = (HyperVServices.SelectedValue != "");
            secOsTemplate.Visible = OsTemplatePanel.Visible = (VirtualMachines.SelectedValue != "");
            AdminPasswordPanel.Visible = EnableRemoteDesktop.Checked;
            RequiredAdminPassword.Enabled = EnableRemoteDesktop.Checked;
            VirtualMachinePanel.Visible = (VirtualMachines.SelectedValue != "");
            ExternalAddressesRow.Visible = (ExternalAdapters.SelectedIndex != 0);
            ManagementAddressesRow.Visible = (ManagementAdapters.SelectedIndex != 0);
        }

        private void BindHyperVServices()
        {
            // bind
            HyperVServices.DataSource = ES.Services.Servers.GetRawServicesByGroupName(ResourceGroups.VPS2012).Tables[0].DefaultView;
            HyperVServices.DataBind();

            // add select value
            HyperVServices.Items.Insert(0, new ListItem(GetLocalizedString("SelectHyperVService.Text"), ""));
        }

        private void BindVirtualMachines()
        {
            // clear list
            VirtualMachines.Items.Clear();

            // bind
            int serviceId = Utils.ParseInt(HyperVServices.SelectedValue, 0);
            if (serviceId > 0)
            {
                VirtualMachines.DataSource = ES.Services.VPS2012.GetVirtualMachinesByServiceId(serviceId);
                VirtualMachines.DataBind();
            }

            // add select value
            VirtualMachines.Items.Insert(0, new ListItem(GetLocalizedString("SelectVirtualMachine.Text"), ""));
        }

        private void BindOsTemplates()
        {
            // clear list
            OsTemplates.Items.Clear();

            int serviceId = Utils.ParseInt(HyperVServices.SelectedValue, 0);
            if (serviceId > 0)
            {
                OsTemplates.DataSource = ES.Services.VPS2012.GetOperatingSystemTemplatesByServiceId(serviceId);
                OsTemplates.DataBind();
            }
            OsTemplates.Items.Insert(0, new ListItem(GetLocalizedString("SelectOsTemplate.Text"), ""));
        }

        private void BindVirtualMachineDetails()
        {
            int serviceId = Utils.ParseInt(HyperVServices.SelectedValue, 0);
            string vmId = VirtualMachines.SelectedValue;
            if (serviceId > 0 && vmId != "")
            {
                // load package context (quotas informations)
                PackageContext cntx = PackagesHelper.GetCachedPackageContext(PanelSecurity.PackageId);

                VirtualMachine vm = ES.Services.VPS2012.GetVirtualMachineExtendedInfo(serviceId, vmId);
                if (vm != null)
                {
                    // bind VM
                    CpuCores.Text = vm.CpuCores.ToString();
                    RamSize.Text = vm.RamSize.ToString();
                    HddSize.Text = vm.HddSize[0].ToString();
                    VhdPath.Text = vm.VirtualHardDrivePath[0];

                    BindAdditionalHddInfo(vm);

                    this.BindSettingsControls(vm);

                    // snapshots number
                    if (cntx.Quotas.ContainsKey(Quotas.VPS2012_SNAPSHOTS_NUMBER))
                    {
                        int snapsNumber = cntx.Quotas[Quotas.VPS2012_SNAPSHOTS_NUMBER].QuotaAllocatedValue;
                        txtSnapshots.Text = (snapsNumber != -1) ? snapsNumber.ToString() : "";
                        txtSnapshots.Enabled = (snapsNumber != 0);
                    }

                    // other settings
                    NumLockEnabled.Value = chkNumLock.Checked = vm.NumLockEnabled;
                    BootFromCd.Value = chkBootFromCd.Checked = vm.BootFromCD;
                    DvdInstalled.Value = chkDvdInstalled.Checked = vm.DvdDriveInstalled;
                    ShowCheckBoxes(false);

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

        private void ShowCheckBoxes(bool showCheckBox)
        {
            divBootFromCdChkOption.Visible = 
                divNumLockChkOption.Visible = 
                divDvdInstalledChkOption.Visible = !showCheckBox;
            divBootFromCdChkBox.Visible = 
                divNumLockChkBox.Visible = 
                divDvdInstalledChkBox.Visible = showCheckBox;
        }

        private void BindExternalAddresses()
        {
            BindAddresses(ExternalAddresses, IPAddressPool.VpsExternalNetwork, ExternalAdapters.SelectedValue);
        }

        private void BindManagementAddresses()
        {
            BindAddresses(ManagementAddresses, IPAddressPool.VpsManagementNetwork, ExternalAdapters.SelectedValue);
        }

        private void BindAddresses(ListBox list, IPAddressPool pool, string selectedmac)
        {
            int serviceId = Utils.ParseInt(HyperVServices.SelectedValue, 0);
            string vmId = VirtualMachines.SelectedValue;
            int adaptervlan = 0;

            if (serviceId > 0 && vmId != "")
            {
                VirtualMachine vm = ES.Services.VPS2012.GetVirtualMachineExtendedInfo(serviceId, vmId);
                if (vm != null)
                {
                    foreach (VirtualMachineNetworkAdapter adapter in vm.Adapters)
                    {
                        if (adapter.MacAddress == selectedmac)
                        {
                            adaptervlan = adapter.vlan;
                        }
                    }
                }
            }
            list.Items.Clear();
            IPAddressInfo[] ips = ES.Services.Servers.GetUnallottedIPAddresses(PanelSecurity.PackageId, ResourceGroups.VPS2012, pool);
            //IPAddressInfo[] ips = ES.Services.Servers.GetUnallottedIPAddresses(-1, serviceId.ToString(), pool); //??? why do we do that??
            foreach (IPAddressInfo ip in ips)
            {
                if (ip.VLAN == adaptervlan)
                {
                    string txt = ip.ExternalIP;
                    if (!String.IsNullOrEmpty(ip.DefaultGateway))
                        txt += "/" + ip.DefaultGateway;
                    list.Items.Add(new ListItem(txt, ip.AddressId.ToString()));
                }
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
                IntResult res = ES.Services.VPS2012.ImportVirtualMachine(PanelSecurity.PackageId,
                    Utils.ParseInt(HyperVServices.SelectedValue),
                    VirtualMachines.SelectedValue,
                    OsTemplates.SelectedValue, adminPassword.Text,
                    chkBootFromCd.Checked, chkDvdInstalled.Checked,
                    AllowStartShutdown.Checked, AllowPause.Checked, AllowReboot.Checked, AllowReset.Checked, AllowReinstall.Checked,
                    ExternalAdapters.SelectedValue, extIps.ToArray(),
                    ManagementAdapters.SelectedValue, manIp,
                    Utils.ParseInt(txtSnapshots.Text.Trim()),
                    chkIgnoreCheckes.Checked);

                if (res.IsSuccess)
                {
                    Response.Redirect(EditUrl("ItemID", res.Value.ToString(), "vps_general",
                        "SpaceID=" + PanelSecurity.PackageId.ToString()));
                }
                else
                {
                    messageBox.ShowMessage(res, "VPS_ERROR_IMPORT", "VPS");
                    ShowCheckBoxes(true);
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
        protected void ExternalAdapters_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindExternalAddresses();
        }

        protected void ManagementAdapters_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindManagementAddresses();
        }

        private void BindAdditionalHddInfo(VirtualMachine vm)
        {
            repHdd.DataSource = GetAdditionalHdd(vm);
            repHdd.DataBind();
        }

        private List<AdditionalHdd> GetAdditionalHdd(VirtualMachine vm)
        {
            List<AdditionalHdd> result = new List<AdditionalHdd>();
            if (vm.HddSize.Length < 2 || vm.HddSize.Length != vm.VirtualHardDrivePath.Length) return result;
            for (int i = 1; i < vm.HddSize.Length; i++)
            {
                if (String.IsNullOrEmpty(vm.VirtualHardDrivePath[i])) continue;
                AdditionalHdd hdd = new AdditionalHdd(vm.HddSize[i], vm.VirtualHardDrivePath[i]);
                result.Add(hdd);
            }

            return result;
        }
    }
}
