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

namespace FuseCP.Portal
{
    public partial class VirtualServersAddServices : FuseCPModuleBase
    {
        DataSet dsServers = null;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindServers();
            }
        }

        private void BindServers()
        {
            dsServers = ES.Services.Servers.GetAvailableVirtualServices(PanelRequest.ServerId);
            dlServers.DataSource = dsServers;
            dlServers.DataBind();
        }

        private void AddServices()
        {
            // iterate through all services
            List<int> ids = new List<int>();

            foreach (DataListItem itemGroup in dlServers.Items)
            {
                DataList dlServices = (DataList)itemGroup.FindControl("dlServices");
                if (dlServices != null)
                {
                    for (int i = 0; i < dlServices.Items.Count; i++)
                    {
                        DataListItem itemService = dlServices.Items[i];
                        CheckBox chkSelected = (CheckBox)itemService.FindControl("chkSelected");
                        if (chkSelected != null && chkSelected.Checked)
                        {
                            int serviceId = (int)dlServices.DataKeys[i];
                            ids.Add(serviceId);
                        }
                    }
                }
            }

            // add virtual services
            try
            {
                int result = ES.Services.Servers.AddVirtualServices(PanelRequest.ServerId, ids.ToArray());
                if (result < 0)
                {
                    ShowResultMessage(result);
                    return;
                }
            }
            catch (Exception ex)
            {
                ShowErrorMessage("VSERVER_ADD_SERVICES", ex);
                return;
            }

            // return
            Response.Redirect(EditUrl("ServerID", PanelRequest.ServerId.ToString(), "edit_server"));
        }

        public DataView GetServerServices(int serverId)
        {
            return new DataView(dsServers.Tables[1], "ServerID=" + serverId.ToString(), "", DataViewRowState.CurrentRows);
        }
        protected void btnAdd_Click(object sender, EventArgs e)
        {
            AddServices();
        }
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect(EditUrl("ServerID", PanelRequest.ServerId.ToString(), "edit_server"));
        }
    }
}
