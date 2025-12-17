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
using System.Collections.Generic;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

using FuseCP.EnterpriseServer;
using FuseCP.Providers.Common;

namespace FuseCP.Portal
{
    public partial class PhoneNumbersEditPhoneNumber : FuseCPModuleBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                try
                {
                    // bind dropdowns
                    BindServers();

                    // bind IP
                    BindPhone();
                }
                catch (Exception ex)
                {
                    ShowErrorMessage("IP_GET_IP", ex);
                    return;
                }
            }
        }

        private void BindPhone()
        {
            int addressId = PanelRequest.AddressID;

            // check if multiple editing
            if (!String.IsNullOrEmpty(PanelRequest.Addresses))
            {
                string[] ids = PanelRequest.Addresses.Split(',');
                addressId = Utils.ParseInt(ids[0], 0);
            }

            // bind first address
            IPAddressInfo addr = ES.Services.Servers.GetIPAddress(addressId);

            if (addr != null)
            {
                Utils.SelectListItem(ddlServer, addr.ServerId);

                Phone.Text = addr.ExternalIP;
                txtComments.Text = addr.Comments;

                ToggleControls();
            }
            else
            {
                // exit
                RedirectBack();
            }
        }

        private void BindServers()
        {
            ServerInfo[] allServers = ES.Services.Servers.GetServers();
            List<ServerInfo> servers = new List<ServerInfo>();
            foreach (ServerInfo server in allServers)
            {
                ServiceInfo[] service = ES.Services.Servers.GetServicesByServerIdGroupName(server.ServerId, ResourceGroups.Lync);
                if (service.Length > 0) servers.Add(server);
            }

            ddlServer.DataSource = servers;

            ddlServer.DataBind();
            ddlServer.Items.Insert(0, new ListItem(GetLocalizedString("Text.NotAssigned"), ""));
        }

        private void RedirectBack()
        {
            Response.Redirect(NavigateURL("PoolID", "PhoneNumbers"));
        }

        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                try
                {
                    int serverId = Utils.ParseInt(ddlServer.SelectedValue, 0);
                    IPAddressPool pool = IPAddressPool.PhoneNumbers;

                    ResultObject res = null;

                    // update single IP address
                    if (!String.IsNullOrEmpty(PanelRequest.Addresses))
                    {
                        // update multiple IPs
                        string[] ids = PanelRequest.Addresses.Split(',');
                        int[] addresses = new int[ids.Length];
                        for (int i = 0; i < ids.Length; i++)
                            addresses[i] = Utils.ParseInt(ids[i], 0);

                        res = ES.Services.Servers.UpdateIPAddresses(addresses,
                            pool, serverId, "", "", txtComments.Text.Trim(), 0);
                    }
                    else
                    {
                        string address = Phone.Text;

                        // update single IP
                        res = ES.Services.Servers.UpdateIPAddress(PanelRequest.AddressID,
                            pool, serverId, address, "", "", "", txtComments.Text.Trim(), 0);
                    }

                    if (!res.IsSuccess)
                    {
                        messageBox.ShowMessage(res, "IP_UPDATE_IP", "IP");
                        return;
                    }

                    //	 Redirect back to the portal home page
                    RedirectBack();
                }
                catch (Exception ex)
                {
                    ShowErrorMessage("IP_UPDATE_IP", ex);
                    return;
                }
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            // Redirect back to the portal home page
            RedirectBack();
        }

        protected void ddlPools_SelectedIndexChanged(object sender, EventArgs e)
        {
            ToggleControls();
        }

        private void ToggleControls()
        {
            bool multipleEdit = !String.IsNullOrEmpty(PanelRequest.Addresses);
            PhoneNumbersRow.Visible = !multipleEdit;
            requireStartPhoneValidator.Enabled = !multipleEdit;
        }
    }
}
