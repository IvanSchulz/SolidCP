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

namespace FuseCP.Portal
{
    public partial class ServersAddServer : FuseCPModuleBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            // add server
            if (!Page.IsValid)
                return;

            ServerInfo server = new ServerInfo();
            server.ServerName = txtName.Text.Trim();
            server.ServerUrl = txtUrl.Text.Trim();
            server.Password = serverPassword.Password;
            server.Comments = "";
            server.VirtualServer = false;

            int serverId = 0;
            try
            {
                // add a server
                serverId = ES.Services.Servers.AddServer(server, cbAutoDiscovery.Checked);

                if (serverId < 0)
                {
                    ShowResultMessage(serverId);
                    return;
                }
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("Invalid URI"))
                {
                    ShowErrorMessage("SERVER_INVALID_URL");
                    return;
                }
                ShowErrorMessage("SERVER_ADD_SERVER", ex);
                return;
            }

            Response.Redirect(EditUrl("ServerID", serverId.ToString(), "edit_server"));
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            RedirectToBrowsePage();
        }
    }
}
