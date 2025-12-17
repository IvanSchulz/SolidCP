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
using FuseCP.Providers.Virtualization;
using FuseCP.EnterpriseServer;
using FuseCP.Providers.Common;
using System.Text;
using System.Data;

namespace FuseCP.Portal.VPS2012
{
    public partial class VpsDetailsNetwork : FuseCPModuleBase
    {
        VirtualMachine vm = null;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindRealAssignedAddresses();
                BindVirtualMachine();
                ToggleButtons();
            }
        }

        private void BindVirtualMachine()
        {
            vm = ES.Services.VPS2012.GetVirtualMachineItem(PanelRequest.ItemID);

            // external network
            if (vm.ExternalNetworkEnabled)
            {
                BindExternalAddresses();
            }
            else
            {
                secExternalNetwork.Visible = false;
                ExternalNetworkPanel.Visible = false;
                btnRestoreExternalAddress.Visible = false;
            }

            // private network
            if (vm.PrivateNetworkEnabled)
            {
                BindPrivateAddresses();
            }
            else
            {
                secPrivateNetwork.Visible = false;
                PrivateNetworkPanel.Visible = false;
                btnRestorePrivateAddress.Visible = false;
            }

            // dmz network
            if (vm.DmzNetworkEnabled)
            {
                BindDmzAddresses();
            }
            else
            {
                secDmzNetwork.Visible = false;
                DmzNetworkPanel.Visible = false;
                btnRestoreDmzAddress.Visible = false;
            }
        }

        private void BindRealAssignedAddresses()
        {
            //VirtualMachine itemVM = VirtualMachines2012Helper.GetCachedVirtualMachine(PanelRequest.ItemID);
            //VirtualMachine _vm = null;
            VirtualMachineNetworkAdapter[] virtualMachineNetworkAdapters = null;
            try
            {
                virtualMachineNetworkAdapters = ES.Services.VPS2012.GetVirtualMachinesNetwordAdapterSettings(PanelRequest.ItemID);
            }
            catch (Exception ex)
            {
                messageBox.ShowErrorMessage("VPS_ERROR_GET_VM_DETAILS", ex);
            }

            try
            {
                repVMNetwork.DataSource = virtualMachineNetworkAdapters;//new VirtualMachineNetworkAdapter[vm.Adapters.Length];
                repVMNetwork.DataBind();
                BindGridViewOfVmIPs(virtualMachineNetworkAdapters);
                CheckIfPossibleToDoIpInjection(virtualMachineNetworkAdapters);
            }
            catch (Exception ex) //TODO: replace by messageBox ????
            {
                VMNetworkError.Text = "Error - " + ex;
                VMNetworkError.Visible = true;
            }                
        }

        private void CheckIfPossibleToDoIpInjection(VirtualMachineNetworkAdapter[] Adapters)
        {
            btnDeletePrivateByInject.Visible = 
                btnDeleteExternalByInject.Visible =
                btnDeleteDmzByInject.Visible =
                btnRestoreExternalAddress.Visible = 
                btnRestorePrivateAddress.Visible =
                btnRestoreDmzAddress.Visible = false;
            foreach (VirtualMachineNetworkAdapter adapter in Adapters)
            {
                if (adapter.IPAddresses != null && adapter.IPAddresses.Length > 0) //if we can get IP information at least from 1 adapter it means that VM support IP Injection.
                {
                    btnDeletePrivateByInject.Visible =
                        btnDeleteExternalByInject.Visible =
                        btnDeleteDmzByInject.Visible =
                        btnRestoreExternalAddress.Visible =
                        btnRestorePrivateAddress.Visible =
                        btnRestoreDmzAddress.Visible = true;
                    break;
                }
            }
        }

        private void BindGridViewOfVmIPs(VirtualMachineNetworkAdapter[] Adapters)
        {
            int i = 0;
            foreach (RepeaterItem item in repVMNetwork.Items)
            {
                DataTable dt = new DataTable();
                dt.Columns.Add("N", typeof(int));
                dt.Columns.Add("IP", typeof(string));
                for (int j = 0; j < Adapters[i].IPAddresses.Length; j++)
                {
                    DataRow NewRow = dt.NewRow();
                    NewRow["N"] = j + 1;
                    NewRow["IP"] = Adapters[i].IPAddresses[j];
                    dt.Rows.Add(NewRow);
                }
                (item.FindControl("gvVMNetwork") as GridView).DataSource = dt;
                (item.FindControl("gvVMNetwork") as GridView).DataBind();
                i++;
            }
        }

        private void BindExternalAddresses()
        {
            // load details
            NetworkAdapterDetails nic = ES.Services.VPS2012.GetExternalNetworkAdapterDetails(PanelRequest.ItemID);

            // bind details
            foreach (NetworkAdapterIPAddress ip in nic.IPAddresses)
            {
                if (ip.IsPrimary)
                {
                    litExtAddress.Text = ip.IPAddress;
                    litExtSubnet.Text = ip.SubnetMask;
                    litExtGateway.Text = ip.DefaultGateway;
                    break;
                }
            }
            litExtVLAN.Text = nic.VLAN.ToString();
            locExtVLAN.Visible = nic.VLAN > 0;
            litExtVLAN.Visible = locExtVLAN.Visible;
            lblTotalExternal.Text = nic.IPAddresses.Length.ToString();

            // bind IP addresses
            gvExternalAddresses.DataSource = nic.IPAddresses;
            gvExternalAddresses.DataBind();
        }

        protected bool IsVlanEnabled(Object VLAN)
        {
            int vlan = 0;
            if (VLAN != null) Int32.TryParse(VLAN.ToString(), out vlan);
            return vlan > 0;
        }

        private void BindPrivateAddresses()
        {
            // load details
            NetworkAdapterDetails nic = ES.Services.VPS2012.GetPrivateNetworkAdapterDetails(PanelRequest.ItemID);

            // bind details
            foreach (NetworkAdapterIPAddress ip in nic.IPAddresses)
            {
                if (ip.IsPrimary)
                {
                    litPrivAddress.Text = ip.IPAddress;
                    break;
                }
            }
            litPrivSubnet.Text = nic.SubnetMask;
            litPrivGateway.Text = nic.DefaultGateway;
            litPrivVLAN.Text = nic.VLAN.ToString();
            locPrivVLAN.Visible = nic.VLAN > 0;
            litPrivVLAN.Visible = locPrivVLAN.Visible;
            lblTotalPrivate.Text = nic.IPAddresses.Length.ToString();

            // bind IP addresses
            gvPrivateAddresses.DataSource = nic.IPAddresses;
            gvPrivateAddresses.DataBind();

            if (nic.IsDHCP)
            {
                PrivateAddressesPanel.Visible = false;
                litPrivAddress.Text = GetLocalizedString("Automatic.Text");
            }
        }

        private void BindDmzAddresses()
        {
            // load details
            NetworkAdapterDetails nic = ES.Services.VPS2012.GetDmzNetworkAdapterDetails(PanelRequest.ItemID);

            // bind details
            foreach (NetworkAdapterIPAddress ip in nic.IPAddresses)
            {
                if (ip.IsPrimary)
                {
                    litDmzAddress.Text = ip.IPAddress;
                    break;
                }
            }
            litDmzSubnet.Text = nic.SubnetMask;
            litDmzGateway.Text = nic.DefaultGateway;
            litDmzVLAN.Text = nic.VLAN.ToString();
            locDmzVLAN.Visible = nic.VLAN > 0;
            litDmzVLAN.Visible = locDmzVLAN.Visible;
            lblTotalDmz.Text = nic.IPAddresses.Length.ToString();

            // bind IP addresses
            gvDmzAddresses.DataSource = nic.IPAddresses;
            gvDmzAddresses.DataBind();

            if (nic.IsDHCP)
            {
                DmzAddressesPanel.Visible = false;
                litDmzAddress.Text = GetLocalizedString("Automatic.Text");
            }
        }

        private void ToggleButtons()
        {
            bool manageAllowed = VirtualMachines2012Helper.IsVirtualMachineManagementAllowed(PanelSecurity.PackageId);

            btnAddExternalAddress.Visible = manageAllowed;
            btnSetPrimaryExternal.Visible = manageAllowed;
            btnDeleteExternal.Visible = manageAllowed;
            gvExternalAddresses.Columns[0].Visible = manageAllowed;

            btnAddPrivateAddress.Visible = manageAllowed;
            btnSetPrimaryPrivate.Visible = manageAllowed;
            btnDeletePrivate.Visible = manageAllowed;
            gvPrivateAddresses.Columns[0].Visible = manageAllowed;

            btnAddDmzAddress.Visible = manageAllowed;
            btnSetPrimaryDmz.Visible = manageAllowed;
            btnDeleteDmz.Visible = manageAllowed;
            gvDmzAddresses.Columns[0].Visible = manageAllowed;
        }

        protected void btnRestoreExternalAddress_Click(object sender, EventArgs e)
        {
            try
            {
                ResultObject res = ES.Services.VPS2012.RestoreVirtualMachineExternalIPAddressesByInjection(PanelRequest.ItemID);
                if (res.IsSuccess)
                {
                    BindRealAssignedAddresses();
                    BindVirtualMachine();
                    return;
                }
                else
                {
                    messageBox.ShowMessage(res, "VPS_ERROR_RESTORE_EXTERNAL_IP", "VPS");
                    return;
                }
            }
            catch (Exception ex)
            {
                messageBox.ShowErrorMessage("VPS_ERROR_RESTORE_EXTERNAL_IP", ex);
            }
        }

        protected void btnRestorePrivateByInject_Click(object sender, EventArgs e)
        {
            try
            {
                ResultObject res = ES.Services.VPS2012.RestoreVirtualMachinePrivateIPAddressesByInjection(PanelRequest.ItemID);
                if (res.IsSuccess)
                {
                    BindRealAssignedAddresses();
                    BindVirtualMachine();
                    return;
                }
                else
                {
                    messageBox.ShowMessage(res, "VPS_ERROR_RESTORE_PRIVATE_IP", "VPS");
                    return;
                }
            }
            catch (Exception ex)
            {
                messageBox.ShowErrorMessage("VPS_ERROR_RESTORE_PRIVATE_IP", ex);
            }
        }

        protected void btnRestoreDmzByInject_Click(object sender, EventArgs e)
        {
            try
            {
                ResultObject res = ES.Services.VPS2012.RestoreVirtualMachineDmzIPAddressesByInjection(PanelRequest.ItemID);
                if (res.IsSuccess)
                {
                    BindRealAssignedAddresses();
                    BindVirtualMachine();
                    return;
                }
                else
                {
                    messageBox.ShowMessage(res, "VPS_ERROR_RESTORE_DMZ_IP", "VPS");
                    return;
                }
            }
            catch (Exception ex)
            {
                messageBox.ShowErrorMessage("VPS_ERROR_RESTORE_DMZ_IP", ex);
            }
        }

        protected void btnAddExternalAddress_Click(object sender, EventArgs e)
        {
            Response.Redirect(EditUrl("ItemID", PanelRequest.ItemID.ToString(), "vps_add_external_ip",
                "SpaceID=" + PanelSecurity.PackageId.ToString()));
        }

        protected void btnAddPrivateAddress_Click(object sender, EventArgs e)
        {
            Response.Redirect(EditUrl("ItemID", PanelRequest.ItemID.ToString(), "vps_add_private_ip",
                "SpaceID=" + PanelSecurity.PackageId.ToString()));
        }

        protected void btnAddDmzAddress_Click(object sender, EventArgs e)
        {
            Response.Redirect(EditUrl("ItemID", PanelRequest.ItemID.ToString(), "vps_add_dmz_ip",
                "SpaceID=" + PanelSecurity.PackageId.ToString()));
        }

        protected void btnSetPrimaryPrivate_Click(object sender, EventArgs e)
        {
            int[] addressIds = GetSelectedItems(gvPrivateAddresses);
            
            // check if at least one is selected
            if (addressIds.Length == 0)
            {
                messageBox.ShowWarningMessage("IP_ADDRESS_NOT_SELECTED");
                return;
            }

            try
            {
                ResultObject res = ES.Services.VPS2012.SetVirtualMachinePrimaryPrivateIPAddress(PanelRequest.ItemID, addressIds[0]);

                if (res.IsSuccess)
                {
                    BindPrivateAddresses();
                    return;
                }
                else
                {
                    messageBox.ShowMessage(res, "VPS_ERROR_SETTING_PRIMARY_IP", "VPS");
                    return;
                }
            }
            catch (Exception ex)
            {
                messageBox.ShowErrorMessage("VPS_ERROR_SETTING_PRIMARY_IP", ex);
            }
        }

        protected void btnSetPrimaryDmz_Click(object sender, EventArgs e)
        {
            int[] addressIds = GetSelectedItems(gvDmzAddresses);

            // check if at least one is selected
            if (addressIds.Length == 0)
            {
                messageBox.ShowWarningMessage("IP_ADDRESS_NOT_SELECTED");
                return;
            }

            try
            {
                ResultObject res = ES.Services.VPS2012.SetVirtualMachinePrimaryDmzIPAddress(PanelRequest.ItemID, addressIds[0]);

                if (res.IsSuccess)
                {
                    BindDmzAddresses();
                    return;
                }
                else
                {
                    messageBox.ShowMessage(res, "VPS_ERROR_SETTING_PRIMARY_IP", "VPS");
                    return;
                }
            }
            catch (Exception ex)
            {
                messageBox.ShowErrorMessage("VPS_ERROR_SETTING_PRIMARY_IP", ex);
            }
        }

        protected void btnDeletePrivateByInject_Click(object sender, EventArgs e)
        {
            DeletePrivate(sender, e, true);
        }

        protected void btnDeletePrivate_Click(object sender, EventArgs e)
        {
            DeletePrivate(sender, e, false);
        }

        protected void DeletePrivate(object sender, EventArgs e, bool byNewMethod)
        {
            int[] addressIds = GetSelectedItems(gvPrivateAddresses);

            // check if at least one is selected
            if (addressIds.Length == 0)
            {
                messageBox.ShowWarningMessage("IP_ADDRESS_NOT_SELECTED");
                return;
            }

            try
            {
                ResultObject res = null;
                if (byNewMethod)
                    res = ES.Services.VPS2012.DeleteVirtualMachinePrivateIPAddressesByInject(PanelRequest.ItemID, addressIds);
                else
                    res = ES.Services.VPS2012.DeleteVirtualMachinePrivateIPAddresses(PanelRequest.ItemID, addressIds);

                if (res.IsSuccess)
                {
                    BindPrivateAddresses();
                    return;
                }
                else
                {
                    messageBox.ShowMessage(res, "VPS_ERROR_DELETING_IP_ADDRESS", "VPS");
                    return;
                }
            }
            catch (Exception ex)
            {
                messageBox.ShowErrorMessage("VPS_ERROR_DELETING_IP_ADDRESS", ex);
            }
        }

        protected void btnDeleteDmzByInject_Click(object sender, EventArgs e)
        {
            DeleteDmz(sender, e, true);
        }

        protected void btnDeleteDmz_Click(object sender, EventArgs e)
        {
            DeleteDmz(sender, e, false);
        }

        protected void DeleteDmz(object sender, EventArgs e, bool byNewMethod)
        {
            int[] addressIds = GetSelectedItems(gvDmzAddresses);

            // check if at least one is selected
            if (addressIds.Length == 0)
            {
                messageBox.ShowWarningMessage("IP_ADDRESS_NOT_SELECTED");
                return;
            }

            try
            {
                ResultObject res = null;
                if (byNewMethod)
                    res = ES.Services.VPS2012.DeleteVirtualMachineDmzIPAddressesByInject(PanelRequest.ItemID, addressIds);
                else
                    res = ES.Services.VPS2012.DeleteVirtualMachineDmzIPAddresses(PanelRequest.ItemID, addressIds);

                if (res.IsSuccess)
                {
                    BindDmzAddresses();
                    return;
                }
                else
                {
                    messageBox.ShowMessage(res, "VPS_ERROR_DELETING_IP_ADDRESS", "VPS");
                    return;
                }
            }
            catch (Exception ex)
            {
                messageBox.ShowErrorMessage("VPS_ERROR_DELETING_IP_ADDRESS", ex);
            }
        }

        protected void btnSetPrimaryExternal_Click(object sender, EventArgs e)
        {
            int[] addressIds = GetSelectedItems(gvExternalAddresses);

            // check if at least one is selected
            if (addressIds.Length == 0)
            {
                messageBox.ShowWarningMessage("IP_ADDRESS_NOT_SELECTED");
                return;
            }

            try
            {
                ResultObject res = ES.Services.VPS2012.SetVirtualMachinePrimaryExternalIPAddress(PanelRequest.ItemID, addressIds[0]);

                if (res.IsSuccess)
                {
                    BindExternalAddresses();
                    return;
                }
                else
                {
                    messageBox.ShowMessage(res, "VPS_ERROR_SETTING_PRIMARY_IP", "VPS");
                    return;
                }
            }
            catch (Exception ex)
            {
                messageBox.ShowErrorMessage("VPS_ERROR_SETTING_PRIMARY_IP", ex);
            }
        }

        protected void btnDeleteExternal_Click(object sender, EventArgs e)
        {
            DeleteExternal(sender, e, false);
        }
        protected void btnDeleteExternalByInject_Click(object sender, EventArgs e)
        {
            DeleteExternal(sender, e, true);
        }

        protected void DeleteExternal(object sender, EventArgs e, bool byNewMethod)
        {
            int[] addressIds = GetSelectedItems(gvExternalAddresses);

            // check if at least one is selected
            if (addressIds.Length == 0)
            {
                messageBox.ShowWarningMessage("IP_ADDRESS_NOT_SELECTED");
                return;
            }

            try
            {
                ResultObject res = null;
                if(byNewMethod)
                    res = ES.Services.VPS2012.DeleteVirtualMachineExternalIPAddressesByInjection(PanelRequest.ItemID, addressIds);
                else
                    res = ES.Services.VPS2012.DeleteVirtualMachineExternalIPAddresses(PanelRequest.ItemID, addressIds);

                if (res.IsSuccess)
                {
                    BindExternalAddresses();
                    return;
                }
                else
                {
                    messageBox.ShowMessage(res, "VPS_ERROR_DELETING_IP_ADDRESS", "VPS");
                    return;
                }
            }
            catch (Exception ex)
            {
                messageBox.ShowErrorMessage("VPS_ERROR_DELETING_IP_ADDRESS", ex);
            }
        }        

        private int[] GetSelectedItems(GridView gv)
        {
            List<int> items = new List<int>();

            for (int i = 0; i < gv.Rows.Count; i++)
            {
                GridViewRow row = gv.Rows[i];
                CheckBox chkSelect = (CheckBox)row.FindControl("chkSelect");
                if (chkSelect.Checked)
                    items.Add((int)gv.DataKeys[i].Value);
            }

            return items.ToArray();
        }
    }
}
