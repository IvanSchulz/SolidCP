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
    public partial class ScheduleInterval : FuseCPControlBase
    {
        public int Interval
        {
            get
            {
                int multiplier = 1;
                if (ddlUnits.SelectedIndex == 0)
                    multiplier = 86400;
                else if (ddlUnits.SelectedIndex == 1)
                    multiplier = 3600;
                else if (ddlUnits.SelectedIndex == 2)
                    multiplier = 60;
                else if (ddlUnits.SelectedIndex == 3)
                    multiplier = 1;

                return Utils.ParseInt(txtInterval.Text.Trim(), 0) * multiplier;
            }
            set
            {
                ListItem item = ddlUnits.SelectedItem;
                if (item != null)
                    item.Selected = false;

                int s = value;
                if (s % 86400 == 0)
                {
                    // days
                    ddlUnits.SelectedIndex = 0;
                    txtInterval.Text = ((int)(s / 86400)).ToString();
                }
                else if (s % 3600 == 0)
                {
                    // hours
                    ddlUnits.SelectedIndex = 1;
                    txtInterval.Text = ((int)(s / 3600)).ToString();
                }
                else if (s % 60 == 0)
                {
                    // minutes
                    ddlUnits.SelectedIndex = 2;
                    txtInterval.Text = ((int)(s / 60)).ToString();
                }
                else
                {
                    // seconds
                    ddlUnits.SelectedIndex = 3;
                    txtInterval.Text = s.ToString();
                }
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
        }
    }
}
