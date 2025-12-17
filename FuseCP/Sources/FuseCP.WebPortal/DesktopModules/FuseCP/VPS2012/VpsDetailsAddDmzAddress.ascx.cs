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
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FuseCP.Providers.Common;
using FuseCP.EnterpriseServer;
using FuseCP.Providers.Virtualization;

namespace FuseCP.Portal.VPS2012
{
    public partial class VpsDetailsAddDmzAddress : FuseCPModuleBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindControls();
            }

            ToggleControls();
        }

        private void BindControls()
        {
            // load adapter details
            NetworkAdapterDetails nic = ES.Services.VPS2012.GetDmzNetworkAdapterDetails(PanelRequest.ItemID);

            // load package context
            PackageContext cntx = PackagesHelper.GetCachedPackageContext(PanelSecurity.PackageId);

            if (cntx.Quotas.ContainsKey(Quotas.VPS2012_DMZ_IP_ADDRESSES_NUMBER))
            {
                // set max number
                QuotaValueInfo privQuota = cntx.Quotas[Quotas.VPS2012_DMZ_IP_ADDRESSES_NUMBER];
                int maxDmz = privQuota.QuotaAllocatedValue;
                if (maxDmz == -1)
                    maxDmz = 10;

                maxDmz -= nic.IPAddresses.Length;

                txtDmzAddressesNumber.Text = maxDmz.ToString();
                litMaxDmzAddresses.Text = String.Format(GetLocalizedString("litMaxDmzAddresses.Text"), maxDmz);
                btnAdd.Enabled = btnAddByInject.Enabled = maxDmz > 0;
                txtGateway.Text = nic.DefaultGateway;
                txtDNS1.Text = nic.PreferredNameServer;
                txtDNS2.Text = nic.AlternateNameServer;
                txtMask.Text = nic.SubnetMask;
            }
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            AddIP(sender, e, false);
        }

        protected void btnAddByInject_Click(object sender, EventArgs e)
        {
            AddIP(sender, e, true);
        }

        protected void AddIP(object sender, EventArgs e, bool byNewMethod)
        {
            int number = Utils.ParseInt(txtDmzAddressesNumber.Text.Trim(), 0);
            string[] dmzIps = Utils.ParseDelimitedString(txtDmzAddressesList.Text, '\n', '\r', ' ', '\t');

            try
            {
                ResultObject res = null;

                if (byNewMethod)
                    res = ES.Services.VPS2012.AddVirtualMachineDmzIPAddressesByInject(PanelRequest.ItemID,
                    radioDmzRandom.Checked, number, dmzIps, chkCustomGateway.Checked, txtGateway.Text, txtDNS1.Text, txtDNS2.Text, txtMask.Text);
                else
                    res = ES.Services.VPS2012.AddVirtualMachineDmzIPAddresses(PanelRequest.ItemID,
                    radioDmzRandom.Checked, number, dmzIps, chkCustomGateway.Checked, txtGateway.Text, txtDNS1.Text, txtDNS2.Text, txtMask.Text);

                if (res.IsSuccess)
                {
                    RedirectBack();
                }
                else
                {
                    messageBox.ShowMessage(res, "VPS_ERROR_ADDING_IP_ADDRESS", "VPS");
                }
            }
            catch (Exception ex)
            {
                messageBox.ShowErrorMessage("VPS_ERROR_ADDING_IP_ADDRESS", ex);
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            RedirectBack();
        }

        private void RedirectBack()
        {
            Response.Redirect(EditUrl("ItemID", PanelRequest.ItemID.ToString(), "vps_network",
                "SpaceID=" + PanelSecurity.PackageId.ToString()));
        }

        private void ToggleControls()
        {
            DmzAddressesNumberRow.Visible = radioDmzRandom.Checked;
            DmzAddressesListRow.Visible = radioDmzSelected.Checked;
            trCustomGateway.Visible = chkCustomGateway.Checked;
        }
    }
}
