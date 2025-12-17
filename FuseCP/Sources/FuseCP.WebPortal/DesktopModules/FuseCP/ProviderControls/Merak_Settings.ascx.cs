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
using System.Collections.Generic;

namespace FuseCP.Portal.ProviderControls
{
    public partial class Merak_Settings : FuseCPControlBase, IHostingServiceProviderSettings
    {
        public const string MailFilterDestinations = "MailFilterDestinations";

        private string[] ConvertDictionaryToArray(StringDictionary settings)
        {
            List<string> list = new List<string>();
            foreach (string key in settings.Keys)
                list.Add(key + "=" + settings[key]);
            return list.ToArray();
        }

        private StringDictionary ConvertArrayToDictionary(string[] settings)
        {
            StringDictionary list = new StringDictionary();
            foreach (string setting in settings)
            {
                int idx = setting.IndexOf('=');
                list.Add(setting.Substring(0, idx), setting.Substring(idx + 1));
            }
            return list;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            List<String> sed = ViewState[MailFilterDestinations] as List<String>;
            if (sed == null)
            {
                sed = new List<string>();

                StringDictionary settings = ConvertArrayToDictionary(ES.Services.Servers.GetServiceSettings(PanelRequest.ServiceId));
                string strList = settings[MailFilterDestinations];
                if (strList != null)
                {
                    string[] list = strList.Split(',');
                    sed.AddRange(list);
                }
                ViewState[MailFilterDestinations] = sed;
                gvSEDestinations.DataSource = sed;
                gvSEDestinations.DataBind();
            }
        }

        public void BindSettings(StringDictionary settings)
        {
            ipAddress.AddressId = (settings["ServerIpAddress"] != null) ? Utils.ParseInt(settings["ServerIpAddress"], 0) : 0;
			Utils.SelectListItem(ddlAccountType, settings["ServerAccountType"]);
            chkSEEnable.Checked = Utils.ParseBool(settings["EnableMailFilter"], false);
        }

        public void SaveSettings(StringDictionary settings)
        {
            settings["ServerIpAddress"] = ipAddress.AddressId.ToString();
			settings["ServerAccountType"] = ddlAccountType.SelectedValue;
            settings["EnableMailFilter"] = chkSEEnable.Checked.ToString();
        }

        protected void gvSEDestinations_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            switch (e.CommandName)
            {
                case "DeleteItem":
                    try
                    {
                        string item = e.CommandArgument.ToString();

                        List<String> itemList = ViewState[MailFilterDestinations] as List<String>;
                        if (itemList == null) return;

                        int i = itemList.FindIndex(x => x == item);
                        if (i >= 0) itemList.RemoveAt(i);

                        ViewState[MailFilterDestinations] = itemList;
                        gvSEDestinations.DataSource = itemList;
                        gvSEDestinations.DataBind();
                        SaveSEDestinations();
                    }
                    catch (Exception)
                    {
                    }

                    break;
            }
        }

        protected void bntAddSEDestination_Click(object sender, EventArgs e)
        {
            List<String> res = ViewState[MailFilterDestinations] as List<String>;
            if (res == null) res = new List<String>();

            res.Add(tbSEDestinations.Text);
            ViewState[MailFilterDestinations] = res;
            gvSEDestinations.DataSource = res;
            gvSEDestinations.DataBind();
            SaveSEDestinations();
        }

        protected void SaveSEDestinations()
        {
            List<String> res = ViewState[MailFilterDestinations] as List<String>;
            if (res == null) return;

            StringDictionary settings = new StringDictionary();
            settings.Add(MailFilterDestinations, string.Join(",", res.ToArray()));

            int result = ES.Services.Servers.UpdateServiceSettings(PanelRequest.ServiceId,
                        ConvertDictionaryToArray(settings));
        }
    }
}
