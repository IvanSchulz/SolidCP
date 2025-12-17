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

namespace FuseCP.Portal.ExchangeServer.UserControls
{
	public partial class DaysBox : System.Web.UI.UserControl
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
				valRequireNumber.Enabled = value;
			}
		}

        public bool RequireValidatorEnabled
        {
            get { return valRequireNumber.Enabled; }
            set { valRequireNumber.Enabled = value; }
        }


		public int ValueDays
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

		protected void Page_Load(object sender, EventArgs e)
		{

		}
	}
}
