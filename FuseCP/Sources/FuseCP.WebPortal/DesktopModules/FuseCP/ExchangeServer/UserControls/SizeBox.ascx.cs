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

namespace FuseCP.Portal.ExchangeServer.UserControls
{
    public partial class SizeBox : System.Web.UI.UserControl
    {
        int emptyValue = -1;

        public int EmptyValue
        {
            get { return emptyValue; }
            set { emptyValue = value; }
        }

        public string ValidationGroup
        {
            get { return valRequireCorrectNumber.ValidationGroup; }
            set { valRequireCorrectNumber.ValidationGroup = valRequireNumber.ValidationGroup = value; }
        }

        public bool Enabled
        {
            get { return txtValue.Enabled; }
            set
            {
                txtValue.Enabled = value;
                valRequireCorrectNumber.Enabled = value;
            }
        }

        public bool RequireValidatorEnabled
        {
            get { return valRequireNumber.Enabled; }
            set { valRequireNumber.Enabled = value; }
        }

        public int ValueKB
        {
            get
            {
                string val = txtValue.Text.Trim();
                return val == "" ? emptyValue : Utils.ParseInt(val, 0);
            }
            set
            {
                txtValue.Text = value == emptyValue ? "" : value.ToString();
            }
        }

        public bool DisplayUnitsKB
        {
            get { return locKB.Visible; }
            set {
                locKB.Visible = value;
            }
        }

        public bool DisplayUnitsMB
        {
            get { return locMB.Visible; }
            set {
                locMB.Visible = value;
            }
        }

        public bool DisplayUnitsPct
        {
            get { return locPct.Visible; }
            set { 
                
                locPct.Visible = value;
                }
        }

        protected void Page_Load(object sender, EventArgs e)
        {

            if (locPct.Visible)
            {
                valRequireCorrectNumber.ValidationExpression = @"(^100)$|^([0-9]{1,2})$";
            }
            else
                valRequireCorrectNumber.ValidationExpression = @"[0-9]{0,15}";

        }
    }
}
