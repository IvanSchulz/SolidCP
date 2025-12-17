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
using System.Web.UI;
using System.Web.UI.WebControls;
using FuseCP.EnterpriseServer;
using FuseCP.Providers.ResultObjects;
using FuseCP.Providers.Common;

namespace FuseCP.Portal
{
    public partial class VLANsAddVLANs : FuseCPModuleBase
    {
        protected void Page_Load(object sender, EventArgs e)
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
                    ShowErrorMessage("VLAN_ADD_INIT_FORM", ex);
                    return;
                }
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
                int serverId = Utils.ParseInt(ddlServer.SelectedValue, 0);
                string comments = txtComments.Text.Trim();

                // add vlan
                if (endVLAN.Text != "")
                {
                    try
                    {
                        // add vlan range
                        ResultObject res = ES.Services.Servers.AddPrivateNetworkVLANsRange(serverId, Int32.Parse(startVLAN.Text), Int32.Parse(endVLAN.Text), comments);
                        if (!res.IsSuccess)
                        {
                            // show error
                            messageBox.ShowMessage(res, "VLAN_ADD_VLAN_RANGE", "VLAN");
                            return;
                        }
                    }
                    catch (Exception ex)
                    {
                        ShowErrorMessage("VLAN_ADD_VLAN_RANGE", ex);
                        return;
                    }
                }
                else
                {
                    // add single vlan
                    try
                    {
                        IntResult res = ES.Services.Servers.AddPrivateNetworkVLAN(serverId, Int32.Parse(startVLAN.Text), comments);
                        if (!res.IsSuccess)
                        {
                            messageBox.ShowMessage(res, "VLAN_ADD_VLAN", "VLAN");
                            return;
                        }
                    }
                    catch (Exception ex)
                    {
                        ShowErrorMessage("VLAN_ADD_VLAN", ex);
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
                returnUrl = NavigateURL("ServerID", ddlServer.SelectedValue);
            }

            Response.Redirect(returnUrl);
        }

        public void CheckVLANs(object sender, ServerValidateEventArgs args)
        {
            startVLAN.Validate(sender, args);
            endVLAN.Validate(sender, args);
        }
    }
}
