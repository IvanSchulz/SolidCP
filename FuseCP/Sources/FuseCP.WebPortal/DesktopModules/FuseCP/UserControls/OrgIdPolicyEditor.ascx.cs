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
using System.Web.UI;

namespace FuseCP.Portal.UserControls
{
    public partial class OrgIdPolicyEditor : UserControl
    {
        #region Properties

        public string Value
        {
            get
            {
                var sb = new StringBuilder();
                sb.Append(enablePolicyCheckBox.Checked.ToString()).Append(";");
                sb.Append(txtMaximumLength.Text).Append(";");

                return sb.ToString();
            }
            set
            {
                if (String.IsNullOrEmpty(value))
                {
                    enablePolicyCheckBox.Checked = true;
                    txtMaximumLength.Text = "128";
                }
                else
                {
                    try
                    {
                        string[] parts = value.Split(';');
                        enablePolicyCheckBox.Checked = Utils.ParseBool(parts[0], false);
                        txtMaximumLength.Text = parts[1];
                    }
                    catch
                    {
                    }
                }

                ToggleControls();
            }
        }

        #endregion

        #region Methods

        protected void Page_Load(object sender, EventArgs e)
        {
        }

        private void ToggleControls()
        {
            PolicyTable.Visible = enablePolicyCheckBox.Checked;
        }

        #endregion

        #region Event Handlers

        protected void EnablePolicy_CheckedChanged(object sender, EventArgs e)
        {
            ToggleControls();
        }

        #endregion
    }
}
