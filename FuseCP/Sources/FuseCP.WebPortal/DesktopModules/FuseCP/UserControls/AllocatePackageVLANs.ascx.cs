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
using System.Web.UI.WebControls;
using FuseCP.EnterpriseServer;
using FuseCP.Providers.Common;

namespace FuseCP.Portal.UserControls
{
    public partial class AllocatePackageVLANs : FuseCPControlBase
    {
        private string listVLANsControl;
        public string ListVLANsControl
        {
            get { return listVLANsControl; }
            set { listVLANsControl = value; }
        }

        private string resourceGroup;
        public string ResourceGroup
        {
            get { return resourceGroup; }
            set { resourceGroup = value; }
        }

        private bool isDmz;
        public bool IsDmz
        {
            get { return isDmz; }
            set { isDmz = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindVLANs();
                ToggleControls();
            }
        }

        private void BindVLANs()
        {
            // bind list
            VLANInfo[] vlans = ES.Services.Servers.GetUnallottedVLANs(PanelSecurity.PackageId, ResourceGroup);
            foreach (VLANInfo vlan in vlans)
            {
                listVLANs.Items.Add(new ListItem(vlan.Vlan.ToString(), vlan.VlanId.ToString()));
            }

            int quotaAllowed = -1;
            string quotaName;
            if (isDmz)
            {
                quotaName = Quotas.VPS2012_DMZ_VLANS_NUMBER;
            }
            else
            {
                quotaName = Quotas.VPS2012_PRIVATE_VLANS_NUMBER;
            }

            PackageContext cntx = PackagesHelper.GetCachedPackageContext(PanelSecurity.PackageId);
            if (cntx.Quotas.ContainsKey(quotaName))
            {
                int quotaAllocated = cntx.Quotas[quotaName].QuotaAllocatedValue;
                int quotaUsed = cntx.Quotas[quotaName].QuotaUsedValue;

                if (quotaAllocated != -1)
                    quotaAllowed = quotaAllocated - quotaUsed;
            }

            // bind controls
            int max = quotaAllowed == -1 ? listVLANs.Items.Count : quotaAllowed;

            txtVLANsNumber.Text = max.ToString();
            listVLANs.Text = String.Format(GetLocalizedString("litMaxVLANs.Text"), max);

            if (max <= 0)
            {
                VLANsTable.Visible = false;
                ErrorMessagesList.Visible = true;
                EmptyVLANsMessage.Visible = (listVLANs.Items.Count == 0);
                QuotaReachedMessage.Visible = (quotaAllowed <= 0);
                btnAdd.Enabled = false;
            }
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                List<int> ids = new List<int>();
                foreach (ListItem item in listVLANs.Items)
                {
                    if (item.Selected)
                        ids.Add(Utils.ParseInt(item.Value));
                }

                ResultObject res = ES.Services.Servers.AllocatePackageVLANs(PanelSecurity.PackageId, ResourceGroup, 
                    radioVLANRandom.Checked, Utils.ParseInt(txtVLANsNumber.Text), ids.ToArray(), isDmz);

                if (res.IsSuccess)
                {
                    // return back
                    if (isDmz)
                    {
                        Response.Redirect(EditUrl(PortalUtils.SPACE_ID_PARAM, PanelSecurity.PackageId.ToString(), "vdc_dmz_network"));
                    }
                    else
                    {
                        Response.Redirect(EditUrl(PortalUtils.SPACE_ID_PARAM, PanelSecurity.PackageId.ToString(), "vdc_private_network"));
                    }
                }
                else
                {
                    // show message
                    messageBox.ShowMessage(res, "VPS_ALLOCATE_PRIVATE_VLANS_ERROR", "VPS");
                }
            }
            catch (Exception ex)
            {
                messageBox.ShowErrorMessage("VPS_ALLOCATE_PRIVATE_VLANS_ERROR", ex);
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            if (isDmz)
            {
                Response.Redirect(EditUrl(PortalUtils.SPACE_ID_PARAM, PanelSecurity.PackageId.ToString(), "vdc_dmz_network"));
            }
            else
            {
                Response.Redirect(EditUrl(PortalUtils.SPACE_ID_PARAM, PanelSecurity.PackageId.ToString(), "vdc_private_network"));
            }
        }

        protected void radioVLANSelected_CheckedChanged(object sender, EventArgs e)
        {
            ToggleControls();
        }

        private void ToggleControls()
        {
            VLANsNumberRow.Visible = radioVLANRandom.Checked;
            VLANsListRow.Visible = radioVLANSelected.Checked;
        }

        protected void radioVLANRandom_CheckedChanged(object sender, EventArgs e)
        {
            ToggleControls();
        }
    }
}
