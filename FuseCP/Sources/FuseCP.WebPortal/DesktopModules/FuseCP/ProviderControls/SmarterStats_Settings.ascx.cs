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

namespace FuseCP.Portal.ProviderControls
{
    public partial class SmarterStats_Settings : FuseCPControlBase, IHostingServiceProviderSettings
    {
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        public void BindSettings(StringDictionary settings)
        {
            // bind servers
            try
            {
                ddlServers.DataSource = ES.Services.StatisticsServers.GetServers(PanelRequest.ServiceId);
                ddlServers.DataBind();
            }
            catch
            { /* skip */ }

            txtSmarterUrl.Text = settings["SmarterUrl"];
            txtUsername.Text = settings["Username"];
            ViewState["PWD"] = settings["Password"];
            Utils.SelectListItem(ddlServers, settings["ServerID"]);
            Utils.SelectListItem(ddlLogFormat, settings["LogFormat"]);
            txtLogWilcard.Text = settings["LogWildcard"];
            txtLogDeleteDays.Text = settings["LogDeleteDays"];
            txtSmarterLogs.Text = settings["SmarterLogsPath"];
            txtSmarterLogDeleteMonths.Text = settings["SmarterLogDeleteMonths"];
			chkBuildUncLogsPath.Checked = Utils.ParseBool(settings["BuildUncLogsPath"], false);

            if (settings["TimeZoneId"] != null)
                timeZone.TimeZoneId = Utils.ParseInt(settings["TimeZoneId"], 1);

            txtStatsUrl.Text = settings["StatisticsURL"];

        }

        public void SaveSettings(StringDictionary settings)
        {
            settings["SmarterUrl"] = txtSmarterUrl.Text.Trim();
            settings["StatisticsURL"] = txtStatsUrl.Text.Trim();
            settings["Username"] = txtUsername.Text.Trim();
            settings["Password"] = (txtPassword.Text.Length > 0) ? txtPassword.Text : (string)ViewState["PWD"];
            settings["ServerID"] = (ddlServers.SelectedIndex != -1) ? ddlServers.SelectedValue : "1";
            settings["LogFormat"] = ddlLogFormat.SelectedValue;
            settings["LogWildcard"] = txtLogWilcard.Text.Trim();
            settings["LogDeleteDays"] = Utils.ParseInt(txtLogDeleteDays.Text, 0).ToString();
            settings["SmarterLogsPath"] = txtSmarterLogs.Text.Trim();
            settings["SmarterLogDeleteMonths"] = Utils.ParseInt(txtSmarterLogDeleteMonths.Text, 0).ToString();
            settings["TimeZoneId"] = timeZone.TimeZoneId.ToString();
			settings["BuildUncLogsPath"] = chkBuildUncLogsPath.Checked.ToString();
        }
    }
}
