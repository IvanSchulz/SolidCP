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
using FuseCP.EnterpriseServer;
using FuseCP.Providers.Common;

namespace FuseCP.Portal.UserControls
{
    public partial class AllocatePackagePhoneNumbers : FuseCPModuleBase
    {
        private IPAddressPool pool;
        public IPAddressPool Pool
        {
            get { return pool; }
            set { pool = value; }
        }

        private string listAddressesControl;
        public string ListAddressesControl
        {
            get { return listAddressesControl; }
            set { listAddressesControl = value; }
        }

        private string resourceGroup;
        public string ResourceGroup
        {
            get { return resourceGroup; }
            set { resourceGroup = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindIPAddresses();
                ToggleControls();
            }
        }

        private void BindIPAddresses()
        {
            // bind list
            IPAddressInfo[] ips = ES.Services.Servers.GetUnallottedIPAddresses(PanelSecurity.PackageId, ResourceGroup, Pool);
            foreach (IPAddressInfo ip in ips)
            {
                string txt = ip.ExternalIP;

                if (!String.IsNullOrEmpty(ip.InternalIP))
                    txt += "/" + ip.InternalIP;

                listExternalAddresses.Items.Add(new ListItem(txt, ip.AddressId.ToString()));
            }

            int quotaAllowed = -1;
            string quotaName = Quotas.LYNC_PHONE;
            PackageContext cntx = PackagesHelper.GetCachedPackageContext(PanelSecurity.PackageId);
            if (cntx.Quotas.ContainsKey(quotaName))
            {
                int quotaAllocated = cntx.Quotas[quotaName].QuotaAllocatedValue;
                //int quotaUsed = cntx.Quotas[quotaName].QuotaUsedValue;

                int quotaUsed = ES.Services.Servers.GetPackageIPAddressesCount(PanelSecurity.PackageId, PanelRequest.ItemID, IPAddressPool.PhoneNumbers);

                if (quotaAllocated != -1)
                    quotaAllowed = quotaAllocated - quotaUsed;
            }

            // bind controls
            int max = quotaAllowed == -1 ? listExternalAddresses.Items.Count : Math.Min(quotaAllowed, listExternalAddresses.Items.Count);

            txtExternalAddressesNumber.Text = max.ToString();
            litMaxAddresses.Text = String.Format(GetLocalizedString("litMaxAddresses.Text"), max);

            if (max == 0)
            {
                AddressesTable.Visible = false;
                ErrorMessagesList.Visible = true;
                EmptyAddressesMessage.Visible = (listExternalAddresses.Items.Count == 0);
                QuotaReachedMessage.Visible = (quotaAllowed == 0);
                btnAdd.Enabled = false;
            }
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                List<int> ids = new List<int>();
                foreach (ListItem item in listExternalAddresses.Items)
                {
                    if (item.Selected)
                        ids.Add(Utils.ParseInt(item.Value));
                }

                ResultObject res = ES.Services.Servers.AllocatePackageIPAddresses(PanelSecurity.PackageId,
                                                     PanelRequest.ItemID,
                                                     ResourceGroup, Pool,
                                                     radioExternalRandom.Checked,
                                                     Utils.ParseInt(txtExternalAddressesNumber.Text),
                                                     ids.ToArray());
                if (res.IsSuccess)
                {
                    // return back
                    Response.Redirect(HostModule.EditUrl("ItemID", PanelRequest.ItemID.ToString(), ListAddressesControl,
                                    PortalUtils.SPACE_ID_PARAM + "=" + PanelSecurity.PackageId));
                }
                else
                {
                    // show message
                    messageBox.ShowMessage(res, "VPS_ALLOCATE_EXTERNAL_ADDRESSES_ERROR", "VPS");
                }
            }
            catch (Exception ex)
            {
                messageBox.ShowErrorMessage("VPS_ALLOCATE_EXTERNAL_ADDRESSES_ERROR", ex);
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect(HostModule.EditUrl("ItemID", PanelRequest.ItemID.ToString(), ListAddressesControl,
                PortalUtils.SPACE_ID_PARAM + "=" + PanelSecurity.PackageId));
        }

        protected void radioExternalSelected_CheckedChanged(object sender, EventArgs e)
        {
            ToggleControls();
        }

        private void ToggleControls()
        {
            AddressesNumberRow.Visible = radioExternalRandom.Checked;
            AddressesListRow.Visible = radioExternalSelected.Checked;
        }

        protected void radioExternalRandom_CheckedChanged(object sender, EventArgs e)
        {
            ToggleControls();
        }
    }
}
