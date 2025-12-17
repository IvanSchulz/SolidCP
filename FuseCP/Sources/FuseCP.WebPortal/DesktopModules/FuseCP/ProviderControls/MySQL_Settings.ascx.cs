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
using FuseCP.EnterpriseServer;

namespace FuseCP.Portal.ProviderControls
{
    public partial class MySQL_Settings : FuseCPControlBase, IHostingServiceProviderSettings
    {
        protected void Page_Load(object sender, EventArgs e)
        {
			if (!IsPostBack)
			{
				//RenderFtuNote();
			}
        }

		/*private void RenderFtuNote()
		{
			string ftuNote = GetLocalizedString("FirsttimeUserNote");
			//
			ServerInfo serverInfo = ES.Services.Servers.GetServerById(PanelRequest.ServerId);
			//
			lblFirsttimeUserNote.InnerHtml = String.Format(ftuNote, serverInfo.ServerName);
		}*/

        public void BindSettings(StringDictionary settings)
        {
            txtInternalAddress.Text = settings["InternalAddress"];
            txtExternalAddress.Text = settings["ExternalAddress"];
            txtBinFolder.Text = settings["InstallFolder"];
			chkOldPassword.Checked = Utils.ParseBool(settings["OldPassword"], false);
            chkSslMode.Checked = Utils.ParseBool(settings["SslMode"], false);
            txtUserName.Text = settings["RootLogin"];
            ViewState["PWD"] = settings["RootPassword"];
            rowPassword.Visible = ((string)ViewState["PWD"]) != "";

			txtBrowseUrl.Text = settings["BrowseURL"];
			Utils.SelectListItem(ddlBrowseMethod, settings["BrowseMethod"]);
			txtBrowseParameters.Text = settings["BrowseParameters"];
        }

        public void SaveSettings(StringDictionary settings)
        {
            settings["InternalAddress"] = txtInternalAddress.Text.Trim();
            settings["ExternalAddress"] = txtExternalAddress.Text.Trim();
            settings["InstallFolder"] = txtBinFolder.Text.Trim();
			settings["OldPassword"] = chkOldPassword.Checked.ToString();
            settings["SslMode"] = chkSslMode.Checked.ToString();
            settings["RootLogin"] = txtUserName.Text.Trim();
            settings["RootPassword"] = (txtPassword.Text.Length > 0) ? txtPassword.Text : (string)ViewState["PWD"];
			settings["BrowseURL"] = txtBrowseUrl.Text.Trim();
			settings["BrowseMethod"] = ddlBrowseMethod.SelectedValue;
			settings["BrowseParameters"] = txtBrowseParameters.Text;
        }
    }
}
