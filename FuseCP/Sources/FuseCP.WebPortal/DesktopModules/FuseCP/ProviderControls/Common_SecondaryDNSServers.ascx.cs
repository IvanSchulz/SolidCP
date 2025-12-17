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
using System.Collections.Specialized;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

using FuseCP.EnterpriseServer;

namespace FuseCP.Portal.ProviderControls
{
    public partial class Common_SecondaryDNSServers : FuseCPControlBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        public void BindSettings(StringDictionary settings)
        {
            // bind DNS services
            DataView dvServices = ES.Services.Servers.GetRawServicesByGroupName(ResourceGroups.Dns).Tables[0].DefaultView;
            foreach (DataRowView dr in dvServices)
            {
                int serviceId = (int)dr["ServiceID"];
                if (PanelRequest.ServiceId != serviceId)
                    ddlService.Items.Add(new ListItem(dr["FullServiceName"].ToString(), serviceId.ToString()));
            }

            // bind selected services
            string sids = settings["SecondaryDNSServices"];
            if (sids != null)
            {
                string[] ids = sids.Split(',');

                foreach (string id in ids)
                {
                    ListItem li = ddlService.Items.FindByValue(id);
                    if (li != null)
                    {
                        ddlService.Items.Remove(li);
                        lbServices.Items.Add(li);
                    }
                }
            }
        }

        public void SaveSettings(StringDictionary settings)
        {
            string[] ids = new string[lbServices.Items.Count];
            for (int i = 0; i < lbServices.Items.Count; i++)
                ids[i] = lbServices.Items[i].Value;

            settings["SecondaryDNSServices"] = String.Join(",", ids);
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            if (ddlService.SelectedIndex != -1)
            {
                ListItem li = ddlService.SelectedItem;
                li.Selected = false;
                ddlService.Items.Remove(li);
                lbServices.Items.Add(li);
            }
        }
        protected void btnRemove_Click(object sender, EventArgs e)
        {
            if (lbServices.SelectedIndex != -1)
            {
                ListItem li = lbServices.Items[lbServices.SelectedIndex];
                li.Selected = false;
                lbServices.Items.Remove(li);
                ddlService.Items.Add(li);
            }
        }
    }
}
