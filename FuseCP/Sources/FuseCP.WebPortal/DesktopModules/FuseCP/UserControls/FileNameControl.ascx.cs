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
    public partial class FileNameControl : FuseCPControlBase
    {
        public string ValidationGroup
        {
            get
            {
                return valCorrectNewName.ValidationGroup;
            }
            set
            {
                valCorrectNewName.ValidationGroup = value;
                valRequireNewName.ValidationGroup = value;
            }
        }

        public string Text
        {
            get { return txtFileName.Text.Trim(); }
            set { txtFileName.Text = value; }
        }

		public Unit Width
		{
			get { return txtFileName.Width; }
			set { txtFileName.Width = value; }
		}

        protected void Page_Load(object sender, EventArgs e)
        {

        }
    }
}
