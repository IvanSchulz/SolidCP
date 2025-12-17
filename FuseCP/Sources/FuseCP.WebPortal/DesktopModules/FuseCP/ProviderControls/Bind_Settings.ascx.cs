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
	public partial class Bind_Settings : FuseCPControlBase, IHostingServiceProviderSettings
    {
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        public void BindSettings(StringDictionary settings)
        {
			txtBindConfigPath.Text = settings["BindConfigPath"];
			txtZonesFolderPath.Text = settings["ZonesFolderPath"];
			txtZoneFileNameTemplate.Text = settings["ZoneFileNameTemplate"];
			txtBindReloadBatch.Text = settings["BindReloadBatch"];

			txtAllowZoneTransfers.Text = settings["AllowZoneTransfers"];
			txtResponsiblePerson.Text = settings["ResponsiblePerson"];
            intRefresh.Interval = Utils.ParseInt(settings["RefreshInterval"], 0);
            intRetry.Interval = Utils.ParseInt(settings["RetryDelay"], 0);
            intExpire.Interval = Utils.ParseInt(settings["ExpireLimit"], 0);
            intTtl.Interval = Utils.ParseInt(settings["MinimumTTL"], 0);

            iPAddressesList.BindSettings(settings);
            secondaryDNSServers.BindSettings(settings);
            nameServers.Value = settings["NameServers"];
        }

        public void SaveSettings(StringDictionary settings)
        {
			settings["BindConfigPath"] = txtBindConfigPath.Text;
			settings["ZonesFolderPath"] = txtZonesFolderPath.Text;
			settings["ZoneFileNameTemplate"] = txtZoneFileNameTemplate.Text;
			settings["BindReloadBatch"] = txtBindReloadBatch.Text;

			settings["AllowZoneTransfers"] = txtAllowZoneTransfers.Text;
			settings["ResponsiblePerson"] = txtResponsiblePerson.Text;
            settings["RefreshInterval"] = intRefresh.Interval.ToString();
            settings["RetryDelay"] = intRetry.Interval.ToString();
            settings["ExpireLimit"] = intExpire.Interval.ToString();
            settings["MinimumTTL"] = intTtl.Interval.ToString();

            iPAddressesList.SaveSettings(settings);
            secondaryDNSServers.SaveSettings(settings);
            settings["NameServers"] = nameServers.Value;
        }
    }
}
