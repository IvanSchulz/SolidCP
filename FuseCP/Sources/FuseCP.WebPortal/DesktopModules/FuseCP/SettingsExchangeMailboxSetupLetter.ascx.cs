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

using FuseCP.EnterpriseServer;

namespace FuseCP.Portal
{
    public partial class SettingsExchangeMailboxSetupLetter : FuseCPControlBase, IUserSettingsEditorControl
    {
        public void BindSettings(UserSettings settings)
        {
            txtFrom.Text = settings["From"];
            txtCC.Text = settings["CC"];
            txtSubject.Text = settings["Subject"];
			Utils.SelectListItem(ddlPriority, settings["Priority"]);
            txtHtmlBody.Text = settings["HtmlBody"];
            txtTextBody.Text = settings["TextBody"];
        }

        public void SaveSettings(UserSettings settings)
        {
            settings["From"] = txtFrom.Text;
            settings["CC"] = txtCC.Text;
            settings["Subject"] = txtSubject.Text;
			settings["Priority"] = ddlPriority.SelectedValue;
            settings["HtmlBody"] = txtHtmlBody.Text;
            settings["TextBody"] = txtTextBody.Text;
        }
    }
}
