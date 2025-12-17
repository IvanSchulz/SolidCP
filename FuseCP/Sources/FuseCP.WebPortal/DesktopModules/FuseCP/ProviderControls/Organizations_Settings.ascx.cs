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

namespace FuseCP.Portal.ProviderControls
{
    public partial class Organizations_Settings : FuseCPControlBase, IHostingServiceProviderSettings
    {
        public const string RootOU = "RootOU";
        public const string PrimaryDomainController = "PrimaryDomainController";
        public const string TemporyDomainName = "TempDomain";
        public const string UserNameFormat = "UserNameFormat";
        public const string ArchiveStoragePath = "ArchiveStoragePath";
        public const string UseStorageSpaces = "UseStorageSpaces";
        
        protected void Page_Load(object sender, EventArgs e)
        {

        }      

        public void BindSettings(System.Collections.Specialized.StringDictionary settings)
        {
            txtPrimaryDomainController.Text = settings[PrimaryDomainController];  
            txtRootOU.Text = settings[RootOU];
            txtTemporyDomainName.Text = settings[TemporyDomainName];

            if (settings.ContainsKey(UserNameFormat))
            {
                UserNameFormatDropDown.SelectedValue =
                    UserNameFormatDropDown.Items.FindByText(settings[UserNameFormat]).Value;
            }

            txtArchiveStorageSpace.Text = settings[ArchiveStoragePath];
            chkUseStorageSpaces.Checked = Utils.ParseBool(settings[UseStorageSpaces], false);

            chkUseStorageSpaces_StateChanged(null, null);
        }

        public void SaveSettings(System.Collections.Specialized.StringDictionary settings)
        {
            settings[RootOU] = txtRootOU.Text.Trim();
            settings[PrimaryDomainController] = txtPrimaryDomainController.Text.Trim();
            settings[TemporyDomainName] = txtTemporyDomainName.Text.Trim();
            settings[UserNameFormat] = UserNameFormatDropDown.SelectedItem.Text;
            settings[ArchiveStoragePath] = txtArchiveStorageSpace.Text.Trim();
            settings[UseStorageSpaces] = chkUseStorageSpaces.Checked.ToString();
        }

        protected void chkUseStorageSpaces_StateChanged(object sender, EventArgs e)
        {
            txtArchiveStorageSpace.Visible = !chkUseStorageSpaces.Checked;
        }
    }
}
