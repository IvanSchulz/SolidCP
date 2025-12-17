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
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace FuseCP.Portal.UserControls
{
    public partial class EditVLANControl : FuseCPControlBase
    {
        public bool Required
        {
            get { return requireVLANValidator.Enabled; }
            set { requireVLANValidator.Enabled = value; }
        }

        public string ValidationGroup
        {
            get { return requireVLANValidator.ValidationGroup; }
            set
            {
                requireVLANValidator.ValidationGroup = value;
                vlanValidator.ValidationGroup = value;
            }
        }

        public Unit Width
        {
            get { return txtVLAN.Width; }
            set { txtVLAN.Width = value; }
        }

        public string Text
        {
            get { return txtVLAN.Text.Trim(); }
            set { txtVLAN.Text = value; }
        }

        public string RequiredErrorMessage
        {
            get { return requireVLANValidator.ErrorMessage; }
            set { requireVLANValidator.ErrorMessage = value; }
        }

        public string FormatErrorMessage
        {
            get { return vlanValidator.ErrorMessage; }
            set { vlanValidator.ErrorMessage = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        public void Validate(object source, ServerValidateEventArgs args)
        {
            try
            {
                var vlanStr = args.Value;
                int vlan = -1;
                if (string.IsNullOrEmpty(vlanStr))
                {
                    args.IsValid = false;
                    return;
                }
                args.IsValid = int.TryParse(vlanStr, out vlan) && vlan >= 0 && vlan <= 4094;
            }
            catch (Exception)
            {
                args.IsValid = false;
            }
        }
    }
}
