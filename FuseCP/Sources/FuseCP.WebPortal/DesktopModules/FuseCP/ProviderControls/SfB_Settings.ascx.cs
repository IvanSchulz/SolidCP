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
using System.Collections.Generic;
using System.Data;
using System.Web.UI.WebControls;
using FuseCP.EnterpriseServer;
using FuseCP.Providers.HostedSolution;

namespace FuseCP.Portal.ProviderControls
{
    public partial class SfB_Settings : FuseCPControlBase, IHostingServiceProviderSettings
    {

        public const string SfBServersData = "SfBServersData";

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        

        public void BindSettings(System.Collections.Specialized.StringDictionary settings)
        {
            txtServerName.Text = settings[SfBConstants.PoolFQDN];
            txtSimpleUrlBase.Text = settings[SfBConstants.SimpleUrlRoot];


            SfBServers = settings["SfBServersServiceID"];

            BindSfBServices(ddlSfBServers);

            Utils.SelectListItem(ddlSfBServers, settings["SfBServersServiceID"]);

            UpdateSfBServersGrid();
        }

        public void SaveSettings(System.Collections.Specialized.StringDictionary settings)
        {
            settings[SfBConstants.PoolFQDN] = txtServerName.Text.Trim();
            settings[SfBConstants.SimpleUrlRoot] = txtSimpleUrlBase.Text.Trim();

            settings["SfBServersServiceID"] = SfBServers;			
        }


        public string SfBServers
        {
            get
            {
                return ViewState[SfBServersData] != null ? ViewState[SfBServersData].ToString() : string.Empty;
            }
            set
            {
                ViewState[SfBServersData] = value;
            }
        }


        protected void btnAddSfBServer_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(SfBServers))
                SfBServers += ",";

            SfBServers += ddlSfBServers.SelectedItem.Value;

            UpdateSfBServersGrid();
            BindSfBServices(ddlSfBServers);

        }

        public List<ServiceInfo> GetServices(string data)
        {
            if (string.IsNullOrEmpty(data))
                return null;
            List<ServiceInfo> list = new List<ServiceInfo>();
            string[] servicesIds = data.Split(',');
            foreach (string current in servicesIds)
            {
                ServiceInfo serviceInfo = ES.Services.Servers.GetServiceInfo(Utils.ParseInt(current));
                list.Add(serviceInfo);
            }


            return list;
        }

        private void UpdateSfBServersGrid()
        {
            gvSfBServers.DataSource = GetServices(SfBServers);
            gvSfBServers.DataBind();
        }


        public void BindSfBServices(DropDownList ddl)
        {
            ddl.Items.Clear();

            ServiceInfo serviceInfo = ES.Services.Servers.GetServiceInfo(PanelRequest.ServiceId);
            DataView dvServices = ES.Services.Servers.GetRawServicesByGroupName(ResourceGroups.SfB).Tables[0].DefaultView;

            foreach (DataRowView dr in dvServices)
            {
                int serviceId = (int)dr["ServiceID"];
                ServiceInfo currentServiceInfo = ES.Services.Servers.GetServiceInfo(serviceId);

                if (currentServiceInfo == null || currentServiceInfo.ProviderId != serviceInfo.ProviderId)
                    continue;

                List<ServiceInfo> services = GetServices(SfBServers);
                bool exists = false;
                if (services != null)
                    foreach (ServiceInfo current in services)
                    {
                        if (current != null && current.ServiceId == serviceId)
                        {
                            exists = true;
                            break;
                        }
                    }

                if (!exists)
                    ddl.Items.Add(new ListItem(dr["FullServiceName"].ToString(), serviceId.ToString()));

            }

            ddl.Visible = ddl.Items.Count != 0;
            btnAddSfBServer.Visible = ddl.Items.Count != 0;

        }

        protected void gvSfBServers_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "RemoveServer")
            {
                string str = string.Empty;
                List<ServiceInfo> services = GetServices(SfBServers);
                foreach (ServiceInfo current in services)
                {
                    if (current.ServiceId == Utils.ParseInt(e.CommandArgument.ToString()))
                        continue;


                    str += current.ServiceId + ",";
                }

                SfBServers = str.TrimEnd(',');
                UpdateSfBServersGrid();
                BindSfBServices(ddlSfBServers);
            }
        }

    }
}
