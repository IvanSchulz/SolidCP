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
using System.Collections.Specialized;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

using FuseCP.EnterpriseServer;

namespace FuseCP.Portal.ProviderControls
{
    public partial class Common_IPAddressesList : FuseCPControlBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        public void BindSettings(StringDictionary settings)
        {
            string sips = settings["ListeningIPAddresses"];
            if (sips != null)
            {
                string[] ips = sips.Split(',');


                foreach (string ip in ips)
                {
                    ListItem li = CreateIPListItem(Utils.ParseInt(ip, 0));
                    if (li != null)
                        lbAddresses.Items.Add(li);
                }
            }
        }

        public void SaveSettings(StringDictionary settings)
        {
            string[] ips = new string[lbAddresses.Items.Count];
            for (int i = 0; i < lbAddresses.Items.Count; i++)
                ips[i] = lbAddresses.Items[i].Value;

            settings["ListeningIPAddresses"] = String.Join(",", ips);
        }

        protected void btnRemove_Click(object sender, EventArgs e)
        {
            if (lbAddresses.SelectedIndex != -1)
            {
                lbAddresses.Items.RemoveAt(lbAddresses.SelectedIndex);
            }
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            // check if the item is already added
            int addressId = ipAddress.AddressId;
            if (addressId == 0)
                return;

            foreach (ListItem li in lbAddresses.Items)
            {
                if (li.Value == addressId.ToString())
                    return;
            }

            lbAddresses.Items.Add(CreateIPListItem(addressId));
        }

        private ListItem CreateIPListItem(int addressId)
        {
            IPAddressInfo addr = ES.Services.Servers.GetIPAddress(addressId);
            if (addr != null)
            {
                string fullIP = addr.ExternalIP;
                if (addr.InternalIP != null &&
                    addr.InternalIP != "" &&
                    addr.InternalIP != addr.ExternalIP)
                    fullIP += " (" + addr.InternalIP + ")";

                return new ListItem(fullIP, addr.AddressId.ToString());
            }
            return null;
        }
    }
}
