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
using FuseCP.Providers.Common;

namespace FuseCP.Portal
{
    public partial class VLANsEditVLAN : FuseCPModuleBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                try
                {
                    // bind dropdowns
                    BindServers();

                    // bind VLAN
                    BindVLAN();
                }
                catch (Exception ex)
                {
                    ShowErrorMessage("VLAN_GET_VLAN", ex);
                    return;
                }
            }
        }

        private void BindVLAN()
        {
            VLANInfo vlan = ES.Services.Servers.GetPrivateNetworVLAN(PanelRequest.VlanID);

            if (vlan != null)
            {
                Utils.SelectListItem(ddlServer, vlan.ServerId);
                txtComments.Text = vlan.Comments;
                etVlan.Text = vlan.Vlan.ToString();
            }
            else
            {
                // exit
                RedirectBack();
            }
        }

        private void BindServers()
        {
            ddlServer.DataSource = ES.Services.Servers.GetServers();
            ddlServer.DataBind();
            ddlServer.Items.Insert(0, new ListItem(GetLocalizedString("Text.NotAssigned"), ""));
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

        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                try
                {
                    int serverId = Utils.ParseInt(ddlServer.SelectedValue, 0);

                    ResultObject res = null;

                    // update VLAN
                    res = ES.Services.Servers.UpdatePrivateNetworVLAN(PanelRequest.VlanID, serverId, Int32.Parse(etVlan.Text), txtComments.Text.Trim());

                    if (!res.IsSuccess)
                    {
                        messageBox.ShowMessage(res, "VLAN_UPDATE_VLAN", "VLAN");
                        return;
                    }

                    //	 Redirect back to the portal home page
                    RedirectBack();
                }
                catch (Exception ex)
                {
                    ShowErrorMessage("VLAN_UPDATE_VLAN", ex);
                    return;
                }
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            // Redirect back to the portal home page
            RedirectBack();
        }

        public void CheckVLAN(object sender, ServerValidateEventArgs args)
        {
            etVlan.Validate(sender, args);
        }
    }
}
