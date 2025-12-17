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
using System.Collections.Specialized;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

namespace FuseCP.Portal.ProviderControls
{
    public partial class MSSQL_Settings : FuseCPControlBase, IHostingServiceProviderSettings
    {
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        public void BindSettings(StringDictionary settings)
        {
            txtInternalAddress.Text = settings["InternalAddress"];
            txtExternalAddress.Text = settings["ExternalAddress"];
			Utils.SelectListItem(ddlCollation, settings["DatabaseCollation"]);

            rblAccountType.SelectedIndex = Utils.ParseBool(settings["UseTrustedConnection"], false) ? 0 : 1;

            txtUserName.Text = settings["SaLogin"];
            ViewState["PWD"] = settings["SaPassword"];
            rowPassword.Visible = ((string)ViewState["PWD"]) != "";

            rblDbLocation.SelectedIndex = Utils.ParseBool(settings["UseDefaultDatabaseLocation"], true) ? 0 : 1;
            txtDatabasesLocation.Text = settings["DatabaseLocation"];

            txtBackupPath.Text = settings["DatabaseBackupLocation"];
            txtBackupNetworkPath.Text = settings["DatabaseBackupNetworkPath"];

            // add default collation
            ddlCollation.Items.Insert(0, new ListItem(GetLocalizedString("DefaultLocation.Text"), ""));

			txtBrowseUrl.Text = settings["BrowseURL"];
			Utils.SelectListItem(ddlBrowseMethod, settings["BrowseMethod"]);
			txtBrowseParameters.Text = settings["BrowseParameters"];

            chkTrustServerCertificate.Checked = Utils.ParseBool(settings["TrustServerCertificate"], false);

            ToggleControls();
        }

        public void SaveSettings(StringDictionary settings)
        {
            settings["InternalAddress"] = txtInternalAddress.Text.Trim();
            settings["ExternalAddress"] = txtExternalAddress.Text.Trim();
			settings["DatabaseCollation"] = ddlCollation.SelectedValue;
            settings["UseTrustedConnection"] = Convert.ToString(rblAccountType.SelectedIndex == 0);
            settings["SaLogin"] = txtUserName.Text.Trim();
            settings["SaPassword"] = (txtPassword.Text.Length > 0) ? txtPassword.Text : (string)ViewState["PWD"];
            settings["TrustServerCertificate"] = Convert.ToString(chkTrustServerCertificate.Checked);
            settings["UseDefaultDatabaseLocation"] = Convert.ToString(rblDbLocation.SelectedIndex == 0);
            settings["DatabaseLocation"] = txtDatabasesLocation.Text.Trim();

            settings["DatabaseBackupLocation"] = txtBackupPath.Text;
            settings["DatabaseBackupNetworkPath"] = txtBackupNetworkPath.Text;


            settings["BrowseURL"] = txtBrowseUrl.Text.Trim();
			settings["BrowseMethod"] = ddlBrowseMethod.SelectedValue;
			settings["BrowseParameters"] = txtBrowseParameters.Text;
        }

        private void ToggleControls()
        {
            tblAccount.Visible = (rblAccountType.SelectedIndex == 1);
            tblLocation.Visible = (rblDbLocation.SelectedIndex == 1);
        }

        protected void rblAccountType_SelectedIndexChanged(object sender, EventArgs e)
        {
            ToggleControls();
        }

        protected void rblDbLocation_SelectedIndexChanged(object sender, EventArgs e)
        {
            ToggleControls();
        }
    }
}
