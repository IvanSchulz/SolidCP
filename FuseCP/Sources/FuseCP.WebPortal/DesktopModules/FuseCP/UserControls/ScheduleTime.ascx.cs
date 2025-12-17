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
    public partial class ScheduleTime : FuseCPControlBase
    {
        public DateTime SelectedValue
        {
            get
            {
                int hours = Utils.ParseInt(txtHours.Text.Trim(), 0);
                int minutes = Utils.ParseInt(txtMinutes.Text.Trim(), 0);
                return new DateTime(2000, 1, 1, hours, minutes, 0);
            }
            set
            {
                txtHours.Text = PadNumber(value.Hour.ToString());
                txtMinutes.Text = PadNumber(value.Minute.ToString());
            }
        }

        private string PadNumber(string s)
        {
            return (s.Length == 1) ? "0" + s : s;
        }

        public bool Enabled
        {
            get
            {
                return txtHours.Enabled;
            }
            set
            {
                txtHours.Enabled = value;
                txtMinutes.Enabled = value;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
        }
    }
}
