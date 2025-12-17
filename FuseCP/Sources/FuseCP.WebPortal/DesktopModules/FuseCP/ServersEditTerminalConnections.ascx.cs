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

namespace FuseCP.Portal
{
    public partial class ServersEditTerminalConnections : FuseCPModuleBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            BindSessions();
        }

        private void BindSessions()
        {
            try
            {
                gvSessions.DataSource = ES.Services.Servers.GetTerminalServicesSessions(PanelRequest.ServerId);
                gvSessions.DataBind();
            }
            catch (Exception ex)
            {
                ShowErrorMessage("SERVER_GET_TERMINAL", ex);
                return;
            }
        }
        protected void gvSessions_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                int sessionId = (int)gvSessions.DataKeys[e.RowIndex][0];
                int result = ES.Services.Servers.CloseTerminalServicesSession(PanelRequest.ServerId, sessionId);
                if (result < 0)
                {
                    ShowResultMessage(result);
                    return;
                }

                // rebind sessions
                BindSessions();
            }
            catch (Exception ex)
            {
                ShowErrorMessage("SERVER_CLOSE_TERMINAL", ex);
                return;
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect(EditUrl("ServerID", PanelRequest.ServerId.ToString(), "edit_server"));
        }
    }
}
