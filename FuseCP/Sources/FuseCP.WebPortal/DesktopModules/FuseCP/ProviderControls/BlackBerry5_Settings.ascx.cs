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
using System.Collections.Specialized;
using FuseCP.Providers.Common;

namespace FuseCP.Portal.ProviderControls
{
    public partial class BlackBerry5_Settings : FuseCPControlBase, IHostingServiceProviderSettings
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        public void BindSettings(StringDictionary settings)
        {
            txtPath.Text = settings[Constants.UtilityPath];
            txtHandheldcleanupPath.Text = settings[Constants.HandheldcleanupPath];
            txtPassword.Text = settings[Constants.Password];
            txtEnterpriseServer.Text = settings[Constants.EnterpriseServer];
            txtEnterpriseServerFQDN.Text = settings[Constants.EnterpriseServerFQDN];
            txtMAPIProfile.Text = settings[Constants.MAPIProfile];
            ViewState["PWD"] = settings[Constants.Password];
            txtUser.Text = settings[Constants.UserName];
        }

        public void SaveSettings(StringDictionary settings)
        {
            settings[Constants.UtilityPath] = txtPath.Text;
            settings[Constants.HandheldcleanupPath] = txtHandheldcleanupPath.Text;
            settings[Constants.EnterpriseServer] = txtEnterpriseServer.Text;
            settings[Constants.EnterpriseServerFQDN] = txtEnterpriseServerFQDN.Text;
            settings[Constants.Password] = (txtPassword.Text.Length > 0) ? txtPassword.Text : (string)ViewState["PWD"];
            settings[Constants.UserName] = txtUser.Text;
            settings[Constants.MAPIProfile] = txtMAPIProfile.Text;
        }
    }
}
