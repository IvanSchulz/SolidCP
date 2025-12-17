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
using FuseCP.Providers.Virtualization;
using FuseCP.EnterpriseServer;

namespace FuseCP.Portal.VPSForPC.RemoteDesktop
{
    public partial class Connect : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            resolution.Text = Request["Resolution"];

            // load server info
            VMInfo vm = ES.Services.VPSPC.GetCachedVirtualMachine(PanelRequest.ItemID);
            litServerName.Text = vm.Name + " - ";
            username.Text = "Administrator";
            // TODO: Review VMInfo class fields and underlying data for correctness
            password.Text = vm.AdminPassword;
            
            // load external network parameters
            NetworkAdapterDetails nic = ES.Services.VPSPC.GetExternalNetworkAdapterDetails(PanelRequest.ItemID);
            if (nic.IPAddresses.Length > 0)
            {
                NetworkAdapterIPAddress ip = nic.IPAddresses[0];
                serverName.Text = !String.IsNullOrEmpty(ip.NATAddress) ? ip.NATAddress : ip.IPAddress;
            }
        }
    }
}
