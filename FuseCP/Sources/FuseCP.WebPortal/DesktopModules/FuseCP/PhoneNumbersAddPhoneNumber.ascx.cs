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
using FuseCP.Providers.ResultObjects;
using FuseCP.Providers.Common;

namespace FuseCP.Portal
{
    public partial class PhoneNumbersAddPhoneNumber : FuseCPModuleBase
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
                ServerInfo[] allServers = ES.Services.Servers.GetServers();
                List<ServerInfo> servers = new List<ServerInfo>();
                foreach(ServerInfo server in allServers)
                    {
                        ServiceInfo[] service = ES.Services.Servers.GetServicesByServerIdGroupName(server.ServerId, ResourceGroups.Lync);

                        if (service.Length > 0) servers.Add(server);
                    }
                foreach (ServerInfo server in allServers)
                {
                    ServiceInfo[] service = ES.Services.Servers.GetServicesByServerIdGroupName(server.ServerId, ResourceGroups.SfB);

                    if (service.Length > 0) servers.Add(server);
                }

                ddlServer.DataSource = servers;
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
                int serverId = Utils.ParseInt(ddlServer.SelectedValue, 0);
                IPAddressPool pool = IPAddressPool.PhoneNumbers;
                string comments = txtComments.Text.Trim();

                string start; 
                string end;

                    start = startPhone.Text; 
                    end = endPhone.Text;

                // add ip address
                if (end != "" || start.Contains("/"))
                {
                    string errorKey = "IP_ADD_PHONE_RANGE";

                    try
                    {
                        // add IP range
                        ResultObject res = ES.Services.Servers.AddIPAddressesRange(pool, serverId, start, end,
                            "", "", "", comments, 0);
                        if (!res.IsSuccess)
                        {
                            // show error
                            messageBox.ShowMessage(res, errorKey, "IP");
                            return;
                        }
                    }
                    catch (Exception ex)
                    {
                        ShowErrorMessage(errorKey, ex);
                        return;
                    }
                }
                else
                {
                    string errorKey = "IP_ADD_PHONE";

                    // add single IP
                    try
                    {
                        IntResult res = ES.Services.Servers.AddIPAddress(pool, serverId, start,
                            "", "", "", comments, 0);
                        if (!res.IsSuccess)
                        {
                            messageBox.ShowMessage(res, errorKey, "IP");
                            return;
                        }
                    }
                    catch (Exception ex)
                    {
                        ShowErrorMessage(errorKey, ex);
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
            Response.Redirect(NavigateURL("PoolID", "PhoneNumbers"));
        }

        protected void ddlPools_SelectedIndexChanged(object sender, EventArgs e)
        {
            ToggleControls();
        }

        private void ToggleControls()
        {
            requireStartPhoneValidator.Enabled = true;
        }

    }
}
