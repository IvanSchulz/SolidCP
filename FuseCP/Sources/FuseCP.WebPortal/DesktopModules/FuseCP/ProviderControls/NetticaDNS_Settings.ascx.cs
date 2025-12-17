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
    public partial class NetticaDNS_Settings : FuseCPControlBase, IHostingServiceProviderSettings
    {
        //public const string Password = "Password";
        
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        public void BindSettings(StringDictionary settings)
        {
            txtPassword.Text = settings[Constants.Password];
            ViewState["PWD"] = settings[Constants.Password];
            rowPassword.Visible = ((string)ViewState["PWD"]) != null;
            txtUserName.Text = settings[Constants.UserName];
            secondaryDNSServers.BindSettings(settings);            
            iPAddressesList.BindSettings(settings);
            cbApplyDefaultTemplate.Checked = Utils.ParseBool(settings["ApplyDefaultTemplate"], false);
        }

        public void SaveSettings(StringDictionary settings)
        {
            settings[Constants.UserName] = txtUserName.Text;
            //settings[Constants.Password] = string.IsNullOrEmpty(txtPassword.Text) ? (ViewState[Password] != null ? ViewState[Password].ToString(): string.Empty): txtPassword.Text;
            settings[Constants.Password] = (txtPassword.Text.Length > 0) ? txtPassword.Text : (string)ViewState["PWD"];
            secondaryDNSServers.SaveSettings(settings);
            iPAddressesList.SaveSettings(settings);
            settings["ApplyDefaultTemplate"] = cbApplyDefaultTemplate.Checked.ToString();
        }
    }
}
