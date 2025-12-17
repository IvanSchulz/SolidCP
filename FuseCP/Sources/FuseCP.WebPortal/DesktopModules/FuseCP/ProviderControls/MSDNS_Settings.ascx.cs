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
    public partial class MSDNS_Settings : FuseCPControlBase, IHostingServiceProviderSettings
    {
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        public void BindSettings(StringDictionary settings)
        {
			txtAllowZoneTransfers.Text = settings["AllowZoneTransfers"];
			txtResponsiblePerson.Text = settings["ResponsiblePerson"];
            intRefresh.Interval = Utils.ParseInt(settings["RefreshInterval"], 0);
            intRetry.Interval = Utils.ParseInt(settings["RetryDelay"], 0);
            intExpire.Interval = Utils.ParseInt(settings["ExpireLimit"], 0);
            intTtl.Interval = Utils.ParseInt(settings["MinimumTTL"], 0);
            chkAdMode.Checked = Utils.ParseBool(settings["AdMode"], false);

            //DNS RecordTTL
            txtRecordDefaultTTL.Text = settings["RecordDefaultTTL"];
            txtRecordMinimumTTL.Text = settings["RecordMinimumTTL"];


            iPAddressesList.BindSettings(settings);
            secondaryDNSServers.BindSettings(settings);
            nameServers.Value = settings["NameServers"];
        }

        public void SaveSettings(StringDictionary settings)
        {
			settings["AllowZoneTransfers"] = txtAllowZoneTransfers.Text;
			settings["ResponsiblePerson"] = txtResponsiblePerson.Text;
            settings["RefreshInterval"] = intRefresh.Interval.ToString();
            settings["RetryDelay"] = intRetry.Interval.ToString();
            settings["ExpireLimit"] = intExpire.Interval.ToString();
            settings["MinimumTTL"] = intTtl.Interval.ToString();
            settings["AdMode"] = chkAdMode.Checked.ToString();

            //DNS RecordTTL
            settings["RecordDefaultTTL"] = txtRecordDefaultTTL.Text;
            settings["RecordMinimumTTL"] = txtRecordMinimumTTL.Text;

            iPAddressesList.SaveSettings(settings);
            secondaryDNSServers.SaveSettings(settings);
            settings["NameServers"] = nameServers.Value;
        }
    }
}
