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
using System.Linq;
using System.Web.UI.WebControls;
using System.Xml;

namespace FuseCP.Portal {
    public partial class AutoUpdateServers : FuseCPModuleBase {
        DataSet dsServers = null;
        string downloadLink = "";

        protected void Page_Load(object sender, EventArgs e) {
            try {
                BindVersions();
                BindServers();
            } catch(Exception ex) {
                ProcessException(ex);
                this.DisableControls = true;
                return;
            }
        }

        private void BindServers() {
            dsServers = ES.Services.Servers.GetRawServers();
            if(!IsPostBack) {
                dlServers.DataSource = dsServers;
                dlServers.DataBind();
            }
            tblEmptyList.Visible = (dlServers.Items.Count == 0);
        }

        private void BindVersions() {
            XmlDocument doc = new XmlDocument();
            doc.Load("http://autoupdate.fusecp.com/version.xml");
            downloadLink = doc.GetElementsByTagName("downloadURL")[0].InnerText;
            XmlNodeList versions = doc.SelectNodes("root/versions/version");
            ddlSelectVersion.Items.Clear();
            foreach(XmlNode n in versions) {
                ddlSelectVersion.Items.Add(
                    new ListItem {
                        Value = n.SelectSingleNode("file").InnerText,
                        Text = n.SelectSingleNode("version-nr").InnerText
                    }
                );
            }
        }

        public int GetServiceIDFromDataView(int serverId) {
            DataView v = new DataView(dsServers.Tables[1], "ServerID=" + serverId.ToString(), "", DataViewRowState.CurrentRows);
            foreach(DataRow r in v.Table.Columns["ServerID"].Table.Rows) {
                if((int)r.ItemArray[1] == serverId) {
                    return (int)r.ItemArray[0];
                }
            }
            return 0;
        }

        public string getServerName(int serverId) {
            return ES.Services.Servers.GetServerById(serverId).ServerName;
        }

        protected void btnUpdateServers_Click(object sender, EventArgs e) {
            int[][] servers = new int[dlServers.Items.Count][];
            Dictionary<int, string> result = new Dictionary<int, string>();
            int i = 0; int s = 0;
            foreach(DataListItem item in dlServers.Items) {
                CheckBox chkServer = ((CheckBox)(item.FindControl("chkServer")));
                if(chkServer.Checked) {
                    int serverID = int.Parse(chkServer.Attributes["Value"]);
                    int serviceID = GetServiceIDFromDataView(serverID);

                    if(serviceID > 0) { 
                        int[] ServerService;

                        ServerService = new int[] {
                            serverID, serviceID
                        };

                        servers[i++] = ServerService;
                    } else {
                        result.Add(serverID, "No services");
                    }
                }
                s++;
            }

            string[] response = ES.Services.Servers.AutoUpdateServer(servers, downloadLink, ddlSelectVersion.SelectedValue);
            foreach(string x in response) {
                string[] parts = x.Split('-');
                result.Add(int.Parse(parts[0]), parts[1]);
            }


            if(result.Count == 0) {
                ShowSuccessMessage("SERVERS_UPDATED");
            } else {
                if(result.Count == dlServers.Items.Count) {
                    ShowErrorMessage("SERVERS_UPDATED");
                } else {
                    ShowWarningMessage("SERVERS_UPDATED");
                }
                lstFailed.DataSource = result;
                lstFailed.DataBind();
                failedList.Visible = true;
            }

        }
    }
}
