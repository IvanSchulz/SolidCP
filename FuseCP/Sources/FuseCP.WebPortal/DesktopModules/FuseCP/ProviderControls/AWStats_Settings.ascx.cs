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
    public partial class AWStats_Settings : FuseCPControlBase, IHostingServiceProviderSettings
    {
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        public void BindSettings(StringDictionary settings)
        {
            txtAwStatsFolder.Text = settings["AwStatsFolder"];
            txtBatchFileName.Text = settings["BatchFileName"];
            txtBatchLineTemplate.Text = settings["BatchLineTemplate"];
            txtConfigFileName.Text = settings["ConfigFileName"];
            txtConfigFileTemplate.Text = settings["ConfigFileTemplate"];
            txtConfigFileTemplatePath.Text = settings["ConfigFileTemplatePath"];
			chkBuildUncLogsPath.Checked = Utils.ParseBool(settings["BuildUncLogsPath"], false);
            txtStatsUrl.Text = settings["StatisticsURL"];
        }

        public void SaveSettings(StringDictionary settings)
        {
            settings["AwStatsFolder"] = txtAwStatsFolder.Text;
            settings["BatchFileName"] = txtBatchFileName.Text;
            settings["BatchLineTemplate"] = txtBatchLineTemplate.Text;
            settings["ConfigFileName"] = txtConfigFileName.Text;
            settings["ConfigFileTemplate"] = txtConfigFileTemplate.Text;
            settings["ConfigFileTemplatePath"] = txtConfigFileTemplatePath.Text;
			settings["BuildUncLogsPath"] = chkBuildUncLogsPath.Checked.ToString();
            settings["StatisticsURL"] = txtStatsUrl.Text;
        }
    }
}
