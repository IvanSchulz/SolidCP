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
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FuseCP.Portal.Code.UserControls;
using FCP = FuseCP.EnterpriseServer;

namespace FuseCP.Portal.ExchangeServer.UserControls
{
    public partial class EnterpriseStorageEditFolderTabs : FuseCPControlBase
    {
        public string SelectedTab { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            BindTabs();
        }

        private void BindTabs()
        {
            List<Tab> tabsList = new List<Tab>();
            tabsList.Add(CreateTab("enterprisestorage_folder_settings", "Tab.GeneralSettings"));
            tabsList.Add(CreateTab("enterprisestorage_folder_settings_folder_permissions", "Tab.FolderPermissions"));

            FCP.SystemSettings settings = ES.Services.System.GetSystemSettings(FCP.SystemSettings.WEBDAV_PORTAL_SETTINGS);
            bool isOwaEnabled = false;
            if (settings != null)
            {
                isOwaEnabled = Utils.ParseBool(settings[FCP.SystemSettings.WEBDAV_OWA_ENABLED_KEY], false);
            }
            if (isOwaEnabled) tabsList.Add(CreateTab("enterprisestorage_folder_settings_owa_editing", "Tab.OwaEditPermissions"));

            int idx = 0;

            foreach (Tab tab in tabsList)
            {
                if (String.Compare(tab.Id, SelectedTab, true) == 0)
                {
                    break;
                }

                idx++;
            }

            esTabs.SelectedIndex = idx;
            esTabs.DataSource = tabsList;
            esTabs.DataBind();
        }

        private Tab CreateTab(string id, string text)
        {
            return new Tab(id,GetLocalizedString(text),
                HostModule.EditUrl("AccountID", PanelRequest.AccountID.ToString(), id,
                "SpaceID=" + PanelSecurity.PackageId.ToString(),
                "ItemID=" + PanelRequest.ItemID.ToString(), "FolderID=" + PanelRequest.FolderID));
        }
    }
}
