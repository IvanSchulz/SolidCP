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
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

namespace FuseCP.Portal
{
    public partial class QuotaViewer : FuseCPControlBase
    {
        public bool DisplayGauge
        {
            get { return gauge.DisplayGauge; }
            set { gauge.DisplayGauge = value; UpdateControl(); }
        }

        public int QuotaTypeId
        {
            get { return (ViewState["QuotaTypeId"] != null) ? (int)ViewState["QuotaTypeId"] : 2; }
            set { ViewState["QuotaTypeId"] = value; UpdateControl(); }
        }

        public int QuotaUsedValue
        {
            set
            {
                // store value
                gauge.Progress = value;

                // upodate control
                UpdateControl();
            }
        }

        public int QuotaValue
        {
            set
            {
                // store value
                gauge.Total = value;

                // update control
                UpdateControl();
            }
        }

        public int QuotaAvailable
        {
            set
            {
                // store value
                gauge.Available = value;

                // update control
                UpdateControl();
            }
        }


        private void UpdateControl()
        {
            int total = gauge.Total;
            if (QuotaTypeId == 1)
            {
                litValue.Text = (total == 0) ? GetLocalizedString("Text.Disabled") : GetLocalizedString("Text.Enabled");
                litValue.CssClass = (total == 0) ? "NormalRed" : "NormalGreen";
                gauge.Visible = false;
            }
            else if (QuotaTypeId == 2)
            {
                string availableText = string.Empty;
                if (gauge.Available != -1) availableText = String.Format("({0} {1})", gauge.Available.ToString(), GetLocalizedString("Text.Available"));

                litValue.Text = String.Format("{0} {1} {2} {3}",
                    gauge.Progress, GetLocalizedString("Text.Of"), ((total == -1) ? GetLocalizedString("Text.Unlimited") : total.ToString()), availableText);

                if ((gauge.Progress < 0) & (total == -1))
                    litValue.Text = GetLocalizedString("Text.Unlimited");

                gauge.Visible = (total != -1);
                //litValue.Visible = (value == -1);
            }
            else if (QuotaTypeId == 3)
            {
                litValue.Text = (total == -1) ? GetLocalizedString("Text.Unlimited") : total.ToString();
                gauge.Visible = false;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {

        }
    }
}
