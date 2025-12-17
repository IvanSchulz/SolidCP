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
using FuseCP.Providers.Filters;

namespace FuseCP.Portal.ProviderControls
{
    public partial class MailCleaner_Settings : FuseCPControlBase, IHostingServiceProviderSettings
    {

        public const string MailCleanerServersData = "MailCleanerServersData";

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        

        public void BindSettings(System.Collections.Specialized.StringDictionary settings)
        { 
            txtServerName.Text = settings[MailCleanerContants.APITitle];
            txtSimpleUrlBase.Text = settings[MailCleanerContants.APIUrl];
            chkIgnoreCheckSSL.Checked = Utils.ParseBool(settings[MailCleanerContants.IgnoreCheckSSL], true);
            MailCleanerServers = settings["MailCleanerServiceID"];
        }

        public void SaveSettings(System.Collections.Specialized.StringDictionary settings)
        {
            settings[MailCleanerContants.APITitle] = txtServerName.Text.Trim();
            settings[MailCleanerContants.APIUrl] = txtSimpleUrlBase.Text.Trim();
            settings[MailCleanerContants.IgnoreCheckSSL] = chkIgnoreCheckSSL.Checked.ToString();
            settings["MailCleanerServiceID"] = MailCleanerServers;			
        }


        public string MailCleanerServers
        {
            get
            {
                return ViewState[MailCleanerServersData] != null ? ViewState[MailCleanerServersData].ToString() : string.Empty;
            }
            set
            {
                ViewState[MailCleanerServersData] = value;
            }
        }


        /*protected void btnAddMailCleanerServer_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(MailCleanerServers))
                MailCleanerServers += ",";

            MailCleanerServers += ddlMailCleanerServers.SelectedItem.Value;

            BindMailCleanerServices(ddlMailCleanerServers);

        }*/

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

        /*private void UpdateMailCleanerServersGrid()
        {
            gvMailCleanerServers.DataSource = GetServices(MailCleanerServers);
            gvMailCleanerServers.DataBind();
        }*/


        public void BindMailCleanerServices(DropDownList ddl)
        {
            ddl.Items.Clear();

            ServiceInfo serviceInfo = ES.Services.Servers.GetServiceInfo(PanelRequest.ServiceId);
            DataView dvServices = ES.Services.Servers.GetRawServicesByGroupName(ResourceGroups.Filters).Tables[0].DefaultView;

            foreach (DataRowView dr in dvServices)
            {
                int serviceId = (int)dr["ServiceID"];
                ServiceInfo currentServiceInfo = ES.Services.Servers.GetServiceInfo(serviceId);

                if (currentServiceInfo == null || currentServiceInfo.ProviderId != serviceInfo.ProviderId)
                    continue;

                List<ServiceInfo> services = GetServices(MailCleanerServers);
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
            //btnAddMailCleanerServer.Visible = ddl.Items.Count != 0;

        }

        protected void gvMailCleanerServers_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "RemoveServer")
            {
                string str = string.Empty;
                List<ServiceInfo> services = GetServices(MailCleanerServers);
                foreach (ServiceInfo current in services)
                {
                    if (current.ServiceId == Utils.ParseInt(e.CommandArgument.ToString()))
                        continue;


                    str += current.ServiceId + ",";
                }

                MailCleanerServers = str.TrimEnd(',');
                //UpdateMailCleanerServersGrid();
                //BindMailCleanerServices(ddlMailCleanerServers);
            }
        }

    }
}
