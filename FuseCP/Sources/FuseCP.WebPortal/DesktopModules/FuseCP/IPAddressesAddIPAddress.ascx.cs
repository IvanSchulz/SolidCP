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
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

using FuseCP.EnterpriseServer;
using FuseCP.Providers.ResultObjects;
using FuseCP.Providers.Common;

namespace FuseCP.Portal
{
    public partial class IPAddressesAddIPAddress : FuseCPModuleBase
    {
        private void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {

                // bind dropdowns
                try
                {
                    BindServers();

                    // set server if found in request
                    if (PanelRequest.ServerId != 0)
                        Utils.SelectListItem(ddlServer, PanelRequest.ServerId);

                    if (!String.IsNullOrEmpty(PanelRequest.PoolId))
                        Utils.SelectListItem(ddlPools, PanelRequest.PoolId);
                }
                catch (Exception ex)
                {
                    ShowErrorMessage("IP_ADD_INIT_FORM", ex);
                    return;
                }

                ToggleControls();
            }
        }

        private void BindServers()
        {
            try
            {
                ddlServer.DataSource = ES.Services.Servers.GetServers();
                ddlServer.DataBind();
            }
            catch (Exception ex)
            {
                Response.Write(ex);
            }

            // add "select" item
            ddlServer.Items.Insert(0, new ListItem(GetLocalizedString("Text.NotAssigned"), ""));
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                bool vps = ddlPools.SelectedIndex > 1;
                int vlantag = 0;
                try
                {
                    vlantag = Convert.ToInt32(VLAN.Text);
                }
                catch
                {
                    vlantag = 0;
                }
                if (vps)
                {
                    if (vlantag > 4096 || vlantag < 0)
                    {
                        ShowErrorMessage("Error updating IP address - Invalid VLAN TAG", "VLANTAG");
                        return;
                    }

                }
                int serverId = Utils.ParseInt(ddlServer.SelectedValue, 0);
                IPAddressPool pool = (IPAddressPool)Enum.Parse(typeof(IPAddressPool), ddlPools.SelectedValue, true);
                string comments = txtComments.Text.Trim();

                // add ip address
                if (endIP.Text != "" || startIP.Text.Contains("/"))
                {
                    try
                    {
                        // add IP range
                        ResultObject res = ES.Services.Servers.AddIPAddressesRange(pool, serverId, startIP.Text, endIP.Text,
                            internalIP.Text, subnetMask.Text, defaultGateway.Text, comments, vlantag);
                        if (!res.IsSuccess)
                        {
                            // show error
                            messageBox.ShowMessage(res, "IP_ADD_IP_RANGE", "IP");
                            return;
                        }
                    }
                    catch (Exception ex)
                    {
                        ShowErrorMessage("IP_ADD_IP_RANGE", ex);
                        return;
                    }
                }
                else
                {
                    // add single IP
                    try
                    {
                        IntResult res = ES.Services.Servers.AddIPAddress(pool, serverId, startIP.Text,
                            internalIP.Text, subnetMask.Text, defaultGateway.Text, comments, vlantag);
                        if (!res.IsSuccess)
                        {
                            messageBox.ShowMessage(res, "IP_ADD_IP", "IP");
                            return;
                        }
                    }
                    catch (Exception ex)
                    {
                        ShowErrorMessage("IP_ADD_IP", ex);
                        return;
                    }
                }

                // Redirect back to the portal home page
                RedirectBack();
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            // Redirect back to the portal home page
            RedirectBack();
        }

        private void RedirectBack()
        {
            var returnUrl = Request["ReturnUrl"];

            if (string.IsNullOrEmpty(returnUrl))
            {
                returnUrl = NavigateURL("PoolID", ddlPools.SelectedValue);
            }

            Response.Redirect(returnUrl);
        }

        protected void ddlPools_SelectedIndexChanged(object sender, EventArgs e)
        {
            ToggleControls();
        }

        private void ToggleControls()
        {
            bool vps = ddlPools.SelectedIndex > 1;
            SubnetRow.Visible = vps;
            GatewayRow.Visible = vps;
        }

		public void CheckIPAddresses(object sender, ServerValidateEventArgs args) {
			startIP.Validate(sender, args);
			endIP.Validate(sender, args);
			subnetMask.Validate(sender, args);
			args.IsValid = startIP.IsV6 == endIP.IsV6 && (startIP.IsV6 == subnetMask.IsV6 || subnetMask.IsMask);
		}
    }
}
