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
using System.Web.UI.WebControls;
using FuseCP.EnterpriseServer;
using FuseCP.Providers.Common;
using FuseCP.Providers.HostedSolution;
using FuseCP.Providers.OS;

using FuseCP.Providers.RemoteDesktopServices;

namespace FuseCP.Portal
{
    public partial class RDSServersAddserver : FuseCPModuleBase
    {
        
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
            }

            // Load Servers
            var services = ES.Services.RDS.GetRdsServices();

            foreach (var service in services)
            {
                ddlRdsController.Items.Add(new ListItem(service.ServiceName, service.ServiceId.ToString()));
            }
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid)
                return;
            try
            {
                RdsServer rdsServer = new RdsServer();

                rdsServer.FqdName = txtServerName.Text;
                rdsServer.Description = txtServerComments.Text;

                ResultObject result = ES.Services.RDS.AddRdsServer(rdsServer, ddlRdsController.SelectedValue);

                if (!result.IsSuccess && result.ErrorCodes.Count > 0)
                {                    
                    messageBox.ShowMessage(result, "RDSSERVER_NOT_ADDED", "");
                    return;
                }

                RedirectToBrowsePage();
            }
            catch (Exception ex)
            {
                ShowErrorMessage("RDSSERVER_NOT_ADDED", ex);
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            RedirectToBrowsePage();
        }
    }
}
