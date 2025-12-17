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
    public partial class PasswordPolicyEditor : FuseCPControlBase
    {
        public bool ShowLockoutSettings { get; set; }

        public string Value
        {
            get
            {
                StringBuilder sb = new StringBuilder();
                sb.Append(chkEnabled.Checked.ToString()).Append(";");
                sb.Append(txtMinimumLength.Text).Append(";");
                sb.Append(txtMaximumLength.Text).Append(";");
                sb.Append(txtMinimumUppercase.Text).Append(";");
                sb.Append(txtMinimumNumbers.Text).Append(";");
                sb.Append(txtMinimumSymbols.Text).Append(";");
                sb.Append(chkNotEqualUsername.Checked.ToString()).Append(";");
                sb.Append(txtLockedOut.Text).Append(";");

                sb.Append(txtEnforcePasswordHistory.Text).Append(";");
                sb.Append(txtAccountLockoutDuration.Text).Append(";");
                sb.Append(txtResetAccountLockout.Text).Append(";");
                sb.Append(chkLockOutSettigns.Checked.ToString()).Append(";");
                sb.Append(chkPasswordComplexity.Checked.ToString()).Append(";");

                sb.Append(txtMaxPasswordAge.Text).Append(";");

                return sb.ToString();
            }
            set
            {
                if (String.IsNullOrEmpty(value))
                {
                    // bind default settings
                    chkEnabled.Checked = false;
                    txtMinimumLength.Text = "3";
                    txtMaximumLength.Text = "10";
                    txtMinimumUppercase.Text = "0";
                    txtMinimumNumbers.Text = "0";
                    txtMinimumSymbols.Text = "0";
                    txtLockedOut.Text = "3";
                    chkPasswordComplexity.Checked = true;
                    txtMaxPasswordAge.Text = "42";
                }
                else
                {
                    try
                    {
                        // parse settings
                        string[] parts = value.Split(';');
                        chkEnabled.Checked = Utils.ParseBool(parts[0], false);
                        txtMinimumLength.Text = parts[1];
                        txtMaximumLength.Text = parts[2];
                        txtMinimumUppercase.Text = parts[3];
                        txtMinimumNumbers.Text = parts[4];
                        txtMinimumSymbols.Text = parts[5];
                        chkNotEqualUsername.Checked = Utils.ParseBool(parts[6], false);
                        txtLockedOut.Text = parts[7];

                        txtEnforcePasswordHistory.Text = GetValueSafe(parts, 8, "0");
                        txtAccountLockoutDuration.Text = GetValueSafe(parts, 9, "0");
                        txtResetAccountLockout.Text = GetValueSafe(parts, 10, "0");
                        chkLockOutSettigns.Checked = GetValueSafe(parts, 11, false) && ShowLockoutSettings;
                        chkPasswordComplexity.Checked = GetValueSafe(parts, 12, true);

                        txtMaxPasswordAge.Text = GetValueSafe(parts, 13, "42");
                    }
                    catch
                    {
                        /* skip */
                    }
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

            ToggleLockOutSettignsControls();
            TogglePasswordCompelxitySettignsControls();

            RowChkLockOutSettigns.Visible = ShowLockoutSettings;
        }

        protected void chkEnabled_CheckedChanged(object sender, EventArgs e)
        {
            ToggleControls();
        }

        private void ToggleLockOutSettignsControls()
        {
            RowAccountLockoutDuration.Visible = chkLockOutSettigns.Checked;
            RowLockedOut.Visible = chkLockOutSettigns.Checked;
            RowResetAccountLockout.Visible = chkLockOutSettigns.Checked;
        }

        private void TogglePasswordCompelxitySettignsControls()
        {
            RowMinimumUppercase.Visible = chkPasswordComplexity.Checked;
            RowMinimumNumbers.Visible = chkPasswordComplexity.Checked;
            RowMinimumSymbols.Visible = chkPasswordComplexity.Checked;
        }

        protected void chkLockOutSettigns_CheckedChanged(object sender, EventArgs e)
        {
            ToggleLockOutSettignsControls();
        }

        protected void chkPasswordComplexity_CheckedChanged(object sender, EventArgs e)
        {
            TogglePasswordCompelxitySettignsControls();
        }

        public static T GetValueSafe<T>(string[] array, int index, T defaultValue)
        {
            try
            {
                if (array.Length > index)
                {
                    if (string.IsNullOrEmpty(array[index]))
                    {
                        return defaultValue;
                    }

                    return (T)Convert.ChangeType(array[index], typeof(T));
                }
            }
            catch{}

            return defaultValue;
        }

    }
}
