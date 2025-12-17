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
    public partial class OCS_Settings : FuseCPControlBase, IHostingServiceProviderSettings
    {
       

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        
        public string EDGEServices
        {
            get
            {
                return ViewState[OCSConstants.EDGEServicesData] != null ? ViewState[OCSConstants.EDGEServicesData].ToString() : string.Empty;
            }
            set
            {
                ViewState[OCSConstants.EDGEServicesData] = value;
            }
        }

        public ServiceInfo[] GetEDGEServices()
        {
            List<ServiceInfo> list = new List<ServiceInfo>();
            string[] services = EDGEServices.Split(';');
            foreach (string current in services)
            {
                string[] data = current.Split(',');
                if (data.Length > 1)
                    list.Add(new ServiceInfo() {ServiceId = Utils.ParseInt(data[1]), ServiceName = data[0]});
            }


            return list.ToArray();
        }

        public void BindSettings(System.Collections.Specialized.StringDictionary settings)
        {
            txtServerName.Text = settings[OCSConstants.PoolFQDN];
            EDGEServices = settings[OCSConstants.EDGEServicesData];
            BindOCSEdgeServices(ddlEdgeServers);
            UpdateGrid();
        }

        private void UpdateGrid()
        {
            gvEdgeServices.DataSource = GetEDGEServices();            
            gvEdgeServices.DataBind();   
        }

        public void SaveSettings(System.Collections.Specialized.StringDictionary settings)
        {
            settings[OCSConstants.EDGEServicesData] = EDGEServices;
            settings[OCSConstants.PoolFQDN] = txtServerName.Text;
        }

        public void BindOCSEdgeServices(DropDownList ddl)
        {
            ddl.Items.Clear();
            DataView dvServices =
                ES.Services.Servers.GetRawServicesByGroupName(ResourceGroups.OCS).Tables[0].DefaultView;
            foreach (DataRowView dr in dvServices)
            {
                if (dr["ProviderName"].ToString() != OCSConstants.ProviderName)
                    continue;

                int serviceId = (int) dr["ServiceID"];
                ServiceInfo[] services = GetEDGEServices();
                bool exists = false;
                foreach (ServiceInfo current in services)
                {
                    if (current.ServiceId == serviceId)
                    {
                        exists = true;
                        break;
                    }
                }
                if (!exists)
                    ddl.Items.Add(new ListItem(dr["FullServiceName"].ToString(), serviceId.ToString()));
            }
         
            ddl.Visible = ddl.Items.Count != 0;
            btnAdd.Visible = ddl.Items.Count != 0;

        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            EDGEServices += ddlEdgeServers.SelectedItem.Text + "," + ddlEdgeServers.SelectedItem.Value + ";";
            UpdateGrid();
            BindOCSEdgeServices(ddlEdgeServers);
            
        }

        protected void gvEdgeServices_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "RemoveServer")
            {
                string str = string.Empty;
                ServiceInfo []services = GetEDGEServices();
                foreach(ServiceInfo current in services)
                {
                    if (current.ServiceId == Utils.ParseInt(e.CommandArgument.ToString()))                                          
                        continue;
                                        

                    str += current.ServiceName + "," + current.ServiceId + ";";
                }

                EDGEServices = str;
                UpdateGrid();
                BindOCSEdgeServices(ddlEdgeServers);
            }
        }

        
        
    }
}
