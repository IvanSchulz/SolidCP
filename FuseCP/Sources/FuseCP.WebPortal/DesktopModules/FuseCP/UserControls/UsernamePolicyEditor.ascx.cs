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
using System.Text;
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
    public partial class UsernamePolicyEditor : FuseCPControlBase
    {
        public string Value
        {
            get
            {
                StringBuilder sb = new StringBuilder();
                sb.Append(chkEnabled.Checked.ToString()).Append(";");
                sb.Append(txtAllowedSymbols.Text).Append(";");
                sb.Append(txtMinimumLength.Text).Append(";");
                sb.Append(txtMaximumLength.Text).Append(";");
                sb.Append(txtPrefix.Text.Trim()).Append(";");
                sb.Append(txtSuffix.Text.Trim()).Append(";");
                return sb.ToString();
            }
            set
            {
                if (String.IsNullOrEmpty(value))
                {
                    // bind default settings
                    chkEnabled.Checked = false;
                    txtAllowedSymbols.Text = "";
                    txtMinimumLength.Text = "3";
                    txtMaximumLength.Text = "10";
                    txtPrefix.Text = "";
                    txtSuffix.Text = "";
                }
                else
                {
                    try
                    {
                        // parse settings
                        string[] parts = value.Split(';');
                        chkEnabled.Checked = Utils.ParseBool(parts[0], false);
                        txtAllowedSymbols.Text = parts[1];
                        txtMinimumLength.Text = parts[2];
                        txtMaximumLength.Text = parts[3];
                        txtPrefix.Text = parts[4];
                        txtSuffix.Text = parts[5];
                    }
                    catch { /* skip */ }
                }
				ToggleControls();
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
        }

		private void ToggleControls()
		{
			PolicyTable.Visible = chkEnabled.Checked;
		}

		protected void chkEnabled_CheckedChanged(object sender, EventArgs e)
		{
			ToggleControls();
		}
    }
}
