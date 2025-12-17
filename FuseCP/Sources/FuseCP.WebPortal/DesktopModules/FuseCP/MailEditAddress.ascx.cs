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
    public partial class MailEditAddress : FuseCPControlBase
    {
        public bool EditMode
        {
            get { return (ViewState["EditMode"] != null) ? (bool)ViewState["EditMode"] : false; }
            set { ViewState["EditMode"] = value; ToggleControls(); }
        }

        public int PackageId
        {
            get { return domainsSelectDomainControl.PackageId; }
            set
            {
                domainsSelectDomainControl.PackageId = value;

                // set account policy
                txtName.SetPackagePolicy(value, "MailPolicy", "AccountNamePolicy");
            }
        }

        public string Email
        {
            get
            {
				if (EditMode)
				{
					return litName.Text;
				}
				  return txtName.Text.Trim() + "@" + domainsSelectDomainControl.DomainName;
				
            }
            set { litName.Text = value; }
        }
        public string Domain
        {
            get { return domainsSelectDomainControl.DomainName; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
        }

        private void ToggleControls()
        {
            EditEmailPanel.Visible = !EditMode;
            DisplayEmailPanel.Visible = EditMode;

            litName.Visible = EditMode;
            txtName.EditMode = EditMode;
        }
    }
}
