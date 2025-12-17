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
using FuseCP.Providers.Common;

namespace FuseCP.Portal
{
    public partial class ServersAddService : FuseCPModuleBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindGroup();
                BindProviders();
            }
        }

        private void BindGroup()
        {
            ResourceGroupInfo group = ES.Services.Servers.GetResourceGroup(PanelRequest.GroupID);
			litGroupName.Text = serviceName.Text = PanelFormatter.GetLocalizedResourceGroupName(group.GroupName);
        }

        private void BindProviders()
        {
            ddlProviders.DataSource = ES.Services.Servers.GetProvidersByGroupId(PanelRequest.GroupID);
            ddlProviders.DataBind();
            ddlProviders.Items.Insert(0, new ListItem(GetLocalizedString("SelectProvider.Text"), ""));
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            // validate input
            if (!Page.IsValid)
                return;

            // register service type
            int providerId = Utils.ParseInt(ddlProviders.SelectedValue, 0);

            // add a new service ...
            try
            {
                ServiceInfo service = new ServiceInfo();
                service.ServerId = PanelRequest.ServerId;
                service.ProviderId = providerId;
                service.ServiceName = serviceName.Text;
                BoolResult res = ES.Services.Servers.IsInstalled(PanelRequest.ServerId, providerId);
                if (res.IsSuccess)
                {
                    if (!res.Value)
                    {
                        ShowErrorMessage("SOFTWARE_IS_NOT_INSTALLED");
                        return;
                    }
                }
                else
                {
                    ShowErrorMessage("SERVER_ADD_SERVICE");
                }
                int serviceId = ES.Services.Servers.AddService(service);

                if (serviceId < 0)
                {
                    ShowResultMessage(serviceId);
                    return;
                }

                // ...and go to service configuration page
                Response.Redirect(EditUrl("ServerID", PanelRequest.ServerId.ToString(), "edit_service",
                    "ServiceID=" + serviceId.ToString()), true);
            }
            catch (Exception ex)
            {
                ShowErrorMessage("SERVER_ADD_SERVICE", ex);
                return;
            }
        }
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect(EditUrl("ServerID", PanelRequest.ServerId.ToString(), "edit_server"), true);
        }
    }
}
