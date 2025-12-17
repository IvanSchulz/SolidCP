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

﻿using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace FuseCP.Portal.UserControls
{
	[Flags]
	public enum IPValidationMode { V4 = 1, V6 = 2, V4AndV6 = 3 };

    public partial class EditIPAddressControl : FuseCPControlBase
    {

		public IPValidationMode Validation { get; set; }

		public EditIPAddressControl() { Validation = IPValidationMode.V4AndV6; AllowSubnet = false; }

        public bool Required
        {
            get { return requireAddressValidator.Enabled; }
            set { requireAddressValidator.Enabled = value; }
        }

        public string ValidationGroup
        {
            get { return requireAddressValidator.ValidationGroup; }
            set
            {
                requireAddressValidator.ValidationGroup = value;
                addressValidator.ValidationGroup = value;
            }
        }

        public string CssClass
        {
            get { return txtAddress.CssClass; }
            set { txtAddress.CssClass = value; }
        }

        public Unit Width
        {
            get { return txtAddress.Width; }
            set { txtAddress.Width = value; }
        }

        public string Text
        {
            get { return txtAddress.Text.Trim(); }
            set { txtAddress.Text = value; }
        }

        public string RequiredErrorMessage
        {
            get { return requireAddressValidator.ErrorMessage; }
            set { requireAddressValidator.ErrorMessage = value; }
        }

        public string FormatErrorMessage
        {
            get { return addressValidator.ErrorMessage; }
            set { addressValidator.ErrorMessage = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {

        }

		public bool AllowSubnet { get; set; }
		public bool IsV6 { get; private set; }
		public bool IsMask { get; private set; }

		public void Validate(object source, ServerValidateEventArgs args) {
			IsMask = IsV6 = false;
			var ip = args.Value;
			int net = 0;
			if (ip.Contains("/")) {
				args.IsValid = AllowSubnet;
				var tokens = ip.Split('/');
				ip = tokens[0];
				args.IsValid &= int.TryParse(tokens[1], out net) && net <= 128;
				if (string.IsNullOrEmpty(ip)) {
					IsMask = true;
					return;
				}
			}
			System.Net.IPAddress ipaddr;
			args.IsValid &= System.Net.IPAddress.TryParse(ip, out ipaddr) && (ip.Contains(":") || ip.Contains(".")) &&
                (((Validation & IPValidationMode.V6) != 0 && (IsV6 = ipaddr.AddressFamily == System.Net.Sockets.AddressFamily.InterNetworkV6)) ||
				((Validation & IPValidationMode.V4) != 0 && ipaddr.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork));
			args.IsValid &= ipaddr.AddressFamily != System.Net.Sockets.AddressFamily.InterNetwork || net < 32;
		}
    }
}
